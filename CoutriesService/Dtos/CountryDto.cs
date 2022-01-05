using System.Collections.Generic;
using Newtonsoft.Json;

namespace CountriesService.Dtos
{
    public class CountryDto
    {
        [JsonProperty("name")]
        public NameType Name { get; set; }

        [JsonProperty("capital")]
        public string[] Capital { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("flags")]
        public Dictionary<string, string> FlagPath { get; set; }

        [JsonProperty("currencies")]
        public Dictionary<string, CurrenciesType> Currencies { get; set; }
    }

    public class NameType
    {
        [JsonProperty("common")]
        public string Common { get; set; }

        [JsonProperty("official")]
        public string Official { get; set; }
    }

    public class CurrenciesType
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}