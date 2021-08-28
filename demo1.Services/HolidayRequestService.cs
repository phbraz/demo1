using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo1.Data;
using Microsoft.EntityFrameworkCore;

namespace demo1.Services
{
   public class HolidayRequestService
    {
        private readonly DataContext _dataContext;

        public HolidayRequestService()
        {
            _dataContext = new DataContext();
        }


        public IEnumerable<HolidayRequestViewModel> GetAll()
        {
            

            return _dataContext.HolidayRequests.GroupBy(x => x.RequesterName)
                .Select(x => new HolidayRequestViewModel
                {
                    RequesterName = x.Key,                    
                    TotalHolidays = x.Sum(y => EF.Functions.DateDiffDay(y.StartDate, y.EndDate)),
                    RemainingHolidays = 28 - x.Sum(y =>  EF.Functions.DateDiffDay(y.StartDate, y.EndDate)) 
                }
            );
        }

        public IEnumerable<HolidayRequestViewModel> GetFullHolidayHistory(string reqName, ApprovalStatus ? status = null)
        {
            var query =  _dataContext.HolidayRequests
                .Select(x => new HolidayRequestViewModel
                {
                    EndDate = x.EndDate,
                    HolidayType = x.HolidayType,
                    RequesterName = x.RequesterName,
                    StartDate = x.StartDate,
                    Status = x.HolidayRequestApprovals.Any(x => x.Status == ApprovalStatus.Approved) 
                        ? ApprovalStatus.Approved 
                        :  x.HolidayRequestApprovals.Any(x => x.Status == ApprovalStatus.Rejected) 
                            ? ApprovalStatus.Rejected 
                            : ApprovalStatus.Pending

                }).Where(x => x.RequesterName == reqName);
            
            if (status != null)
            {
                query = query.Where(x => x.Status == status);

            }
            return query;

        }


        public bool AddHoliday(HolidayRequestViewModel holiday)
        {

            var allowHolidayrequest = AllowHolidays(holiday.RequesterName, holiday.StartDate, holiday.EndDate);

            if (allowHolidayrequest)
            {
                var holidayRequest = _dataContext.Set<HolidayRequest>();
                holidayRequest.Add(new HolidayRequest
                {
                    EndDate = holiday.EndDate,
                    StartDate = holiday.StartDate,
                    RequesterName = holiday.RequesterName,
                    HolidayType = holiday.HolidayType

                });

                _dataContext.SaveChanges();

            }

            return allowHolidayrequest;
                             
        }
        
        public bool AllowHolidays(string reqName, DateTime startDate, DateTime endDate)
        {
            var result = true;

            var list = GetAll();

            var daysRequested = (endDate - startDate).TotalDays; //from current request

            foreach (var i in list)
            {
                if (i.RequesterName == reqName)
                {
                    if (i.RemainingHolidays <= 0 || daysRequested > i.RemainingHolidays)
                    {
                        result = false;

                    }

                }

            }

            return result;

        }

        public IEnumerable<HolidayRequestViewModel> GetRequestersNames()
        {
            return _dataContext.HolidayRequests.GroupBy(x => x.RequesterName)
                .Select(x => new HolidayRequestViewModel
                    {
                        RequesterName = x.Key                        
                    }
                );            
        }

    }
}
