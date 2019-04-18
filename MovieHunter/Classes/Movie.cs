using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieHunter.Classes
{
    public sealed partial class Movie
    {
        //Setters and Getters for class

        public string ImageSource
        {
            get;
            set;
        }
        public string CoverTitle
        {
            get;
            set;
        }
        public string Category
        {
            get;
            set;
        }
        public string Director
        {
            get;
            set;
        }
        public string Writer
        {
            get;
            set;
        }
        public string Stars
        {
            get;
            set;
        }
        public string Summary
        {
            get;
            set;
        }
        public string CoverUri
        {
            get;
            set;
        }


        //Setter for creating new movie object
        public Movie(string CoverTitle, string Category, string Director, string Stars, string Summary, string CoverUri)
        {
            this.CoverTitle = CoverTitle;
            this.Category = Category;
            this.Director = Director;
            this.Stars = Stars;
            this.Summary = Summary;
            this.CoverUri = CoverUri;
        }
    }
}
    
