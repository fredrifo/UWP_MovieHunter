using System;

using MovieHunter.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MovieHunter.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel
        {
            get { return ViewModelLocator.Current.ShellViewModel; }
        }

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView, KeyboardAccelerators);

            //new MainPage().Frame.Navigate(typeof(MovieListPage));
            
            //((Frame)Window.Current.Content).Navigate(typeof(Views.MainPage));



        }

    }
}
