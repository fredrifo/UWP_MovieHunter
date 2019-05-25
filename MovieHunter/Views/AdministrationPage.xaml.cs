using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using MovieHunter.Classes;
using MovieHunter.DataAccess.Models;
using MovieHunter.ViewModels;
using Newtonsoft.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace MovieHunter.Views
{
    public sealed partial class AdministrationPage : Page
    {
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
                            suggestions.Add("" + item.firstName + " " + item.lastName); 
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

        private void AutoSuggestBox_Person_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
                sender.Text = args.ChosenSuggestion.ToString();
            }
            else
            {
                // Use args.QueryText to determine what to do.
                sender.Text = sender.Text;
            }
        }

        private void AutoSuggestBox_Person_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Set sender.Text. You can use args.SelectedItem to build your text string.
            sender.Text = "Choosen";
            sender.IsSuggestionListOpen = false;
            suggestions.Clear();
        }

        private async void Btn_AddGenre(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string genreName = inp_Genre.Text;

            //Logic for adding genre via HttpPost
            if (genreName.Length < 1)
            {
                return;
            }

            //HttpPost
            string jsonInput = JsonConvert.SerializeObject(new Genre() { GenreName = genreName});

            var client = new HttpClient();
            try
            {
                string uri = "http://localhost:59713/api/Genres/new";
                var httpResponse = await client.PostAsync(uri, new StringContent(jsonInput, Encoding.UTF8, "application/json"));

                if (httpResponse.Content != null)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    await new MessageDialog(json, "Response").ShowAsync();
                }
            }
            catch
            {
                await new MessageDialog("Failed to add genre to database", "Response").ShowAsync();
            }
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

                //HttpPost
            string jsonInput = JsonConvert.SerializeObject(newPerson);

            var client = new HttpClient();
            try
            {
                string uri = "http://localhost:59713/api/People/add";
                var httpResponse = await client.PostAsync(uri, new StringContent(jsonInput, Encoding.UTF8, "application/json"));

                if (httpResponse.Content != null)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    await new MessageDialog(json, "Response").ShowAsync();
                }
            }
            catch
            {
                await new MessageDialog("Failed to add person to database", "Response").ShowAsync();
            }
            
        }

        private void Btn_AddMovie(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string movieTitle = inp_MovieTitle.Text;
            string movieImageLink = inp_MovieImageLink.Text;
           // int movieGentreId = Convert.ToInt32(inp_MovieGenreId.Text);
            string movieSummary = inp_MovieSummary.Text;
            int movieRating = Convert.ToInt32(inp_MovieRating.Text);
           //// int movieDirectorId = Convert.ToInt32(inp_MovieDirectorId.Text);
           // int movieWriterId = Convert.ToInt32(inp_MovieWriterId.Text);
           // int movieStarId = Convert.ToInt32(inp_MovieStarActorId.Text);

            //Logic for adding Movie via HttpPost
        }

        private async void AutoSuggestBox_Genre_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing,
            // otherwise assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
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
                            suggestions.Add("" + item.genreName);
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

        private void AutoSuggestBox_Genre_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
                sender.Text = args.ChosenSuggestion.ToString();
            }
            else
            {
                // Use args.QueryText to determine what to do.
                sender.Text = sender.Text;
            }

        }

        private void AutoSuggestBox_Genre_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Set sender.Text. You can use args.SelectedItem to build your text string.
            sender.Text = args.SelectedItem.ToString();
            sender.IsSuggestionListOpen = false;
            suggestions.Clear();
        }
    }
}
