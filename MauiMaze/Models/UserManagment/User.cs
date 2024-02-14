using MauiMaze.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models.UserManagment
{
    public class User : IDisposable
    {
        public int Id { get; set; }
        private readonly int role;
        public string Name { get; set; }

        public PersonalData personalData { get; set; }

        public User(int Id,string Name, int role = 0)
        {
            this.Id = Id;
            this.Name = Name;
            this.role = role;
        }
        public RoleEnum getRole()
        { 
            switch(role)
            {

            case 0:
                    return RoleEnum.User;
            case 1:
                    return RoleEnum.Reseacher;
            case 2:
                    return RoleEnum.Admin;
            
            }
            return RoleEnum.User;
        }
        public void Dispose()
        {
            
        }
    }
}
