﻿using Firebase.Database;
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
        public async Task<List<UserModel>> GetAllUsers()
        {

            return (await firebase
              .Child("Users")
              .OnceAsync<UserModel>()).Select(item => new UserModel
              {
                  UserId=item.Object.UserId,
                  Name = item.Object.Name,
                  UserName = item.Object.UserName,
                  Phone = item.Object.Phone,
                  State=item.Object.State,
                  Ci=item.Object.Ci,
                  Password=item.Object.Password
                  
              }).ToList();
        }

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
            return userActivities.ToList();
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
                Phone=userModel.Phone,
                State=userModel.State,
                Photo = userModel.Photo,
                Ci=userModel.Ci              
            });
        }

        public async Task AddActivity(ActivityModel activity)
        {
            await firebase
            .Child("Activity")
            .PostAsync(new ActivityModel()
            {
                DateTime = activity.DateTime,
                Latitude = activity.Latitude,
                Longitude = activity.Longitude,
                Message = activity.Message,
                Type = activity.Type,
                UserId = activity.UserId
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
              .PutAsync(new UserModel() { UserId = userModel.UserId, Name = userModel.Name, UserName = userModel.UserName, Password=userModel.Password,Photo=userModel.Photo,Phone=userModel.Phone,State=userModel.State,Ci=userModel.Ci });
        }

        public async Task DisableUser(UserModel userModel)
        {
            var toUpdateUser = (await firebase
              .Child("Users")
              .OnceAsync<UserModel>()).Where(a => a.Object.UserId == userModel.UserId).FirstOrDefault();
            await firebase
              .Child("Users")
              .Child(toUpdateUser.Key)
              .PutAsync(new UserModel() {
                  UserId = userModel.UserId,
                  Name = userModel.Name,
                  UserName = userModel.UserName,
                  Password = userModel.Password,
                  Phone = userModel.Phone,
                  State = 0,
                  Photo = userModel.Photo,
                  Ci = userModel.Ci
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
                  Phone = userModel.Phone,
                  State = 1,
                  Photo = userModel.Photo,
                  Ci = userModel.Ci
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
                  UserId=item.Object.UserId,
                  Name = item.Object.Name,
                  UserName = item.Object.UserName,
                  Phone = item.Object.Phone,
                  State = item.Object.State,
                  Ci = item.Object.Ci,
                  Password = item.Object.Password
              }).ToList();
        }


        public async Task<UserModel> GetPerson(string userName, string password)
        {
            var allPersons = await LogGet();
            await firebase
              .Child("Users")
              .OnceAsync<UserModel>();
            return allPersons.Where(a => a.UserName == userName && a.Password==password).FirstOrDefault();
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
                  Phone = item.Object.Phone,
                  State = item.Object.State,
                  Ci = item.Object.Ci,
                  Password = item.Object.Password

              }).Where(item => item.State == 1).ToList();

        }
        /*
        public async Task<UserModel> GetPerson(int personId)
        {
            var allPersons = await GetAllPersons();
            await firebase
              .Child("Persons")
              .OnceAsync<UserModel>();
            return allPersons.Where(a => a.PersonId == personId).FirstOrDefault();
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
