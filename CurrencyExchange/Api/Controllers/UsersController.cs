using Api.Models;
using BussinesServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(IServiceProvider serviceProvider, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly ILogger<UsersController> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        [HttpPost]
        public async Task<ActionResult> Post(CreateUserRequestDto dto, CancellationToken cancellationToken)
        {

            var userService = _serviceProvider.GetRequiredService<UserService>();

            var result = await userService.CreateAsync(dto, cancellationToken);

            if (result.Error != null)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }


        [HttpGet("{userId:guid}/balance")]
        public decimal GetBalance(Guid userId)
        {
            return 1;
        }

        [HttpPut("{userId:guid}/balance")]
        public decimal UpdateBalance(Guid userId, [FromBody]UpdateBalanceRequest request)
        {
            return 1;
        }
    }
}
