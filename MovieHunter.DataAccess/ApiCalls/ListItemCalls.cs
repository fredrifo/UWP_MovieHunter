using MovieHunter.DataAccess.Client.Models;
using MovieHunter.DataAccess.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieHunter.DataAccess.Client.ApiCalls
{
    public static class ListItemCalls
    {

        /// <summary>Posting a list item to the database.</summary>
        /// <param name="listItem">The list item object.</param>
        /// <returns></returns>
        public static async Task<string> PostListItem(ListItem listItem)
        {
            //Serializing movie object
            string input = JsonConvert.SerializeObject(listItem);
            string output = "";

            //Posting a new item to a list
            using (var client = new HttpClient())
            {
                try
                {
                    string uri = "http://localhost:59713/api/ListItems/";

                    //Sends the post request
                    using (var httpResponse = await client.PostAsync(uri, new StringContent(input, Encoding.UTF8, "application/json")))
                    {
                        //If there is a response from the server
                        if (httpResponse.Content != null)
                        {
                            var json = await httpResponse.Content.ReadAsStringAsync();
                        }

                        //If there was no response message
                        else
                        {
                            output += "The api did not send a response\r\n";
                            return output;
                        }
                    }                    
                }
                catch
                {
                    output += "Failed to add Movie to database\r\n";
                    return output;
                }
            }
            //Returns a string to the user client
            output = "Successfully uploaded";
            return output;
        }



        /// <summary>Gets all the list items from a list with a getRequest.</summary>
        /// <param name="ListId">The id of the list.</param>
        /// <returns> A list of all list items</returns>
        public static async Task<ObservableCollection<AllListItems>> GetListItems(int ListId)
        {
            //the ListId is used to find the client owners lists
            
            //Creates the List that will store list returned by the api
            ObservableCollection<AllListItems> returnList = new ObservableCollection<AllListItems>();

            //Adding the ListId to the get request
            using (var client = new HttpClient())
            {
                string uri = "http://localhost:59713/api/ListItems/" + ListId;

                //Get request for getting all the listItems
                using (var httpResponse = await client.GetAsync(uri))
                {
                    if (httpResponse.Content != null)
                    {
                        var json = await httpResponse.Content.ReadAsStringAsync();
                        JsonTextReader reader = new JsonTextReader(new StringReader(json));
                        while (reader.Read())
                        {
                            if (reader.Value != null)
                            {
                                //Console testing
                                Console.WriteLine("Value: {0}", reader.Value);
                            }
                            if ((reader.TokenType == JsonToken.StartObject))
                            {
                                // Load each object from the stream and do something with it
                                JObject obj = JObject.Load(reader);

                                //adding an allListItems object to the return list
                                returnList.Add(new AllListItems
                                {
                                    ListItemId = Convert.ToInt32(obj["listItemId"]),
                                    ListId = Convert.ToInt32(obj["listId"]),
                                    MovieId = Convert.ToInt32(obj["movieId"])
                                });
                            }
                        }
                    }
                }
            }
            //returning the list
            return returnList;
        }

    }
}
