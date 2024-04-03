using MauiMaze.Engine;
using MauiMaze.Models.DTOs;
using MauiMaze.Services;
using MazeUnitTests.Mock;
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
    public class RecordFetcherTests
    {
        [Fact]
        public async Task SaveRecordOnline_SuccessfulRequest_ReturnsTrue()
        {
            GameRecord gr = new(0, 1);
            var gameRecord = gr.GetRecordDTO();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "saveRecord")
                .Respond(HttpStatusCode.OK);

            var httpClient = new HttpClient(mockHttp);

            var result = await RecordFetcher.SaveRecordOnline(gameRecord, httpClient);

            Assert.True(result);
        }

        [Fact]
        public async Task SaveRecordOnline_UnsuccessfulRequest_ReturnsFalse()
        {
            GameRecord gr = new(0, 1);
            var gameRecord = gr.GetRecordDTO();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "saveRecord")
                .Respond(HttpStatusCode.InternalServerError);

            var httpClient = new HttpClient(mockHttp);

            var result = await RecordFetcher.SaveRecordOnline(gameRecord, httpClient);

            Assert.False(result);
        }
        [Fact]
        public async Task LoadRecordsByUser_SuccessfulRequest_ReturnsGameRecords()
        {
            int userId = 1;
            var expectedRecords = new GameRecord[] { new GameRecord(0,1), new GameRecord(1,2) };

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "loadRecordByUser")
                .Respond(HttpStatusCode.OK, "application/json", JsonConvert.SerializeObject(expectedRecords));

            var httpClient = new HttpClient(mockHttp);

            var result = await RecordFetcher.loadRecordsByUser(userId, httpClient);

            Assert.NotNull(result);
            Assert.Equal(expectedRecords.Length, result.Length);
        }

        [Fact]
        public async Task LoadRecordsByUser_UnsuccessfulRequest_ReturnsEmptyArray()
        {
            int userId = 1;

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "loadRecordByUser")
                .Respond(HttpStatusCode.InternalServerError);

            var httpClient = new HttpClient(mockHttp);

            var result = await RecordFetcher.loadRecordsByUser(userId, httpClient);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task LoadRecordsByMaze_SuccessfulRequest_ReturnsGameRecords()
        {
            int mazeId = 1;
            var expectedRecords = new GameRecord[] { new GameRecord(0,1), new GameRecord(2,3) };

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "loadRecordByMaze")
                .Respond(HttpStatusCode.OK, "application/json", JsonConvert.SerializeObject(expectedRecords));

            var httpClient = new HttpClient(mockHttp);

            var result = await RecordFetcher.loadRecordsByMaze(mazeId, httpClient);

            Assert.NotNull(result);
            Assert.Equal(expectedRecords.Length, result.Length);
        }

        [Fact]
        public async Task LoadRecordsByMaze_UnsuccessfulRequest_ReturnsEmptyArray()
        {
            int mazeId = 1;

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "loadRecordByMaze")
                .Respond(HttpStatusCode.InternalServerError);

            var httpClient = new HttpClient(mockHttp);

            var result = await RecordFetcher.loadRecordsByMaze(mazeId, httpClient);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }


}
