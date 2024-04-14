using CommunityToolkit.Mvvm.DependencyInjection;
using MauiMaze;
using MauiMaze.Helpers;
using MauiMaze.Services;
using MauiMaze.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeUnitTests.UITests
{
    public class LoginTests
    {

        [Fact]
        public async Task TryToLogin_WithInternetAccess_UnsuccessfulLogin()
        {
            var viewModel = new LoginPageViewModel();

            await viewModel.tryToLogin(NetworkAccess.Internet);

            Assert.Equal("Zjistěna chyba při zjišťovaní stavu sítě", viewModel.ErrorMessage); 
            Assert.True(viewModel.OfflineButton); 
        }

        [Theory]
        [InlineData(NetworkAccess.Local)]
        [InlineData(NetworkAccess.None)]
        [InlineData(NetworkAccess.Unknown)]
        public async Task TryToLogin_WithoutInternetAccess(NetworkAccess networkAccess)
        {
            var viewModel = new LoginPageViewModel();
            await viewModel.tryToLogin(networkAccess);
            Assert.Equal("Nemáte přístup k internetu", viewModel.ErrorMessage); 
            Assert.True(viewModel.OfflineButton);
        }

        [Fact]
        public async Task TryToLogin_WithExceptionThrown()
        {

            var viewModel = new LoginPageViewModel();

            await viewModel.tryToLogin(NetworkAccess.Internet);

            Assert.Equal("Zjistěna chyba při zjišťovaní stavu sítě", viewModel.ErrorMessage); 
            Assert.True(viewModel.OfflineButton); 
        }
    }

}
