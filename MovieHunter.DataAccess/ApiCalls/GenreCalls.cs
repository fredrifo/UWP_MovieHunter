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
    public static class GenreCalls
    {
        public static async Task<string> PostGenre(Genre genre)
        {
            if (genre.GenreName.Length < 1)
            {
                return "Please specify a genre name.";
            }



            //HttpPost
            string jsonInput = JsonConvert.SerializeObject(genre);

            var client = new HttpClient();
            try
            {
                string uri = "http://localhost:59713/api/Genres/new";
                var httpResponse = await client.PostAsync(uri, new StringContent(jsonInput, Encoding.UTF8, "application/json"));

                if (httpResponse.Content != null)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    return json;
                }
            }
            catch
            {
                return "Failed to add genre to database";
            }
            return "lal";
        }


        //HTTPGet for retrieving a list of all the genres in the database
        public static async Task<ObservableCollection<Genre>> GetGenres()
        {

            ObservableCollection<Genre> returnList = new ObservableCollection<Genre>();

            using (var client = new HttpClient())
            {
                try
                {
                    string uri = "http://localhost:59713/api/Genres/";

                    //Sends the post request
                    using (
                    var httpResponse = await client.GetAsync(uri))
                    {
                        //If there is a response from the server
                        if (httpResponse.Content != null)
                        {
                            var json = await httpResponse.Content.ReadAsStringAsync();

                            //Reading the response so that it will only return the string Name
                            JsonTextReader reader = new JsonTextReader(new StringReader(json));

                            //Loops through all elements in the Genre HttpResponse
                            while (reader.Read())
                            {
                                if ((reader.TokenType == JsonToken.StartObject))
                                {
                                    // Load object from the stream and do something with it
                                    JObject obj = JObject.Load(reader);

                                    //Adding the object to the returnList
                                    returnList.Add(new Genre
                                    {
                                        GenreId = Convert.ToInt32(obj["genreId"]),
                                        GenreName = Convert.ToString(obj["genreName"]),
                                    });
                                }
                            }
                        }
                        else
                        {
                            return returnList;
                        }
                    }

                        
                }

                catch
                {
                    return returnList;
                }
                //Returning the Genre List
                return returnList;
            }
        }

                


        //Uses a list of Genres to find the title using the genreId
        public static string GetGenreNameFromList(ObservableCollection<Genre> list, int genreId)
        {
            //Looking at all movie object
            foreach (Genre genre in list)
            {
                //If the movie object matches the id
                if (genre.GenreId == genreId)
                {
                    //Return the title
                    return genre.GenreName;
                }
            }
            //Could not find that movieId in the list
            return "Could not find name in list";

        }
    }
}
