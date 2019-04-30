using System;
using MovieHunter.Classes;
using MovieHunter.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MovieHunter.Views
{
    public sealed partial class MainPage : Page
    {
        public string ImageSource {
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

        public string Rating
        {
            get;
            set;
        }

        public string ReleaseYear
        {
            get;
            set;
        }

        public string Runtime
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



        private MainViewModel ViewModel
        {
            get { return ViewModelLocator.Current.MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
            string coverurl = "https://pdfimages.wondershare.com/forms-templates/medium/movie-poster-template-3.png";

            //If page opens with no movie params
            this.CoverTitle = "Null";
            this.Category = "Null";
            this.Director = "Null";
            this.Writer = "Null";
            this.Stars = "Null";
            this.Summary = "Summary: Null";
            this.CoverUri = coverurl;

            //MainPage movie = new MainPage("Null", "Null", "Null", "Null", "Null", coverurl);
        }

        public MainPage(string CoverTitle,  string Category, string Director, string Stars, string Summary, string CoverUri, int Rating)
        {
            InitializeComponent();

            //this.CoverTitle = CoverTitle;
            //this.Category = Category;
            //this.Director = Director;
            //this.Stars = Stars;
            //this.Summary = Summary;
            //this.CoverUri = CoverUri;

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameters = e.Parameter as Movie;

            if(parameters != null)
            {
                this.CoverTitle = parameters.CoverTitle;
                this.Category = parameters.Category;
                this.Director = parameters.Director;
                this.Writer = parameters.Writer;
                this.Stars = parameters.Stars;
                this.Summary = parameters.Summary;
                this.CoverUri = parameters.CoverUri;
                this.starRating.Width = (6 * 68) / 2; //(6 IMDB score * 68pixels each star) / 2 to get it to a  3/5
            }
                
                
        }

    }
}
