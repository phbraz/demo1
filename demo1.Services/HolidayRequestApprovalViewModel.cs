using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using demo1.Data;

namespace demo1.Services
{
    public class HolidayRequestApprovalViewModel
    {
        public IEnumerable<HolidayRequestViewModel> RequesterName { get; set; }
        public int HolidayRequestId { get; set; }

        [Required(ErrorMessage = "Created Date is required")]
        public DateTime CreatedDate { get; set; }
        
        public string Note { get; set; }
        
        public ApprovalStatus Status { get; set; }



    }
}
