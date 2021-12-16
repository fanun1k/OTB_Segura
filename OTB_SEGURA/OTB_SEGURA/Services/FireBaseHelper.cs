using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTB_SEGURA.Models;

namespace OTB_SEGURA.Services
{
    class FireBaseHelper
    {
        public static UserModel staticUser;
        public async Task<List<UserModel>> GetAllUsers()
        {

            return (await firebase
              .Child("Users")
              .OnceAsync<UserModel>()).Select(item => new UserModel
              {
                  UserId = item.Object.UserId,
                  Name = item.Object.Name,
                  UserName = item.Object.UserName,
                  Cell_phone = item.Object.Cell_phone,
                  State = item.Object.State,
                  Ci = item.Object.Ci,
                  Password = item.Object.Password,
                  Email = item.Object.Email

              }).ToList();
        }
        public async Task<List<ActivityModel>> GetAllActivitiesId(string id)
        {
            var allActivities = (await firebase
                .Child("Activity")
                .OnceAsync<ActivityModel>()).Select(item => new ActivityModel
                {
                    UserId = item.Object.UserId,
                    DateTime = item.Object.DateTime,
                    Type = item.Object.Type,
                    Message = item.Object.Message
                }).ToList();
            return allActivities.Where(a => a.UserId == id).OrderByDescending(dt => dt.DateTime).Take(5).ToList();
        }


        public async Task<UserModel> ValidateEmail(string email)
        {
            var usersEmail = (await firebase
                .Child("Users")
                .OnceAsync<UserModel>()).Select(item => new UserModel
                {
                    UserId=item.Object.UserId,
                    UserName=item.Object.UserName,
                    Password=item.Object.Password,
                    Cell_phone=item.Object.Cell_phone,
                    Name=item.Object.Name,
                    Ci=item.Object.Ci,
                    Type=item.Object.Type,
                    State = item.Object.State,
                    Email=item.Object.Email
                }).ToList();
            return usersEmail.Where(a => a.State == 1 && a.Email==email).FirstOrDefault();
        }

        // Metodo que obtiene la lista de actividades de los usuarios del sistema
        public async Task<List<UserActivityModel>> GetAllActivities()
        {
            List<UserActivityModel> userActivities = new List<UserActivityModel>();
            var allUsers = await GetAllUsers();
            var allActivities = (await firebase
                  .Child("Activity")
                  .OnceAsync<UserActivityModel>()).Select(item => new UserActivityModel
                  {
                      UserId = item.Object.UserId,
                      Message = item.Object.Message,
                      Type = item.Object.Type,
                      Latitude = item.Object.Latitude,
                      Longitude = item.Object.Longitude,
                      DateTime = item.Object.DateTime
                  }).ToList();

            var listAct = from x in allActivities
                          join allU in allUsers
                          on x.UserId equals allU.UserId
                          where allU.UserId == x.UserId
                          select new
                          {
                              allU.Name,
                              x.Message,
                              x.Type,
                              x.Latitude,
                              x.Longitude,
                              x.DateTime
                          };

            foreach (var item in listAct)
            {
                userActivities.Add(new UserActivityModel
                {
                    Message = item.Message,
                    Type = item.Type,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    DateTime = item.DateTime,
                    Name = item.Name
                });
            }
            return userActivities.OrderByDescending(dt => dt.DateTime).Take(10).ToList();
        }

        /*
        public async Task AddPerson(string name, int age)
        {
            await firebase
              .Child("Persons")
              .PostAsync(new UserModel() { PersonId = Guid.NewGuid(), NameField = name, AgeField = age});
        }
        */

        public async Task AddUser(UserModel userModel)
        {
            await firebase
            .Child("Users")
            .PostAsync(new UserModel()
            {
                UserId = Guid.NewGuid(),
                Name = userModel.Name,
                UserName = userModel.UserName,
                Password = userModel.Password,
                Cell_phone = userModel.Cell_phone,
                State = userModel.State,
                Photo = userModel.Photo,
                Ci = userModel.Ci,
                Type = 0,
                Email = userModel.Email

            });
        }
        private class res
        {
            public int Estado { get; set; }
        }
        //Metodo de insercion de actividades a firebase
        public async Task<int> EnableDisableAlarm()
        {
            try
            {
                var estado = (await firebase.Child("AlarmaPlaza").OnceAsync<res>()).FirstOrDefault();

                if (estado.Object.Estado == 1)
                {
                    await firebase
                    .Child("Activity")
                    .PutAsync(0);
                    await firebase.Child("AlarmaPlaza").Child("-Mc6C7YP8iuk7adm7Hc2").PutAsync(new res {Estado=0 });
                    return 0;
                }
                else
                {
                    await firebase
                   .Child("Activity")
                   .PutAsync(1);
                    await firebase.Child("AlarmaPlaza").Child("-Mc6C7YP8iuk7adm7Hc2").PutAsync(new res { Estado = 1 });
                    return 1;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
           
           
        }
        //Metodo de insercion de emergencias a firebase
        public async Task AddEmergency(EmergencyModel emergency)
        {
            await firebase
            .Child("Emergency")
            .PostAsync(new EmergencyModel()
            {
                DateTime = emergency.DateTime,
                Latitude = emergency.Latitude,
                Longitude = emergency.Longitude,
                UserId = emergency.UserId
            });
        }


        public async Task UpdateUser(UserModel userModel)
        {
            var toUpdatePerson = (await firebase
              .Child("Users")
              .OnceAsync<UserModel>()).Where(a => a.Object.UserId == userModel.UserId).FirstOrDefault();

            await firebase
              .Child("Users")
              .Child(toUpdatePerson.Key)
              .PutAsync(new UserModel() { UserId = userModel.UserId, Name = userModel.Name, UserName = userModel.UserName, Password = userModel.Password, Photo = userModel.Photo, Cell_phone = userModel.Cell_phone, State = userModel.State, Ci = userModel.Ci,Type=userModel.Type,Email=userModel.Email });
        }

        public async Task DisableUser(UserModel userModel)
        {
            var toUpdateUser = (await firebase
              .Child("Users")
              .OnceAsync<UserModel>()).Where(a => a.Object.UserId == userModel.UserId).FirstOrDefault();
            await firebase
              .Child("Users")
              .Child(toUpdateUser.Key)
              .PutAsync(new UserModel()
              {
                  UserId = userModel.UserId,
                  Name = userModel.Name,
                  UserName = userModel.UserName,
                  Password = userModel.Password,
                  Cell_phone = userModel.Cell_phone,
                  State = 0,
                  Photo = userModel.Photo,
                  Ci = userModel.Ci,
                  Email = userModel.Email
              });
        }
        public async Task EnableUser(UserModel userModel)
        {
            var toUpdateUser = (await firebase
              .Child("Users")
              .OnceAsync<UserModel>()).Where(a => a.Object.UserId == userModel.UserId).FirstOrDefault();
            await firebase
              .Child("Users")
              .Child(toUpdateUser.Key)
              .PutAsync(new UserModel()
              {
                  UserId = userModel.UserId,
                  Name = userModel.Name,
                  UserName = userModel.UserName,
                  Password = userModel.Password,
                  Cell_phone = userModel.Cell_phone,
                  State = 1,
                  Photo = userModel.Photo,
                  Ci = userModel.Ci,
                  Email = userModel.Email
              });
        }

        public async Task DeleteUser(Guid userId)
        {
            var toDeletePerson = (await firebase
              .Child("Users")
              .OnceAsync<UserModel>()).Where(a => a.Object.UserId == userId).FirstOrDefault();
            await firebase.Child("Users").Child(toDeletePerson.Key).DeleteAsync();

        }
        public async Task<List<UserModel>> LogGet()
        {

            return (await firebase
              .Child("Users")
              .OnceAsync<UserModel>()).Select(item => new UserModel
              {
                  UserId = item.Object.UserId,
                  Name = item.Object.Name,
                  UserName = item.Object.UserName,
                  Cell_phone = item.Object.Cell_phone,
                  State = item.Object.State,
                  Ci = item.Object.Ci,
                  Email = item.Object.Email,
                  Password = item.Object.Password,
                  Type = item.Object.Type,
             
              }).ToList();
        }


        public async Task<UserModel> GetPerson(string userName, string password)
        {
            var allPersons = await LogGet();
            await firebase
              .Child("Users")
              .OnceAsync<UserModel>();
            staticUser = allPersons.Where(a => a.UserName == userName && a.Password == password).FirstOrDefault();
            return allPersons.Where(a => a.UserName == userName && a.Password == password).FirstOrDefault();
        }
        public async Task<List<UserModel>> GetActiveUsers()
        {
            return (await firebase
              .Child("Users")
              .OnceAsync<UserModel>()).Select(item => new UserModel
              {
                  UserId = item.Object.UserId,
                  Name = item.Object.Name,
                  UserName = item.Object.UserName,
                  Cell_phone = item.Object.Cell_phone,
                  State = item.Object.State,
                  Ci = item.Object.Ci,
                  Email = item.Object.Email,
                  Password = item.Object.Password

              }).Where(item => item.State == 1).ToList();

        }
        /*
        public async Task<UserModel> GetPerson(int personId)
        {
            var allActivities = await GetAllPersons();
            await firebase
              .Child("Persons")
              .OnceAsync<UserModel>();
            return allActivities.Where(a => a.PersonId == personId).FirstOrDefault();
        }
        public async Task UpdatePerson(int personId, string name)
        {
            var toUpdatePerson = (await firebase
              .Child("Persons")
              .OnceAsync<UserModel>()).Where(a => a.Object.PersonId == personId).FirstOrDefault();
            await firebase
              .Child("Persons")
              .Child(toUpdatePerson.Key)
              .PutAsync(new UserModel() { PersonId = personId, NameField = name });
        }
        public async Task DeletePerson(int personId)
        {
            var toDeletePerson = (await firebase
              .Child("Persons")
              .OnceAsync<UserModel>()).Where(a => a.Object.PersonId == personId).FirstOrDefault();
            await firebase.Child("Persons").Child(toDeletePerson.Key).DeleteAsync();
        }
        */
        FirebaseClient firebase;

        public FireBaseHelper()
        {
            firebase = new FirebaseClient("https://proyecto-emergencia-default-rtdb.firebaseio.com/");
        }
    }
}
