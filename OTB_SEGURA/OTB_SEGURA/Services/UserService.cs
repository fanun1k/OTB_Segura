using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OTB_SEGURA.Models;
using System.Net;
using System.Collections.Generic;
using System.IO;

namespace OTB_SEGURA.Services
{
    class UserService: BaseRestFullApi<UserModel>
    {
        string urlApiUser = "restUser";

        public async Task<ResponseHTTP<UserModel>> Login(string email,string password)
        {
            try
            {
                string urlLogin = urlApiUser+"/login";
                var bodyRequest = new
                {
                    Email=email,
                    Password=password
                };
                string json = JsonConvert.SerializeObject(bodyRequest);
               return await POST(json, urlLogin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }
        public async Task<ResponseHTTP<UserModel>> UsersByOtb(int otbId)
        {
            try
            {
                string urlGetUsersByOtb = urlApiUser + $"/byotb/{otbId}";
                return await GET(urlGetUsersByOtb);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ResponseHTTP<UserModel>> UserInsert(UserModel user)
        {
            try
            {
                string urlInsert = urlApiUser;
                var bodyRequest = new
                {
                    Name=user.Name,
                    Password=user.Password,
                    Cell_phone=user.Cell_phone,
                    Ci=user.Ci,
                    Email=user.Email
                };

                
                string json = JsonConvert.SerializeObject(bodyRequest);
                return await POST(json, urlInsert);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<ResponseHTTP<UserModel>> UserUpdate(UserModel user)
        {
            try
            {
                string urlUpdate = urlApiUser + $"/{user.User_ID}";

                var bodyRequest = new
                {
                    Name = user.Name,
                    Password = user.Password,
                    Cell_phone = user.Cell_phone
                };
                var json = JsonConvert.SerializeObject(bodyRequest);
                return await PUT(json, urlUpdate);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<ResponseHTTP<UserModel>> RecoveryPassword(string email, string ci)
        {
            try
            {
                string urlUpdate = urlApiUser + $"/recoverypass";
                var bodyRequest = new
                {
                    Email = email,
                    Ci = ci
                };
                string json = JsonConvert.SerializeObject(bodyRequest);
                return await POST(json, urlUpdate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseHTTP<UserModel>> SetAdmin(UserModel user)
        {
            try
            {
                string urlUpdate = urlApiUser + "/setadmin";
                var bodyRequest = new
                {
                    User_ID = user.User_ID
                };
                string json = JsonConvert.SerializeObject(bodyRequest);
                return await POST(json, urlUpdate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseHTTP<UserModel>> RemoveAdmin(UserModel user)
        {
            try
            {
                string urlUpdate = urlApiUser + "/removeadmin";
                var bodyRequest = new
                {
                    User_ID = user.User_ID
                };

                string json = JsonConvert.SerializeObject(bodyRequest);
                return await POST(json, urlUpdate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseHTTP<UserModel>> RemoveOTB(UserModel user)
        {
            try
            {
                string urlUpdate = urlApiUser + "/removeotb";
                var bodyRequest = new
                {
                    User_ID = user.User_ID
                };

                string json = JsonConvert.SerializeObject(bodyRequest);
                return await POST(json, urlUpdate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseHTTP<UserModel>> GetUser(int id)
        {
            try
            {
                string urlUpdate = urlApiUser + $"/{id}";
                return await GET(urlUpdate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseHTTP<UserModel>> UploadProfile(string id, Stream imgProfile)
        {
            try
            {
                string urlUpload = urlApiUser+ "/upload";

                HttpContent stringContent = new StringContent(id);
                HttpContent bytesContent = new StreamContent(imgProfile);

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(stringContent, "User_ID");
                    formData.Add(bytesContent, "Profile", "profile.png");

                    return await UPLOAD(formData, urlUpload);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserModel>> GetListImageProfile(List<UserModel> listUsers)
        {
            try
            {
                await Task.Run(() => {
                    
                    for (int i = 0; i < listUsers.Count; i++)
                    {
                        listUsers[i].Photo = urlserver +"uploads/"  + listUsers[i].User_ID + ".png";
                    }
                    
                });
                return listUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<UserModel> GetImageProfile(UserModel userModel)
        {
            try
            {
                await Task.Run(() => {
                    userModel.Photo = urlserver + "uploads/" + userModel.User_ID + ".png";
                });
                return userModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseHTTP<UserModel>> VerifyEmail(string email)
        {
            try
            {
                string urlInsert = urlApiUser + "/verifyEmail";
                var bodyRequest = new
                {
                    Email = email
                };

                string json = JsonConvert.SerializeObject(bodyRequest);
                return await POST(json, urlInsert);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
