using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo1.Data
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Password { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsAdmin { get; set; }
    }
}
