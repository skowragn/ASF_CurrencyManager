using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using CurrencyManagerService.Models;

namespace CurrencyManagerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly IReliableStateManager _stateManager;

        public CurrencyController(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        // GET api/currency
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ct = new CancellationToken();

            IReliableDictionary<string, int> currencyDictionary = await _stateManager.GetOrAddAsync<IReliableDictionary<string,int>>("currency");

            using var tx = _stateManager.CreateTransaction();
            {
                var list = await currencyDictionary.CreateEnumerableAsync(tx);

                var enumerator = list.GetAsyncEnumerator();

                List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();


                while (await enumerator.MoveNextAsync(ct))
                {
                    result.Add(enumerator.Current);
                }

                return Ok(result);
            }
        }

        // PUT api/currency/name
        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name)
        {
            var currencyDictionary = await _stateManager.GetOrAddAsync<IReliableDictionary<string, int>>("currency");

            using (var tx = _stateManager.CreateTransaction())
            {
                await currencyDictionary.AddOrUpdateAsync(tx, name, 1, (key, oldvalue) => oldvalue + 1);
                await tx.CommitAsync();
            }

            return new OkResult();
        }

        // DELETE api/currency/name
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var currencyDictionary = await _stateManager.GetOrAddAsync<IReliableDictionary<string, Currency>>("currency");

            using var tx = _stateManager.CreateTransaction();

            if (await currencyDictionary.ContainsKeyAsync(tx, name))
            {
                await currencyDictionary.TryRemoveAsync(tx, name);

                await tx.CommitAsync();
                
                return new OkResult();
            }

            return new NotFoundResult();
        }
    }
}
