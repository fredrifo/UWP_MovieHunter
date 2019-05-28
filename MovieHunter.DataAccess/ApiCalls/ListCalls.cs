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
        public static async Task<ObservableCollection<AllList>> getTokenOwnerLists(string token)
        {
            //Uses the LoginPage.Token to find the id; 
            //the id is used to find the client owners lists

            string jsonInput = JsonConvert.SerializeObject(token);
            ObservableCollection<AllList> returnList = new ObservableCollection<AllList>();
            var client = new HttpClient();
            
                string uri = "http://localhost:59713/api/Lists/userLists";

                //Post request for getting all the listItems
                var httpResponse = await client.PostAsync(uri, new StringContent(jsonInput, Encoding.UTF8, "application/json"));

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
                        if((reader.TokenType == JsonToken.StartObject))
                        {
                        // Load each object from the stream and do something with it
                        JObject obj = JObject.Load(reader);


                        returnList.Add(new AllList
                        {
                            listName = (obj["listName"]).ToString(),
                            listId = Convert.ToInt32( obj["listId"]),
                            userId = Convert.ToInt32(obj["userId"]) 
                        }


                            );
                        }
                        
                    };
                //returnList.Add(new JsonSerializer().Deserialize<AllList>(reader));



                //dynamic dynJson = JsonConvert.DeserializeObject(json);
                    //foreach (var item in dynJson)
                    //{
                    //    //Adding object to the list
                    //    returnList.Add(new AllList() { userId = item.userId, listId = item.listId, listName = item.listName });
                    //}
                }
                
                return returnList;
            
        }
    }
}
