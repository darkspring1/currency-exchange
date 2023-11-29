using Api.Models;
using BussinesServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrenciesController(IServiceProvider serviceProvider, ILogger<CurrenciesController> logger) : ControllerBase
    {
        private readonly ILogger<CurrenciesController> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        [HttpGet]
        public string[] Get()
        {
            return new [] { "USD", "EUR" };
        }


        [HttpPost("exchange")]
        public async Task<ActionResult> Exchange(ExchangeRequestDto request)
        {
            var exchangeService = _serviceProvider.GetRequiredService<ExchangeService>();
           
            var result = await exchangeService.ExchangeAsync(request);

            if (result.Error != null)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }
    }
}
