using System;
using MovieHunter.Classes;
using MovieHunter.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MovieHunter.Views
{
    public sealed partial class MoviePage : Page
    {
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

        private MovieViewModel ViewModel
        {
            get { return ViewModelLocator.Current.MovieViewModel; }
        }

        public MoviePage()
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameters = e.Parameter as Movie;

            if (parameters != null)
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

        private void OpenTrailerPopupClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            if (!TrailerPopup.IsOpen) {
                TrailerPopup.IsOpen = true;
            }
            var c = Window.Current.Bounds;
            webView2.Width = c.Width - 400;
            webView2.Height = c.Height-50;

            string videoID = "tYrND5hMY3A";
            string html = @"<iframe width=""100%"" height=""100%"" src=""http://www.youtube.com/embed/" + videoID + @"?rel=0"" frameborder=""0""></iframe>";
            webView2.NavigateToString(html);
        }

        private void CloseTrailerPopupClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (TrailerPopup.IsOpen) {
                TrailerPopup.IsOpen = false;
            }
        }
    }
}
