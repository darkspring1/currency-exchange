using BussinesServices.Dto;
using BussinesServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrenciesController(IServiceProvider serviceProvider, ILogger<CurrenciesController> logger) : BaseController
    {
        private readonly ILogger<CurrenciesController> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string? id, CancellationToken cancellation)
        {
            var currencyService = _serviceProvider.GetRequiredService<CurrencyService>();
            var result = await currencyService.GetAsync(id, cancellation);

            return ServiceResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateCurrencyDto dto, CancellationToken cancellation)
        {
            var currencyService = _serviceProvider.GetRequiredService<CurrencyService>();
            var result = await currencyService.CreateAsync(dto, cancellation);

            return ServiceResult(result);
        }


        [HttpPost("exchange")]
        public async Task<ActionResult> Exchange(ExchangeRequestDto request, CancellationToken cancellationToken)
        {
            var exchangeService = _serviceProvider.GetRequiredService<ExchangeService>();
           
            var result = await exchangeService.ExchangeAsync(request, cancellationToken);

            return ServiceResult(result);
        }
    }
}
