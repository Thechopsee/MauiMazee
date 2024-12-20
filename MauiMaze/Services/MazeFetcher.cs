﻿
using MauiMaze.Engine;
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Models.DTOs;
using MauiMaze.Models.RoundedMaze;
using MauiMaze.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace MauiMaze.Services
{
    public class MazeFetcher
    {
        public static async Task<(Maze[], MazeDescription[])> getOfflineMazes()
        {
            string data = await SecureStorage.Default.GetAsync("mazelist");
            if (data is null)
            {
                return (new Maze[0],new MazeDescription[0]);
            }
            string[] splited = data.Split(";");
            Maze[] mazeDescriptions = new Maze[splited.Length];
            MazeDescription[] Descriptions = new MazeDescription[splited.Length];
            for (int i=0;i<splited.Length;i++)
            {
                string maze = await SecureStorage.Default.GetAsync("maze" + splited[i].Trim()) ;
                if (maze is null)
                {
                    continue;
                }
                else
                {
                   mazeDescriptions[i]=JsonConvert.DeserializeObject<Maze>(maze);
                   Descriptions[i] = new MazeDescription(Int32.Parse(splited[i]), mazeDescriptions[i].mazeType,DateTime.Now,LoginCases.Offline);
                }
            }
            return (mazeDescriptions,Descriptions);
        }
        public static async Task<GameMaze> getMazeLocalbyID(int id)
        {
            string maze = await SecureStorage.Default.GetAsync("maze" + id);
            GameMaze gm = JsonConvert.DeserializeObject<GameMaze>(maze);
            if (gm.mazeType == MazeType.Rounded)
            {
                RoundedMaze rm = new RoundedMaze(new Size(gm.Width, gm.Height), gm.Edges);
                return rm;
            }
            else
            {
                return gm;
            }   
        }
        public static async Task saveMazeLocally(GameMaze maze)
        {
            string data = await SecureStorage.Default.GetAsync("mazelist");
            if (data is null || data == " ")
            {
                await SecureStorage.Default.SetAsync("mazelist", "1");
                if (maze.mazeType == MazeType.Classic)
                {
                    await SecureStorage.Default.SetAsync("maze1", JsonConvert.SerializeObject(maze));
                }
                else
                {
                    GameMaze gm = (GameMaze)maze.Clone();
                    await SecureStorage.Default.SetAsync("maze1", JsonConvert.SerializeObject(gm));
                }
            }
            else
            {
                string[] splited = data.Split(";");
                int last = Int32.Parse(splited[splited.Length - 1]);
                await SecureStorage.Default.SetAsync("mazelist", data + ";" + (last + 1));
                if (maze.mazeType == MazeType.Classic)
                {
                    await SecureStorage.Default.SetAsync("maze" + (last + 1), JsonConvert.SerializeObject(maze));
                }
                else
                {
                    GameMaze gm = (GameMaze)maze.Clone();
                    await SecureStorage.Default.SetAsync("maze" + (last + 1), JsonConvert.SerializeObject(gm));
                }
            }
        }
        public static async Task deleteMazelocaly(int mid)
        {
            string data = await SecureStorage.Default.GetAsync("mazelist");
            string[] splited = data.Split(mid+";");
            if (splited.Length == 1)
            {
                await SecureStorage.Default.SetAsync("mazelist"," ");
                await SecureStorage.Default.SetAsync("maze" + mid, " ");
            }
            else
            {
                string result = string.Join("", splited);
                await SecureStorage.Default.SetAsync("mazelist", result);
                await SecureStorage.Default.SetAsync("maze" + mid, " ");
            }
        }
        public static async Task<bool> SaveMazeOnline(int userIDD, MazeDTO maze, HttpClient? httpClient = null)
        {
            string apiUrl = ServiceConfig.serverAdress + "mazes";

            var userData = new
            {
                userID = userIDD,
                AT = UserDataProvider.GetInstance().getUserAT(),
                mazedto = maze
            };
            if (httpClient is null)
            {
                httpClient = new HttpClient();
            }
            using (httpClient)
            {
                string jsonUserData = JsonConvert.SerializeObject(userData);
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                return response.IsSuccessStatusCode;
            }
        }
        public static async Task<MazeDescription[]> getMazeList(int userid, HttpClient? httpClient = null)
        {
            await getMazeCountForUser(userid);
            string apiUrl = ServiceConfig.serverAdress+"users/"+userid+"/mazes";

            if (httpClient is null)
            {
                httpClient = new HttpClient();
            }
            using (httpClient)
            {
                string jsonUserData = JsonConvert.SerializeObject(null);
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                if (response.IsSuccessStatusCode)
                {
                    MazeDescriptionDTO mm = JsonConvert.DeserializeObject<MazeDescriptionDTO>(responseContent);
                    MazeDescription[] mazeDescriptions = new MazeDescription[mm.descriptions.Count];
                    for (int i = 0; i < mazeDescriptions.Length; i++)
                    {
                        MazeDescription md = new();
                        md.ID = Int32.Parse(mm.descriptions[i][0]);
                        md.mazeType = (MazeType)Enum.Parse(typeof(MazeType),mm.descriptions[i][2]);
                        md.creationDate = DateTime.Parse(mm.descriptions[i][3]);
                        md.description=(md.ID)+" " + mm.descriptions[i][2] + " " + mm.descriptions[i][3];
                        md.whereIsMazeSaved = LoginCases.Online;
                        mazeDescriptions[i] = md;
                    }
                    return mazeDescriptions;
                }
                else
                {
                    return new MazeDescription[0];
                }
            }
        }
        public static async Task<int> getMazeCountForUser(int userid, HttpClient? httpClient = null)
        {
            int rsp = 0;
            string apiUrl = ServiceConfig.serverAdress + "users/" + userid + "/mcount";
            var userData = new
            {
                userID = userid,
            };
            if (httpClient is null)
            {
                httpClient = new HttpClient();
            }
            using (httpClient)
            {
                string jsonUserData = JsonConvert.SerializeObject(userData);
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                if (response.IsSuccessStatusCode)
                {
                    MazeCountDTO mm = JsonConvert.DeserializeObject<MazeCountDTO>(responseContent);
                    rsp = mm.message;
                }
            }
            return rsp;
        }
        public static async Task<int[]> deleteAllMazes()
        {
            (Maze[] mazes, MazeDescription[] md) = await getOfflineMazes();
            List<int> deletedIDs = new List<int>();
            foreach (MazeDescription m in md)
            {
                if (m != null)
                {
                    deletedIDs.Add(m.ID);
                    await MazeFetcher.deleteMazelocaly(m.ID);
                }
            }
            await SecureStorage.Default.SetAsync("mazelist", " ");
            return deletedIDs.ToArray();
        }
        public static async Task<Maze> getMaze(int mazeid, HttpClient? httpClient = null)
        {
            string apiUrl = ServiceConfig.serverAdress +"mazes/"+mazeid;
            if (httpClient is null)
            {
                httpClient = new HttpClient();
            }
            using (httpClient)
            {
                string jsonUserData = JsonConvert.SerializeObject(null);
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl,content).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

               if (response.IsSuccessStatusCode)
                {
                    MazeDTO mm = JsonConvert.DeserializeObject<MazeDTO>(responseContent);

                    Maze mz=new Maze(mm.size,mm.size, Helpers.GeneratorEnum.Sets);
                    mz.MazeID = mazeid;
                    mz.Edges = mm.edges;
                    mz.start = new Engine.Start(-1,-1,mm.startCell);
                    mz.end =new Engine.End(-1,-1,-1,-1,mm.endCell);
                    return mz;
                }
                else
                {
                    return new Maze(1, 1, Helpers.GeneratorEnum.Sets);
                }
            }
        }
    }
}
