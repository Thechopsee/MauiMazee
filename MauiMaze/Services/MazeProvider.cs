using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Services
{
    public class MazeProvider
    {
        private static MazeProvider instance;
        private List<MazeDescription> cache;

        private MazeProvider()
        {
            cache = new();
        }

        public static MazeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MazeProvider();
                }
                return instance;
            }
        }

        public async Task<List<MazeDescription>> loadMazes()
        {

            int datacount = await MazeFetcher.getMazeCountForUser(UserDataProvider.GetInstance().getUserID()).ConfigureAwait(false);
            
            if (this.cache.Count == datacount)
            {
                return this.cache;
            }
            else
            {
                MazeDescription[] data = await MazeFetcher.getMazeList(UserDataProvider.GetInstance().getUserID()).ConfigureAwait(false);
                cache = data.ToList();
            }
            

            return this.cache;
        }
        public async Task<(Maze[], MazeDescription[])> loadLocalMazes()
        {
            (Maze[] mazes,MazeDescription[] md) = await MazeFetcher.getOfflineMazes();
            return (mazes,md);
        }
    }
}
