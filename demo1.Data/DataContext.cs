using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo1.Data
{
    public class DataContext : DbContext
    {
        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<HolidayRequest> HolidayRequests { get; set; }

        public DbSet<HolidayRequestApproval> HolidayRequestApprovals { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=.;Database=Demo1;Trusted_Connection=True;ConnectRetryCount=0");
        }
    }
}
