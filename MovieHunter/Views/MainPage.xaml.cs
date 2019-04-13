using System;

using MovieHunter.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MovieHunter.Views
{
    public sealed partial class MainPage : Page
    {
        public string ImageSource {
            get;
            set;
        }
        public string CoverTitle
        {
            get;
            set;
        }
        public string Category
        {
            get;
            set;
        }
        public string Director
        {
            get;
            set;
        }
        public string Writer
        {
            get;
            set;
        }
        public string Stars
        {
            get;
            set;
        }
        public string Summary
        {
            get;
            set;
        }
        public string CoverUri
        {
            get;
            set;
        }



        private MainViewModel ViewModel
        {
            get { return ViewModelLocator.Current.MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
            string coverurl = "https://upload.wikimedia.org/wikipedia/en/thumb/6/67/Forrest_Gump_poster.jpg/220px-Forrest_Gump_poster.jpg";

            //If page opens with no movie params
            this.CoverTitle = "CoverTitle";
            this.Category = "Category";
            this.Director = "Director";
            this.Writer = "Writer";
            this.Stars = "Stars";
            this.Summary = "Summary: The presidencies of Kennedy and Johnson, the events of Vietnam, Watergate, and other history unfold through the perspective of an Alabama man with an IQ of 75.";
            this.CoverUri = coverurl;

            MainPage movie = new MainPage("Forrest Gump", "Drama", null, null, null, coverurl);
        }

        public MainPage(string CoverTitle,  string Category, string Director, string Stars, string Summary, string CoverUri)
        {
            InitializeComponent();

            this.CoverTitle = CoverTitle;
            this.Category = Category;
            this.Director = Director;
            this.Stars = Stars;
            this.Summary = Summary;
            this.CoverUri = CoverUri;
        }
    }
}
