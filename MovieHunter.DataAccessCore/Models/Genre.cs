using System;
using System.Collections.Generic;
using System.Text;

namespace MovieHunter.DataAccessCore.Models
{
    public partial class Genre
    {

        public Genre()
        {
            Movie = new HashSet<Movie>();
        }

        public int GenreId { get; set; }
        public string GenreName { get; set; }

        public ICollection<Movie> Movie { get; set; }
    }
}
