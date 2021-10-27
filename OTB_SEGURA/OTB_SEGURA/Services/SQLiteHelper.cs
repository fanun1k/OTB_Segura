using OTB_SEGURA.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;

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
            db.CreateTableAsync<AlertModel>().Wait();
            db.CreateTableAsync<UserActivityModel>().Wait();
            db.CreateTableAsync<ImageProfileModel>().Wait();
            db.CreateTableAsync<OtbModel>().Wait();
            db.CreateTableAsync<AlertTypeModel>().Wait();
        }
        
        public async Task ClearTables()
        {
            await db.DropTableAsync<UserModel>();
            await db.DropTableAsync<SessionModel>();
            await db.DropTableAsync<AlertModel>();
            await db.DropTableAsync<UserActivityModel>();
            await db.DropTableAsync<ImageProfileModel>();
            await db.DropTableAsync<OtbModel>();
            await db.DropTableAsync<AlertTypeModel>();
        }
        
        /// <summary>
        /// Guardar lista de usuarios en la tabla del SQLite
        /// Tambien se realizan las acciones de DropTableAsync y CreateTableAsync para evitar errores
        /// la variable userList guarda la lista de usuarios
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        public async Task SaveUserAsync(List<UserModel> userList)
        {
            try
            {
                await db.DropTableAsync<UserModel>();
                await db.CreateTableAsync<UserModel>(); 
                foreach (var user in userList)
                {
                    await db.InsertAsync(user);
     
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Muestra los datos realizando un Get a la tabla UserModel
        /// para la posterior visualizacion
        /// </summary>
        /// <returns></returns>
        public Task<List<UserModel>> GetUserAsync()
        {
            return db.Table<UserModel>().ToListAsync();
        }

        /// <summary>
        /// Guardar lista de alertas en la tabla del SQLite
        /// Tambien se realizan las acciones de DropTableAsync y CreateTableAsync para evitar errores
        /// la variable alertList guarda la lista de alerts
        /// </summary>
        /// <param name="alertList"></param>
        /// <returns></returns>
        public async Task SaveAlertAsync(List<AlertModel> alertList)
        {
            try
            {
                await db.CreateTableAsync<AlertModel>();
                await db.DropTableAsync<AlertModel>();
                await db.CreateTableAsync<AlertModel>();
                foreach (var alert in alertList)
                {
                    await db.InsertAsync(alert);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Muestra los datos realizando un Get a la tabla AlertModel
        /// para la posterior visualizacion
        /// </summary>
        /// <returns></returns>
        public Task<List<AlertModel>> GetAlertAsync()
        {
            return db.Table<AlertModel>().ToListAsync();
        }

        /// <summary>
        /// Guardar lista de userActivity en la tabla del SQLite
        /// Tambien se realizan las acciones de DropTableAsync y CreateTableAsync para evitar errores
        /// la variable userActivityList guarda la lista de alerts
        /// </summary>
        /// <param name="userActivityList"></param>
        /// <returns></returns>
        public async Task SaveUserActivitytAsync(List<UserActivityModel> userActivityList)
        {
            try
            {
                await db.DropTableAsync<UserActivityModel>();
                await db.CreateTableAsync<UserActivityModel>();
                foreach (var userActivity in userActivityList)
                {
                    await db.InsertAsync(userActivity);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Muestra los datos realizando un Get a la tabla UserActivityModel
        /// para la posterior visualizacion
        /// </summary>
        /// <returns></returns>
        public Task<List<UserActivityModel>> GetUserActivitytAsync()
        {
            return db.Table<UserActivityModel>().ToListAsync();
        }

        /// <summary>
        /// Guardar lista de alertType en la tabla del SQLite
        /// Tambien se realizan las acciones de DropTableAsync y CreateTableAsync para evitar errores
        /// la variable alertTypeList guarda la lista de alerts
        /// </summary>
        /// <param name="alertTypeList"></param>
        /// <returns></returns>
        public async Task SaveAlertTypeAsync(List<AlertTypeModel> alertTypeList)
        {
            try
            {
                await db.DropTableAsync<AlertTypeModel>();
                await db.CreateTableAsync<AlertTypeModel>();
                foreach (var alertType in alertTypeList)
                {
                    await db.InsertAsync(alertType);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Muestra los datos realizando un Get a la tabla AlertTypeModel
        /// para la posterior visualizacion
        /// </summary>
        /// <returns></returns>
        public Task<List<AlertTypeModel>> GetAlertTypeAsync()
        {
            return db.Table<AlertTypeModel>().ToListAsync();
        }
        #region Session
        public async Task<int> SaveSession(SessionModel session)
        {
            try
            {
                await db.DropTableAsync<SessionModel>();
                await db.CreateTableAsync<SessionModel>();
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
        #region ImageProfile
        public async Task<int> SaveImageProfile(ImageProfileModel imgProfile)
        {
            try
            {
                await db.DropTableAsync<ImageProfileModel>();
                await db.CreateTableAsync<ImageProfileModel>();
                return await db.InsertAsync(imgProfile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ImageProfileModel> GetImageProfile()
        {
            return await db.Table<ImageProfileModel>().FirstOrDefaultAsync();
        }
        #endregion
        #region Otb
        public async Task<int> SaveMyOtb(OtbModel myOtb)
        {
            try
            {
                await db.DropTableAsync<OtbModel>();
                await db.CreateTableAsync<OtbModel>();
                return await db.InsertAsync(myOtb);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> GetMyOtb()
        {
            var otbName = await db.Table<OtbModel>().FirstOrDefaultAsync();
            return otbName.Name;
        }
        #endregion
    }
}
