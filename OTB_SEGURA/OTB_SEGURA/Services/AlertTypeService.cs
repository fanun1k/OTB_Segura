using Newtonsoft.Json;
using OTB_SEGURA.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OTB_SEGURA.Services
{
    class AlertTypeService : BaseRestFullApi<AlertTypeModel>
    {
        private string urlAlertType = "restAlertType";
        public async Task<ResponseHTTP<AlertTypeModel>> AddAlertType(string name)
        {
            try
            {
                int otbID = int.Parse(Application.Current.Properties["Otb_ID"].ToString());
                var bodyRequest = new
                {
                    Name = name,
                    Otb_ID = otbID
                };
                string json = JsonConvert.SerializeObject(bodyRequest);
                return await POST(json, urlAlertType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseHTTP<AlertTypeModel>> GetAlertTypes()
        {
            try
            {
                int otbID = int.Parse(Application.Current.Properties["Otb_ID"].ToString());
                return await GET(urlAlertType+"/"+otbID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseHTTP<AlertTypeModel>> DeleteAlertType(int alertTypeId)
        {
            try
            {
                return await DELETE(urlAlertType + "/" + alertTypeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
