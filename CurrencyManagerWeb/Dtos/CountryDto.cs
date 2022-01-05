using System.Collections.Generic;

namespace CurrencyManagerWeb.Dtos
{
    public class CountryDto
    {
        public NameType Name { get; set; }
        public string[] Capital { get; set; }
        public string Region { get; set; }
        public Dictionary<string, string> FlagPath { get; set; }
        public Dictionary<string, CurrenciesType> Currencies { get; set; }
    }

    public class NameType
    {
        public string Common { get; set; }
        public string Official { get; set; }
    }

    public class CurrenciesType
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}