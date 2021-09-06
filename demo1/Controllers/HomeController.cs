using demo1.Services;
using demo1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using demo1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace demo1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly HolidayRequestService _holidayService;

        private readonly TokenService _tokenService;

        public IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;

            _holidayService = new HolidayRequestService();

            _configuration = config;

            _tokenService = new TokenService();

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginPage()
        {
            if (ValidateToken())
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var vm = new UserHolidayRequestViewModel
                {
                    UserName = null,
                    Token = null
                };

                return View(vm);

            }            
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult LoginUser(UserHolidayRequestViewModel user)
        {

            var userDetails = _holidayService.LoginUser(user);

            if (userDetails != null)
            {
                CreateAuthToken(userDetails);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("LoginPage", "Home");
            }           

        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult RegisterUser(UserHolidayRequestViewModel user)
        {
            _holidayService.CreateUser(user);

            TempData["Message"] = "User has been created, please login";

            return RedirectToAction("LoginPage", "Home");

        }

        [HttpGet]
        public IActionResult LogOut()
        {
            if (ValidateToken())
            {
                //cleaning cookies 

                HttpContext.Response.Cookies.Delete("Authorization");
                HttpContext.Response.Cookies.Delete("user");
                HttpContext.Response.Cookies.Delete("permission");

                return RedirectToAction("LoginPage", "Home");


            }
            else
            {
                return RedirectToAction("LoginPage", "Home");
            }

        }


        [HttpGet]
        public IActionResult Index()
        {

            if (ValidateToken())
            {
                var user = HttpContext.Request.Cookies
                .Where(x => x.Key == "user").FirstOrDefault();

                var vm = new HomeIndexViewModel
                {
                    userHolidayRequests = _holidayService.GetholidayPerCurrent(user.Value)
                };

                return View(vm);
            }
            else
            {
                return RedirectToAction("LoginPage", "Home");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddHolidayRequest()
        {
            if (ValidateToken())
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginPage", "Home");
            }
        }

        [HttpGet]
        public IActionResult SpecificHolidayRequest(string reqName, ApprovalStatus ? status = null)
        {
            if (ValidateToken())
            {
                var vm = new HomeIndexViewModel
                {
                    holidayRequests = _holidayService.GetFullHolidayHistory(reqName, status)
                };

                return View(vm);
            }
            else
            {
                return RedirectToAction("LoginPage", "Home");
            }
        }

        [HttpPost]
        public IActionResult InsertHolidayRequest(HolidayRequestViewModel holiday)
        {
            if (ValidateToken())
            {
                //catching values from cookies it seems easier than through  view
                var fullName = HttpContext.Request.Cookies
                .Where(x => x.Key == "fullname").FirstOrDefault();

                var userName = HttpContext.Request.Cookies
                .Where(x => x.Key == "user").FirstOrDefault();





                holiday.UserName = userName.Value;
                holiday.RequesterName = fullName.Value;

                if (!_holidayService.AddHoliday(holiday))
                {
                    TempData["Message"] = "Requester does not have any remaining holidays or request exceed the remaining days";
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("LoginPage", "Home");
            }
                                         
        }



        [HttpGet]
        public IActionResult HolidayRequestApprovals(string ? reqName = null)
        {
            if (ValidateToken())
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
                        });
                }
                return View(vm);
            }
            else
            {
                return RedirectToAction("LoginPage", "Home");
            } 
        }


        [HttpGet]
        public IActionResult GetHolidayDetailsForRequester(string reqName)
        {
            if (ValidateToken())
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
            else
            {
                return RedirectToAction("LoginPage", "Home");
            }
        
        }

        //Generate token
        public void CreateAuthToken(UserHolidayRequestViewModel user)
        {
            //Grab Token 
            var myToken = _tokenService.GenerateToken(user, _configuration);

            //Convert token to string bearer
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(myToken).ToString();


            //create a new cookie with that token string and some user information to retrive later, maybe a better way to handle it? 
            HttpContext.Response.Cookies.Append("Authorization", tokenStr);
            HttpContext.Response.Cookies.Append("user", user.UserName);
            HttpContext.Response.Cookies.Append("permission", user.IsAdmin.ToString());
            HttpContext.Response.Cookies.Append("fullname", user.FirstName + " "+ user.LastName);







            user.Token = tokenStr;


        }


        //Need to find a better way to validate the token for now this one might work.
        public bool  ValidateToken()
        {
            var authCookie = HttpContext.Request.Cookies
                .Where(x => x.Key == "Authorization").FirstOrDefault();

            if (authCookie.Value !=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
