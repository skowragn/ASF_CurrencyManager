using System.Collections.Generic;
using CurrencyManagerWeb.Dtos;

namespace CurrencyManagerWeb.Models
{
    public class CountryViewModel : CountryNameViewModel
    {
        public string Capital { get; set; }

        public string Region { get; set; }

        public string Flag { get; set; }

        public IEnumerable<CurrenciesType> Currency { get; set; }
    }
    
}
