using System;
using System.Collections.Generic;

namespace MovieHunter.DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            List = new HashSet<List>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public ICollection<List> List { get; set; }
    }
}
