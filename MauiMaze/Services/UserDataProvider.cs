﻿using MauiMaze.Models.UserManagment;
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
        DateTime expireDate;
        //TODO add refresh token
        private bool IsExpired => checkExpiration();

        public void LogoutUser()
        {
            user = null;
            expireDate = DateTime.MinValue;
        }
        public async Task<bool> LoginUser(string name,string password)
        {
            int vysl=await UserComunicator.tryToLogin(name, password);
            if (vysl>0)
            {
                expireDate = DateTime.Now;
                expireDate.AddMonths(1);
                user = new User(vysl, "Admin");
                //TODO set with Token
                return true;
            }
            else
            {
                return false;
            }
            /*
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
            */
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
                        //TODO connect with UserFetcher
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
