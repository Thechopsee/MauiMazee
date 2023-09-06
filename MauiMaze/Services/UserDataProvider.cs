using MauiMaze.Models.UserManagment;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Services
{
    public class UserDataProvider
    {
        private static UserDataProvider instance;
        public bool isUserValid =>checkUserValidity();
        User user;
        public string getUserName()
        {
            return user.Name;
        }
        DateTime expireDate;
        //TODO add refresh token
        private bool isExpired => checkExpiration();

        public void LogoutUser()
        {
            user.Dispose();
            expireDate = DateTime.MinValue;
        }
        public bool LoginUser(string name,string password)
        {
            if (name == "admin" && password == "admin")
            {
                expireDate = DateTime.Now;
                expireDate.AddMonths(1);
                user = new User(1,"Admin");
                //TODO set with Token
                return true;
            }
            else
            {
                return false;
            }
            //TODO connect with UserFetcher
        }

        private UserDataProvider()
        {
            
        }
        private bool checkExpiration() {
            if (DateTime.Compare(expireDate,DateTime.Now)<0)
            {
                return false;
            }
                return true;
        }
        private bool checkUserValidity() {
            if (user != null )
            {
                if (!isExpired)
                {
                    return true;
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Upozornění", "expired ", "OK");
                    //TODO connect with UserFetcher
                }
            }
            return false;
        }
        public static UserDataProvider GetInstance()
        {
            if (instance == null)
            {
                instance = new UserDataProvider();
            }
            return instance;
        }

        
    }

}
