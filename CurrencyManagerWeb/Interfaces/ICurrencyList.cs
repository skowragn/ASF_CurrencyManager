using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using CurrencyManagerWeb.Models;

namespace CurrencyManagerWeb.Interfaces
{
    public interface ICurrencyList
    {
        Task<IEnumerable<KeyValuePair<string, int>>> GetAsync();

        Task<HttpStatusCode> PutAsync(string name);

        Task<HttpStatusCode> DeleteAsync(string name);

        Task<CurrencyListViewModel> GetCurrenciesListViewModelInput();
    }
}
