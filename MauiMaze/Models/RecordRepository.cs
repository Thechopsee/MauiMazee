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
        // Private static instance variable to hold the single instance of the class
        private static RecordRepository instance;
        private List<GameRecord> records = new List<GameRecord>();

        // Private constructor prevents instantiation from other classes
        private RecordRepository()
        {
            // Initialization code, if any
        }
        public void addRecord(GameRecord record)
        {
            records.Add(record);
        }   
        public List<GameRecord> getRecords()
        {
            return records;
        }   

        // Public static method to provide access to the single instance
        public static RecordRepository GetInstance()
        {
            // Lazy initialization - create the instance only when needed
            if (instance == null)
            {
                instance = new RecordRepository();
            }

            return instance;
        }

    }
}
