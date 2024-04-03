using MauiMaze.Helpers;
using MauiMaze.Models;
using MauiMaze.Models.DTOs;
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
        public bool isUserValid =>checkUserValidity;
        User user;
        public string getUserName()
        {
            if (user is not null)
            {
                return user.Name;
            }
            else
            {
                return "OfflineUser";
            }
        }
        public int getUserID()
        {
            if (user is not null)
            {
                return user.Id;
            }
            return -1;
        }
        public LoginCases getLoginCase()
        {
            if (user is not null)
            {
                return LoginCases.Online;
            }
            return LoginCases.Offline;
        }
        public RoleEnum getUserRole()
        {
            if (user is not null)
            {
                return user.getRole();
            }
            return RoleEnum.User;
        }
        DateTime expireDate;
        private bool IsExpired => checkExpiration();

        public void LogoutUser()
        {
            user = null;
            expireDate = DateTime.MinValue;
        }
        public async Task<bool> LoginUser(string name,string password)
        {
            UserDataDTO vysl=await UserComunicator.tryToLogin(name, password);
            if (vysl.id>0)
            {
                expireDate = DateTime.Now;
                expireDate.AddMonths(1);
                user = new User(vysl.id, vysl.firstname,vysl.role);
                return true;
            }
            else
            {
                return false;
            }
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
        private bool checkUserValidity
        {
            get
            {
                if (user != null)
                {
                    if (!IsExpired)
                    {
                        return true;
                    }
                    else
                    {
                        Application.Current.MainPage.DisplayAlert("Upozornění", "expired ", "OK");
                    }
                }
                return false;
            }
        }

        public static UserDataProvider GetInstance()
        {
            instance ??= new UserDataProvider();
            return instance;
        }

        
    }

}
