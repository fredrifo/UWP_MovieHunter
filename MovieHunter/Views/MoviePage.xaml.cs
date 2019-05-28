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
        // Creating objects so that i can use binding
        // This Movie object already contains all of the definitions so It would be better to not redefine everything
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

        private void CloseTrailerPopupClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (TrailerPopup.IsOpen) {
                TrailerPopup.IsOpen = false;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog notification = new ContentDialog
            {
                Title = "Login Token",
                Content = LoginPage.token,
                CloseButtonText = "Ok"
            };
            ContentDialogResult resulst = await notification.ShowAsync();
            
        }

        private void OpenTrailerPopupClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            if (!TrailerPopup.IsOpen)
            {
                TrailerPopup.IsOpen = true;
            }
            var c = Window.Current.Bounds;
            webView2.Width = c.Width - 400;
            webView2.Height = c.Height - 50;

            string videoID = "tYrND5hMY3A";
            string html = @"<iframe width=""100%"" height=""100%"" src=""http://www.youtube.com/embed/" + videoID + @"?rel=0"" frameborder=""0""></iframe>";
            webView2.NavigateToString(html);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameters = e.Parameter as MovieHunter.DataAccess.Models.Movie;


            if (parameters != null)
            {
                try
                {
                    //Setting the bindings to be equal to the parameters
                    this.CoverTitle = parameters.Title;
                    this.Category = parameters.GenreId.ToString();
                    this.Director = parameters.DirectorId.ToString();
                    this.Writer = parameters.WriterId.ToString();
                    this.Stars = parameters.Star.ToString();
                    this.Summary = parameters.Summary;
                    this.CoverUri = parameters.CoverImage;
                    //Converting the rating to a string to be displayed in the bound textblock
                    this.Rating = parameters.Rating.ToString();

                    //Defines the width of the yellow fill in the star image located above the icon
                    //Since rating is from 0-10 i need to divide it by 2. This makes the rating go from 0-5.
                    //There are 5 stars with the width of 68.
                    //Rating times width fills the stars width colour so that it represents the real value.
                    this.starRating.Width = (Convert.ToDouble(parameters.Rating) * 68) / 2; // Rating/2 * 68pixels each star)
                }
                catch
                {

                }
                


                
            
            }
        }
    }
}
