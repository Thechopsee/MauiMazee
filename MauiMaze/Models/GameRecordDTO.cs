using MauiMaze.Engine;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models
{
    public class GameRecordDTO
    {
        public readonly int grID;
        public readonly int mazeID;
        public readonly int userID;
        public readonly int timeInMilliSeconds;
        public readonly int hitWallsCount;
        public readonly string cellPathString;
        public readonly MoveRecord[] records;
        public GameRecordDTO(int grID, int mazeID, int userID, int timeInMilliSeconds, int hitWallsCount, string cellPathString, MoveRecord[] records)
        {
            this.grID = grID;
            this.mazeID = mazeID;
            this.userID = userID;
            this.timeInMilliSeconds = timeInMilliSeconds;
            this.hitWallsCount = hitWallsCount;
            this.cellPathString = cellPathString;
            this.records = records;
        }
        public GameRecord transformToGamerecord()
        {
            GameRecord gr = new GameRecord(mazeID, userID);
            if (records is not null)
            {
                gr.moves = records.ToList();
            }
            else
            {
                gr.moves = new List<MoveRecord>();
            }
            gr.hitWallsCount = hitWallsCount;
            gr.timeInMilliSeconds = timeInMilliSeconds;
            gr.grID = gr.grID;
            string[] splited_path = cellPathString.Split("->");
            foreach (string s in splited_path)
            {
                gr.cellPath.Add(Int32.Parse(s));
            }
            return gr;
        }
    }
}
