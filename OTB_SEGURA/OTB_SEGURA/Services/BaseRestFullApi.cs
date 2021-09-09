using Newtonsoft.Json;
using OTB_SEGURA.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OTB_SEGURA.Services
{
    class BaseRestFullApi<T>
    {
        string urlserver = "https://otbsegura.000webhostapp.com/otbapi/v3/";
        ResponseHTTP<T> res = new ResponseHTTP<T>();


        public async Task<ResponseHTTP<T>> POST(T obj, string url)
        {
            try
            {
                Uri RequestUri = new Uri(urlserver + url);
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(obj);
                var contJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contJson);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    res = JsonConvert.DeserializeObject<ResponseHTTP<T>>(jsonString);
                    return res;
                }
                else
                {
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
        public async Task<ResponseHTTP<T>> GET(string url)
        {
            try
            {
                Uri RequestUri = new Uri(urlserver + url);
                var client = new HttpClient();
                var response = await client.GetAsync(RequestUri);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ResponseHTTP<T>>(jsonString);
                }
                else
                {
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
        public async Task<ResponseHTTP<T>> PUT(T obj, string url)
        {
            try
            {
                Uri RequestUri = new Uri(urlserver + url);
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(obj);
                var contJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(RequestUri, contJson);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    res = JsonConvert.DeserializeObject<ResponseHTTP<T>>(jsonString);
                    return res;
                }
                else
                {
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
