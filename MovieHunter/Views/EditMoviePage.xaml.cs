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

        public EditMoviePage()
        {
            InitializeComponent();

            //Loading the information sent from the onNavigateTo() parameters
            Loaded += (sender, args) => FillContentAsync(this);
        }

        private void FillContentAsync(EditMoviePage editMoviePage)
        {

                //Creating strings for a Movie object
                inp_MovieTitle.Text = MovieContainer.Title;
                inp_MovieSummary.Text = MovieContainer.Summary;
                image_Cover.UriSource = new System.Uri(MovieContainer.CoverImage);

        }

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                var parameters = e.Parameter as MovieHunter.DataAccess.Models.Movie;
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
