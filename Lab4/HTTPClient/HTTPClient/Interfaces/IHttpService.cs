using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Interfaces
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task PostAsync(string endpoint, object data);
        Task PutAsync(string endpoint, object data);
        Task DeleteAsync(string endpoint);
    }
}
