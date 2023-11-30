using BussinesServices.Dto;
using BussinesServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BalanceController(BalanceService balanceService, ILogger<UsersController> logger) : BaseController
    {
        private readonly ILogger<UsersController> _logger = logger;
        private readonly BalanceService _balanceService = balanceService;

        [HttpPut]
        public async Task<ActionResult> Put(BalanceRequestDto dto, CancellationToken cancellationToken)
        {
            var result = await _balanceService.CreateOrUpdateAsync(dto, cancellationToken);
            return ServiceResult(result);
        }


        [HttpGet]
        public async Task<ActionResult> Get(BalanceRequestDto dto, CancellationToken cancellationToken)
        {
            var result = await _balanceService.GetAsync(dto, cancellationToken);
            return ServiceResult(result);
        }

    }
}
