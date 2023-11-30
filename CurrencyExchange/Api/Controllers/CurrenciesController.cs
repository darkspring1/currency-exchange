using BussinesServices.Dto;
using BussinesServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrenciesController(IServiceProvider serviceProvider, ILogger<CurrenciesController> logger) : BaseController(serviceProvider)
    {
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string? id, CancellationToken cancellation)
        {
            var result = await CurrencyService.GetAsync(id, cancellation);
            return ServiceResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateCurrencyDto dto, CancellationToken cancellation)
        {
            var result = await CurrencyService.CreateAsync(dto, cancellation);
            return ServiceResult(result);
        }


        [HttpPost("exchange")]
        public async Task<ActionResult> Exchange(ExchangeRequestDto request, CancellationToken cancellationToken)
        {
            var result = await ExchangeService.ExchangeAsync(request, cancellationToken);
            return ServiceResult(result);
        }

        private readonly ILogger<CurrenciesController> _logger = logger;
        private CurrencyService CurrencyService => GetRequired<CurrencyService>();
        private ExchangeService ExchangeService => GetRequired<ExchangeService>();
    }
}
