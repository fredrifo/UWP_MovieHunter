using System;

using GalaSoft.MvvmLight.Ioc;

using MovieHunter.Services;
using MovieHunter.Views;

namespace MovieHunter.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;

        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        private ViewModelLocator()
        {
            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            SimpleIoc.Default.Register<ShellViewModel>();
            Register<MainViewModel, MainPage>();
            Register<MediaPlayerViewModel, MediaPlayerPage>();
            Register<ToWatchViewModel, ToWatchPage>();
            Register<ToWatchDetailViewModel, ToWatchDetailPage>();
            Register<ToWatchedViewModel, ToWatchedPage>();
            Register<ToWatchedDetailViewModel, ToWatchedDetailPage>();
            Register<SettingsViewModel, SettingsPage>();
            Register<MovieViewModel, MoviePage>();
            Register<LoginViewModel, LoginPage>();
        }

        public LoginViewModel LoginViewModel => SimpleIoc.Default.GetInstance<LoginViewModel>();

        public MovieViewModel MovieViewModel => SimpleIoc.Default.GetInstance<MovieViewModel>();

        public SettingsViewModel SettingsViewModel => SimpleIoc.Default.GetInstance<SettingsViewModel>();

        public ToWatchedDetailViewModel ToWatchedDetailViewModel => SimpleIoc.Default.GetInstance<ToWatchedDetailViewModel>();

        public ToWatchedViewModel ToWatchedViewModel => SimpleIoc.Default.GetInstance<ToWatchedViewModel>();

        public ToWatchDetailViewModel ToWatchDetailViewModel => SimpleIoc.Default.GetInstance<ToWatchDetailViewModel>();

        public ToWatchViewModel ToWatchViewModel => SimpleIoc.Default.GetInstance<ToWatchViewModel>();

        // A Guid is generated as a unique key for each instance as reusing the same VM instance in multiple MediaPlayerElement instances can cause playback errors
        public MediaPlayerViewModel MediaPlayerViewModel => SimpleIoc.Default.GetInstance<MediaPlayerViewModel>(Guid.NewGuid().ToString());

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();

        public ShellViewModel ShellViewModel => SimpleIoc.Default.GetInstance<ShellViewModel>();

        public NavigationServiceEx NavigationService => SimpleIoc.Default.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
