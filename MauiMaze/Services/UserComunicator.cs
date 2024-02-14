﻿
using MauiMaze.Models;
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
        public static async Task<UserDataDTO> tryToLogin(string emaill,string pas)
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
                UserDataDTO dto = JsonConvert.DeserializeObject<UserDataDTO>(responseContent);
                if (response.IsSuccessStatusCode)
                {
                    return dto;
                }
                else
                {
                    UserDataDTO tmp = new();
                    tmp.id = -1;
                    return tmp;
                }
            }
        }
        public static async Task<UserDataDTO[]> getUsers(string emaill, string pas)
        {
            string apiUrl = ServiceConfig.serverAdress + "loadUsers";

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
                UserDataDTO[] dto = JsonConvert.DeserializeObject<UserDataDTO[]>(responseContent);
                if (response.IsSuccessStatusCode)
                {
                    return dto;
                }
                else
                {
                    UserDataDTO[] tmp=new UserDataDTO[0];
                    return tmp;
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
