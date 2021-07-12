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
        public List<HolidayRequestViewModel> GetAll()
        {
            using var context = new DataContext();

            return context.HolidayRequests.Select(x => new HolidayRequestViewModel 
            {
                EndDate = x.EndDate,
                HolidayType = x.HolidayType,
                RequesterName = x.RequesterName,
                StartDate = x.StartDate
            }
            ).ToList();
        }

    }
}
