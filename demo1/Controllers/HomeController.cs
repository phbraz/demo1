using demo1.Services;
using demo1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using demo1.Data;

namespace demo1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            var holidayService = new HolidayRequestService();

            var vm = new HomeIndexViewModel
            {
                holidayRequests = holidayService.GetAll()
            };

            return View(vm);                
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddHolidayRequest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertHolidayRequest(HolidayRequestViewModel holiday)
        {
            var startDate = holiday.StartDate;
            
            var endDate = holiday.EndDate;
            
            var requestorName = holiday.RequesterName;

            var holidayType = holiday.HolidayType;


            using (var db = new DataContext())
            {
                var holidayRequest = db.Set<HolidayRequest>();
                holidayRequest.Add(new HolidayRequest
                {
                    EndDate = endDate,
                    StartDate = startDate,
                    RequesterName = requestorName,
                    HolidayType = holidayType

                });

                db.SaveChanges();
            }


                return RedirectToAction("Index", "Home");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
