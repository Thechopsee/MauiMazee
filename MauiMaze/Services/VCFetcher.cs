using MauiMaze.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Services
{
    public class VCFetcher
    {
        public static async Task<VerificationCode[]> getUnusedCodes(HttpClient? httpClient = null)
        {
            string apiUrl = ServiceConfig.serverAdress + "codes/unused";
            if (httpClient is null)
            {
                httpClient = new HttpClient();
            }
            using (httpClient)
            {
                var content = new StringContent(null, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                if (response.IsSuccessStatusCode)
                {
                    VerificationCode[] mm = JsonConvert.DeserializeObject<VerificationCode[]>(responseContent);
                    return mm;
                }
                else
                {
                    return new VerificationCode[0];
                }

            }
        }
        public static async Task GenerateNewCodes(HttpClient? httpClient = null)
        {
            string apiUrl = ServiceConfig.serverAdress + "codes";
            if (httpClient is null)
            {
                httpClient = new HttpClient();
            }
            using (httpClient)
            {
                var content = new StringContent(null, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                return;

            }
        }
    }
}
