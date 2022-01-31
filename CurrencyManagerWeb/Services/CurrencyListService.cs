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

namespace CurrencyManagerWeb.Services
{
    public class CurrencyListService : ICurrencyList
        {
           private readonly IHttpClientFactory _httpClientFactory;
           private readonly FabricClient _fabricClient;
           private readonly string _reverseProxyBaseUri;
           private readonly StatelessServiceContext _serviceContext;
           private readonly ICountryList _countryListService;
           private IEnumerable<CountryViewModel> _countriesList;

        public CurrencyListService(IHttpClientFactory httpClientFactory, StatelessServiceContext context, FabricClient fabricClient,
                                  ICountryList countryListService)
           {
               _fabricClient = fabricClient;
               _httpClientFactory = httpClientFactory;
               _serviceContext = context;
               _reverseProxyBaseUri = Environment.GetEnvironmentVariable("ReverseProxyBaseUri");
               _countryListService = countryListService;
               _countriesList = new List<CountryViewModel>();
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

                    var httpClient = _httpClientFactory.CreateClient();
                    using HttpResponseMessage response = await httpClient.GetAsync(proxyUrl);

                       if (!response.IsSuccessStatusCode)
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

               if(!_countriesList.Any())
                _countriesList = await _countryListService.GetAsync();

               var items = _countriesList.Take(10).Select((item, counter) => new ItemList()
               {
                   CurrencyName = item.Currency.FirstOrDefault().Name,
                   Value = counter + 1
               }).ToList();

               results.CurrencyList = items;
               
            return results;
           }

        public async Task<HttpStatusCode> PutAsync(string name)
        {
            Uri proxyAddress = GetProxyAddress();

            long partitionKey = GetPartitionKey(name);

            string proxyUrl = $"{proxyAddress}/api/Currency/{name}?PartitionKey={partitionKey}&PartitionKind=Int64Range";

            StringContent putContent = new($"{{ 'name' : '{name}' }}", Encoding.UTF8, "application/json");
            putContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpClient = _httpClientFactory.CreateClient();
            using HttpResponseMessage response = await httpClient.PutAsync(proxyUrl, putContent);
            return response.StatusCode;
        }

        public async Task<HttpStatusCode> DeleteAsync(string name)
        {
            Uri proxyAddress = GetProxyAddress();
            var partitionKey = GetPartitionKey(name);
            var proxyUrl = $"{proxyAddress}/api/Currency/{name}?PartitionKey={partitionKey}&PartitionKind=Int64Range";

            var httpClient = _httpClientFactory.CreateClient();
            using HttpResponseMessage response = await httpClient.DeleteAsync(proxyUrl);
            return response.IsSuccessStatusCode ? HttpStatusCode.OK : response.StatusCode;
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
