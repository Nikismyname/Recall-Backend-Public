#region INIT
namespace Recall.Web.Controllers
{
    using GetReady.Web.Middleware;
    using Microsoft.AspNetCore.Mvc;
    using Recall.Services.Directories;
    using Recall.Services.Exceptions;
    using Recall.Services.Jwt;
    using Recall.Services.Models.DirectoryModels;
    using Recall.Services.Models.NavigationModels;
    using Recall.Services.Utilities;

    [Route("api/[controller]")]
    [ApiController]
    public class DirectoryController : ControllerBase
    {
        private readonly IJwtService jwtService;
        private readonly IDirectoryService dirService;

        public DirectoryController(IJwtService jwtService, IDirectoryService dirService)
        {
            this.jwtService = jwtService;
            this.dirService = dirService;
        }
        #endregion

        #region CREATE
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult Create([FromBody] DirectoryCreate data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                DirectoryIndex result = dirService.Create(data, userData.UserId);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region EDIT
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult Edit([FromBody] DirectoryEdit data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                dirService.Edit(data, userData.UserId);
                return Ok();
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region DELETE
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult Delete([FromBody] int data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                int result = dirService.Delete(data, userData.UserId);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region GET_ALL
        [HttpGet("GetAllFolders")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult GetAllFolders()
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var result = dirService.GetForFolderSelect(userData.UserId);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetAllItems")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult GetAllItems()
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var result = dirService.GetForItemSelection(userData.UserId);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    }
}

















//[HttpGet("GetOne/{id:int}")]
//[ClaimRequirement(Constants.RoleType, "User")]
//public IActionResult GetOne(int id)
//{
//    try
//    {
//        var userData = jwtService.ParseData(this.User);
//        return Ok();
//    }
//    catch (ServiceException e)
//    {
//        return BadRequest(e.Message);
//    }
//}
