using System.Threading.Tasks;
using CountriesService.Dtos;
using System.Collections.Generic;

namespace CountriesService.Interfaces
{
    public interface ICountriesRepository
    {
        Task<IEnumerable<CountryDto>> GetCountriesAsync();
    }
}
