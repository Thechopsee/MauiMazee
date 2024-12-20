﻿using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Models.DTOs;
using MauiMaze.Services;
using MauiMaze.ViewModels;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MazeUnitTests
{
    public class MazeFetcherTests
    { 
        [Fact]
        public async Task GetMazeList_SuccessfulRequest_ReturnsMazeDescriptions()
        {
            int userId = 1;
            var expectedMazeDescriptions = new MazeDescriptionDTO();
            expectedMazeDescriptions.descriptions = new List<string[]>();
            expectedMazeDescriptions.descriptions.Add(new string[] { "1","1", "Classic", "2024-04-01" });

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "users/1/mazes")
                .Respond(HttpStatusCode.OK, "application/json", JsonConvert.SerializeObject(expectedMazeDescriptions));

            var httpClient = new HttpClient(mockHttp);

            var result = await MazeFetcher.getMazeList(userId, httpClient);

            Assert.NotNull(result);
            Assert.Equal(expectedMazeDescriptions.descriptions.Count, result.Length);
        }

        [Fact]
        public async Task GetMazeList_UnsuccessfulRequest_ReturnsEmptyArray()
        {
            int userId = 1;
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "users/1/mazes")
                .Respond(HttpStatusCode.InternalServerError);

            var httpClient = new HttpClient(mockHttp);
            var result = await MazeFetcher.getMazeList(userId, httpClient);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
        [Fact]
        public async Task SaveMazeOnline_SuccessfulRequest_ReturnsTrue()
        {
            int userId = 1;
            var mazeDto = new MazeDTO();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "mazes")
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(mockHttp);
            var result = await MazeFetcher.SaveMazeOnline(userId, mazeDto, httpClient);

            Assert.True(result);
        }


        [Fact]
        public async Task SaveMazeOnline_UnsuccessfulRequest_ReturnsFalse()
        {
            int userId = 1;
            var mazeDto = new MazeDTO();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "mazes")
                .Respond(HttpStatusCode.InternalServerError);

            var httpClient = new HttpClient(mockHttp);

            var result = await MazeFetcher.SaveMazeOnline(userId, mazeDto, httpClient);

            Assert.False(result);
        }
        [Fact]
        public async Task SaveMazeOnline_BadRequest_ReturnsFalse()
        {
            int userId = 1;
            var mazeDto = new MazeDTO();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "mazes")
                .Respond(HttpStatusCode.BadRequest);

            var httpClient = new HttpClient(mockHttp);

            var result = await MazeFetcher.SaveMazeOnline(userId, mazeDto, httpClient);

            Assert.False(result);
        }
        [Fact]
        public async Task GetMaze_SuccessfulRequest_ReturnsMaze()
        {
            int mazeId = 1;
            var expectedMaze = new Maze(10,10,MauiMaze.Helpers.GeneratorEnum.Sets); 

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "mazes/1")
                .Respond(HttpStatusCode.OK, "application/json", JsonConvert.SerializeObject(expectedMaze));

            var httpClient = new HttpClient(mockHttp);
            var result = await MazeFetcher.getMaze(mazeId, httpClient);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetMaze_SuccessfulRequest_TestMaze()
        {
            int mazeId = 1;
            var expectedMaze = new MazeDTO();
            expectedMaze.size = 10;
            expectedMaze.startCell = 5;
            expectedMaze.endCell = 6;
            expectedMaze.edges = new Maze(10, 10, MauiMaze.Helpers.GeneratorEnum.Sets).Edges;

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "mazes/1")
                .Respond(HttpStatusCode.OK, "application/json", JsonConvert.SerializeObject(expectedMaze));

            var httpClient = new HttpClient(mockHttp);
            var result = await MazeFetcher.getMaze(mazeId, httpClient);

            Assert.Equal(10, result.Width);
            Assert.Equal(10, result.Height);
            Assert.Equal(1, result.MazeID);
            Assert.NotEmpty(result.Edges);
        }
        [Fact]
        public async Task IsStartandEndSettedProperly()
            {
                int mazeId = 1;
                var expectedMaze = new MazeDTO();
                expectedMaze.size = 10;
                expectedMaze.startCell = 5;
                expectedMaze.endCell = 6;
                expectedMaze.edges = new Maze(10, 10, MauiMaze.Helpers.GeneratorEnum.Sets).Edges;

                var mockHttp = new MockHttpMessageHandler();
                mockHttp.When(ServiceConfig.serverAdress + "mazes/1")
                    .Respond(HttpStatusCode.OK, "application/json", JsonConvert.SerializeObject(expectedMaze));

                var httpClient = new HttpClient(mockHttp);
                var result = await MazeFetcher.getMaze(mazeId, httpClient);
                int id = 0;
                Assert.Equal(5, result.start.cell);
                Assert.Equal(6, result.end.cell);
            }

        [Fact]
        public async Task IsSendedRight()
        {
            int mazeId = 1;
            var expectedMaze = new MazeDTO();
            expectedMaze.size = 10;
            expectedMaze.startCell = 5;
            expectedMaze.endCell = 6;
            expectedMaze.edges = new Maze(10, 10, MauiMaze.Helpers.GeneratorEnum.Sets).Edges;
            string expected = JsonConvert.SerializeObject(expectedMaze);
            string edges = JsonConvert.SerializeObject(expectedMaze.edges);
            string neco = "{\"size\":10,\"startCell\":5,\"endCell\":6,\"edges\":" + edges + "}";
        }

        [Fact]
        public async Task GetMaze_UnsuccessfulRequest_ReturnsEmptyMaze()
        {
            int mazeId = 1;

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "mazes/1")
                .Respond(HttpStatusCode.InternalServerError);
            var httpClient = new HttpClient(mockHttp);
            var result = await MazeFetcher.getMaze(mazeId, httpClient);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetMazeCountForUser_SuccessfulRequest_ReturnsCount()
        {
            int userId = 1;
            int expectedCount = 5;

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "users/1/mcount")
                .Respond(HttpStatusCode.OK, "application/json", JsonConvert.SerializeObject(new MazeCountDTO { message = expectedCount }));

            var httpClient = new HttpClient(mockHttp);
            var result = await MazeFetcher.getMazeCountForUser(userId, httpClient);

            Assert.Equal(expectedCount, result);
        }

        [Fact]
        public async Task GetMazeCountForUser_UnsuccessfulRequest_ReturnsZero()
        {
            int userId = 1;
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "users/1/mcount")
                .Respond(HttpStatusCode.InternalServerError);
            var httpClient = new HttpClient(mockHttp);
            var result = await MazeFetcher.getMazeCountForUser(userId, httpClient);
            Assert.Equal(0, result);
        }


    }
}
