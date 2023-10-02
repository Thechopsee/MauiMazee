using MauiMaze.Engine;
using MauiMaze.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Services
{
    public class RecordFetcher
    {
        public static List<GameRecord> FetchRecords(int uid)
        {
           return RecordRepository.GetInstance().getRecords();
        }
    }
}
