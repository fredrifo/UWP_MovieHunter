using System;

using MovieHunter.ViewModels;

using Windows.UI.Xaml.Controls;

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
        }
    }
}
