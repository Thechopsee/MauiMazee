using MauiMaze.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models
{
    public class RecordRepository
    {
        private static RecordRepository instance;
        private List<GameRecord> records = new List<GameRecord>();

        private RecordRepository()
        {

        }
        public void addRecord(GameRecord record)
        {
            records.Add(record);
        }   
        public List<GameRecord> getRecords()
        {
            return records;
        }
        public IEnumerable<GameRecord> getRecordsByMazeId(int mazeid)
        {
            return records.Where(x => x.mazeID == mazeid);
        }
        public static RecordRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new RecordRepository();
            }

            return instance;
        }

    }
}
