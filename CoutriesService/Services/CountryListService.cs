using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CountriesService.Dtos;
using CountriesService.Interfaces;

namespace CountriesService.Services
{
    public class CountryListService : ICountryList
    {
        private readonly HttpClient _httpClient;

        public CountryListService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CountryDto>> GetAsync()
        {

            var response = await _httpClient.GetAsync("https://restcountries.com/v3.1/all");

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");

            var content = await response.Content.ReadAsStringAsync();
            var countryList = JsonConvert.DeserializeObject<IEnumerable<CountryDto>>(content);
            return countryList;
        }

        public async Task<CountryDto> GetAsync(string countryName)
        {
            var response = await _httpClient.GetAsync($"https://restcountries.eu/rest/v2/name/{countryName}");

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException(
                    $"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");

            var content = await response.Content.ReadAsStringAsync();
            var countryList = JsonConvert.DeserializeObject<IEnumerable<CountryDto>>(content);
            return countryList != null ? countryList.FirstOrDefault() : new CountryDto();
        }
    }
}
