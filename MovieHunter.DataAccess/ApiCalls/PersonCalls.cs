using MovieHunter.DataAccess.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieHunter.DataAccess.Client.ApiCalls
{
    public static class PersonCalls
    {
        public static async Task<string> postPerson(Person newPerson)
        {
            //HttpPost
            string jsonInput = JsonConvert.SerializeObject(newPerson);

            var client = new HttpClient();
            try
            {
                string uri = "http://localhost:59713/api/People/add";
                var httpResponse = await client.PostAsync(uri, new StringContent(jsonInput, Encoding.UTF8, "application/json"));

                if (httpResponse.Content != null)
                {
                    var json = await httpResponse.Content.ReadAsStringAsync();
                    return json;
                }
            }
            catch
            {
                return "Failed to add person to database";
            }
            return "";
        }
    }
}
