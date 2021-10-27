using OTB_SEGURA.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OTB_SEGURA.Services
{
    class AlarmService:BaseRestFullApi<AlarmModel>
    {
        string urlService = "restAlarm";
        public async Task<ResponseHTTP<AlarmModel>> GetAlarmList(int Otb_ID)
        {
            try
            {
                urlService = urlService + "/" + Otb_ID;
                return await GET(urlService);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
