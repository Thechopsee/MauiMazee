using MauiMaze.Engine;
using MauiMaze.Helpers;
using Microsoft.VisualBasic;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Services
{
    public class MovesDatabase
    {
        SQLiteAsyncConnection Database;

        public MovesDatabase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

        }
       
    }
}
