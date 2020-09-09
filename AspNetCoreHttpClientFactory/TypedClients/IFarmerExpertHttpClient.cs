using AspNetCoreHttpClientFactory.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCoreHttpClientFactory.Core.TypedClients
{
    public interface IFarmerExpertHttpClient
    {
        Task<IList<CountryDTO.Response>> TypeClientList(CountryDTO.Request src);
    }
}
