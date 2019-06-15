using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MovieHunter.DataAccess.Client.ApiCalls;
using MovieHunter.DataAccess.Client.Models;
using MovieHunter.DataAccess.Models;
using MovieHunter.ViewModels;
using Newtonsoft.Json;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using static MovieHunter.Views.ManageListsPage;

namespace MovieHunter.Views
{
    public sealed partial class MovieListPage : Page
    {
        private MovieListViewModel ViewModel
        {
            get { return ViewModelLocator.Current.MovieListViewModel; }
        }

        /// <summary>Initializes a new instance of the <see cref="MovieListPage"/> class.
        /// Calling method that fills up the list
        /// </summary>
        public MovieListPage()
        {
            InitializeComponent();

            ListItems.Add(new AllListItems() { ListMessage = "Loading..." });

            //Using loaded so that the OnNavigationTo has time to finish before running the getListAsync
            Loaded += (sender, args) => GetListsAsync();
        }



        private ObservableCollection<AllListItems> _listItems = new ObservableCollection<AllListItems>();

        //Collection for listItems in the listview
        private ObservableCollection<AllListItems> ListItems
        {
            get { return this._listItems; }
        }

        static public int? ListId {
            get;
            set;
        }


        /// <summary>Gets the lists asynchronous.
        /// Getting listItems for the chosen list
        /// </summary>
        private async void GetListsAsync()
        {
            //Adding object to the list
            //If database api request fails delete listview content.
            ListItems.Clear();


            //Loading Output
            ListItems.Add(new AllListItems()
            {
                ListMessage = "Loading listItems"
            });

            // If the listId is not set
            if (ListId == null)
            {
                //The listId doesnt exist, and the user cant retrieve from database
                ListItems.Add(new AllListItems() {
                    ListMessage = "The list was not found"
                });
                return;
            }

            //Get Request to the server asking for listitems in specific list
            ObservableCollection<AllListItems> returnedCollection = await ListItemCalls.GetListItems(Convert.ToInt32(ListId));

            //There are no list items
            if(returnedCollection.Count == 0)
            {
                //Add message
                ListItems.Clear();
                ListItems.Add(new AllListItems() { ListMessage = "No listitems was retrieved." });
                return;
            }

            //Clear list
            ListItems.Clear();

            //Getting the movie list for finding Movie name
            ObservableCollection<Movie> allMovies = await MovieCalls.GetMovies();

            //Looking through the list of items and adding it to the UI List
            foreach (AllListItems a in returnedCollection)
            {
                ListItems.Add(
                    new AllListItems()
                    {
                        ListId = a.ListId,
                        ListItemId = a.ListItemId,
                        MovieId = a.MovieId,

                        //Looking through the movie list for the title of the movie
                        MovieName = MovieCalls.GetMovieNameFromList(allMovies, a.MovieId)
                    }
                    );
            }           
        }

        //Get the id of the list that will be displayed.
        //On navigated override cant be defined here.. Look at MovieListPage.xaml.cs


        /// <summary>Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// Saves the parameter id that came as a parameter. 
        /// </summary>
        /// <param name="e">
        /// Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // passing the int as nullable.
            //This is because if the navigation parameter fails it will be set to null
            var parameters = e.Parameter as Nullable<int>;


            if (parameters != null)
            {
                try
                {
                    //Setting the bindings to be equal to the parameters
                    ListId = parameters;
                    
                }
                catch
                {
                    //The correct listId was not recieved
                    return;
                }
            }
        }
    }
}
