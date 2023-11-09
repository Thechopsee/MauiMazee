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
