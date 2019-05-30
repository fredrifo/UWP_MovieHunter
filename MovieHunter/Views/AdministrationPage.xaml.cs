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

        /// <summary>Initializes a new instance of the <see cref="AdministrationPage"/> class.</summary>
        public AdministrationPage()
        {
            suggestions = new ObservableCollection<string>();
            InitializeComponent();
        }

        private ObservableCollection<String> suggestions;

        /// <summary>
        /// When the Person textBoxses are changed (Director, Writer, Actor Star)
        /// it will post a httpPostRequest to the api.
        /// This will check for any FirstNames matching with the rows in the Person Table.
        /// The api returns a list with all matching Persons. This list is presented in an autoSuggestionBox
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxTextChangedEventArgs"/> instance containing the event data.</param>
        /// <returns>Void. However it writes to the suggestionbox sender</returns>
        private async void AutoSuggestBox_Person_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing,
            // otherwise assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Clearing all suggestions
                suggestions.Clear();

                //Does not add suggestions if user hasn't typed anything.
                if (sender.Text.Length < 1)
                {
                    return;
                }

                //Fetching the text input
                string currentSearch = sender.Text;

                //Making ready for HttpPost
                string jsonInput = JsonConvert.SerializeObject(currentSearch);

                var client = new HttpClient();
                try
                {
                    //Defining the api link
                    string uri = "http://localhost:59713/api/People/search";

                    //sending postRequest
                    var httpResponse = await client.PostAsync(uri, new StringContent(jsonInput, Encoding.UTF8, "application/json"));

                    //Checking the post response
                    if (httpResponse.Content != null)
                    {
                        //Reading as string
                        var json = await httpResponse.Content.ReadAsStringAsync();

                        //Clearing all suggestions
                        suggestions.Clear();

                        //Itterating through the response dyamically so that it will fetch the item parameters at runtime
                        dynamic dynJson = JsonConvert.DeserializeObject(json);
                        foreach (var item in dynJson)
                        {
                            suggestions.Add("" + item.personId + ": " + item.firstName + " " + item.lastName); 
                        }

                        //If there are no items in the response
                        if (!suggestions.Any())
                        {
                            suggestions.Add("No Results");
                        }

                        //Adding the new suggestion to the autosuggestionbox
                        sender.ItemsSource = suggestions;
                    }
                }
                //If something goes wrong while fetching the Search items
                catch
                {
                    await new MessageDialog("Could not fetch", "Response").ShowAsync();
                }

                
            }
        }

        /// <summary>
        /// When the user selected an item from the Person suggest box.  Query is submitted.
        /// Get the id and save it as an INT
        ///This value will be used for creating a post request when Add movie button is clicked
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxQuerySubmittedEventArgs"/> instance containing the event data.</param>
        private void AutoSuggestBox_Person_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // Changing the chosen suggestion text to the clicked one
                sender.Text = args.ChosenSuggestion.ToString();

                //Getting searchId by getting the string before the Colon
                string searchId = ColonSeperatorAsync(args.ChosenSuggestion.ToString()).ToString();

                //If searcHId returns false it does not contain an Id.
                if (searchId == false.ToString())
                {
                    //This happens when No Result is chosen
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

        /// <summary>Getting the string text before a colon: ": ".</summary>
        /// <param name="search">The search.</param>
        /// <returns>Returns the Number before the colon</returns>
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

                //Returning the searchId
                return searchId;
            }
            else
            {
                //if there is no colon, there was no results
                return false.ToString();
            }
        }


        /// <summary>When the "Add Genre" button is clicked Post the new genre to the database</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private async void Btn_AddGenre(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Getting the genreName
            string genreName = inp_Genre.Text;

            //If genreName is empty Do not post
            if (genreName.Length < 1)
            {
                return;
            }

            //Creating a genre object
            Genre genre = new Genre() { GenreName = genreName };

            //Logic for adding genre object via HttpPost
            await GenreCalls.PostGenre(genre);


        }

        /// <summary>
        /// Handles the AddPerson event of the Btn control.
        ///Creates a Person Object and posts it to the database
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private async void Btn_AddPerson(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Fetching user input
            string firstName = inp_PersonFirstName.Text;
            string lastName = inp_PersonLastName.Text;
            DateTime birthDate = inp_PersonBirthDate.Date.DateTime;
            string pictureLink = inp_PersonPicture.Text;

            //TODO: add logic for checking if the user input is following criterias


            
            //Creating Person object
            Person newPerson = new Person()
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                Picture = pictureLink
            };

            //Logic for adding person via HttpPost
            await PersonCalls.postPerson(newPerson);
            
        }

        /// <summary>
        /// Handles the AddMovie event of the Btn control.
        /// Checking User input for criterias.
        /// If criterias are not followed it will write an error code to the user
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private async void Btn_AddMovie(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Resetting the output
            output_Movie.Text = "";

            //Checks if every step in the addmovie method runs correctly
            bool addedSuccessfully = true;

            //Checks if the suggestionboxes does not contain values
            if (starId == 0 || directorId == 0 || writerId == 0 || genreId == 0)
            {
                //Makes the outPut textblock visible in the UI
                output_Movie.Visibility = Visibility.Visible;
                //Giving the user an error response
                output_Movie.Text += "Set values in all suggestion boxes\r\n";

                //The method was not successful
                addedSuccessfully = false;
               
            }
            //Getting information from user input
            string movieTitle = inp_MovieTitle.Text;
            string movieImageLink = inp_MovieImageLink.Text;
            string movieSummary = inp_MovieSummary.Text;


            try
            {
                //Dont accept movie ratings of 0. All movies deserve to be number 1..
                if (inp_MovieRating.Value == 0)
                {
                    output_Movie.Text += "Set a Movie Rating\r\n";
                    addedSuccessfully = false;
                }

                double movieRating = inp_MovieRating.Value;
            }
            //If the value is not a number. This happened when using a textblock.  Its using a slider now..
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

            //Movielinks must be more that 10 characters. I should add try catch to verify if the link is valid
            if(movieImageLink.Length < 10)
            {
                movieImageLink = "https://pdfimages.wondershare.com/forms-templates/medium/movie-poster-template-3.png";
            }

            //Create object for post request
            try
            {
                //Creating Movie object
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

                //Posting movie object
                output_Movie.Text = await MovieCalls.PostMovieAsync(newMovie);
            }

            //If object creation fails
            catch
            {
                //output UI
                output_Movie.Text += "Could not create movie object\r\n";
            }
      


        }


        /// <summary>Automatics the suggest box genre text changed.
        ///
        /// /// When the Genre suggestionBoxes are changed
        /// it will post a httpPostRequest to the api.
        /// This will check for any FirstNames matching with the rows in the Person Table.
        /// 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxTextChangedEventArgs"/> instance containing the event data.</param>
        private async void AutoSuggestBox_Genre_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when a user typed,
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Clearing suggestions
                suggestions.Clear();

                //Checking user input
                string currentSearch = sender.Text;

                //HttpPost
                //Serializing the object for HTTPPost
                string jsonInput = JsonConvert.SerializeObject(currentSearch);

                var client = new HttpClient();
                try
                {
                    string uri = "http://localhost:59713/api/Genres/search";

                    //GetAsync would be better here.
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
                            //Adding the retrieved suggestions
                            suggestions.Add(""+ item.genreId + ": "+ item.genreName);
                        }

                        if (!suggestions.Any())
                        {
                            //The request returned 0 suggestions.
                            suggestions.Add("No Results");
                        }
                            
                        //Adding new suggestions
                        sender.ItemsSource = suggestions;
                    }
                }
                catch
                {
                    await new MessageDialog("Failed to find genres", "Response").ShowAsync();
                }

            }

        }

        /// <summary>
        /// When selecting suggestion for genre
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxQuerySubmittedEventArgs"/> instance containing the event data.</param>
        private void AutoSuggestBox_Genre_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            //If a suggestion got chose
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
                //Get the object ID
                string searchId = ColonSeperatorAsync(args.ChosenSuggestion.ToString()).ToString();
                genreId = Convert.ToInt32(searchId);
            }
            else
            {
                // No changes in selection
                sender.Text = sender.Text;
            }

        }

        /// <summary>
        /// When the Movie rating slider is changed.
        /// Adding the slider value to the textblock located next to it.
        /// This value is used when add movie Button is clicked. 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs"/> instance containing the event data.</param>
        private void MovieRatingChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            txt_rating.Text = Convert.ToByte(inp_MovieRating.Value).ToString();
        }

        /// <summary>
        /// Handles the TextChanged event of the Inp_MovieImageLink control.
        /// Adding the new image to the preview Image on UI.
        /// Saving the Uri. It will be used when add button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the TextChanged event of the Inp_PersonPicture control.
        /// Saving Uri. Used when adding new Person.
        /// Adding image to preview in UI
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
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
