using MauiMaze.Engine;
using MauiMaze.Helpers;
using MauiMaze.Models.DTOs;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Services
{
    public class RecordFetcher
    {
        public static async Task deleteRecordsByMazeOffline(int mid)
        {
            string data = await SecureStorage.Default.GetAsync("recordsCounts" + mid).ConfigureAwait(false);
            if (data is null)
            {
                return;
            }
            int len = Int32.Parse(data);;
            for (int i = 1; i <= len; i++)
            {
                string record = await SecureStorage.Default.GetAsync("records+" + mid + "+" + i);
                if (record is not null)
                {
                    await SecureStorage.Default.SetAsync("records+" + mid + "+" + i," ");
                }
            }
            await SecureStorage.Default.SetAsync("recordsCounts" + mid, "0");
        }
        public static async Task<List<GameRecord>> loadRecordByMazeOffline(int mid)
        {
            string data = await SecureStorage.Default.GetAsync("recordsCounts"+mid).ConfigureAwait(false);
            if (data is null)
            {
                return new List<GameRecord>();
            }
            int len = Int32.Parse(data);
            List<GameRecord> records = new();
            for (int i= 1;i <= len;i++)
            {
                string record = await SecureStorage.Default.GetAsync("records+" + mid + "+" + i);
                if (record is not null)
                {
                    records.Add(JsonConvert.DeserializeObject<GameRecord>(record));
                }
            }
            return records;

        }
        public static async Task saveRecordByMazeOffline(GameRecord gr)
        {
            string data = await SecureStorage.Default.GetAsync("recordsCounts" + gr.mazeID).ConfigureAwait(false);
            int len;
            if (data is null)
            {
                len = 0;
            }
            else
            {
                len = Int32.Parse(data);
            }
            await SecureStorage.Default.SetAsync("recordsCounts" + gr.mazeID, ""+(len+1));
            string json = JsonConvert.SerializeObject(gr);
            //SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Constantss.DatabasePath, Constantss.Flags);
            //var result = await Database.CreateTableAsync<MoveRecord>();
            gr.moves = new List<MoveRecord>();
            await SecureStorage.Default.SetAsync("records+" + gr.mazeID + "+" + (len + 1), JsonConvert.SerializeObject(gr)) ;
            string red = await SecureStorage.Default.GetAsync("records+" + gr.mazeID + "+" + (len + 1));
            string kr = "karel";
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
