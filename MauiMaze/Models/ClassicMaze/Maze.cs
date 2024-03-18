using MauiMaze.Engine;
using MauiMaze.Helpers;
using MauiMaze.Models.Generatory;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MauiMaze.Models.ClassicMaze
{
    public class Maze : GameMaze
    {
        public void setupFromMaze(GameMaze mz)
        {
            this.Edges = mz.Edges;
            this.start = mz.start;
            this.end = mz.end;
            this.MazeID = mz.MazeID;
            this.mazeType = mz.mazeType;
        }
        public Maze(int width,int height, GeneratorEnum ge)
        {
            IGenerator generator;
            if (ge == GeneratorEnum.Sets)
            {
                generator = new SetsGenerator();
            }
            else
            {
                generator = new HuntAndKillMazeGenerator();
            }
            mazeType = ViewModels.MazeType.Classic;
            Edges = generator.GenerateMaze(height, width).ToArray();
            this.Width= width;
            this.Height= height;
            //Size = size;
        }

        

    }
}
