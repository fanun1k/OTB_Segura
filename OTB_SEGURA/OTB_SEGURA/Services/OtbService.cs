using Newtonsoft.Json;
using OTB_SEGURA.Models;
using System.Threading.Tasks;

namespace OTB_SEGURA.Services
{
    class OtbService : BaseRestFullApi<OtbModel>
    {
        readonly string urlApiOtb = "/restOtb";
        public async Task<ResponseHTTP<OtbModel>> CreateOtb(string name,int userId)
        {
            try
            {
                var bodyRequest = new
                {
                    Name=name,
                    User_ID=userId
                };

                string json = JsonConvert.SerializeObject(bodyRequest);
                return await POST(json, urlApiOtb);
               
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ResponseHTTP<OtbModel>> JoinOtb(int id,string otbCode)
        {
            try
            {
                string urlJoin = urlApiOtb + "/joinOtb";

                var bodyRequest = new
                {
                   User_ID=id,
                   Code=otbCode
                };
                string json=JsonConvert.SerializeObject(bodyRequest);

                return await POST(json, urlJoin);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ResponseHTTP<OtbModel>> GetOtb(int otbId)
        {
            try
            {
                string urlGetUsersByOtb = urlApiOtb + $"/{otbId}";
                return await GET(urlGetUsersByOtb);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
    }
}
