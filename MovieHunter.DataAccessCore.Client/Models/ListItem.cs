using System;
using System.Collections.Generic;

namespace MovieHunter.DataAccessCore.Client.Models
{
    public partial class ListItem
    {
        public int ListItemId { get; set; }
        public int ListId { get; set; }
        public int MovieId { get; set; }

        public List List { get; set; }
        public Movie Movie { get; set; }
    }
}
