using System;
using System.Collections.Generic;
using System.Text;

namespace MovieHunter.DataAccess.Client.Models
{
    public partial class AllListItems
    {
        public int ListItemId { get; set; }
        public int ListId { get; set; }
        public int MovieId { get; set; }

        public string ListMessage { get; set; }
    }
}
