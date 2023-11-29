using Api.Models;
using BussinesServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrenciesController : ControllerBase
    {
        private readonly ILogger<CurrenciesController> _logger;

        public CurrenciesController(ILogger<CurrenciesController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public string[] Get()
        {
            var service = new ExchangeService();
            service.Change();
            return new [] { "USD", "EUR" };
        }


        [HttpPost("exchange")]
        public decimal Exchange(ExchangeRequest request)
        {
            var service = new ExchangeService();
            service.Change();
            return 1;
        }
    }
}
