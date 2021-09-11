using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OTB_SEGURA.Models;
using System.Net;
using System.Collections.Generic;

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
                string urlGetUsersByOtb = urlApiUser + "/byotb/4";
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
    }
}
