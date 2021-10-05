using Newtonsoft.Json;
using OTB_SEGURA.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OTB_SEGURA.Services
{
    class BaseRestFullApi<T>
    {
        //string urlserver = "https://otbsegura.000webhostapp.com/otbapi/v3/";
        readonly protected string urlserver = "http://ec2-3-22-172-219.us-east-2.compute.amazonaws.com/otbapi/"; //
        ResponseHTTP<T> res = new ResponseHTTP<T>();

        protected async Task<ResponseHTTP<T>> POST(string json, string url)
        {
            try
            {   
                Uri RequestUri = new Uri(urlserver + url);
                var client = new HttpClient();

                var contJson = new StringContent(json, Encoding.UTF8, "application/json");
                
                if (Application.Current.Properties.ContainsKey("Token"))
                {
                    string token = Application.Current.Properties["Token"].ToString();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
                var response = await client.PostAsync(RequestUri, contJson);
                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    res=  JsonConvert.DeserializeObject<ResponseHTTP<T>>(jsonString);
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
        protected async Task<ResponseHTTP<T>> UPLOAD(MultipartFormDataContent formData, string actionUrl)
        {
            actionUrl = urlserver + actionUrl;
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(actionUrl, formData);
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
        }

        protected async Task<ResponseHTTP<T>> GET(string url)
        {
            try
            {
                Uri RequestUri = new Uri(urlserver + url);
                var client = new HttpClient();
                if (Application.Current.Properties.ContainsKey("Token"))
                {
                    string token = Application.Current.Properties["Token"].ToString();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
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
        protected async Task<ResponseHTTP<T>> PUT(string json, string url)
        {
            try
            {
                Uri RequestUri = new Uri(urlserver + url);
                var client = new HttpClient();
                var contJson = new StringContent(json, Encoding.UTF8, "application/json");
                if (Application.Current.Properties.ContainsKey("Token"))
                {
                    string token = Application.Current.Properties["Token"].ToString();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
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

        protected async Task<ResponseHTTP<T>> DELETE(string url)
        {
            try
            {
                Uri RequestUri = new Uri(urlserver + url);
                var client = new HttpClient();
                if (Application.Current.Properties.ContainsKey("Token"))
                {
                    string token = Application.Current.Properties["Token"].ToString();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                }
                var response = await client.DeleteAsync(RequestUri);

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
    }
}
