using MovieHunter.DataAccess.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    }
}
