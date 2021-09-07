using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo1.Services
{
    public class UserHolidayRequestViewModel
    {
        //user details
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public string Token { get; set; }

        //Holiday 
        
        public double TotalHolidays { get; set; }

        public double RemainingHolidays { get; set; }

        //test date 

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
