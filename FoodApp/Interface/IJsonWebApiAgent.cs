using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodApp.Interface
{
    public interface IJsonWebApiAgent
    {
        Task<T> SendGetAsyncRequest<T>(string actionUrl);
        Task<List<T>> SendGetAllAsyncRequest<T>(string actionUrl);
        Task<T> SendPosAsyncRequest<T>(string actionUrl, object request);
        Task<T> SendPutAsyncRequest<T>(string actionUrl, object request);
        Task<T> SendDeleteAsyncRequest<T>(string actionUrl);
        Task<T> SendGetAsyncRequestMaps<T>(string actionUrl);
    }
}
