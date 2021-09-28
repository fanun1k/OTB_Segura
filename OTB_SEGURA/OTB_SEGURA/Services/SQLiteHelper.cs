using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using OTB_SEGURA.Models;
using System.Threading.Tasks;

namespace OTB_SEGURA.Services
{
    class SQLiteHelper
    {
        SQLiteAsyncConnection dataBase;

        public SQLiteHelper(string dbPath)
        {
            dataBase = new SQLiteAsyncConnection(dbPath);
            dataBase.CreateTableAsync<UserModel>().Wait();
        }

        //Para guardar registros
        public Task<int> SaveUserAsync (UserModel user)
        {
            if (user.User_ID == 0)
            {
                return dataBase.InsertAsync(user);
            }
            else
            {
                return null;
            }
        }

        //Para mostrar regisros
        public Task<List<UserModel>> GetUserAsync()
        {
            return dataBase.Table<UserModel>().ToListAsync();
        }

    }
}
