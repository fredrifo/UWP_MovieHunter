using System;
using System.Collections.Generic;

namespace MovieHunter.DataAccessCore.Client.Models
{
    public partial class Person
    {
        public Person()
        {
            MovieDirector = new HashSet<Movie>();
            MovieStarNavigation = new HashSet<Movie>();
            MovieWriter = new HashSet<Movie>();
        }

        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Picture { get; set; }

        public ICollection<Movie> MovieDirector { get; set; }
        public ICollection<Movie> MovieStarNavigation { get; set; }
        public ICollection<Movie> MovieWriter { get; set; }
    }
}
