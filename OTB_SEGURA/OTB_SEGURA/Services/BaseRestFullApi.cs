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
        //readonly string urlserver = "http://ec2-18-224-252-198.us-east-2.compute.amazonaws.com/otbapi/v1/";
        ResponseHTTP<T> res = new ResponseHTTP<T>();

        protected async Task<ResponseHTTP<T>> POST(string json, string url)
        {
            try
            {
                Uri RequestUri = new Uri(urlserver + url);
                var client = new HttpClient();
                var contJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contJson);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    res= await JsonConvert.DeserializeObject<ResponseHTTP<T>>(jsonString);
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
        protected async Task<ResponseHTTP<T>> GET(string url)
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
