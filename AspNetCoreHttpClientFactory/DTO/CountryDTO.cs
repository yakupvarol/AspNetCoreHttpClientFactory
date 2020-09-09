using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreHttpClientFactory.DTO
{
    public class CountryDTO
    {
        public class Request
        {
            public int lngid { get; set; }
        }

        public class Response
        {
            public int SATIRNO { get; set; }
            public string ULKEKODU { get; set; }
            public string ACIKLAMA { get; set; }
            public string TELKODU { get; set; }
            public string TELFORMAT { get; set; }
            public string TELUZUNLUK { get; set; }
            public string KIMLIKUZUNLUK { get; set; }
            public int VERUZUNLUK { get; set; }
        }

    }
}
