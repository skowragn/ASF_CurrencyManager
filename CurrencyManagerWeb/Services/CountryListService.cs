using System;
using System.Collections.Generic;
using System.Linq;
using System.Fabric;
using System.Fabric.Query;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using CurrencyManagerWeb.Interfaces;
using CurrencyManagerWeb.Models;
using CurrencyManagerWeb.Dtos;
using AutoMapper;
using Newtonsoft.Json;

//using Flurl;
//using Flurl.Http;

namespace CurrencyManagerWeb.Services
{
    public class CountryListService : ICountryList
    {
        private readonly HttpClient _httpClient;
        private readonly FabricClient _fabricClient;
        private readonly string _reverseProxyBaseUri;
        private readonly StatelessServiceContext _serviceContext;
        private readonly IMapper _mapper;
       
        public CountryListService(HttpClient httpClient, StatelessServiceContext context, FabricClient fabricClient, IMapper mapper)
        {
            _fabricClient = fabricClient;
            _httpClient = httpClient;
            _serviceContext = context;
            _reverseProxyBaseUri = Environment.GetEnvironmentVariable("ReverseProxyBaseUri");
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryViewModel>> GetAsync()
        {
            Uri serviceName = CurrencyManagerWeb.GetCountriesServiceName(_serviceContext);

            Uri proxyAddress = GetProxyAddress();

            ServicePartitionList partitions = await _fabricClient.QueryManager.GetPartitionListAsync(serviceName);

            var finalResult = new List<CountryDto>();

            foreach (Partition partition in partitions)
            {
                var proxyUrl =
                    $"{proxyAddress}/api/Countries?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using HttpResponseMessage response = await _httpClient.GetAsync(proxyUrl);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    continue;
                }

                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var jsonString = await response.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeObject<IEnumerable<CountryDto>>(jsonString, jsonSettings);

                finalResult.AddRange(results);
            }

            var result = finalResult.Take(5).Select(countryDto => _mapper.Map<CountryViewModel>(countryDto));
            return result;
        }

        public async Task<IEnumerable<string>> GetCountryNamesAsync()
        {
            Uri proxyAddress = GetProxyAddress();

            var response = await _httpClient.GetAsync($"{ proxyAddress}/api/countries");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var countryList = JsonConvert.DeserializeObject<IEnumerable<CountryDto>>(content);

                var countries = countryList.Select(item => item.Name);
                //return countries;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async Task<CountryViewModel> GetCountryDetails(string countryName)
        {
            Uri proxyAddress = GetProxyAddress();

            var response = await _httpClient.GetAsync($"{proxyAddress}/api/countries/details?countryName={countryName}");
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var countryDto = JsonConvert.DeserializeObject<CountryDto>(content);

                //var country = countryDto.ToCountryViewModel();
                //return country;
                return new CountryViewModel();
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");

        }

        private Uri GetProxyAddress()
        {
            Uri serviceName = CurrencyManagerWeb.GetCountriesServiceName(_serviceContext);
            Uri proxyAddress = new Uri($"{_reverseProxyBaseUri}{serviceName.AbsolutePath}");
            return proxyAddress;
        }


        /* public async Task<IEnumerable<CountryViewModel>> GetAsync()
         {
             var countries = await $"{ _countryListBaseAddress}/api/countries"
                                    .AppendPathSegment("country")
                                    .GetJsonAsync<IEnumerable<CountryViewModel>>();

             return countries;
         }        

         public async Task<CountryViewModel> GetCountryDetails(string countryName)
         {
             var countries = await $"{ _countryListBaseAddress}/api/countries/details?countryName={countryName}"
                                    .AppendPathSegment("country")
                                    .GetJsonAsync<IEnumerable<CountryViewModel>>();

             return countries.FirstOrDefault();
         }

         public async Task<IEnumerable<string>> GetCountryNamesAsync()
         {
             var countries = await $"{ _countryListBaseAddress}/api/countries"
                                     .AppendPathSegment("country")
                                     .GetJsonAsync<IEnumerable<CountryDto>>();

             var countriesName = countries.Select(item => item.Name);

             return countriesName;
         }*/
    }

}