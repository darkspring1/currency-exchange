using BussinesServices.Dto;
using BussinesServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(IServiceProvider serviceProvider, ILogger<UsersController> logger) : BaseController(serviceProvider)
    {
        [HttpPost]
        public async Task<ActionResult> Post(CreateUserRequestDto dto, CancellationToken cancellationToken)
        {
            var result = await UserService.CreateAsync(dto, cancellationToken);

            return ServiceResult(result);
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult> Get(Guid userId, CancellationToken cancellationToken)
        {
            var result = await UserService.GetAsync(userId, cancellationToken);
            return ServiceResult(result);
        }

        [HttpGet("{userId:guid}/balance/currencyId")]
        public async Task<ActionResult> GetBalance(Guid userId, string currencyId, CancellationToken cancellationToken)
        {
            var requestDto = new BalanceRequestDto { UserId = userId, CurrencyId = currencyId };
            var result = await BalanceService.GetAsync(requestDto, cancellationToken);

            return ServiceResult(result);
        }

        [HttpPut("{userId:guid}/balance/currencyId")]
        public async Task<ActionResult> UpdateBalnce(Guid userId, string currencyId, CreateBalanceDto dto, CancellationToken cancellationToken)
        {
            var requestDto = new CreateBalanceRequestDto { UserId = userId, CurrencyId = currencyId, Balance = dto.Balance };
            var result = await BalanceService.CreateOrUpdateAsync(requestDto, cancellationToken);
            return ServiceResult(result);
        }

        private UserService UserService => GetRequired<UserService>();
        private BalanceService BalanceService => GetRequired<BalanceService>();
        private readonly ILogger<UsersController> _logger = logger;

    }
}
