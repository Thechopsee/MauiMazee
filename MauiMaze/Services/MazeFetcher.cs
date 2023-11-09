﻿
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Services
{
    public class MazeFetcher
    {
        public class ResponseData
        {
            public int message { get; set; }
        }
        public static async Task<Maze[]> getOfflineMazes()
        {
            //1;2;

            string data = await SecureStorage.Default.GetAsync("mazelist");
            if (data is null)
            {
                return new Maze[0];
            }
            string[] splited = data.Split(";");
            Maze[] mazeDescriptions = new Maze[splited.Length];
            for(int i=0;i<splited.Length;i++)
            {
                string maze = await SecureStorage.Default.GetAsync("maze" + splited[i]);
                if (maze is null)
                {
                    continue;
                }
                else
                {
                   mazeDescriptions[i]=JsonConvert.DeserializeObject<Maze>(maze);
                }
            }
            return mazeDescriptions;
        }
        public static async Task saveMazeLocally(Maze maze)
        {
            string data = await SecureStorage.Default.GetAsync("mazelist");
            if (data is null)
            {
                data = "1";
            }
            if (data.Length == 1)
            {
                await SecureStorage.Default.SetAsync("mazelist", "1");
                await SecureStorage.Default.SetAsync("maze1", JsonConvert.SerializeObject(maze));
            }
            else
            {
                string[] splited = data.Split(";");
                int last = Int32.Parse(splited[splited.Length - 1]);
                await SecureStorage.Default.SetAsync("mazelist", data + ";" + (last + 1));
                await SecureStorage.Default.SetAsync("maze" + (last + 1), JsonConvert.SerializeObject(maze));
            }
        }
        public static async Task saveMazeDescriptionLocally()
        {
            string data = await SecureStorage.Default.GetAsync("mazeDeslist");
            MazeDescription md = new MazeDescription();
            
            if (data is null)
            {
                data = "1";
            }
            if (data.Length == 1)
            {
                md.ID = 1;
                md.creationDate = DateTime.Now;
                md.mazeType = MazeType.Classic;
                await SecureStorage.Default.SetAsync("mazeDeslist", "1");
                await SecureStorage.Default.SetAsync("mazeDes1", JsonConvert.SerializeObject(md));
            }
            else
            {
                string[] splited = data.Split(";");
                int last = Int32.Parse(splited[splited.Length - 1]);
                md.ID = last+1;
                md.creationDate = DateTime.Now;
                md.mazeType = MazeType.Classic;
                
                await SecureStorage.Default.SetAsync("mazeDeslist", data + ";" + (last + 1));
                await SecureStorage.Default.SetAsync("mazeDes" + (last + 1), JsonConvert.SerializeObject(md));
            }
        }
        public static async Task<bool> SaveMazeOnline(int userIDD, MauiMaze.Models.ClassicMaze.Edge[] edgess)
        {
            string apiUrl = ServiceConfig.serverAdress + "saveMaze";

            var userData = new
            {
                userID = userIDD,
                edges = edgess
            };
            using (HttpClient client = new HttpClient())
            {
                string jsonUserData = JsonConvert.SerializeObject(userData);
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                return response.IsSuccessStatusCode;
               
            }
        }
        public static async Task<MazeDescription[]> getMazeList(int userid)
        {
            await getMazeCountForUser(userid);
            string apiUrl = ServiceConfig.serverAdress+"loadMazeList";

            var userData = new
            {
                userID = userid,
            };

            using (HttpClient client = new HttpClient())
            {
                string jsonUserData = JsonConvert.SerializeObject(userData);
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                   // await Application.Current.MainPage.DisplayAlert("Upozornění", "run " + JsonConvert.DeserializeObject(responseContent).ToString(), "OK");
                    MazeDescriptionDTO mm = JsonConvert.DeserializeObject<MazeDescriptionDTO>(responseContent);
                    MazeDescription[] mazeDescriptions = new MazeDescription[mm.descriptions.Count];
                   // await Application.Current.MainPage.DisplayAlert("Upozornění", "count" + mm.descriptions.Count, "OK");
                    for (int i = 0; i < mazeDescriptions.Length; i++)
                    {
                        MazeDescription md = new();
                        md.ID = Int32.Parse(mm.descriptions[i][0]);
                        md.mazeType = (MazeType)Enum.Parse(typeof(MazeType),mm.descriptions[i][2]);
                        md.creationDate = DateTime.Parse(mm.descriptions[i][3]);
                        md.description=(md.ID)+" " + mm.descriptions[i][2] + " " + mm.descriptions[i][3];
                        mazeDescriptions[i] = md;
                    }
                    return mazeDescriptions;
                }
                else
                {
                    return new MazeDescription[2];
                }
            }
        }
        public static async Task<int> getMazeCountForUser(int userid)
        {
            int rsp = 0;
            string apiUrl = ServiceConfig.serverAdress + "loadMazeCount";

            var userData = new
            {
                userID = userid,
            };
            using (HttpClient client = new HttpClient())
            {
                string jsonUserData = JsonConvert.SerializeObject(userData);
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content).ConfigureAwait(false);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                //await Application.Current.MainPage.DisplayAlert("Upozornění", "run "+ JsonConvert.DeserializeObject(responseContent).ToString(), "OK");

                if (response.IsSuccessStatusCode)
                {
                    ResponseData mm = JsonConvert.DeserializeObject<ResponseData>(responseContent);
                    rsp = mm.message;
                }

            }
        
            return rsp;
        }

        public static async Task<Maze> getMaze(int userid)
        {
            string apiUrl = ServiceConfig.serverAdress +"loadMaze";

            var userData = new
            {
                mazeID = userid,
            };

            using (HttpClient client = new HttpClient())
            {
                string jsonUserData = JsonConvert.SerializeObject(userData);
                var content = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string responseContent = await response.Content.ReadAsStringAsync();

                //await Application.Current.MainPage.DisplayAlert("Upozornění", "run "+ JsonConvert.DeserializeObject(responseContent).ToString(), "OK");
                
               if (response.IsSuccessStatusCode)
                {
                    MazeMessage mm = JsonConvert.DeserializeObject<MazeMessage>(responseContent);
                    List<Edge> edges = new();
                    foreach (int[] tup in mm.message)
                    {
                        edges.Add(new Edge(tup[2], tup[3]));
                    }
                    Maze mz=new Maze(new Size(10, 10));
                    mz.MazeID = userid;
                    mz.Edges = edges.ToArray();
                    return mz;
                }
                else
                {
                    return new Maze(new Size(1, 1));
                }

            }
        }
    }
}
