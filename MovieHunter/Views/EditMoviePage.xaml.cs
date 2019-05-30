using System;
using MovieHunter.DataAccess.Client.ApiCalls;
using MovieHunter.DataAccess.Models;
using MovieHunter.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MovieHunter.Views
{
    public sealed partial class EditMoviePage : Page
    {
        private EditMovieViewModel ViewModel
        {
            get { return ViewModelLocator.Current.EditMovieViewModel; }
        }
        static public DataAccess.Models.Movie MovieContainer
        {
            get;
            set;
        }

        /// <summary>Initializes a new instance of the <see cref="EditMoviePage"/> class.</summary>
        public EditMoviePage()
        {
            InitializeComponent();

            //Loading the information sent from the onNavigateTo() parameters
            Loaded += (sender, args) => FillContentAsync(this);
        }

        /// <summary>Fills the content in the UI asynchronous.</summary>
        /// <param name="editMoviePage">The edit movie page.</param>
        private void FillContentAsync(EditMoviePage editMoviePage)
        {

                //Creating strings for a Movie object
                inp_MovieTitle.Text = MovieContainer.Title;
                inp_MovieSummary.Text = MovieContainer.Summary;
                image_Cover.UriSource = new System.Uri(MovieContainer.CoverImage);

        }

        /// <summary>Handles the UpdateMovie event of the Btn control.
        /// Creates the updated movie object
        /// Updates the database by sending a putRequest
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private async void Btn_UpdateMovie(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Movie updateMovieObject = new Movie()
            {
                MovieId = MovieContainer.MovieId,
                Title = inp_MovieTitle.Text,
                Summary = inp_MovieSummary.Text,
                DirectorId = MovieContainer.DirectorId,
                WriterId = MovieContainer.WriterId,
                Star = MovieContainer.Star,
                Rating = MovieContainer.Rating,
                GenreId = MovieContainer.GenreId,
                CoverImage = MovieContainer.CoverImage
            };

            //Sending PutRequest
            await MovieCalls.PutMovie(updateMovieObject, MovieContainer.MovieId);
        }

        private void MovieRatingChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {

        }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// When a frame navigations to this page with parameters.
        /// The parameter is in this Case a Movie Object. This object is then saved in the MovieContainer object.
        /// This makes it easy to handle the object data.
        /// </summary>
        /// <param name="e">
        /// Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                var parameters = e.Parameter as MovieHunter.DataAccess.Models.Movie;
                if (parameters != null)
                {
                    //Adding to Movie MovieContainer object
                    MovieContainer = parameters;
                }
            }
            catch
            {
                //Could not add parameter to Movie Object
                return;
            }
        }
    }
}
