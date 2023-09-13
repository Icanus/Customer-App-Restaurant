using FoodApp.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Diagnostics;

namespace FoodApp.Utilities
{
    public class JsonWebApiAgent : IJsonWebApiAgent
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        public async Task<T> SendGetAsyncRequest<T>(string actionUrl)
        {
            using (var client = new HttpClient())
            {
                var apiUrl = App.APIUrl + actionUrl;
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var contentStream = response.Content.ReadAsStreamAsync().Result;
                        var reader = new StreamReader(contentStream);
                        var jsonData = reader.ReadToEnd();

                        // The next line prevents issues related to 
                        // arguments being set to null that occurs when 
                        // peeking at the request stream content prior 
                        // to parameter binding process.
                        // https://blogs.msdn.microsoft.com/kiranchalla/2012/05/06/asp-net-mvc4-web-api-stack-diagram-currently-in-development/
                        contentStream.Position = 0;

                        if (!string.IsNullOrEmpty(jsonData))
                        {
                            if (jsonData.StartsWith("["))
                            {
                                var objectValue = JsonConvert.DeserializeObject<List<T>>(jsonData, settings);
                                return objectValue[0];
                            }
                            return JsonConvert.DeserializeObject<T>(jsonData, settings);
                        }
                    }
                }
                catch (Exception)
                {

                }

                throw new HttpRequestException("Failed GET request @ " + actionUrl);
            }
        }

        public async Task<T> SendPosAsyncRequest<T>(string actionUrl, object request)
        {
            //request.ServiceId = serviceId;
            //request.ApiSessionId = GetApiSessionId();

            using (var client = new HttpClient())
            {
                var useBaseApiUrl = App.APIUrl;
                client.BaseAddress = new Uri(useBaseApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = TimeSpan.FromMinutes(30);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonRequest = JsonConvert.SerializeObject(request);
                Debug.WriteLine(jsonRequest);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(actionUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var contentStream = response.Content.ReadAsStreamAsync().Result;
                    var reader = new StreamReader(contentStream);
                    var jsonData = reader.ReadToEnd();

                    // The next line prevents issues related to 
                    // arguments being set to null that occurs when 
                    // peeking at the request stream content prior 
                    // to parameter binding process.
                    // https://blogs.msdn.microsoft.com/kiranchalla/2012/05/06/asp-net-mvc4-web-api-stack-diagram-currently-in-development/
                    contentStream.Position = 0;

                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        return JsonConvert.DeserializeObject<T>(jsonData, settings);
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    //ShowMaintenancePopup();
                }

                throw new HttpRequestException("Failed POST request @ " + App.APIUrl + actionUrl);
            }
        }

        public T SendGetRequest<T, U>(string actionUrl, U request)
        {
            //request.ServiceId = serviceId;
            //request.ApiSessionId = GetApiSessionId();

            using (var client = new HttpClient())
            {
                var useBaseApiUrl = App.APIUrl;
                client.BaseAddress = new Uri(useBaseApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonRequest = JsonConvert.SerializeObject(request);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");


                var response = client.PostAsync(actionUrl, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var contentStream = response.Content.ReadAsStreamAsync().Result;
                    var reader = new StreamReader(contentStream);
                    var jsonData = reader.ReadToEnd();

                    // The next line prevents issues related to 
                    // arguments being set to null that occurs when 
                    // peeking at the request stream content prior 
                    // to parameter binding process.
                    // https://blogs.msdn.microsoft.com/kiranchalla/2012/05/06/asp-net-mvc4-web-api-stack-diagram-currently-in-development/
                    contentStream.Position = 0;

                    return JsonConvert.DeserializeObject<T>(jsonData, settings);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    //ShowMaintenancePopup();
                }

                throw new HttpRequestException("Failed Get request @ " + App.APIUrl + actionUrl);
            }
        }

        public async Task<List<T>> SendGetAllAsyncRequest<T>(string actionUrl)
        {
            using (var client = new HttpClient())
            {
                var apiUrl = App.APIUrl + actionUrl;
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                   var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var contentStream = response.Content.ReadAsStreamAsync().Result;
                        var reader = new StreamReader(contentStream);
                        var jsonData = reader.ReadToEnd();

                        // The next line prevents issues related to 
                        // arguments being set to null that occurs when 
                        // peeking at the request stream content prior 
                        // to parameter binding process.
                        // https://blogs.msdn.microsoft.com/kiranchalla/2012/05/06/asp-net-mvc4-web-api-stack-diagram-currently-in-development/
                        contentStream.Position = 0;
                        if (!string.IsNullOrEmpty(jsonData))
                        {
                            return JsonConvert.DeserializeObject<List<T>>(jsonData, settings);
                        }
                    }
                }
                catch (Exception)
                {

                }

                throw new HttpRequestException("Failed GET request @ " + actionUrl);
            }
        }

        public async Task<T> SendPutAsyncRequest<T>(string actionUrl, object request)
        {
            using (var client = new HttpClient())
            {
                var useBaseApiUrl = App.APIUrl;
                client.BaseAddress = new Uri(useBaseApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonRequest = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(actionUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var contentStream = response.Content.ReadAsStreamAsync().Result;
                    var reader = new StreamReader(contentStream);
                    var jsonData = reader.ReadToEnd();

                    // The next line prevents issues related to 
                    // arguments being set to null that occurs when 
                    // peeking at the request stream content prior 
                    // to parameter binding process.
                    // https://blogs.msdn.microsoft.com/kiranchalla/2012/05/06/asp-net-mvc4-web-api-stack-diagram-currently-in-development/
                    contentStream.Position = 0;
                    return JsonConvert.DeserializeObject<T>(jsonData, settings);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    //ShowMaintenancePopup();
                }

                throw new HttpRequestException("Failed PUT request @ " + App.APIUrl + actionUrl);
            }
        }

        public async Task<T> SendDeleteAsyncRequest<T>(string actionUrl)
        {
            using (var client = new HttpClient())
            {
                var useBaseApiUrl = App.APIUrl;
                client.BaseAddress = new Uri(useBaseApiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.DeleteAsync(actionUrl);

                if (response.IsSuccessStatusCode)
                {
                    var contentStream = response.Content.ReadAsStreamAsync().Result;
                    var reader = new StreamReader(contentStream);
                    var jsonData = reader.ReadToEnd();

                    // The next line prevents issues related to 
                    // arguments being set to null that occurs when 
                    // peeking at the request stream content prior 
                    // to parameter binding process.
                    // https://blogs.msdn.microsoft.com/kiranchalla/2012/05/06/asp-net-mvc4-web-api-stack-diagram-currently-in-development/
                    contentStream.Position = 0;
                    return JsonConvert.DeserializeObject<T>(jsonData, settings);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    //ShowMaintenancePopup();
                }

                throw new HttpRequestException("Failed PUT request @ " + App.APIUrl + actionUrl);
            }
        }

        public async Task<T> SendGetAsyncRequestMaps<T>(string actionUrl)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(string.Format(App.googleApiUrl));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(actionUrl);

                if (response.IsSuccessStatusCode)
                {
                    var contentStream = response.Content.ReadAsStreamAsync().Result;
                    var reader = new StreamReader(contentStream);
                    var jsonData = reader.ReadToEnd();

                    // The next line prevents issues related to 
                    // arguments being set to null that occurs when 
                    // peeking at the request stream content prior 
                    // to parameter binding process.
                    // https://blogs.msdn.microsoft.com/kiranchalla/2012/05/06/asp-net-mvc4-web-api-stack-diagram-currently-in-development/
                    contentStream.Position = 0;

                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        return JsonConvert.DeserializeObject<T>(jsonData, settings);
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    //ShowMaintenancePopup();
                }

                throw new HttpRequestException("Failed POST request @ " + App.googleApiUrl + actionUrl);
            }
        }
    }
}
