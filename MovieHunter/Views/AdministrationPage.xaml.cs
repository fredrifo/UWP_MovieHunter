using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using MovieHunter.DataAccess.Models;
using MovieHunter.DataAccess.Client.ApiCalls;
using MovieHunter.ViewModels;
using Newtonsoft.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MovieHunter.Views
{
    public sealed partial class AdministrationPage : Page
    {
        public int directorId;
        public int writerId;
        public int starId;

        public int genreId;

        public Visibility Visibility { get; set; }

        private AdministrationViewModel ViewModel
        {
            get { return ViewModelLocator.Current.AdministrationViewModel; }
        }

        public AdministrationPage()
        {
            suggestions = new ObservableCollection<string>();
            InitializeComponent();
        }

        private ObservableCollection<String> suggestions;

        private async void AutoSuggestBox_Person_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing,
            // otherwise assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                //sender.ItemsSource = dataset;
                suggestions.Clear();
                if (sender.Text.Length < 1)
                {
                    return;
                }
                string currentSearch = sender.Text;

                //HttpPost
                string jsonInput = JsonConvert.SerializeObject(currentSearch);

                var client = new HttpClient();
                try
                {
                    string uri = "http://localhost:59713/api/People/search";
                    var httpResponse = await client.PostAsync(uri, new StringContent(jsonInput, Encoding.UTF8, "application/json"));

                    if (httpResponse.Content != null)
                    {
                        var json = await httpResponse.Content.ReadAsStringAsync();
                        //await new MessageDialog(json, "Response").ShowAsync();

                        //Clearing all suggestions
                        suggestions.Clear();

                        //Itterating through the response dyamically so that it will fetch the item parameters at runtime
                        dynamic dynJson = JsonConvert.DeserializeObject(json);
                        foreach (var item in dynJson)
                        {
                            suggestions.Add("" + item.personId + ": " + item.firstName + " " + item.lastName); 
                        }

                        if (!suggestions.Any())
                        {
                            suggestions.Add("No Results");
                        }

                        sender.ItemsSource = suggestions;
                    }
                }
                catch
                {
                    await new MessageDialog("Failed to add person to database", "Response").ShowAsync();
                }

                
            }
        }

        private async void AutoSuggestBox_Person_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
                sender.Text = args.ChosenSuggestion.ToString();

                //Get the object ID
                string searchId = ColonSeperatorAsync(args.ChosenSuggestion.ToString()).ToString();

                if (searchId == false.ToString())
                {
                    output_Movie.Text += "'No result' is not a valid choice\r\n";
                    return;
                }
                //Setting the appropriate string value
                if (sender.Name == "txtAutoSuggestBox_StarActor")
                {
                    starId = Convert.ToInt32(searchId);
                }
                else if (sender.Name == "txtAutoSuggestBox_Writer")
                {
                    writerId = Convert.ToInt32(searchId);
                }
                else if (sender.Name == "txtAutoSuggestBox_Director")
                {
                    directorId = Convert.ToInt32(searchId);
                }



            }
        }

        private string ColonSeperatorAsync(string search)
        {
            string searchId = search;
            //Code to find Id from the suggestionBox. This could be avoided by using a combobox that would store the id as well
            //but the search functiunality in a combobox is built in it has to contain all the rows from the database.

            //Get the id that is before the colon in the suggestionbox
            if (searchId.ToLower().Contains(": "))
            {
                int index = searchId.IndexOf(": ");
                if (index > 0)
                {
                    searchId = searchId.Substring(0, index);
                }

                return searchId;
            }
            else
            {
                //if there is no colon, there was no results
                return false.ToString();
            }
        }


        private async void Btn_AddGenre(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string genreName = inp_Genre.Text;

            //Logic for adding genre via HttpPost
            if (genreName.Length < 1)
            {
                return;
            }

            Genre genre = new Genre() { GenreName = genreName };

            await GenreCalls.PostGenre(genre);


        }

        private async void Btn_AddPerson(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string firstName = inp_PersonFirstName.Text;
            string lastName = inp_PersonLastName.Text;
            DateTime birthDate = inp_PersonBirthDate.Date.DateTime;
            string pictureLink = inp_PersonPicture.Text;



            //Logic for adding person via HttpPost
            //Creating Person object
            Person newPerson = new Person()
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                Picture = pictureLink
            };

            await PersonCalls.postPerson(newPerson);
            
        }

        private async void Btn_AddMovie(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Resetting the output
            output_Movie.Text = "";

            //Checks if every step in the addmovie method runs correctly
            bool addedSuccessfully = true;

            //Checks if the suggestionboxes does not contain values
            if (starId == 0 || directorId == 0 || writerId == 0 || genreId == 0)
            {
                output_Movie.Visibility = Visibility.Visible;
                output_Movie.Text += "Set values in all suggestion boxes\r\n";

                //The method was not successful
                addedSuccessfully = false;
               
            }
            //Getting information from user input
            string movieTitle = inp_MovieTitle.Text;
            string movieImageLink = inp_MovieImageLink.Text;
           // int movieGentreId = Convert.ToInt32(inp_MovieGenreId.Text);
            string movieSummary = inp_MovieSummary.Text;

            try
            {
                if (inp_MovieRating.Value == 0)
                {
                    output_Movie.Text += "Set a Movie Rating\r\n";
                    addedSuccessfully = false;
                }

                double movieRating = inp_MovieRating.Value;
            }
            catch
            {
                output_Movie.Visibility = Visibility.Visible;
                output_Movie.Text += "Movie Rating must be an integer between 0 and 10\r\n"; 
            }

            //If the above code was not run successfully dont upload data
            if (!addedSuccessfully)
            {
                return;
            }

            if(movieImageLink.Length < 3)
            {
                movieImageLink = "https://pdfimages.wondershare.com/forms-templates/medium/movie-poster-template-3.png";
            }

            //Create object for post request
            try
            {
                DataAccess.Models.Movie newMovie = new DataAccess.Models.Movie()
                {
                    Star = starId,
                    WriterId = writerId,
                    DirectorId = directorId,
                    GenreId = genreId,
                    Title = movieTitle,
                    CoverImage = movieImageLink,
                    Summary = movieSummary,
                    Rating = Convert.ToByte(inp_MovieRating.Value)

                };

                //Upload object
                output_Movie.Text = await MovieCalls.PostMovieAsync(newMovie);
            }

            //If object creation fails
            catch
            {
                output_Movie.Text += "Could not create movie object\r\n";
            }
      


        }
        

        private async void AutoSuggestBox_Genre_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when a user typed,
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                //sender.ItemsSource = dataset;
                suggestions.Clear();
                string currentSearch = sender.Text;

                //HttpPost
                string jsonInput = JsonConvert.SerializeObject(currentSearch);

                var client = new HttpClient();
                try
                {
                    string uri = "http://localhost:59713/api/Genres/search";
                    var httpResponse = await client.PostAsync(uri, new StringContent(jsonInput, Encoding.UTF8, "application/json"));

                    if (httpResponse.Content != null)
                    {
                        var json = await httpResponse.Content.ReadAsStringAsync();
                        //await new MessageDialog(json, "Response").ShowAsync();

                        //Clearing all suggestions
                        suggestions.Clear();

                        //Itterating through the response dyamically so that it will fetch the item parameters at runtime
                        dynamic dynJson = JsonConvert.DeserializeObject(json);
                        foreach (var item in dynJson)
                        { 
                            suggestions.Add(""+ item.genreId + ": "+ item.genreName);
                        }

                        if (!suggestions.Any())
                        {
                            suggestions.Add("No Results");
                        }
                            

                        sender.ItemsSource = suggestions;
                    }
                }
                catch
                {
                    await new MessageDialog("Failed to add person to database", "Response").ShowAsync();
                }

            }

        }

        private async void AutoSuggestBox_Genre_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
                //Get the object ID
                string searchId = ColonSeperatorAsync(args.ChosenSuggestion.ToString()).ToString();
                genreId = Convert.ToInt32(searchId);
            }
            else
            {
                // Use args.QueryText to determine what to do.
                sender.Text = sender.Text;
            }

        }

        private void MovieRatingChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            txt_rating.Text = Convert.ToByte(inp_MovieRating.Value).ToString();
        }

        private void Inp_MovieImageLink_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Automatically shows a preview of the cover link
            try
            {
                //check if inp_MovieImageLink.Text ends with jpg or other supportet types
                output_Movie.Visibility = Visibility.Collapsed;
                image_Cover.UriSource = new System.Uri(inp_MovieImageLink.Text);
            }

            //Catches Uriformatting error
            catch(System.UriFormatException uriFormat)
            {
                //Make error textblock visible
                output_Movie.Visibility = Visibility.Visible;

                //Add error to output box
                output_Movie.Text = "Movie poster link does not work due to an "+ uriFormat.Message +"\r\n";
            }
            
        }

        private void Inp_PersonPicture_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Automatically shows a preview of the cover link
            try
            {
                //check if inp_MovieImageLink.Text ends with jpg or other supportet types
                output_Movie.Visibility = Visibility.Collapsed;
                image_Cover.UriSource = new System.Uri(inp_PersonPicture.Text);
            }

            //Catches Uriformatting error
            catch (System.UriFormatException uriFormat)
            {
                //Make error textblock visible
                output_Person.Visibility = Visibility.Visible;

                //Add error to output box
                output_Person.Text = "Person picture link does not work due to an " + uriFormat.Message + "\r\n";
            }
        }
    }
}
