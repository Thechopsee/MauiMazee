using Castle.Core.Smtp;
using MauiMaze;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeUnitTests.UITests
{
    public class BugReportTests
    {
        [Theory]
        [InlineData("valid@email.com", true)]
        [InlineData("invalidemail.com", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsValidEmailValidatesEmail(string email, bool expected)
        {
            var page = new ReportBugPage();
            var isValid = page.IsValidEmail(email);
            Assert.Equal(expected, isValid);
        }
    }
}
