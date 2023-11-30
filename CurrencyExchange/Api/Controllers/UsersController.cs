using BussinesServices.Dto;
using BussinesServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(IServiceProvider serviceProvider, ILogger<UsersController> logger) : BaseController
    {
        private readonly ILogger<UsersController> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        [HttpPost]
        public async Task<ActionResult> Post(CreateUserRequestDto dto, CancellationToken cancellationToken)
        {

            var userService = _serviceProvider.GetRequiredService<UserService>();

            var result = await userService.CreateAsync(dto, cancellationToken);

            return ServiceResult(result);
        }

    }
}
