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

namespace CurrencyManagerWeb.Services
{
    public class CountryListService : ICountryList
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FabricClient _fabricClient;
        private readonly string _reverseProxyBaseUri;
        private readonly StatelessServiceContext _serviceContext;
        private readonly IMapper _mapper;

        public CountryListService(IHttpClientFactory httpClientFactory, StatelessServiceContext context, FabricClient fabricClient, IMapper mapper)
        {
            _fabricClient = fabricClient;
            _httpClientFactory = httpClientFactory;
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

                var httpClient = _httpClientFactory.CreateClient();
                using HttpResponseMessage response = await httpClient.GetAsync(proxyUrl);

                if (!response.IsSuccessStatusCode)
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

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{ proxyAddress}/api/countries");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var countryList = JsonConvert.DeserializeObject<IEnumerable<CountryDto>>(content);

                var countries = countryList.Select(item => item.Name.Common).ToList();
                return countries;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async Task<CountryViewModel> GetCountryDetails(string countryName)
        {
            Uri proxyAddress = GetProxyAddress();

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{proxyAddress}/api/countries/details?countryName={countryName}");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var countryDto = JsonConvert.DeserializeObject<CountryDto>(content);

                var country = _mapper.Map<CountryViewModel>(countryDto);
                return country;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");

        }

        private Uri GetProxyAddress()
        {
            Uri serviceName = CurrencyManagerWeb.GetCountriesServiceName(_serviceContext);
            Uri proxyAddress = new Uri($"{_reverseProxyBaseUri}{serviceName.AbsolutePath}");
            return proxyAddress;
        }
    }

}