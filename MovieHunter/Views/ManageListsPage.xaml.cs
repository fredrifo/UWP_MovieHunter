using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
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

        private ObservableCollection<AllLists> _listItems = new ObservableCollection<AllLists>();

        //Collection for listItems in the listview
        private ObservableCollection<AllLists> ListItems
        {
            get { return this._listItems; }
        }


        public ManageListsPage()
        {
            InitializeComponent();

            //Default listitem
            ListItems.Add(new AllLists() { listName = "Loading"});

            getListsAsync();


        }

        


        private async void getListsAsync()
        {
            //The users current login token
            string token = LoginPage.token;
            string jsonInput = JsonConvert.SerializeObject(token);

            var client = new HttpClient();
            try
            {
                string uri = "http://localhost:59713/api/Lists/userLists";
                var httpResponse = await client.PostAsync(uri, new StringContent(jsonInput, Encoding.UTF8, "application/json"));

                if (httpResponse.Content != null)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();

                    //await new MessageDialog(json, "Title of the message dialog").ShowAsync(); //Display message

                    //Deleting content in Listview
                    ListItems.Clear();

                    //Itterating through the response dyamically so that it will fetch the item parameters at runtime
                    dynamic dynJson = JsonConvert.DeserializeObject(json);
                    foreach (var item in dynJson)
                    {
                        ListItems.Add(new AllLists() { userId = item.userId, listId = item.listId, listName = item.listName });
                    }
                            
                       
                }
            }
            catch
            {
                ListItems.Clear();
                ListItems.Add(new AllLists() { listName = "Loading failed.." });
            }
        }

        private async void SelectedItem_ListView(object sender, ItemClickEventArgs e)
        {
            AllLists clickedItem = e.ClickedItem as AllLists;

            await new MessageDialog(clickedItem.listName, "Title of the message dialog").ShowAsync(); //Display message
        }
    }
}
