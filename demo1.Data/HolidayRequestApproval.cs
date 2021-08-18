using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo1.Data
{
    public class HolidayRequestApproval
    {
        
        public int Id { get; set; }

        public int HolidayRequestId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Note { get; set; }
        public virtual HolidayRequest HolidayRequest { get; set; }
        
        public ApprovalStatus Status { get; set; }
    }


    public enum ApprovalStatus
    {
        None,
        Pending,
        Approved,
        Rejected        
    }
}
