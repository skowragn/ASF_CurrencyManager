using Microsoft.AspNetCore.Mvc;
using CurrencyManagerWeb.Interfaces;
using System.Threading.Tasks;

namespace CurrencyManagerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyApiController : ControllerBase
    {
        private readonly ICurrencyList _currencyListService;

        public CurrencyApiController(ICurrencyList currencyListService)
        {
            _currencyListService = currencyListService;
        }


        // GET: api/CurrencyApi
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var results = await _currencyListService.GetAsync();
            return Ok(results);
        }

        // PUT: api/CurrencyApi/name
        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name)
        {
            var results = await _currencyListService.PutAsync(name);
            return Ok(results);
        }

        // DELETE: api/CurrencyApi/name
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var results = await _currencyListService.DeleteAsync(name);
            return Ok(results);
        }
    }
}
