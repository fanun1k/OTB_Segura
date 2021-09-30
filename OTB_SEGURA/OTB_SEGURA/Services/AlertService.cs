using Newtonsoft.Json;
using OTB_SEGURA.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<ResponseHTTP<AlertModel>> insertarAlerta(int idOtb)
        {
            try
            {
                string urlInsert = urlAlert;
                var bodyRequest = new
                {
                   
                };

                string json = JsonConvert.SerializeObject(bodyRequest);
                return await POST(json, urlInsert);
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
