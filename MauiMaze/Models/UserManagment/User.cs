using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models.UserManagment
{
    public class User :IDisposable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PersonalData personalData { get; set; }

        public User(int Id,string Name) {
            this.Id = Id;
            this.Name = Name;
        }
        public void Dispose()
        {
            
        }
    }
}
