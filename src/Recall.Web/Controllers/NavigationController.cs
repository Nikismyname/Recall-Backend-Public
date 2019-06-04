using GetReady.Web.Middleware;
using Microsoft.AspNetCore.Mvc;
using Recall.Services.Exceptions;
using Recall.Services.Jwt;
using Recall.Services.Models.NavigationModels;
using Recall.Services.Navigation;
using Recall.Services.Utilities;

namespace Recall.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavigationController : ControllerBase
    {
        private readonly IJwtService jwtService;
        private readonly INavigationService navService;

        public NavigationController(INavigationService navService, IJwtService jwtService)
        {
            this.navService = navService;
            this.jwtService = jwtService;
        }

        [HttpGet("GetIndex/{id:int}")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult GetIndex (int id)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var result = navService.GetIndex(id, userData.UserId);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult ReorderDirectories([FromBody] ReorderData data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                navService.ReorderDirectories(data, userData.UserId);
                return Ok();
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult ReorderVodeos([FromBody] ReorderData data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                navService.ReorderVideos(data, userData.UserId);
                return Ok();
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult GetVideoIndex([FromBody] int videoId)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var result = navService.GetVideoIndex(videoId, userData.UserId);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
