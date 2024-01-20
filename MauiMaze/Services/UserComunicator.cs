
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                HttpResponseMessage response = await client.PostAsync(apiUrl, content).ConfigureAwait(false);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                
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

        public static async Task<int> tryToRegister(string email,string pas ,string code)
        {
            string apiUrl = ServiceConfig.serverAdress + "register";

            var userData = new
            {
                email = email,
                password = pas,
                code=code,
            };

            using (HttpClient client = new HttpClient())
            {
                string jsonUserData = JsonConvert.SerializeObject(userData);
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content).ConfigureAwait(false);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return 0;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }



        }

    }
}
