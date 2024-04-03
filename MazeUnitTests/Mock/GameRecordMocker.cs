using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MauiMaze.Engine;

namespace MazeUnitTests.Mock
{
    public class GameRecordMocker
    {
        public static async Task<GameRecord> getMock(Size size,int cell_width,int moves_count)
        {
            MauiMaze.Engine.GameRecord gameDriver = new (-1,-1);
            long last_time = 0;
            gameDriver.stopwatch.Start();
            
            for (int i = 0; i < moves_count; i++)
            {
                Random random = new Random();
                double randomX = random.NextDouble();
                double randomY = random.NextDouble();
                int randomNumber = random.Next(0, 2);
                long time = gameDriver.stopwatch.ElapsedMilliseconds - last_time;
                last_time = gameDriver.stopwatch.ElapsedMilliseconds;
                double mazesize = size.Width/cell_width;
                double xid = ((size.Width*randomX) / cell_width);
                double yid = ((size.Width*randomY) / cell_width);
                //await Task.Delay(1);
                int cellID = (int)mazesize * (int)Math.Floor(yid) + (int)Math.Floor(xid);
                gameDriver.addMoveRecord(new MoveRecord(0, randomX, randomY, (int)randomNumber,(int)time,cellID ));
                gameDriver.addCellMoveRecord(cellID);
            }
            gameDriver.stopMeasuremnt();
            return gameDriver;
        }
    }
}
