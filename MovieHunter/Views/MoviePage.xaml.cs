using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MovieHunter.DataAccess.Client.ApiCalls;
using MovieHunter.DataAccess.Models;
using MovieHunter.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MovieHunter.Views
{
    public sealed partial class MoviePage : Page
    {

        private ObservableCollection<Movie> _listItems = new ObservableCollection<Movie>();

        //Collection for listItems in the listview
        public ObservableCollection<Movie> ListItems
        {
            get { return this._listItems; }
        }

        static public int? ListId
        {
            get;
            set;
        }

        private MovieViewModel ViewModel
        {
            get { return ViewModelLocator.Current.MovieViewModel; }
        }

        /// <summary>Initializes a new instance of the <see cref="MoviePage"/> class.
        /// Adds the default template values for the movie. then calls the fillContent method
        /// </summary>
        public MoviePage()
        {
            InitializeComponent();

            string coverurl = "https://pdfimages.wondershare.com/forms-templates/medium/movie-poster-template-3.png";


            Genre genreName = new Genre() { GenreName = "Fetching" };
            ListItems.Add(
                new Movie
                {
                    Title = "Title: Fetching...",
                    GenreName = "Fetching",
                    Genre = genreName,
                    DirectorName = "Fetching",
                    WriterName = "Fetching",
                    StarName = "Fetching",
                    Summary = "Summary: Fetching",
                    CoverImage = coverurl,
                    Rating = 5
                });
                

            //Loading the information sent from the onNavigateTo() parameters
            Loaded += (sender, args) => FillContentAsync(this);
        }

        static public DataAccess.Models.Movie MovieContainer
        {
            get;
            set;
        }

        /// <summary>Adds the infomation from the chosen Movie to the UI</summary>
        /// <param name="thisT">The current page</param>
        public async void FillContentAsync(MoviePage thisT)
        {
            //Getting a list of all Genres
            ObservableCollection<Genre> allGenres = await GenreCalls.GetGenres();

            ////Getting a list of all People
            ObservableCollection<Person> allPeople = await PersonCalls.GetPeople();

            
            Genre genreName = new Genre() { GenreName = "Fetching" };

            //Creating strings for a Movie object
            string title = "title";
            string genre = "genre";
            string director = "director";
            string writer = "writer";
            string movieStar = "movieStar";
            string summary = "summary";
            string coveruri = "https://pdfimages.wondershare.com/forms-templates/medium/movie-poster-template-3.png";
            byte rating = 0;


            try
            {
                //Adding the name of the genreId with an api call 
                genre = GenreCalls.GetGenreNameFromList(allGenres, Convert.ToInt32(MovieContainer.GenreId));
            }
            catch
            {
                //Category will not be displayed with the name
                genre = MovieContainer.GenreId.ToString();
            }
            try
            {
                //finding the name of the director with help of the id from the parameter (API CALL)
                //if "parameters.directorid" == null the convertion to int32 fails
                director = PersonCalls.GetPersonNameFromList(allPeople, Convert.ToInt32(MovieContainer.DirectorId));
            }
            catch
            {
                //person will not be displayed with the name
                director = MovieContainer.DirectorId.ToString();
            }
            try
            {
                //finding the name of the writer with help of the id from the parameter
                //if "parameters.writerid" == null the convertion to int32 fails
                writer = PersonCalls.GetPersonNameFromList(allPeople, Convert.ToInt32(MovieContainer.WriterId));
            }
            catch
            {
                //person will not be displayed with the name
                writer = MovieContainer.WriterId.ToString();
            }
            try
            {
                //finding the name of the actor star with help of the id from the parameter
                //if "parameters.star" == null the convertion to int32 fails and is Catched
                movieStar = PersonCalls.GetPersonNameFromList(allPeople, Convert.ToInt32(MovieContainer.Star));
            }
            catch
            {
                //person will not be displayed with the name
                movieStar = MovieContainer.Star.ToString();
            }

            //self explainatory
            summary = MovieContainer.Summary;
            coveruri = MovieContainer.CoverImage;
            title = MovieContainer.Title;

            //converting the rating to a string to be displayed in the bound textblock
            rating = Convert.ToByte(MovieContainer.Rating);

            //defines the width of the yellow fill in the star image located above the cover image
            //there are 5 stars with the width of 68px.
            //since rating is up to 10 i need to divide it by 2. this makes the stars and rating equal.
            //the "starrating.width" fills the stars so that it represents the real value.
            double amountRatingStars = Convert.ToDouble((MovieContainer.Rating * 68) / 2); // rating/2 * 68pixels each star)

            //Clearing the listitems
            ListItems.Clear();

            //Adding a listitem with values specified above
            ListItems.Add(
                new Movie()
                {
                    Title = title,
                    GenreName = genre,
                    Genre = genreName,
                    DirectorName = director,
                    WriterName = writer,
                    StarName = movieStar,
                    Summary = summary,
                    CoverImage = coveruri,
                    Rating = rating, //Rating
                    RatingImageWidth = amountRatingStars,
                });
        }


        /// <summary>Closes the trailer popup.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseTrailerPopupClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (TrailerPopup.IsOpen) {
                TrailerPopup.IsOpen = false;
            }
        }

        /// <summary>Other button clicked. Displays the user token.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>Opens the trailer in a popup</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// Saves the parametes as a Movie that can be retrieved in this class.
        /// </summary>
        /// <param name="e">
        /// Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                var parameters = (e.Parameter as MovieHunter.DataAccess.Models.Movie);
                if (parameters != null)
                {
                    MovieContainer = parameters;
                }

            }
            catch
            {
                return;
            }
        }
    }
}
