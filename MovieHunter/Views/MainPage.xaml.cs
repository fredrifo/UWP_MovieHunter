using System;
using MovieHunter.Classes;
using MovieHunter.ViewModels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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

            CreateRandomGrid();
        }
        public void CreateRandomGrid()
        {
            //Grid content = contentGrid;

            //Random random = new Random();

            //int columnAmount = 10;
            //int rowAmount = 5;


            //for (int x = 0; x < rowAmount; x++)
            //{
            //    RowDefinition rw = new RowDefinition();
            //    rw.Height = new GridLength(1, GridUnitType.Star);
            //    content.RowDefinitions.Add(rw);

            //    for (int i = 0; i < columnAmount; i++)
            //    {


            //        ColumnDefinition clm = new ColumnDefinition();
            //        clm.Width = new GridLength(1, GridUnitType.Star);

            //        content.ColumnDefinitions.Add(clm);

            //        Frame btn = new Frame();
            //        btn.Background = new SolidColorBrush(Color.FromArgb(255, 48, 179, 221));
            //        btn.Margin = new Windows.UI.Xaml.Thickness(2);
            //        btn.SetValue(Grid.RowProperty, x);
            //        btn.SetValue(Grid.ColumnProperty, i);
            //        content.Children.Add(btn);
            //    }

            //}
        }
    }
}
