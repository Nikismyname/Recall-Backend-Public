#region INIT
namespace Recall.Web.Controllers
{
    using GetReady.Web.Middleware;
    using Microsoft.AspNetCore.Mvc;
    using Recall.Services.Admin;
    using Recall.Services.Exceptions;
    using Recall.Services.Jwt;
    using Recall.Services.Utilities;

    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly IJwtService jwtService;

        public AdminController(IAdminService adminService, IJwtService jwtService)
        {
            this.adminService = adminService;
            this.jwtService = jwtService;
        }
        #endregion

        #region SEEDING
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult GeneratePublicVideos([FromBody] string data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                this.adminService.SeedPublicVideos(userData.UserId);
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
        public IActionResult DeleteTestPublicVideos([FromBody] string data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                this.adminService.DeletePublicTestVideos(userData.UserId);
                return Ok();
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region USER_MANAGEMENT
        [HttpGet("GetAllUsers/{adminsOnly?}")]
        [ClaimRequirement(Constants.RoleType, "Admin")]
        public IActionResult GetAllUsers(bool adminsOnly = false)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var result = adminService.GetAllUsers(userData.UserId, adminsOnly);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "Admin")]
        public IActionResult PromoteUser([FromBody] int userId)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                this.adminService.PromoteUser(userId, userData.UserId);
                return Ok();
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "Admin")]
        public IActionResult DemoteUser([FromBody] int userId)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                this.adminService.DemoteUser(userId, userData.UserId);
                return Ok();
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    }
}