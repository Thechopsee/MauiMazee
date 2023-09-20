
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
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
        public static async Task<MazeDescription[]> getMazeList(int userid)
        {
            string apiUrl = "http://localhost:8085/loadMazeList";

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
                        MazeDescription md = new MazeDescription();
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

        public static async Task<Maze> getMaze(int userid)
        {
            string apiUrl = "http://localhost:8085/loadMaze";

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

                await Application.Current.MainPage.DisplayAlert("Upozornění", "run "+ JsonConvert.DeserializeObject(responseContent).ToString(), "OK");
                
               if (response.IsSuccessStatusCode)
                {
                    MazeMessage mm = JsonConvert.DeserializeObject<MazeMessage>(responseContent);
                    List<Edge> edges = new List<Edge>();
                    foreach (int[] tup in mm.message)
                    {
                        edges.Add(new Edge(tup[2], tup[3]));
                    }
                    Maze mz=new Maze(new Size(10, 10));
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
