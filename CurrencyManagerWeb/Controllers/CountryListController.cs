using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CurrencyManagerWeb.Interfaces;

namespace CurrencyManagerWeb.Controllers
{
    public class CountryListController : Controller
    {
        private readonly ICountryList _countryListService;

        public CountryListController(ICountryList countryListService)
        {
            _countryListService = countryListService;
        }
        public async Task<ActionResult> Index()
        {
            return View(await _countryListService.GetAsync());
        }
    }
}