namespace Recall.Web.Controllers
{
    using GetReady.Web.Middleware;
    using Microsoft.AspNetCore.Mvc;
    using Recall.Services.Exceptions;
    using Recall.Services.Jwt;
    using Recall.Services.Public;
    using Recall.Services.Utilities;

    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly IPublicService publicService;
        private readonly IJwtService jwtService;

        public PublicController(IPublicService publicService, IJwtService jwtService)
        {
            this.publicService = publicService;
            this.jwtService = jwtService;
        }

        //[HttpPost]
        //[Route("[action]")]
        //[ClaimRequirement(Constants.RoleType, "User")]
        //public IActionResult GeneratePublicVideos([FromBody] string data)
        //{
        //    try
        //    {
        //        var userData = jwtService.ParseData(this.User);
        //        this.publicService.getlat(userData.UserId);
        //        return Ok();
        //    }
        //    catch (ServiceException e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpGet("GetLatest/{page}")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult GetLatest(int page)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var result = publicService.GetLatest(page);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}