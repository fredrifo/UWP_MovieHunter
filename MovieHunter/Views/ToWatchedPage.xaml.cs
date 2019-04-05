using System;

using MovieHunter.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MovieHunter.Views
{
    public sealed partial class ToWatchedPage : Page
    {
        private ToWatchedViewModel ViewModel
        {
            get { return ViewModelLocator.Current.ToWatchedViewModel; }
        }

        public ToWatchedPage()
        {
            InitializeComponent();
        }
    }
}
