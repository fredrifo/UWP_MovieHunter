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
    public static class MovieCalls
    {

        /// <summary>Method for Posting a new Movie object to the db.</summary>
        /// <param name="movie">The movie object</param>
        /// <returns>Returning a message string</returns>
        public static async Task<string> PostMovieAsync(Movie movie)
        {
            //Serializing movie object
            string input = JsonConvert.SerializeObject(movie);
            string output = "";

            //Post request
            using (var client = new HttpClient())
            {
                try
                {
                    string uri = "http://localhost:59713/api/Movies/";

                    //Sends the post request
                    var httpResponse = await client.PostAsync(uri, new StringContent(input, Encoding.UTF8, "application/json"));
                }
                catch
                {
                    output += "Failed to add Movie to database\r\n";
                    return output;
                }
                output = "Successfully uploaded";
            }
            //returning the output string
            return output;
        }
        

        /// <summary>Gets the complete list of movies from the api.</summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<Movie>> GetMovies()
        {
            //Creates a list
            ObservableCollection<Movie> returnList = new ObservableCollection<Movie>();

            using (var client = new HttpClient())
            {
                try
                {
                    string uri = "http://localhost:59713/api/Movies/";

                    //Sends the post request
                    using (var httpResponse = await client.GetAsync(uri))
                    {
                        //If there is a response from the server
                        if (httpResponse.Content != null)
                        {
                            var json = await httpResponse.Content.ReadAsStringAsync();

                            //Reading the response so that it will only return the string Name
                            JsonTextReader reader = new JsonTextReader(new StringReader(json));

                            while (reader.Read())
                            {
                                if ((reader.TokenType == JsonToken.StartObject))
                                {
                                    // Load object from the stream and do something with it
                                    JObject obj = JObject.Load(reader);

                                    //returning the title string to the user
                                    returnList.Add(new Movie
                                    {
                                        MovieId = Convert.ToInt32(obj["movieId"]),
                                        Title = Convert.ToString(obj["title"]),
                                        CoverImage = Convert.ToString(obj["coverImage"]),
                                        Summary = Convert.ToString(obj["summary"]),
                                        Rating = Convert.ToByte(obj["rating"]),
                                        GenreId = Convert.ToInt32(obj["genreId"]),
                                        DirectorId = Convert.ToInt32(obj["directorId"]),
                                        WriterId = Convert.ToInt32(obj["writerId"]),
                                        Star = Convert.ToInt32(obj["star"])

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
            }
            //Returning the movie name
            return returnList;
        }


        /// <summary>HttpPut for Updating/Overwriting a movie in the database</summary>
        /// <param name="movieObject">The movie object that is replacing.</param>
        /// <param name="movieId">The id to the movie that is replaced.</param>
        /// <returns></returns>
        public static async Task PutMovie(Movie movieObject, int movieId)
        {
            ObservableCollection<Movie> returnList = new ObservableCollection<Movie>();

            using (var client = new HttpClient())
            {
                string input = JsonConvert.SerializeObject(movieObject);
                try
                {
                    //Address for HttpPut
                    string uri = "http://localhost:59713/api/Movies/" + movieId;

                    //Sends the put request
                    var httpResponse = await client.PutAsync(uri, new StringContent(input, Encoding.UTF8, "application/json"));
                }

                catch
                {
                    return;
                }
            }
            return;
        }



        /// <summary>Uses a list of Movies to find the title using the movieId.
        /// This can be helpfull if the movieId is in a ListItem object that doesnt contain the title.
        /// </summary>
        /// <param name="list">  Movie list that will be compared.</param>
        /// <param name="movieId"> movieId that is compared with the list</param>
        /// <returns>The title of the movie</returns>
        public static string GetMovieNameFromList(ObservableCollection<Movie> list, int movieId)
        {
            //Looking at all movie object
            foreach(Movie movie in list)
            {
                //If the movie object matches the id
                if (movie.MovieId == movieId)
                {
                    //Return the title
                    return movie.Title;
                }
            }
            //Could not find that movieId in the list
            return "Could not find name in list";
        }
    }
}
