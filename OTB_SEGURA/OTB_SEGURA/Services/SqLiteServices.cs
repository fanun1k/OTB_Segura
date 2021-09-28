using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using OTB_SEGURA.Models;
using System.Linq;

namespace OTB_SEGURA.Services
{
    class SqLiteServices
    {
        private SQLiteConnection connection; 
        private static SqLiteServices instance;
        public string MessageStatus; //Mensaje de verificacion

        private SQLiteAsyncConnection connection2;

        public static SqLiteServices Instance
        {
            get
            {
                try
                {
                    if (instance == null)
                    {
                        throw new Exception("Debe llamar al inicializador");
                    }
                    return instance;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //Iicializador nuestra instancia
        public static void Initializer(string filename)
        {
            try
            {
                if (filename == null)
                {
                    throw new ArgumentNullException();
                }

                if (instance != null) //Si es diferente a nulo es porque existe una conexion
                {
                    instance.connection.Close(); //cerramos la conexion
                }

                instance = new SqLiteServices(filename);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SqLiteServices(string dbPath) //Video min 8:10
        {
            try
            {
                connection = new SQLiteConnection(dbPath); //Ruta de la base de datos

                connection.CreateTable<UserModel>(); //Creamos la tabla
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Insert
        public int InsertEmergencyNumber(Guid userId, string name, string userName, int cellPhone, byte state, int ci, string email, string password)
        {
            int result = 0;
            try
            {
                result = connection.Insert(new UserModel()
                {
                    UserId = userId,
                    Name = name,
                    UserName = userName,
                    Cell_phone = cellPhone,
                    State = state,
                    Ci = ci,
                    Email = email,
                    Password = password,

                });
                MessageStatus = string.Format("Cantidad de filas afectadas: {0}", result);
            }
            catch (Exception ex)
            {
                MessageStatus = ex.Message;
            }
            return result;
        }
        //Select
        public IEnumerable<UserModel> GetAllNumbersEmergency()
        {
            try
            {
                return connection.Table<UserModel>();
            }
            catch (Exception ex)
            {
                MessageStatus = ex.Message;
            }
            return Enumerable.Empty<UserModel>();
        }
    }
}
