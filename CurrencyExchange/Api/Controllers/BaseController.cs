using BussinesServices.ServiceResult;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BaseController: ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public BaseController(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        protected T GetRequired<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        protected ActionResult ServiceResult<T>(IResult<T> result)
        {
            if (result.Error != null)
            {
                return BadRequest(result.Error);
            }

            if(result.Data == null)
            {
                return NotFound();
            }

            return Ok(result.Data);
        }
    }
}
