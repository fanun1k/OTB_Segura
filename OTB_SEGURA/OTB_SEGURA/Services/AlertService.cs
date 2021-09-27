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
    }
}
