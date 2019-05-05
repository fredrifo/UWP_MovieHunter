using System;
using System.Collections.Generic;

namespace MovieHunter.DataAccess.Models
{
    public partial class List
    {
        public List()
        {
            ListItem = new HashSet<ListItem>();
        }

        public int ListId { get; set; }
        public int UserId { get; set; }
        public string ListName { get; set; }

        public User User { get; set; }
        public ICollection<ListItem> ListItem { get; set; }
    }
}
