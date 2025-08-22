using HTTPClient.Constants;
using HTTPClient.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(AppConstants.BASE_URL + endpoint);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public async Task PostAsync(string endpoint, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, AppConstants.CONTENT_TYPE);
            var response = await _httpClient.PostAsync(AppConstants.BASE_URL + endpoint, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task PutAsync(string endpoint, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, AppConstants.CONTENT_TYPE);
            var response = await _httpClient.PutAsync(AppConstants.BASE_URL + endpoint, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(AppConstants.BASE_URL + endpoint);
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
