using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using MovieHunter.DataAccess.Client.ApiCalls;
using MovieHunter.DataAccess.Client.Models;
using MovieHunter.DataAccess.Models;
using MovieHunter.ViewModels;
using Newtonsoft.Json;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MovieHunter.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return ViewModelLocator.Current.MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();

            TableItems.Add(new Movie() { Title = "Loading.." });

            //Sends an empty string so that it will load all movies
            getListsAsync("");
        }

        
        private ObservableCollection<Movie> _tableItems = new ObservableCollection<Movie>();

        //Collection for listItems in the listview
        public ObservableCollection<Movie> TableItems
        {
            get { return this._tableItems; }
        }

        private async void getListsAsync(string getExtention)
        {
            //The users current login token
            string token = LoginPage.token;
            string jsonInput = JsonConvert.SerializeObject(token);

            var client = new HttpClient();
            try
            {
                string uri = "http://localhost:59713/api/Movies";
                //adding the search parameter to the link
                uri += getExtention;
                //Get request for getting all the listItems
                var httpResponse = await client.GetAsync(uri);

                if (httpResponse.Content != null)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();

                    //await new MessageDialog(json, "Title of the message dialog").ShowAsync(); //Display message

                    //Deleting content in Listview
                    TableItems.Clear();

                    //Itterating through the response dyamically so that it will fetch the item parameters at runtime
                    dynamic dynJson = JsonConvert.DeserializeObject(json);
                    foreach (var item in dynJson)
                    {
                        Movie movieObject = new Movie()
                        {
                            MovieId = item.movieId,
                            Title = item.title,
                            CoverImage = item.coverImage,
                            GenreId = item.genreId,
                            Summary = item.summary,
                            Rating = item.rating,
                            DirectorId = item.directorId,
                            WriterId = item.writerId,
                            Genre = item.genre
                        };

                        TableItems.Add(movieObject);
                        //await new MessageDialog(movieObject.ToString(), "Title of the message dialog").ShowAsync(); //Display message
                    }
                }
            }
            catch
            {
                TableItems.Clear();
                TableItems.Add(new Movie() { Title = "Loading failed.." });
            }
        }


        private void TextBox_SearchChanged(object sender, TextChangedEventArgs e)
        {
            //Reads search input
            string search = inp_SearchBox.Text;

            //If the input has more than 1 character
            if(search.Length > 0)
            {
                //Sends a string so that it will send getrequest with search parameter
                getListsAsync("/" + search + "? searchParameter="+search);
            }

            else
            {
                //Empty extension. It will get all movies
                getListsAsync("");
            }
        }

        private async void BtnAddToDo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

        }

        private async void GridViewItemTapped(object sender, ItemClickEventArgs e)
        {
            //Delay the task so that the selected item will have time to get registered.
            await Task.Delay(100);

            //Casting the clicked item back to the orginal object type (Movie).
            Movie clickedObject = e.ClickedItem as Movie;

            //Give the user an option to open Movie page when there is only one object selected.
            //Amount of selected items
            int selectedAmount = gridViewMovies.SelectedItems.Count;


            //If 1 selected popup an option
            if (selectedAmount == 1)
            {
                var userMessage = new MessageDialog(
                    //First row
                    "Click \"Yes\" to get more information about " + clickedObject.Title.ToString()

                    //Second row
                    + ".\r\nClick \"No\" to select multiple items"

                    //Third row
                    + "\r\n\r\n PS: To manage the selected items click at Manage list in bottom right corner",

                    //Title
                    "Do you want to open the movie page");

                //Using delegate command for yes and no buttons
                userMessage.Commands.Add(new UICommand("Yes!", async delegate (IUICommand command)
                 {
                     await new MessageDialog("Writer: " + clickedObject.WriterId, "|Open move").ShowAsync();
                     //Open the MoviePage

                     //Creating parameter for tapped movie object
                     var parameters = clickedObject;

                     //Task delay to ensure that the parameters are loaded correctly
                     await Task.Delay(50);


                     //Navigate to new page, while also sending the Movie object parameters. T
                     //he parameters are handled in the override OnNavigatedTo method in MoviePage
                     Frame.Navigate(typeof(MoviePage), parameters);
                 }));

                //Delay before opening messagebox
                await Task.Delay(100);
                userMessage.Commands.Add(new UICommand("No!", delegate (IUICommand command)
                {
                    //Let the user select more items
                    return;
                }));

                ////Commenting out the popup since it was annoying to see every time i selected 1 item
                //await userMessage.ShowAsync();
            }

            //If more than one movie is selected do not ask 
            else
            {
                return;
            }            
        }

        private async void Combobox_Options_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedAmount = gridViewMovies.SelectedItems.Count;
            // Get the ComboBox instance
            ComboBox comboBox = sender as ComboBox;

            string selectedName;

            try
            {
                selectedName = comboBox.SelectedValue.ToString();
            }
            catch (System.NullReferenceException nullReffEx)
            {
                //Catches if there is selectedValue is null
                //This occurs if the combobox is reset. Since the selection is changed into an invalid value the method must be stopped
                selectedName = nullReffEx.ToString();

                //Exiting method.
                return;
            }

            //If no movies are selected return
            if (selectedAmount == 0)
            {
                await new MessageDialog(
                    "No items are selected",
                    //Failed to (Open/Edit/Delete/Add to ToWatch list/Add to Watched list)
                    "Failed to " + selectedName).ShowAsync(); //Display message
                return;
            }

            //Creates string with all selected MovieNames
            var movies = gridViewMovies.SelectedItems;

            string selectedMoviesString = "";
            foreach (var m in movies)
            {
                var movie = m as Movie;

                selectedMoviesString += movie.Title + "\r\n";
            }


            // Logic for reading the selected combobox items is written in the if tests below



            //SELECTED "Open" IN THE COMBOBOX
            if (selectedName == "Open")
            {
                //If multiple movies are selected
                if (selectedAmount > 1)
                {
                    await new MessageDialog("Open movies failed","You can only open 1 movie at the same time").ShowAsync(); //Display message

                    //Resets the combobox
                    comboBox_Options.SelectedIndex = -1;
                    //Exit method
                    return;
                }


                //Opens movie
                Movie selectedMovie = gridViewMovies.SelectedItem as Movie;

                //Creating parameter for tapped movie object
                var parameters = selectedMovie;


                //Task delay to ensure that the parameters are loaded correctly
                await Task.Delay(50);


                //Navigate to new page, while also sending the Movie object parameters. T
                //he parameters are handled in the override OnNavigatedTo method in MoviePage
                Frame.Navigate(typeof(MoviePage), selectedMovie);

                //Resets the combobox
                comboBox_Options.SelectedIndex = -1;
            }






            //SELECTED "Edit" IN THE COMBOBOX
            if (selectedName == "Edit")
            {
                //If more than 1 movie is selected
                if (selectedAmount > 1)
                {
                    await new MessageDialog("You can only edit 1 movie at the same time", "Edit movies failed").ShowAsync(); //Display message

                    //Resets the combobox
                    comboBox_Options.SelectedIndex = -1;
                    //Exit method
                    return;
                }


                //Logic for edit


            }



            //SELECTED "Delete" IN THE COMBOBOX
            if (selectedName == "Delete")
            {

                

                var userMessage = new MessageDialog(
                    //Fill body with names of all the selected movie
                    selectedMoviesString,

                    //Title
                    "Are you sure you want to delete the selected movies shown below?\r\n");

                //Logic for deleteing all of the selected items

                //To prevent users accidentally deleting movies users must accept this option in a popup

                //Using delegate command for yes and no buttons
                userMessage.Commands.Add(new UICommand("Yes!", async delegate (IUICommand command)
                {

                    //Looping through all elements that are selected
                    ObservableCollection<Movie> toRemove = new ObservableCollection<Movie>();

                    //Creates a list of all the objects that are to be removed
                    //This is because it's not possible to modify the same list that is enumerating. (without deleting an incorrect amount of objects)
                    foreach (Movie elem in movies)
                    {
                            toRemove.Add(elem); 
                    }

                    // Removing Movie objects that are stored in the toRemoveList
                    foreach (Movie remove in toRemove)

                    {
                        //Removing objects from TableItems ObservableCollection. (Bound to gridViewModels)
                        TableItems.Remove(remove);
                        await deleteMovieAsync(remove.MovieId);
                    }
                }));

                userMessage.Commands.Add(new UICommand("No!", delegate (IUICommand command)
                {
                    //Resets the combobox
                    comboBox_Options.SelectedIndex = -1;

                    //exits method
                    return;
                }));

                //Open the popup specified above
                await userMessage.ShowAsync();

                //Resets the combobox
                comboBox_Options.SelectedIndex = -1;

            }





            //SELECTED "Add to Towatch" or "Add to watched" IN THE COMBOBOX
            if (selectedName == "Add to ToWatch list" || selectedName == "Add to Watched list")
            {

                //Getting all user owned lists with a httpRequest
                ObservableCollection<AllList> userLists = await ListCalls.getTokenOwnerLists(LoginPage.token);

                //Id of the chosen userlist
                Nullable<int> chosenUserList = null;

                //Checking the id for the lists owned by the user
                foreach (AllList ali in userLists)
                {
                    if (selectedName == "Add to ToWatch list")
                    {
                        //Checking for the for ToWatch list
                        if (ali.listName == "ToWatch")
                        {
                            chosenUserList = ali.listId;
                            await new MessageDialog("", chosenUserList.ToString()).ShowAsync();
                        }
                    }

                    if (selectedName == "Add to Watched list")
                    {
                        //Checking for the id for Watched list
                        if (ali.listName == "Watched")
                        {
                            chosenUserList = ali.listId;
                            await new MessageDialog("", chosenUserList.ToString()).ShowAsync();
                        }
                    }
                }

                //Checking if userlist was defined
                if(chosenUserList == null)
                {
                    //returning if not defined
                    return;
                }

                //Posting all of the selected movies to the chosen list specified above (chosenUserList)
                foreach (Movie elem in movies)
                {
                    ListItem newListItem = new ListItem()
                    {
                        MovieId = elem.MovieId,

                        //converting nullable int to int.
                        //The convertion will succeed since I have already checked that the int is not null
                        ListId = Convert.ToInt32(chosenUserList)

                    };
                    //api/ListItems
                    string feedback = await ListItemCalls.postListItem(newListItem);

                    //
                    await new MessageDialog(feedback, "Added to list?").ShowAsync();
                }

            }

        }

        

        private async Task<bool> deleteMovieAsync(int movieId)
        {
            //If movie Id is not valid
            if(movieId == 0)
            {
                //Display message
                await new MessageDialog("", "movieId was not valid").ShowAsync();

                //Deleting failed
                return false;
            }
            //Serializing the movieId
            string jsonInput = JsonConvert.SerializeObject(movieId);

            try
            {
                var client = new HttpClient();
                string uri = "http://localhost:59713/api/Movies/";
                //adding the search parameter to the link
                uri += movieId;

                //Delete request for deleting movie
                var httpResponse = await client.DeleteAsync(uri);
            }
            catch
            {
                //Deleting failed
                return false;
            }
            //Deleted successfully
            return true;
        }
    }
}
