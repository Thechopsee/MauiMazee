using MauiMaze.Engine;
using MauiMaze.Helpers;
using MauiMaze.Models.DTOs;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Services
{
    public class RecordFetcher
    {
        public static async Task deleteRecordsByMazeOffline(int mid )
        {
            string data= await SecureStorage.Default.GetAsync("recordsCounts" + mid).ConfigureAwait(false);
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
            gr.moves = new List<MoveRecord>();
            await SecureStorage.Default.SetAsync("records+" + gr.mazeID + "+" + (len + 1), JsonConvert.SerializeObject(gr)) ;
            string red = await SecureStorage.Default.GetAsync("records+" + gr.mazeID + "+" + (len + 1));
            string kr = "karel";
        }


        public static async Task<bool> SaveRecordOnline(GameRecordDTO gameRecord, HttpClient? httpClient = null)
        {
            string apiUrl = ServiceConfig.serverAdress + "records";
            var userData = new
            {
                AT = UserDataProvider.GetInstance().getUserID(),
                GR = gameRecord
            };
            if (httpClient is null)
            {
                httpClient = new HttpClient();
            }
            using (httpClient)
            {
                string jsonUserData = JsonConvert.SerializeObject(userData);
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                return response.IsSuccessStatusCode;
            }
        }
        public static async Task<GameRecord[]> loadRecordsByMaze(int mid, HttpClient? httpClient = null)
        {
            string apiUrl = ServiceConfig.serverAdress + "mazes/"+mid+"/records";
            if (httpClient is null)
            {
                httpClient = new HttpClient();
            }
            using (httpClient)
            {
                string jsonUserData = JsonConvert.SerializeObject(new { mazeID = mid });
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content).ConfigureAwait(true);
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
        public static async Task<GameRecord[]> loadRecordsByUser(int uid, HttpClient? httpClient = null)
        {
            string apiUrl = ServiceConfig.serverAdress + "users/"+uid+"/records";
            if (httpClient is null)
            {
                httpClient = new HttpClient();
            }
            using (httpClient)
            {
                string jsonUserData = JsonConvert.SerializeObject(new { userID = uid ,AT= UserDataProvider.GetInstance().getUserAT() });
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content).ConfigureAwait(true);
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
    }
}
