using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MovieHunter.DataAccess.Client.ApiCalls;
using MovieHunter.DataAccess.Client.Models;
using MovieHunter.ViewModels;
using Newtonsoft.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MovieHunter.Views
{
    public sealed partial class ManageListsPage : Page
    {
        private ManageListsViewModel ViewModel
        {
            get { return ViewModelLocator.Current.ManageListsViewModel; }
        }

        public class AllLists
        {
            public int UserId { get; set; }
            public int ListId { get; set; }
            public string ListName { get; set; }
        }

        private ObservableCollection<AllList> _listItems = new ObservableCollection<AllList>();

        //Collection for listItems in the listview
        private ObservableCollection<AllList> ListItems
        {
            get { return this._listItems; }
            set { this._listItems = value; }
        }


        /// <summary>Initializes a new instance of the <see cref="ManageListsPage"/> class.</summary>
        public ManageListsPage()
        {
            InitializeComponent();

            //Default listitem
            ListItems.Add(new AllList() { ListName = "Loading"});

            //Loading user owned lists
            GetListsAsync();


        }




        /// <summary>Gets the lists asynchronous. Creates listitems from user owned lists in the database</summary>
        private async void GetListsAsync()
        {
            //The users current login token
            string token = LoginPage.token;
            
                        //Adding object to the list
                           //If database api request fails delete listview content.
            ListItems.Clear();
            ObservableCollection<AllList> returnedCollection = await ListCalls.GetTokenOwnerLists(token);

            foreach( AllList a in returnedCollection)
            {
                ListItems.Add(
                    new AllList()
                    {
                        ListId = a.ListId,
                        ListName = a.ListName,
                        UserId = a.UserId
                    }
                    );
            }
            
            
        }

        /// <summary>
        /// Handles the ListView event of the SelectedItem control.
        /// Navigates to the movie that was clicked on.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private async void SelectedItem_ListView(object sender, ItemClickEventArgs e)
        {
            //Getting the clicked item
            AllList clickedItem = e.ClickedItem as AllList;

            try
            {
                var param = clickedItem.ListId as int?;

                //Navigate to the selected list
                Frame.Navigate(typeof(MovieListPage), param);


            } 
            catch
            {
                await new MessageDialog(clickedItem.ListName, "Could not open list").ShowAsync(); //Display message
            }
        }
    }
}
