using MauiMaze.Models.DTOs;
using MauiMaze.Services;
using Moq;
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
    public class UserFetcherTests
    {

        [Fact]
        public async Task TryToLogin_ValidCredentials_ReturnsUserDataDTO()
        {
            const string email = "test@example.com";
            const string password = "password";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://10.0.0.23:8087/login")
                .Respond(HttpStatusCode.OK, "application/json", "{'id':1}"); 

            var httpClient = new HttpClient(mockHttp);

            var result = await UserComunicator.tryToLogin(email, password, httpClient);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task TryToLogin_ValidCredentials_FALSE()
        {
            string email = "test@example.com";
            string password = "password";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://10.0.0.23:8087/login")
                .Respond(HttpStatusCode.ServiceUnavailable, "application/json", "{'id':1}");

            var httpClient = new HttpClient(mockHttp);

            var result = await UserComunicator.tryToLogin(email, password, httpClient);

            Assert.Equal(-1,result.id);
        }


        [Fact]
        public async Task GetUsers_SuccessfulResponse_ReturnsUserDataArray()
        {
            string email = "test@example.com";
            string password = "password";
            var expectedUserData = new[] { new UserDataDTO(), new UserDataDTO() }; 

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress+"loadUsers")
                .Respond(HttpStatusCode.OK, "application/json", JsonConvert.SerializeObject(expectedUserData)); 

            var httpClient = new HttpClient(mockHttp);
            var result = await UserComunicator.getUsers(email, password,httpClient);

            Assert.NotNull(result);
            Assert.Equal(expectedUserData.Length, result.Length);
        }

        [Fact]
        public async Task GetUsers_UnsuccessfulResponse_ReturnsEmptyUserDataArray()
        {
            string email = "test@example.com";
            string password = "password";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "loadUsers")
                .Respond(HttpStatusCode.BadRequest); 


            var httpClient = new HttpClient(mockHttp);
            var result = await UserComunicator.getUsers(email, password, httpClient);

            Assert.NotNull(result);
            Assert.Empty(result);
        }


        [Fact]
        public async Task TryToRegister_SuccessfulRegistration_ReturnsZero()
        {
            string email = "test@example.com";
            string password = "password";
            string code = "12345678";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "register")
                .Respond(HttpStatusCode.OK); 

            var httpClient = new HttpClient(mockHttp);

            var result = await UserComunicator.tryToRegister(email, password,code, httpClient);
            Assert.Equal(0, result);
        }
        [Fact]
        public async Task TryToRegister_UnauthorizedAccess_ReturnsOne()
        {
            string email = "test@example.com";
            string password = "password";
            string code = "12345678";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "register")
                .Respond(HttpStatusCode.Unauthorized);

            var httpClient = new HttpClient(mockHttp);

            var result = await UserComunicator.tryToRegister(email, password, code, httpClient);
            Assert.Equal(1, result);
        }
        [Fact]
        public async Task TryToRegister_ServerError_ReturnsTwo()
        {
            string email = "test@example.com";
            string password = "password";
            string code = "12345678";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ServiceConfig.serverAdress + "register")
                .Respond(HttpStatusCode.InternalServerError);

            var httpClient = new HttpClient(mockHttp);

            var result = await UserComunicator.tryToRegister(email, password, code, httpClient);
            Assert.Equal(2, result);
        }

    }

}
