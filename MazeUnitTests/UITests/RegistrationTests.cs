using MauiMaze.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeUnitTests.UITests
{
    public class RegistrationTests
    {
        [Theory]
        [InlineData("invalidemail", "password", "password1", "1234", "Email has bad format\nPasswords not match\nBad Code", false)]
        [InlineData("test@example.com", null, "password", "12345678", "Password is null\n", false)]
        [InlineData("test@example.com", "password", "password", null, "Bad Code", false)]
        public async Task RegisterInputVariationsErrorMessagesSetInCorrectly(string email, string password, string rePassword, string code, string expectedErrorMessage, bool expectLoading)
        {

            var viewModel = new RegisterPageViewModel()
            {
                Email = email,
                Password = password,
                RePassword = rePassword,
                Code = code,
                First ="first",
                Last = "last",
            };

            await viewModel.register();

            Assert.Equal(expectedErrorMessage, viewModel.ErrorMessage); 
            Assert.Equal(expectLoading, viewModel.Loading); 
           
        }
    }

}
