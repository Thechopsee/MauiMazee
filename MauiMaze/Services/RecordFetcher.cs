using MauiMaze.Engine;
using MauiMaze.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Services
{
    public class RecordFetcher
    {
        private static async Task<List<GameRecord>> FetchLocal()
        {
            //TODO add similar to Mazes
            string data = await SecureStorage.Default.GetAsync("recordlist_json").ConfigureAwait(false);
            if (data is null)
            {
                return new List<GameRecord>();
            }

            List<int> list2 = JsonConvert.DeserializeObject<List<int>>(data);
            List<GameRecord> records = new();
            foreach (int x in list2)
            {
                string record = await SecureStorage.Default.GetAsync("record_json"+x).ConfigureAwait(false);
                records.Add(JsonConvert.DeserializeObject<GameRecord>(record));
            }
            return records;


        }
        public static async Task<bool> SaveRecordOnline(GameRecordDTO gameRecord)
        {
            string apiUrl = ServiceConfig.serverAdress + "saveRecord";

            using (HttpClient client = new HttpClient())
            {
                string jsonUserData = JsonConvert.SerializeObject(gameRecord);
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                return response.IsSuccessStatusCode;

            }
        }
        public static async Task<GameRecord[]> loadRecordsByMaze(int mid)
        {
            string apiUrl = ServiceConfig.serverAdress + "loadRecordByMaze";

            using (HttpClient client = new HttpClient())
            {
                string jsonUserData = JsonConvert.SerializeObject(new { mazeID = mid });
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                if (response.IsSuccessStatusCode)
                {
                    GameRecordDTO[] dto = JsonConvert.DeserializeObject<GameRecordDTO[]>(responseContent);
                    List<GameRecord> gameRecords = new List<GameRecord>();
                    foreach (GameRecordDTO d in dto)
                    {
                        gameRecords.Add(d.transformToGamerecord());
                    }
                    return gameRecords.ToArray();
                }
                return new List<GameRecord>().ToArray();
            }

        }
        public static async Task<GameRecord[]> loadRecordsByUser(int uid)
        {
            string apiUrl = ServiceConfig.serverAdress + "loadRecordByUser";

            using (HttpClient client = new HttpClient())
            {
                string jsonUserData = JsonConvert.SerializeObject(new { userID = uid });
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                if (response.IsSuccessStatusCode)
                {
                    GameRecordDTO[] dto = JsonConvert.DeserializeObject<GameRecordDTO[]>(responseContent);
                    List<GameRecord> gameRecords = new List<GameRecord>();
                    foreach (GameRecordDTO d in dto)
                    {
                        gameRecords.Add(d.transformToGamerecord());
                    }
                    return gameRecords.ToArray();
                }
                return new List<GameRecord>().ToArray();
            }
        }
            //public static async Task<List<GameRecord>> FetchRecords(int uid)
            //{
            //    var local = await FetchLocal();
            //    //get online
            //    //check if offline is online
            //    //save offline to online
            //        //check maze id
            //        //if current userid have maze with mazeid ->synchronize
            //        //else dont synchronize wait to maze to be synchronized
            //        //else manual synchro later

            //    //delete synchronized

            //   return RecordRepository.GetInstance().getRecords();
            //}
        }
}
