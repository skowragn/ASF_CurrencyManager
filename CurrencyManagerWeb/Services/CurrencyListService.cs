using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Query;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CurrencyManagerWeb.Interfaces;
using CurrencyManagerWeb.Models;
using AutoMapper;

namespace CurrencyManagerWeb.Services
{
    public class CurrencyListService : ICurrencyList
        {
           private readonly HttpClient _httpClient;
           private readonly FabricClient _fabricClient;
           private readonly string _reverseProxyBaseUri;
           private readonly StatelessServiceContext _serviceContext;
           private readonly IMapper _mapper;
           private readonly ICountryList _countryListService;

        public CurrencyListService(HttpClient httpClient, StatelessServiceContext context, FabricClient fabricClient,
                                   IMapper mapper, ICountryList countryListService)
           {
               _fabricClient = fabricClient;
               _httpClient = httpClient;
               _serviceContext = context;
               _reverseProxyBaseUri = Environment.GetEnvironmentVariable("ReverseProxyBaseUri");
               _mapper = mapper;
               _countryListService = countryListService;
        }

           public async Task<IEnumerable<KeyValuePair<string, int>>> GetAsync()
           {
               Uri serviceName = CurrencyManagerWeb.GetCurrencyManagerServiceName(_serviceContext);

               Uri proxyAddress = GetProxyAddress();

               ServicePartitionList partitions = await _fabricClient.QueryManager.GetPartitionListAsync(serviceName);

               List<KeyValuePair<string, int>> finalResult = new();

               foreach (Partition partition in partitions)
               {
                   var proxyUrl =
                       $"{proxyAddress}/api/Currency?PartitionKey={((Int64RangePartitionInformation) partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                       using HttpResponseMessage response = await _httpClient.GetAsync(proxyUrl);

                       if (response.StatusCode != HttpStatusCode.OK)
                       {
                           continue;
                       }

                       var currencyResult = JsonConvert.DeserializeObject<List<KeyValuePair<string, int>>>(
                               await response.Content.ReadAsStringAsync());

                       finalResult.AddRange(currencyResult);
               }

               return finalResult;
           }

           public async Task<CurrencyListViewModel> GetCurrenciesListViewModelInput()
           {
               var results = new CurrencyListViewModel();

               var countryList = await _countryListService.GetAsync();

               var items = countryList.Take(10).Select((item, counter) => new ItemList()
               {
                   CurrencyName = item.Currency.FirstOrDefault().Name,
                   Value = counter + 1
               }).ToList();

               results.CurrencyList = items;
               
            return results;
           }

           private async Task<List<KeyValuePair<string, CurrencyViewModel>>> GetCurrencyListViewModels(List<KeyValuePair<string, int>> result)
        {
            var finalCurrencyResults = new List<KeyValuePair<string, CurrencyViewModel>>();

            var countryList = await _countryListService.GetAsync();

            foreach (var currency in result)
            {
                var selectedCountry =
                    countryList.Where(item => item.Currency.FirstOrDefault().Name == currency.Key)
                               .Select(item => new KeyValuePair<string, CurrencyViewModel>(currency.Key, CreateCurrency(currency, item))).ToList();

                finalCurrencyResults.AddRange(selectedCountry);
            }

            return finalCurrencyResults;
        }
        
        public async Task<HttpStatusCode> PutAsync(string name)
        {
            Uri proxyAddress = GetProxyAddress();

            long partitionKey = GetPartitionKey(name);

            string proxyUrl = $"{proxyAddress}/api/Currency/{name}?PartitionKey={partitionKey}&PartitionKind=Int64Range";

            StringContent putContent = new StringContent($"{{ 'name' : '{name}' }}", Encoding.UTF8, "application/json");
            putContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (HttpResponseMessage response = await _httpClient.PutAsync(proxyUrl, putContent))
            {
                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> DeleteAsync(string name)
        {
            Uri proxyAddress = GetProxyAddress();

            long partitionKey = GetPartitionKey(name);

            string proxyUrl = $"{proxyAddress}/api/Currency/{name}?PartitionKey={partitionKey}&PartitionKind=Int64Range";

            using (HttpResponseMessage response = await _httpClient.DeleteAsync(proxyUrl))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return response.StatusCode;
                }
            }
            return HttpStatusCode.OK;
        }

        private static CurrencyViewModel CreateCurrency(KeyValuePair<string, int> basicCurrency, CountryViewModel country)
        {
            return new CurrencyViewModel()
            {
                CurrencyName = basicCurrency.Key,
                CurrencyFlag = country.Flag,
                CurrencyQuantity = basicCurrency.Value,
                CurrencySymbol = (country.Currency != null && country.Currency.Count() > 1) ? country.Currency.FirstOrDefault().Symbol : string.Empty
            };
        }

        private Uri GetProxyAddress()
        {
           Uri serviceName = CurrencyManagerWeb.GetCurrencyManagerServiceName(_serviceContext);
           Uri proxyAddress = new Uri($"{_reverseProxyBaseUri}{serviceName.AbsolutePath}");
           return proxyAddress;
        }

        private static long GetPartitionKey(string name)
        {
            return char.ToUpper(name.First()) - 'A';
        }
    }
}
