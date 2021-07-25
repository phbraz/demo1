using demo1.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo1.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<HolidayRequestViewModel> holidayRequests { get; set; }
    }
}
