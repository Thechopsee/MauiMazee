using MauiMaze.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeUnitTests.GameRecordTests
{
    public class RecordClassTests
    {
        [Fact]
        public void AddMoveRecordAddsMoveRecordToList()
        {
            var gameRecord = new GameRecord(1, 1);
            var moveRecord = new MoveRecord(1,0.5, 0.5,1,100,5);
            gameRecord.addMoveRecord(moveRecord);

            Assert.Single(gameRecord.moves);
            Assert.Equal(moveRecord, gameRecord.moves[0]);
        }
        [Fact]
        public void StopMeasurementStopsStopwatchAndCalculatesTimeAndHitWallsCount()
        {
            var gameRecord = new GameRecord(1, 1);
            gameRecord.stopwatch.Start();
            var move1 = new MoveRecord(1, 0.5, 0.5, 1, 100, 5);
            var move2 = new MoveRecord(1, 0.6, 0.6, 1, 100, 5);
            gameRecord.addMoveRecord(move1);
            gameRecord.addMoveRecord(move2);

            gameRecord.stopMeasuremnt();

            Assert.True(gameRecord.stopwatch.IsRunning == false);
            Assert.True(gameRecord.hitWallsCount == 1);
        }
        [Fact]
        public void PathToStrConvertsCellPathToString()
        {
            var gameRecord = new GameRecord(1, 1);
            gameRecord.addCellMoveRecord(0);
            gameRecord.addCellMoveRecord(1);
            gameRecord.addCellMoveRecord(2);
            var pathString = gameRecord.pathToStr();

            Assert.Equal("0->1->2", pathString);
        }

    }
}
