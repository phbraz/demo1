﻿using demo1.Services;
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

        private readonly HolidayRequestService _holidayService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            _holidayService = new HolidayRequestService();            
        }

        public IActionResult Index()
        {
            var vm = new HomeIndexViewModel
            {
                holidayRequests = _holidayService.GetAll()
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

        [HttpGet]
        public IActionResult SpecificHolidayRequest(string reqName, ApprovalStatus ? status = null)
        {
            var vm = new HomeIndexViewModel
            {
                holidayRequests = _holidayService.GetFullHolidayHistory(reqName, status)
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult InsertHolidayRequest(HolidayRequestViewModel holiday)
        {
            if (!ModelState.IsValid)
            {
                return View("AddHolidayRequest");

            }

            if (!_holidayService.AddHoliday(holiday))
            {

                TempData["Message"] = "Requester does not have any remaining holidays or request exceed the remaining days";
                return RedirectToAction("Index", "Home");

            }
                                          
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult HolidayRequestApprovals(string ? reqName = null)
        {
           
            var vm = new HolidayRequestApprovalViewModel
            {
                RequesterName = _holidayService.GetRequestersNames()
                                    
            };


            if (reqName != null)
            {
                    vm.PendingHolidays = _holidayService.GetFullHolidayHistory(reqName, ApprovalStatus.Pending)
                    .Select(
                        x => new PendingHolidayRequestViewModel
                        {
                            EndDate = x.EndDate,
                            StartDate = x.StartDate,
                            RequesterName = x.RequesterName
                        }
                        );

            }


            return View(vm);
        }

        [HttpGet]
        public IActionResult GetHolidayDetailsForRequester(string reqName)
        {
            var query = _holidayService.GetFullHolidayHistory(reqName, ApprovalStatus.Pending)
                    .Select(
                        x => new PendingHolidayRequestViewModel
                        {
                            EndDate = x.EndDate,
                            StartDate = x.StartDate,
                            RequesterName = x.RequesterName
                        }
                        );

            return Json(query);
        
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
