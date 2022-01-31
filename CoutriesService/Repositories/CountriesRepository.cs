using System.Net.Http;
using CountriesService.Dtos;
using CountriesService.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace CountriesService.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private static readonly string baseUrl = "https://restcountries.com/v3.1/all";
        private readonly IHttpClientFactory _httpClientFactory;

        public CountriesRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IEnumerable<CountryDto>> GetCountriesAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            using HttpResponseMessage response = await httpClient.GetAsync(baseUrl);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");

            var content = await response.Content.ReadAsStringAsync();
            var countryList = JsonConvert.DeserializeObject<IEnumerable<CountryDto>>(content);
            return countryList;
        }
    }
}
