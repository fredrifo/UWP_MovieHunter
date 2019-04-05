using System;

using MovieHunter.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MovieHunter.Views
{
    public sealed partial class ToWatchPage : Page
    {
        private ToWatchViewModel ViewModel
        {
            get { return ViewModelLocator.Current.ToWatchViewModel; }
        }

        public ToWatchPage()
        {
            InitializeComponent();
        }
    }
}
