using MauiMaze.Models;
using MauiMaze.ViewModels;
namespace MazeUnitTests
{
    public class UserMenuTests
    {
        [Fact]
        public void OfflineUserTest()
        {
            UserMenuViewModel uvm = new UserMenuViewModel(LoginCases.Offline);
            Assert.Equal("Exit 🚪",uvm.ExitText);
            Assert.False(uvm.IsRecordsButtonVisible);
            Assert.False(uvm.IsDailyButtonVisible);
        }

        [Fact]
        public void OnlineUserTest()
        {
            UserMenuViewModel uvm = new UserMenuViewModel(LoginCases.Online);
            Assert.Equal("Logout 🚪", uvm.ExitText);
            Assert.True(uvm.IsRecordsButtonVisible);
            Assert.True(uvm.IsDailyButtonVisible);
        }

        //Facts are tests which are always true. They test invariant conditions.

        //Theories are tests which are only true for a particular set of data.
    }
}