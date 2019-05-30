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

        public MovieListPage()
        {
            InitializeComponent();

            ListItems.Add(new AllListItems() { ListMessage = "Loading..." });

            //Using loaded so that the OnNavigationTo has time to finish before running the getListAsync
            Loaded += (sender, args) => getListsAsync();
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


        private async void getListsAsync()
        {
            //Adding object to the list
            //If database api request fails delete listview content.
            ListItems.Clear();

            ListItems.Add(new AllListItems()
            {
                ListMessage = "Loading listItems"
            });

            //Small delay so that 
            //await Task.Delay(50);
            if (ListId == null)
            {
                //The listId doesnt exist, and the user cant retrieve from database
                ListItems.Add(new AllListItems() {
                    ListMessage = "The list was not found"
                });
                return;
            }
            ObservableCollection<AllListItems> returnedCollection = await ListItemCalls.getListItems(Convert.ToInt32(ListId));

            //If the list is empty
            if(returnedCollection.Count == 0)
            {
                //Add message
                ListItems.Clear();
                ListItems.Add(new AllListItems() { ListMessage = "No listitems was retrieved." });
                return;
            }
            ListItems.Clear();

            //Getting the movie list for finding Movie name
            ObservableCollection<Movie> allMovies = await MovieCalls.GetMovies();

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

                }
            }
        }
    }
}
