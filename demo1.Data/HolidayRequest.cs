using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo1.Data
{
    public class HolidayRequest
    {
        [Required]
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RequesterName { get; set; }
        public HolidayType HolidayType { get; set; }

        public ICollection<HolidayRequestApproval> HolidayRequestApprovals { get; set; }

        public virtual User User { get; set; }


    }

    public enum HolidayType
    {
        Sickness,
        Maternity,
        AnnualLeave
    }
}
