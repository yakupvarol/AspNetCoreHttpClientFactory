using AspNetCoreHttpClientFactory.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreHttpClientFactory.Business
{
    public interface ICountryService
    {
        Task<IList<CountryDTO.Response>> BaseList(CountryDTO.Request src);
        Task<IList<CountryDTO.Response>> NamedClientsList(CountryDTO.Request src);
    }
}
