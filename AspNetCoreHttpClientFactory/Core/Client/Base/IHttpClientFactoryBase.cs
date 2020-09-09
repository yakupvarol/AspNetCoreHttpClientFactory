using AspNetCoreHttpClientFactory.Core.Client.DTO;
using System.Threading.Tasks;

namespace AspNetCoreHttpClientFactory.Core.Client.Base
{
    public interface IHttpClientFactoryBase
    {
        Task<T> GetAsync<T>(HttpClientFactoryDTO.GetAsync src);
        Task<T> PostAsync<T>(HttpClientFactoryDTO.PostAsync src);
        Task<T> SendAsync<T>(HttpClientFactoryDTO.SendAsync dt);
    }
}
