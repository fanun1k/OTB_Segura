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

        public async Task<ResponseHTTP<UserModel>> Login(UserModel user)
        {
            try
            {
                string urlLogin = urlApiUser+"/login";
               return await POST(user, urlLogin);
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
                return await POST(user, urlInsert);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
