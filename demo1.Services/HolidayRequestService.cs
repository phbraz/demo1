using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo1.Data;

namespace demo1.Services
{
   public class HolidayRequestService
    {
        private readonly DataContext _dataContext;

        public HolidayRequestService()
        {
            _dataContext = new DataContext();
        }


        public List<HolidayRequestViewModel> GetAll()
        {


            return _dataContext.HolidayRequests.Select(x => new HolidayRequestViewModel 
            {
                EndDate = x.EndDate,
                HolidayType = x.HolidayType,
                RequesterName = x.RequesterName,
                StartDate = x.StartDate
            }
            ).ToList();
        }


        public void AddHoliday(HolidayRequestViewModel holiday)
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
    }
}
