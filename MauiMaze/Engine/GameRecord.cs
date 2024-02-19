using MauiMaze.Models;
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
        public int grID { get; set; }
        public int hitWallsCount { get; set; }
        public double travelLenght { get; set; }
        public int mazeID { get; set; }
        public int userID { get; set; }
        public bool finished { get; set; }
        public int timeInMilliSeconds { get; set; }
        public List<int> cellPath { get; }

        public List<MoveRecord> moves;
        public Stopwatch stopwatch { get; set; }
        public Color color { get; set; }

        public GameRecord(int mazeID,int userID)
        {
            stopwatch = new Stopwatch();
            this.mazeID = mazeID;
            this.userID = userID;
            cellPath = new List<int>();
            moves = new List<MoveRecord>();
        }
        public GameRecordDTO GetRecordDTO()
        {
            return new GameRecordDTO(grID,mazeID,userID,timeInMilliSeconds,hitWallsCount, pathToStr(), moves.ToArray());   
        }
        public string pathToStr()
        {
            string stringpath = "";
            for (int i = 0; i < cellPath.Count; i++)
            {
                stringpath += cellPath[i];
                if (i != cellPath.Count - 1)
                {
                    stringpath += "->";
                }
            }
            return stringpath;
        }
        public void addCellMoveRecord(int cellID)
        {
            if (cellPath.Count == 0)
            {
                stopwatch.Start();
                cellPath.Add(cellID);
            }
            else if (cellPath.Last() != cellID)
            {
                cellPath.Add(cellID);
            }
        }
        public void addMoveRecord(MoveRecord move)
        {
            if (moves.Count > 0)
            {
                if (moves.Last().positionx == move.positionx && moves.Last().positiony == move.positiony)
                {
                    return;
                }
            }
            moves.Add(move);
        }
        public void stopMeasuremnt()
        { 
            stopwatch.Stop();
            timeInMilliSeconds = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
            int lastX=0;
            int lastY=0;
            bool hit_diffuser = false;
            foreach(MoveRecord move in moves)
            {
                if (move.hitWall==1 && hit_diffuser == false)
                {
                    hitWallsCount++;
                    hit_diffuser = true;
                }
                else if (move.hitWall==0 && hit_diffuser == true)
                {
                    hit_diffuser = false;
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
