using System.Collections.Generic;
using System.Threading.Tasks;
using CountriesService.Dtos;

namespace CountriesService.Interfaces
{
    public interface ICountryList
    {
        Task<IEnumerable<CountryDto>> GetAsync();
        Task<CountryDto> GetAsync(string countryName);
    }
}