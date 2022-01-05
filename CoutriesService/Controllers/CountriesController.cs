using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CountriesService.Interfaces;
using Microsoft.ServiceFabric.Data;
using CountriesService.Dtos;

namespace CountriesService.Controllers
{
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryList _countryListService;

        public CountriesController(ICountryList countryListService)
        {
            _countryListService = countryListService;
        }

        [HttpGet]
        public async Task<IEnumerable<CountryDto>> GetAsync()
        {
            return await _countryListService.GetAsync();
        }

    }
}