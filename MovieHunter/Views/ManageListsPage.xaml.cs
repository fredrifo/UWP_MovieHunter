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
            public int userId { get; set; }
            public int listId { get; set; }
            public string listName { get; set; }
        }

        private ObservableCollection<AllList> _listItems = new ObservableCollection<AllList>();

        //Collection for listItems in the listview
        private ObservableCollection<AllList> ListItems
        {
            get { return this._listItems; }
            set { this._listItems = value; }
        }


        public ManageListsPage()
        {
            InitializeComponent();

            //Default listitem
            ListItems.Add(new AllList() { listName = "Loading"});

            getListsAsync();


        }

        


        private async void getListsAsync()
        {
            //The users current login token
            string token = LoginPage.token;
            
                        //Adding object to the list
                           //If database api request fails delete listview content.
            ListItems.Clear();
            ObservableCollection<AllList> returnedCollection = await ListCalls.getTokenOwnerLists(token);

            foreach( AllList a in returnedCollection)
            {
                ListItems.Add(
                    new AllList()
                    {
                        listId = a.listId,
                        listName = a.listName,
                        userId = a.userId
                    }
                    );
            }
            
            
        }

        private async void SelectedItem_ListView(object sender, ItemClickEventArgs e)
        {
            AllList clickedItem = e.ClickedItem as AllList;

            //await new MessageDialog(clickedItem.listId.ToString(), "Title of the message dialog").ShowAsync(); //Display message




            try
            {
                var param = clickedItem.listId as int?;
                //Task delay to fix a bug that causes the OnNavigateTo function to not recieve
                //Found the answer from a similar issue at https://stackoverflow.com/questions/23995504/listview-containerfromitem-returns-null-after-a-new-item-is-added-in-windows-8-1
                //Another sollution is to use a viewmodel
                await Task.Delay(50);

                
                Frame.Navigate(typeof(MovieListPage), param);


            } 
            catch
            {
                await new MessageDialog(clickedItem.listName, "Could not open list").ShowAsync(); //Display message
            }
        }
    }
}
