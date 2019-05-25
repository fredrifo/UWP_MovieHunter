using System;
using System.Collections.Generic;

namespace MovieHunter.DataAccessCore.Models
{
    public partial class Movie
    {
        public Movie()
        {
            ListItem = new HashSet<ListItem>();
        }

        public int MovieId { get; set; }
        public string Title { get; set; }
        public string CoverImage { get; set; }
        public int? GenreId { get; set; }
        public string Summary { get; set; }
        public byte? Rating { get; set; }
        public int? DirectorId { get; set; }
        public int? WriterId { get; set; }
        public int? Star { get; set; }

        public Person Director { get; set; }
        public Genre Genre { get; set; }
        public Person StarNavigation { get; set; }
        public Person Writer { get; set; }
        public ICollection<ListItem> ListItem { get; set; }
    }
}
