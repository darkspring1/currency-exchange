using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
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
