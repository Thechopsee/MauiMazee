
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
            string apiUrl = "http://localhost:8080/loadMazeList";

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
                    string ids = responseContent.Split("(")[1];
                    string id = ids.Split(")")[0];
                    
                    string[] nums = id.Split(",");
                    MazeDescription[] final = new MazeDescription[nums.Length];
                    for(int i= 0;i < final.Length-1;i++)
                    {
                        MazeDescription md = new MazeDescription();
                        md.name = nums[i];
                        md.ID= Int32.Parse(nums[i]);
                        final[i]= md;
                    }
                    return final;
                }
                else
                {
                    return new MazeDescription[1];
                }
            }
        }

        public static async Task<Maze> getMaze(int userid)
        {
            string apiUrl = "http://localhost:8080/loadMaze";

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
