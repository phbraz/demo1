using System;
using System.ComponentModel.DataAnnotations;
using demo1.Data;

namespace demo1.Services
{
    public class HolidayRequestViewModel
    {
        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage ="End Date is required")]
        public DateTime EndDate { get; set; }
        public string RequesterName { get; set; }
        public HolidayType HolidayType { get; set; }

        public double TotalHolidays { get; set; }

        public double RemainingHolidays { get; set; }

        public ApprovalStatus Status { get; set; }

        public string  UserName { get; set; }

        public int userId { get; set; }

    }
}
