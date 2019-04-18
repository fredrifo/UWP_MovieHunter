using System;
using System.Collections.ObjectModel;
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

            string coverurl = "https://upload.wikimedia.org/wikipedia/en/thumb/6/67/Forrest_Gump_poster.jpg/220px-Forrest_Gump_poster.jpg";

            movies = new ObservableCollection<Movie>()
            {

                new Movie("Forrest Gump", "Drama", "1", "2", "3", coverurl),
                new Movie("Forrest Gump", "Drama", "1", "2", "3", coverurl),
                new Movie("Forrest Gump", "Drama", "1", "2", "3", coverurl),
                new Movie("Forrest Gump", "Drama", "1", "2", "3", coverurl),
                new Movie("Forrest Gump", "Drama", "1", "2", "3", coverurl),
            };

            //list.ItemsSource = movies //OLD listView;


            DynamicListViewCreator();
            DynamicListViewCreator();
            DynamicListViewCreator();



        }

        //programmatically clickevent for listview

        private void DynamicListViewCreator()
        {
            //listview programmatically

            //Genre title
            TextBlock Genre = new TextBlock();
            Genre.Text = "Specify Genre";
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
            listview.SetValue(ScrollViewer.HorizontalScrollModeProperty, ScrollMode.Enabled);
            listview.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Visible);
            listview.SetValue(ScrollViewer.IsHorizontalRailEnabledProperty, false);
            listview.SetValue(ScrollViewer.VerticalScrollModeProperty, ScrollMode.Disabled);
            listview.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);


            //Set dataTemplate
            string sXAML = @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">

                        
                        <Grid>

                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width=""auto""/>
                            <ColumnDefinition Width = ""200""/>
 
                         </Grid.ColumnDefinitions>
 
                         <Grid.RowDefinitions>
 
                             <RowDefinition Height = ""40""/>
  
                              <RowDefinition Height = ""40"" />
   
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
                                       Text = ""{Binding CoverTitle}"" />

                        <TextBlock Grid.Column = ""1""
                                       Grid.Row = ""1""
                                       Padding = ""24,0,0,0""
                                       Text = ""Rating 8.8/10"" />

                        <TextBlock Grid.Column = ""1""
                                       Grid.Row = ""2""
                                       Padding = ""6,0,0,0""
                                       TextWrapping = ""WrapWholeWords""
                                       Text = ""{Binding Summary}"" />

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

                // Window.Current.Content = new MainPage(tappeditem.CoverTitle, "Category", "string Director", "string Stars", tappeditem.Summary, tappeditem.CoverUri);
                var parameters = tappeditem;
                Frame.Navigate(typeof(MainPage), parameters);

                ContentDialog notification = new ContentDialog
                {
                    Title = tappeditem.CoverTitle,
                    Content = tappeditem.Summary,
                    CloseButtonText = "Ok"
                };

                ContentDialogResult result = await notification.ShowAsync();
            }

            
            //
            
        }

    }
}
