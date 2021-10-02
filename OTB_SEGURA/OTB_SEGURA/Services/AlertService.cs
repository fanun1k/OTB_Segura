using Newtonsoft.Json;
using OTB_SEGURA.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OTB_SEGURA.Services
{
    class AlertService : BaseRestFullApi<AlertModel>
    {
        string urlAlert = "restAlert";
        public async Task<ResponseHTTP<AlertModel>> listarAlertas(int idOtb)
        {
            try
            {
                return await GET(urlAlert +"/"+ idOtb);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ResponseHTTP<AlertModel>> insertarAlerta(AlertModel alert)
        {
            try
            {          
                var bodyRequest = new
                {
                   Date=alert.Date,
                   Longitude=alert.Longitude,
                   Latitude=alert.Latitude,
                   Message=alert.Message,
                   User_ID = int.Parse(Application.Current.Properties["User_ID"].ToString()),
                   Otb_ID = int.Parse(Application.Current.Properties["Otb_ID"].ToString()),
                   Alert_type_ID=alert.Alert_type_ID
            };

                string json = JsonConvert.SerializeObject(bodyRequest);
                return await POST(json, urlAlert);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseHTTP<AlertModel>> GetAlertsByUser(Nullable<int> idOtb,int idUser)
        {
            try
            { 
                if (idOtb==null)
                {
                    throw new Exception("El Otb_ID no puede ser nulo");
                }
                return await GET(urlAlert + $"/alertsbyuser/{idOtb}/{idUser}");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
