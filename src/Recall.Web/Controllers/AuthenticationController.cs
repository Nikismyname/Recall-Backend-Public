using Microsoft.AspNetCore.Mvc;
using Recall.Services.Authentication;
using Recall.Services.Exceptions;
using Recall.Services.Models.Authentication;

namespace Recall.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Login([FromBody] LoginData data)
        {
            try
            {
                var result = authService.Login(data);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Register([FromBody] RegisterData data)
        {
            try
            {
                authService.Register(data);
                return Ok();
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
