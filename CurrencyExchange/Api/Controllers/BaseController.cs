using BussinesServices.ServiceResult;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BaseController: ControllerBase
    {
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
