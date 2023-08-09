using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Engine
{
    public class GameRecord
    {
        public List<MoveRecord> moves;
        public int hitWallsCount { get; set; }
        public double travelLenght { get; set; }
        public string mazeID { get; set; }
        public int numOfTry { get; set; }
        public int userID { get; set; } 
        public string name { get; set; }
        Stopwatch stopwatch = new Stopwatch();
        private int timeInMilliSeconds { get; set; }

        public GameRecord(String mazeID,int numOfTry)
        {
            this.mazeID = mazeID;
            this.numOfTry = numOfTry;
            name =mazeID+"_"+numOfTry;
            moves = new List<MoveRecord>();
            stopwatch.Start();
        }
        public void addMoveRecord(MoveRecord move)
        {
            moves.Add(move);
        }
        public int numberOfMoves()
        {
            return moves.Count;
        }
        public void stopMeasuremnt()
        { 
            stopwatch.Stop();
            timeInMilliSeconds = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
            int lastX=0;
            int lastY=0;
            foreach(MoveRecord move in moves)
            {
                if(move.hitWall)
                {
                    hitWallsCount++;
                }
                double deltaX = move.positionx - lastX;
                double deltaY = move.positiony - lastY;

                double distance = Math.Abs(Math.Sqrt(deltaX * deltaX + deltaY * deltaY));
                lastX = move.positionx;
                lastY = move.positiony;
                travelLenght += distance;
            }

        }
    }
}
