
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MauiMaze.Services
{
    public class UserComunicator
    {
        public static async Task<int> tryToLogin(string emaill,string pas)
        {
            string apiUrl = ServiceConfig.serverAdress + "login";

            var userData = new
            {
                email = emaill,
                password = pas
            };

            using (HttpClient client = new HttpClient())
            {
                string jsonUserData = JsonConvert.SerializeObject(userData);
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string responseContent = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    string ids = responseContent.Split(":")[1];
                    string id = ids.Split("}")[0];
                    return Int32.Parse(id);
                }
                else
                {
                    return -1;
                }
            }
        }
        public static async Task<bool> TryToSaveMaze(int userIDD, MauiMaze.Models.ClassicMaze.Edge[] edgess)
        {
            string apiUrl = ServiceConfig.serverAdress + "saveMaze";

            var userData = new
            {
                userID = userIDD,
                edges = edgess
            };
            using (HttpClient client = new HttpClient())
            {
                string jsonUserData = JsonConvert.SerializeObject(userData);
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return true; 
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
