using MovieHunter.DataAccess.Client.Models;
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
    public static class ListCalls
    {
        /// <summary>Posting a token to the database. 
        /// Then it returns the lists that are owned by the user. 
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static async Task<ObservableCollection<AllList>> GetTokenOwnerLists(string token)
        {
            //Uses the LoginPage.Token to find the id; 
            //the id is used to find the client owners lists

            string jsonInput = JsonConvert.SerializeObject(token);
            ObservableCollection<AllList> returnList = new ObservableCollection<AllList>();

            using (var client = new HttpClient())
            {
                string uri = "http://localhost:59713/api/Lists/userLists";

                //Post request for getting all the listItems
                var httpResponse = await client.PostAsync(uri, new StringContent(jsonInput, Encoding.UTF8, "application/json"));

                if (httpResponse.Content != null)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    JsonTextReader reader = new JsonTextReader(new StringReader(json));

                    //Reading through the response
                    while (reader.Read())
                    {
                        if (reader.Value != null)
                        {
                            //Console testing
                            Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
                        }
                        if ((reader.TokenType == JsonToken.StartObject))
                        {
                            // Loading all objects from the stream 
                            JObject obj = JObject.Load(reader);

                            //Adds an AllList object to the return list
                            returnList.Add(new AllList
                            {
                                ListName = (obj["listName"]).ToString(),
                                ListId = Convert.ToInt32(obj["listId"]),
                                UserId = Convert.ToInt32(obj["userId"])
                            });
                        }

                    };
                }
            }
            //Returning the new list 
            return returnList;
            
        }
    }
}
