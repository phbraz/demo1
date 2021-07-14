using System;
using demo1.Data;

namespace demo1.Services
{
    public class HolidayRequestViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RequesterName { get; set; }
        public HolidayType HolidayType { get; set; }

        public int TotalHolidays { get; set; }

    }
}
