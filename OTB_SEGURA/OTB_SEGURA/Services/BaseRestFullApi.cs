using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OTB_SEGURA.Models;
using System.Net;

namespace OTB_SEGURA.Services
{
    class BaseRestFullApi<T>
    {
        string urlApi = "https://otbsegura.000webhostapp.com/otbapi/v1/";

      
        public async Task<ResponseHTTP<T>> POST(T obj,string url)
        {
            try
            {
                Uri RequestUri = new Uri(urlApi +url);
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(obj);
                var contJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contJson);

                if (response.StatusCode==HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ResponseHTTP<T>>(jsonString);
                }
                else
                {
                    ResponseHTTP<T> res = new ResponseHTTP<T>();
                    res.Code = response.StatusCode;
                    if (res.Msj == null) res.Msj = response.StatusCode.ToString();
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
