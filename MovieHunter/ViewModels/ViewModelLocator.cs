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
            Register<ToWatchedViewModel, ToWatchedPage>();
            Register<SettingsViewModel, SettingsPage>();
            Register<MovieViewModel, MoviePage>();
            Register<LoginViewModel, LoginPage>();
            Register<RegisterViewModel, RegisterPage>();
            Register<AdministrationViewModel, AdministrationPage>();
            Register<ManageListsViewModel, ManageListsPage>();
            Register<MovieListViewModel, MovieListPage>();
        }

        public MovieListViewModel MovieListViewModel => SimpleIoc.Default.GetInstance<MovieListViewModel>();

        public ManageListsViewModel ManageListsViewModel => SimpleIoc.Default.GetInstance<ManageListsViewModel>();

        public AdministrationViewModel AdministrationViewModel => SimpleIoc.Default.GetInstance<AdministrationViewModel>();


        public RegisterViewModel RegisterViewModel => SimpleIoc.Default.GetInstance<RegisterViewModel>();

        public LoginViewModel LoginViewModel => SimpleIoc.Default.GetInstance<LoginViewModel>();

        public MovieViewModel MovieViewModel => SimpleIoc.Default.GetInstance<MovieViewModel>();

        public SettingsViewModel SettingsViewModel => SimpleIoc.Default.GetInstance<SettingsViewModel>();


        public ToWatchedViewModel ToWatchedViewModel => SimpleIoc.Default.GetInstance<ToWatchedViewModel>();


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
