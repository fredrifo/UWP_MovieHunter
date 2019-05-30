using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MovieHunter.DataAccess.Client.ApiCalls;
using MovieHunter.DataAccess.Client.Models;
using MovieHunter.DataAccess.Models;
using MovieHunter.ViewModels;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MovieHunter.Views
{
    public sealed partial class ToWatchPage : Page
    {

        private ObservableCollection<Movie> allMovies;


        private ToWatchViewModel ViewModel
        {
            get { return ViewModelLocator.Current.ToWatchViewModel; }
        }

        public ToWatchPage()
        {
            InitializeComponent();



            //Generate Lists for Categories
            GetAllLists();
        }

        public async void GetAllLists()
        {
            //Getting the user owned lists
            ObservableCollection<AllList> userLists = await ListCalls.getTokenOwnerLists(LoginPage.token);

            //Creating objects that contains the listId's
            AllList toWatchList = new AllList();
            AllList watchedList = new AllList();

            //Adding the listId's to the appropiat list object
            foreach (AllList n in userLists)
            {
                //Adding to ToWatch List object
                if (n.listName == "ToWatch")
                {
                    toWatchList.listId = n.listId;
                    toWatchList.listName = n.listName;
                }
                //Adding to watched List object
                if (n.listName == "Watched")
                {
                    watchedList.listId = n.listId;
                    watchedList.listName = n.listName;
                }
            }

            //Getting all the listItems from the ToWatch List
            ObservableCollection<AllListItems> allTowatchListObjects = await ListItemCalls.getListItems(toWatchList.listId);

            //Getting all the listItems from the ToWatch List
            ObservableCollection<AllListItems> allWatchedListObjects = await ListItemCalls.getListItems(watchedList.listId);

            //Getting All of the Movies
            allMovies = await MovieCalls.GetMovies();

            //Creating lists for storing allMovie object for the toWatch and watched list
            ObservableCollection<Movie> allToWatchMovies = new ObservableCollection<Movie>();
            ObservableCollection<Movie> allWatchedMovies = new ObservableCollection<Movie>();

            //Comparing the All ListObjects movie id with tall movies (Intensive task that can be optimised)
            foreach (Movie m in allMovies)
            {
                //ToWatch
                foreach (AllListItems toWatchI in allTowatchListObjects)
                {
                    //If the movie exists in the towatchlist add the Movie object to the new list
                    //This is so that the Movie object that contains all of the information can be displayed in the UI
                    if(m.MovieId == toWatchI.MovieId)
                    {
                        //Adding the movie to the new list
                        allToWatchMovies.Add(m);
                    }
                }
                //Watched
                foreach (AllListItems watched in allWatchedListObjects)
                {
                    //See comments on the foreach loop above
                    if(m.MovieId == watched.MovieId)
                    {
                        allWatchedMovies.Add(m);
                    }
                }
            }

            //Creating reference to the stackPanel that should contain the list
            StackPanel stack_toWatch = Stack_listViews;
            StackPanel stack_Watched = Stack_Watched;

            //Creating the stack lists in the ToWatch Tab
            DynamicListViewCreator("All Genres", allToWatchMovies, stack_toWatch);

            //Creating the stack lists in the Watched Tab
            DynamicListViewCreator("All Genres", allWatchedMovies, Stack_Watched);




            //The new lists can now be used to create seperate lists for every genreId that exists in the Movie collection lists

            //Firstly we need all of the Genre objects that excist.
            //Then we can compare all existing genreId's with the ones that exist in the Watched and toWatch lists.
            ObservableCollection<Genre> allGenres = await GenreCalls.GetGenres();

            //Looping through the allGenres list
            foreach (Genre existingGenres in allGenres)
            {
                //Creates a temporary list to store the movie that contain the current genre
                ObservableCollection<Movie> genreListWatched = new ObservableCollection<Movie>();
                foreach (Movie allWatched in allWatchedMovies)
                {
                    //Finding out that if the allWatched contains the genre
                    if (existingGenres.GenreId == allWatched.GenreId)
                    {
                        //Adding movie with same genre to the list
                        genreListWatched.Add(allWatched);
                       
                    }
                }
                //If any movies got added create a new List in the UI
                if (genreListWatched.Count > 0)
                {
                    DynamicListViewCreator(existingGenres.GenreName, genreListWatched, Stack_Watched);
                }

                //Creates a temporary list to store the movie that contain the current genre
                ObservableCollection<Movie> genreListToWatch = new ObservableCollection<Movie>();
                foreach (Movie allToWatch in allToWatchMovies)
                {
                    //Finding out that if the allToWatch contains the genre
                    if (existingGenres.GenreId == allToWatch.GenreId)
                    {
                        //Adding movie with same genre to the list
                        genreListToWatch.Add(allToWatch);
                    }
                }
                //If any movies got added create a new List in the UI
                if (genreListToWatch.Count > 0)
                {
                    DynamicListViewCreator(existingGenres.GenreName, genreListToWatch, stack_toWatch);
                }
                //End of Genre Loop
            }
        }

        //programmatically clickevent for listview
        private void DynamicListViewCreator(string genre, ObservableCollection<Movie> movieCollection, StackPanel stackPanel)
        {
            //listview programmatically

            //If list is empty return
            if (movieCollection == null)
            {
                return;
            }
            //Genre title
            TextBlock Genre = new TextBlock();
            Genre.Text = genre;
            Genre.FontSize = 24;
            Genre.Margin = new Thickness(20, 30, 0, 0);
            stackPanel.Children.Add(Genre);


            //ListView

            string[] test = { "test", "test2", "test3", "test4" };
            ListView listview = new ListView();
            listview.ItemsSource = movieCollection;
            //listview.SelectionChanged += listViewEvent;
            stackPanel.Children.Add(listview);

            //Setting listView Properties

            //Making the listView horizontal
            listview.SetValue(ScrollViewer.HorizontalScrollModeProperty, ScrollMode.Enabled);
            listview.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Visible);
            listview.SetValue(ScrollViewer.IsHorizontalRailEnabledProperty, false);
            listview.SetValue(ScrollViewer.VerticalScrollModeProperty, ScrollMode.Disabled);
            listview.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);

            //Enabling ItemClick
            listview.IsItemClickEnabled = true;
            listview.ItemClick += openMovieAsync;


            //Set dataTemplate
            string sXAML = @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">

                        
                        <Grid Background=""White"">

                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width=""auto""/>
                            <ColumnDefinition Width = ""200""/>
 
                         </Grid.ColumnDefinitions>
 
                         <Grid.RowDefinitions>
 
                             <RowDefinition Height = ""60""/>
  
                              <RowDefinition Height = ""20"" />
   
                               <RowDefinition Height = ""160"" />
    

                            </Grid.RowDefinitions >
    

                            <Image Grid.Column = ""0""
                                   Grid.Row = ""0""
                                   Grid.RowSpan = ""3"" >
                            <Image.Source >
                                <BitmapImage UriSource = ""{Binding CoverImage}"" />
 
                             </Image.Source >
 
                         </Image >
 

                         <TextBlock Grid.Column = ""1""
                                       Grid.Row = ""0""
                                       FontSize = ""22""
                                       Padding = ""6,0,0,0""
                                       TextWrapping = ""WrapWholeWords""
                                       Text = ""{Binding Title}""
                                       Foreground=""Black""/>  
                        <StackPanel Orientation=""Horizontal""
                                       Grid.Column = ""1""
                                       Grid.Row = ""1""
                                       Padding = ""15,0,0,0"">
                                <TextBlock 
                                       Text = ""Rating:""
                                       Foreground=""Black""
                                       Padding = ""5,0,0,0""/>

                                <TextBlock 
                                       Text = ""{Binding Rating}""
                                       Foreground=""Black""/>
                        </StackPanel>
                        

                        <TextBlock Grid.Column = ""1""
                                       Grid.Row = ""2""
                                       Padding = ""6,0,0,0""
                                       TextWrapping = ""WrapWholeWords""
                                       Text = ""{Binding Summary}""
                                       Foreground=""Black""/>

                    </Grid >

</DataTemplate>";
            var itemTemplate = Windows.UI.Xaml.Markup.XamlReader.Load(sXAML) as DataTemplate;


            listview.ItemTemplate = itemTemplate;

            string xxaml = @"

                        <ItemsPanelTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">

                                <VirtualizingStackPanel Orientation=""Horizontal""
                                                Background = ""Transparent"">
                                </VirtualizingStackPanel >
                        </ItemsPanelTemplate>

                        ";

            var itemsPanelTemplate = Windows.UI.Xaml.Markup.XamlReader.Load(xxaml) as ItemsPanelTemplate;

            listview.ItemsPanel = itemsPanelTemplate;
        }


        private async void listViewEvent(object sender, SelectionChangedEventArgs e)
        {
            ListView l1 = sender as ListView;
            string selected = l1.SelectedItem.ToString();
            MessageDialog dialog = new MessageDialog("Selected : " + selected);
            await dialog.ShowAsync();
        }

        private async void openMovieAsync(object sender, ItemClickEventArgs e)
        {
            Movie tappeditem = e.ClickedItem as Movie;
            

            if (e.ClickedItem != null)
            {
                //saving a copy of the selected list object:
                tappeditem = e.ClickedItem as Movie;



                var parameters = tappeditem;
                

               
                if (parameters.Equals(tappeditem))
                {
                    //Task delay to fix a bug that causes the OnNavigateTo function to not recieve
                    //Found the answer from a similar issue at https://stackoverflow.com/questions/23995504/listview-containerfromitem-returns-null-after-a-new-item-is-added-in-windows-8-1
                    //Another sollution is to use a viewmodel
                    await Task.Delay(50);
                    Frame.Navigate(typeof(MoviePage), parameters);
                    return;
                }
                
                



                /** 
                 *  DisplayAlert for the clicked item
                 **/
                //ContentDialog notification = new ContentDialog
                //{
                //    Title = tappeditem.CoverTitle,
                //    Content = tappeditem.Summary,
                //    CloseButtonText = "Ok"
                //};
                ////ContentDialogResult result = await notification.ShowAsync();
                

            }
        }
    }
}
