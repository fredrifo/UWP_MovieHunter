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

        public static async Task<string> postListItem(ListItem listItem)
        {
            //Serializing movie object
            string input = JsonConvert.SerializeObject(listItem);
            string output = "";
            var client = new HttpClient();

            try
            {
                string uri = "http://localhost:59713/api/ListItems/";

                //Sends the post request
                var httpResponse = await client.PostAsync(uri, new StringContent(input, Encoding.UTF8, "application/json"));

                //If there is a response from the server
                if (httpResponse.Content != null)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                }

                //
                else
                {
                    output += "The api did not send a response\r\n";
                    return output;
                }
            }

            catch
            {
                output += "Failed to add Movie to database\r\n";
                return output;
            }
            output = "Successfully uploaded";
            return output;
        }



        public static async Task<ObservableCollection<AllListItems>> getListItems(int id)
        {
            //Uses the LoginPage.Token to find the id; 
            //the id is used to find the client owners lists

            string jsonInput = JsonConvert.SerializeObject(id);
            ObservableCollection<AllListItems> returnList = new ObservableCollection<AllListItems>();
            var client = new HttpClient();

            string uri = "http://localhost:59713/api/ListItems/";

            //Post request for getting all the listItems
            var httpResponse = await client.GetAsync(uri);

            if (httpResponse.Content != null)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                JsonTextReader reader = new JsonTextReader(new StringReader(json));
                while (reader.Read())
                {
                    if (reader.Value != null)
                    {
                        Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
                    }
                    if ((reader.TokenType == JsonToken.StartObject))
                    {
                        // Load each object from the stream and do something with it
                        JObject obj = JObject.Load(reader);


                        returnList.Add(new AllListItems
                        {
                            ListItemId = Convert.ToInt32(obj["listItemId"]),
                            ListId = Convert.ToInt32(obj["listId"]),
                            MovieId = Convert.ToInt32(obj["movieId"])
                        });
                    }

                }
            }

            return returnList;

        }

    }
}
