using System.Collections.Generic;
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
        private readonly ICountriesRepository _countriesRepository;

        public CountryListService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<IEnumerable<CountryDto>> GetAsync()
        {
            var countryList = await _countriesRepository.GetCountriesAsync();
            return countryList;
        }
    }
}
