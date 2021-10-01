using OTB_SEGURA.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OTB_SEGURA.Services
{
    class CameraService:BaseRestFullApi<CameraModel>
    {
        string urlService ="restCamera";
        public async Task<ResponseHTTP<CameraModel>> GetCameraList(int Otb_ID)
        {
            try {
                urlService = urlService + "/" + Otb_ID;
                return await GET(urlService);
            }catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
