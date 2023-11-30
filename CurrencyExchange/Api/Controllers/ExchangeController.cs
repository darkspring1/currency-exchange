using BussinesServices.Dto;
using BussinesServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeController(IServiceProvider serviceProvider, ILogger<ExchangeController> logger) : BaseController(serviceProvider)
    {
        [HttpPost]
        public async Task<ActionResult> Exchange(ExchangeRequestDto request, CancellationToken cancellationToken)
        {
            var result = await ExchangeService.ExchangeAsync(request, cancellationToken);
            return ServiceResult(result);
        }

        private readonly ILogger<ExchangeController> _logger = logger;
        private ExchangeService ExchangeService => GetRequired<ExchangeService>();
    }
}
