using System;

using Microsoft.Toolkit.Uwp.UI.Animations;

using MovieHunter.Services;
using MovieHunter.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MovieHunter.Views
{
    public sealed partial class ToWatchDetailPage : Page
    {
        string ImageSource;
        private ToWatchDetailViewModel ViewModel
        {
            get { return ViewModelLocator.Current.ToWatchDetailViewModel; }
        }

        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

        public ToWatchDetailPage()
        {
            InitializeComponent();
            ImageSource = "https://upload.wikimedia.org/wikipedia/en/thumb/6/67/Forrest_Gump_poster.jpg/220px-Forrest_Gump_poster.jpg";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is long orderId)
            {
                ViewModel.Initialize(orderId);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }
}
