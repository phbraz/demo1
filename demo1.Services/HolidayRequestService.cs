using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using demo1.Data;
using Microsoft.EntityFrameworkCore;

namespace demo1.Services
{
   public class HolidayRequestService
    {
        private readonly DataContext _dataContext;

        public HolidayRequestService()
        {
            _dataContext = new DataContext();
        }


        public IEnumerable<HolidayRequestViewModel> GetAll()
        {
            

            return _dataContext.HolidayRequests.GroupBy(x => x.RequesterName)
                .Select(x => new HolidayRequestViewModel
                {
                    RequesterName = x.Key,                    
                    TotalHolidays = x.Sum(y => EF.Functions.DateDiffDay(y.StartDate, y.EndDate)),
                    RemainingHolidays = 28 - x.Sum(y =>  EF.Functions.DateDiffDay(y.StartDate, y.EndDate)) 
                }
            );
        }

        public IEnumerable<UserHolidayRequestViewModel> GetholidayPerCurrentUser(string userName)
        {

            var t = _dataContext.HolidayRequests.
                Join(
                    _dataContext.Users,
                    hr => hr.UserId,
                    u => u.Id,
                    (hr, u) => new UserHolidayRequestViewModel
                    {
                        UserId = hr.UserId,
                        UserName = u.UserName,
                        StartDate = hr.StartDate,
                        EndDate = hr.EndDate
                    }
                ).Where(x => x.UserName == userName);

            var result = t.GroupBy(x => x.UserId)
                .Select(x => new UserHolidayRequestViewModel
                {
                    UserId = x.Key,
                    TotalHolidays = x.Sum(y => EF.Functions.DateDiffDay(y.StartDate, y.EndDate)),
                    RemainingHolidays = 28 - x.Sum(y => EF.Functions.DateDiffDay(y.StartDate, y.EndDate))

                }
                );

            return result;
        }

        public IEnumerable<HolidayRequestViewModel> GetFullHolidayHistory(string reqName, ApprovalStatus ? status = null)
        {
            var query =  _dataContext.HolidayRequests
                .Select(x => new HolidayRequestViewModel
                {
                    EndDate = x.EndDate,
                    HolidayType = x.HolidayType,
                    RequesterName = x.RequesterName,
                    StartDate = x.StartDate,
                    Status = x.HolidayRequestApprovals.Any(x => x.Status == ApprovalStatus.Approved) 
                        ? ApprovalStatus.Approved 
                        :  x.HolidayRequestApprovals.Any(x => x.Status == ApprovalStatus.Rejected) 
                            ? ApprovalStatus.Rejected 
                            : ApprovalStatus.Pending

                }).Where(x => x.RequesterName == reqName);
            
            if (status != null)
            {
                query = query.Where(x => x.Status == status);

            }
            return query;

        }


        public bool AddHoliday(HolidayRequestViewModel holiday)
        {

            var allowHolidayrequest = AllowHolidays(holiday.RequesterName, holiday.StartDate, holiday.EndDate);


            if (allowHolidayrequest)
            {
                var holidayRequestDataContext = _dataContext.Set<HolidayRequest>();

                
                var userName = new User
                {
                    UserName = holiday.UserName
                };

                //fetching currentUser Details
                var userDetails = GetUser(userName).FirstOrDefault(); 


                var holidaRequest = new HolidayRequest
                {
                    EndDate = holiday.EndDate,
                    StartDate = holiday.EndDate,
                    RequesterName = holiday.RequesterName,
                    HolidayType = holiday.HolidayType,
                    UserId = userDetails.Id
                };

                holidayRequestDataContext.Add(holidaRequest);
                _dataContext.SaveChanges();

            }

            return allowHolidayrequest;
                             
        }
        
        public bool AllowHolidays(string reqName, DateTime startDate, DateTime endDate)
        {
            var result = true;

            var list = GetAll();

            var daysRequested = (endDate - startDate).TotalDays; //from current request

            foreach (var i in list)
            {
                if (i.RequesterName == reqName)
                {
                    if (i.RemainingHolidays <= 0 || daysRequested > i.RemainingHolidays)
                    {
                        result = false;

                    }

                }

            }

            return result;

        }

        public IEnumerable<HolidayRequestViewModel> GetRequestersNames()
        {
            return _dataContext.HolidayRequests.GroupBy(x => x.RequesterName)
                .Select(x => new HolidayRequestViewModel
                    {
                        RequesterName = x.Key                        
                    }
                );            
        }


        public IEnumerable<User> GetUser(User ? user = null)
        {
            return _dataContext.Users
                .Select(x => new User
                {
                    Id = x.Id,
                    IsAdmin = x.IsAdmin,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Password = x.Password,
                    UserName = x.UserName,
                    CreatedDate = x.CreatedDate


                }).Where(x => x.UserName == user.UserName);
        }

        public void CreateUser(UserHolidayRequestViewModel user)
        {
            //encrypting the pass

            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var encrypt = new Rfc2898DeriveBytes(user.Password, salt, 10000);
            byte[] hash = encrypt.GetBytes(20);

            //combine salt and hashed password

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var savePassowordHash = Convert.ToBase64String(hashBytes);

            var addUserRequest = _dataContext.Set<User>();

            addUserRequest.Add(new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedDate = DateTime.Now,
                UserName = user.UserName,
                Password = savePassowordHash

               
            });

            _dataContext.SaveChanges();
            
        }


        public UserHolidayRequestViewModel LoginUser(UserHolidayRequestViewModel user)
        {
            var userVm = new User
            {
                UserName = user.UserName
            };

            var userData = GetUser(userVm).FirstOrDefault();

            var vm = new UserHolidayRequestViewModel
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                IsAdmin = userData.IsAdmin,
                UserName = userData.UserName,
                UserId = userData.Id

            };


            var savedHashPass = userData.Password.ToString();

            
            byte[] hashBytes = Convert.FromBase64String(savedHashPass);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            
            // Compute the hash on the password the user entered
            
            var encrypt = new Rfc2898DeriveBytes(user.Password, salt, 10000);
            byte[] hash = encrypt.GetBytes(20);
            
            // Compare the results
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    throw new UnauthorizedAccessException();

                }

            }

            return vm;
        }

    }
}
