using MovieHunter.DataAccess.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieHunter.DataAccess.Client.ApiCalls
{
    public static class MovieCalls
    {

        public static async Task<string> PostMovieAsync(Movie movie)
        {
            //Serializing movie object
            string input = JsonConvert.SerializeObject(movie);
            string output = "";
            var client = new HttpClient();

            try
            {
                string uri = "http://localhost:59713/api/Movies/";

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
    }
}
