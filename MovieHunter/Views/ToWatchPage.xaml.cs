using System;
using System.Collections.ObjectModel;
using MovieHunter.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MovieHunter.Views
{
    public sealed partial class ToWatchPage : Page
    {
        private ObservableCollection<MainPage> movies;

        private ToWatchViewModel ViewModel
        {
            get { return ViewModelLocator.Current.ToWatchViewModel; }
        }

        public ToWatchPage()
        {
            InitializeComponent();

            string coverurl = "https://upload.wikimedia.org/wikipedia/en/thumb/6/67/Forrest_Gump_poster.jpg/220px-Forrest_Gump_poster.jpg";

            movies = new ObservableCollection<MainPage>()
            {
                new MainPage(){CoverTitle = "Forrest Gump", CoverUri = coverurl},
                new MainPage(){CoverTitle = "Pirates of the", CoverUri = coverurl},


                new MainPage("This one", "Drama", "1", "2", "3", coverurl),


                new MainPage(){CoverTitle = "Pirates of the", CoverUri = coverurl},
                new MainPage(){CoverTitle = "Forrest Gump", CoverUri = coverurl},
                new MainPage(){CoverTitle = "Pirates of the", CoverUri = coverurl},
                new MainPage(){CoverTitle = "Forrest Gump", CoverUri = coverurl},
                new MainPage(){CoverTitle = "Pirates of the", CoverUri = coverurl},
                new MainPage(){CoverTitle = "Forrest Gump", CoverUri = coverurl},
                new MainPage(){CoverTitle = "Pirates of the", CoverUri = coverurl},
            };

            list.ItemsSource = movies;
        }

        private async void openMovieAsync(object sender, ItemClickEventArgs e)
        {
            MainPage tappeditem = e.ClickedItem as MainPage;

            if (e.ClickedItem != null)
            {
                //saving a copy of the selected list object:
                tappeditem = e.ClickedItem as MainPage;

                // Window.Current.Content = new MainPage(tappeditem.CoverTitle, "Category", "string Director", "string Stars", tappeditem.Summary, tappeditem.CoverUri);
                var parameters = tappeditem;
                Frame.Navigate(typeof(MainPage), parameters);

                ContentDialog noWifiDialog = new ContentDialog
                {
                    Title = tappeditem.CoverTitle,
                    Content = tappeditem.Summary,
                    CloseButtonText = "Ok"
                };

                ContentDialogResult result = await noWifiDialog.ShowAsync();
            }

            
            //
            
        }

    }
}
