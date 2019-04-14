using System;
using System.Collections.ObjectModel;
using MovieHunter.ViewModels;

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

            movies = new ObservableCollection<MainPage>()
            {
                new MainPage(){CoverTitle = "Forrest Gump"},
                new MainPage(){CoverTitle = "Pirates of the"}
            };

            list.ItemsSource = movies;
        }
    }
}
