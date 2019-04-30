using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MovieHunter.Classes;
using MovieHunter.ViewModels;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MovieHunter.Views
{
    public sealed partial class ToWatchPage : Page
    {
        private ObservableCollection<Movie> movies;

        private ToWatchViewModel ViewModel
        {
            get { return ViewModelLocator.Current.ToWatchViewModel; }
        }

        public ToWatchPage()
        {
            InitializeComponent();

            movies = new ObservableCollection<Movie>()
            {

                new Movie(
                    //Title
                    "Forrest Gump (1994)",

                    //Genre
                    "Drama, Romance",

                    //Director
                    "Robert Zemeckis",

                    //Stars
                    "Tom Hanks, Robin Wright, Gary Sinise",

                    //Summary
                    "The presidencies of Kennedy and Johnson, the events of " +
                    "Vietnam, Watergate, and other history unfold through the " +
                    "perspective of an Alabama man with an IQ of 75.",

                    //Cover Image
                    "https://upload.wikimedia.org/wikipedia/en/thumb/6/67/Forrest_Gump_poster.jpg/220px-Forrest_Gump_poster.jpg")

                ,

                new Movie(
                    //Title
                    "Fight Club (1999)",

                    //Genre
                    "Drama",

                    //Director
                    "David Fincher",

                    //Stars
                    "Brad Pitt, Edward Norton, Meat Loaf ",

                    //Summary
                    "An insomniac office worker and a devil-may-care" +
                    " soapmaker form an underground fight club that " +
                    "evolves into something much, much more.",

                    //Cover Image
                    "https://m.media-amazon.com/images/M/MV5BMjJmYTNkNmItYjYyZC00MGUxLWJhNWMtZDY4Nzc1MDAwMzU5XkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_UX182_CR0,0,182,268_AL_.jpg")

                ,

                new Movie(
                    //Title
                    "Se7en (1995)",

                    //Genre
                    "Crime, Drama, Mystery",

                    //Director
                    "David Fincher",

                    //Stars
                    "Morgan Freeman, Brad Pitt, Kevin Spacey",

                    //Summary
                    "Two detectives, a rookie and a veteran, " +
                    "hunt a serial killer who uses the seven " +
                    "deadly sins as his motives.",

                    //Cover Image
                    "https://m.media-amazon.com/images/M/MV5BOTUwODM5MTctZjczMi00OTk4LTg3NWUtNmVhMTAzNTNjYjcyXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_UX182_CR0,0,182,268_AL_.jpg")

                ,

                new Movie(
                    //Title
                    "Saving Private Ryan (1998)",

                    //Genre
                    "Drama, War",

                    //Director
                    "Steven Spielberg",

                    //Stars
                    "Tom Hanks, Matt Damon, Tom Sizemore ",

                    //Summary
                    "Following the Normandy Landings, a group of U.S. soldiers " +
                    "go behind enemy lines to retrieve a paratrooper whose " +
                    "brothers have been killed in action.",

                    //Cover Image
                    "https://m.media-amazon.com/images/M/MV5BZjhkMDM4MWItZTVjOC00ZDRhLThmYTAtM2I5NzBmNmNlMzI1XkEyXkFqcGdeQXVyNDYyMDk5MTU@._V1_UY268_CR0,0,182,268_AL_.jpg")

                ,

                new Movie(
                    //Title
                    "The Green Mile (1999)",

                    //Genre
                    "Crime, Drama, Fantasy",

                    //Director
                    "Frank Darabont",

                    //Stars
                    "Tom Hanks, Michael Clarke Duncan, David Morse",

                    //Summary
                    "The lives of guards on Death Row are affected by " +
                    "one of their charges: a black man accused of child " +
                    "murder and rape, yet who has a mysterious gift.",

                    //Cover Image
                    "https://m.media-amazon.com/images/M/MV5BMTUxMzQyNjA5MF5BMl5BanBnXkFtZTYwOTU2NTY3._V1_UX182_CR0,0,182,268_AL_.jpg")

                ,

                new Movie(
                    //Title
                    "First Man (2018)",

                    //Genre
                    "Biography, Drama, History",

                    //Director
                    "Damien Chazelle",

                    //Stars
                    "Ryan Gosling, Claire Foy, Jason Clarke",

                    //Summary
                    "A look at the life of the astronaut, Neil Armstrong, " +
                    "and the legendary space mission that led him to become" +
                    " the first man to walk on the Moon on July 20, 1969.",

                    //Cover Image
                    "https://m.media-amazon.com/images/M/MV5BMDBhOTMxN2UtYjllYS00NWNiLWE1MzAtZjg3NmExODliMDQ0XkEyXkFqcGdeQXVyMjMxOTE0ODA@._V1_UX182_CR0,0,182,268_AL_.jpg")

                ,

                new Movie(
                    //Title
                    "Dunkirk (2017)",

                    //Genre
                    "Action, Drama, History ",

                    //Director
                    "Christopher Nolan",

                    //Stars
                    "Fionn Whitehead, Barry Keoghan, Mark Rylance",

                    //Summary
                    "Allied soldiers from Belgium, the British Empire," +
                    " and France are surrounded by the German Army, " +
                    "and evacuated during a fierce battle in World War II.",

                    //Cover Image
                    "https://m.media-amazon.com/images/M/MV5BN2YyZjQ0NTEtNzU5MS00NGZkLTg0MTEtYzJmMWY3MWRhZjM2XkEyXkFqcGdeQXVyMDA4NzMyOA@@._V1_UX182_CR0,0,182,268_AL_.jpg")

                ,

            };

            //list.ItemsSource = movies //OLD listView;


            DynamicListViewCreator("Drama");
            DynamicListViewCreator("Action");
            DynamicListViewCreator("Thriller");
            DynamicListViewCreator("Romance");
            DynamicListViewCreator("History");



        }

        //programmatically clickevent for listview

        private void DynamicListViewCreator(string genre)
        {
            //listview programmatically

            //Genre title
            TextBlock Genre = new TextBlock();
            Genre.Text = genre;
            Genre.FontSize = 24;
            Genre.Margin = new Thickness(20, 30, 0, 0);
            Stack_listViews.Children.Add(Genre);


            //ListView

            string[] test = { "test", "test2", "test3", "test4" };
            ListView listview = new ListView();
            listview.ItemsSource = movies;
            //listview.SelectionChanged += listViewEvent;
            Stack_listViews.Children.Add(listview);

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
                                <BitmapImage UriSource = ""{Binding CoverUri}"" />
 
                             </Image.Source >
 
                         </Image >
 

                         <TextBlock Grid.Column = ""1""
                                       Grid.Row = ""0""
                                       FontSize = ""22""
                                       Padding = ""6,0,0,0""
                                       TextWrapping = ""WrapWholeWords""
                                       Text = ""{Binding CoverTitle}""
                                       Foreground=""Black""/>  

                        <TextBlock Grid.Column = ""1""
                                       Grid.Row = ""1""
                                       Padding = ""24,0,0,0""
                                       Text = ""Rating 8.8/10""
                                       Foreground=""Black""/>

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
                    Frame.Navigate(typeof(MainPage), parameters);
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
