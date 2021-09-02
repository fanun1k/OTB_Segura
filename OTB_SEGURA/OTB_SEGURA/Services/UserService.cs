using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OTB_SEGURA.Models;
using System.Net;

namespace OTB_SEGURA.Services
{
    class UserService: BaseRestFullApi<UserModel>
    {
        string urlApiUser = "restUser/";

        public async Task<ResponseHTTP<UserModel>> Login(UserModel user)
        {
            try
            {
               return await POST(user, urlApiUser + "login");
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }
    }
}
