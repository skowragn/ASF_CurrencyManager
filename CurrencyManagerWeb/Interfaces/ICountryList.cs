using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyManagerWeb.Models;

namespace CurrencyManagerWeb.Interfaces
{
    public interface ICountryList
    {
        Task<IEnumerable<CountryViewModel>> GetAsync();

        Task<CountryViewModel> GetCountryDetails(string countryName);

        Task<IEnumerable<string>> GetCountryNamesAsync();
    }
}
