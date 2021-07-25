﻿using System;
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
            

            return _dataContext.HolidayRequests.GroupBy(x => x.RequesterName)/*Select(x => x.Sum(y => y.EndDate.Subtract(y.StartDate).TotalDays == 0 ? 1 : y.EndDate.Subtract(y.StartDate).TotalDays))*/
                .Select(x => new HolidayRequestViewModel
                {
                    //EndDate = null,
                    //HolidayType = null,
                    RequesterName = x.Key,
                    //StartDate = null,
                    TotalHolidays = x.Sum(y => EF.Functions.DateDiffDay(y.StartDate, y.EndDate)),
                    RemainingHolidays = 28 - x.Sum(y =>  EF.Functions.DateDiffDay(y.StartDate, y.EndDate)) 
                }
            );
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
