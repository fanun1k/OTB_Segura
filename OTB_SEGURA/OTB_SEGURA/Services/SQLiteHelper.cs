using OTB_SEGURA.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OTB_SEGURA.Services
{
    public class SQLiteHelper
    {
        SQLiteAsyncConnection db;

        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<UserModel>().Wait();
            db.CreateTableAsync<SessionModel>().Wait();
        }

        //Para guardar registros
        public Task<int> SaveUserAsync(UserModel user)
        {
            if (user.User_ID == 0)
            {
                return db.InsertAsync(user);
            }
            else
            {
                return null;
            }
        }

        //Para mostrar regisros
        public Task<List<UserModel>> GetUserAsync()
        {
            return db.Table<UserModel>().ToListAsync();
        }
        #region Session
        public async Task<int> SaveSession(SessionModel session)
        {
            try
            {
                db.DropTableAsync<SessionModel>().Wait();
                db.CreateTableAsync<SessionModel>().Wait();
                return await db.InsertAsync(session);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }          
        }
        public async Task DestroySession()
        {
            try
            {
                await db.DropTableAsync<SessionModel>();     
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        public Task<SessionModel> GetSession()
        {
            return db.Table<SessionModel>().FirstOrDefaultAsync();
        }
        #endregion
    }
}
