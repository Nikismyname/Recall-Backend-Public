#region INIT
namespace Recall.Web.Controllers
{
    using GetReady.Web.Middleware;
    using Microsoft.AspNetCore.Mvc;
    using Recall.Services.Exceptions;
    using Recall.Services.Jwt;
    using Recall.Services.Models.NavigationModels;
    using Recall.Services.Models.VideoModels;
    using Recall.Services.Utilities;
    using Recall.Services.Videos;

    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService videoService;
        private readonly IJwtService jwtService;

        public VideoController(IVideoService videoService, IJwtService jwtService)
        {
            this.videoService = videoService;
            this.jwtService = jwtService;
        }
        #endregion

        #region CREATE
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult Create([FromBody] VideoCreate data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                VideoIndex result = videoService.Create(data, userData.UserId);
                return Ok(result);
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
                var result = videoService.Delete(data, userData.UserId);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region SAVE
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult Save([FromBody]VideoSave saveData)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var result = this.videoService.Save(
                    saveData.videoId, saveData.seekTo, saveData.duration, 
                    saveData.name, saveData.description, saveData.url, 
                    saveData.changes, saveData.newItems,
                    saveData.finalSave, userData.UserId);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region GET_FOR_EDIT
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult GetForEdit([FromBody] int videoId)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var videoEdit = videoService.GetVideoForEdit(videoId, userData.UserId);
                return Ok(videoEdit);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult GetForView([FromBody] int videoId)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var videoEdit = videoService.GetVideoForView(videoId, userData.UserId);
                return Ok(videoEdit);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region GET_FOR_CONNECTIONS
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult GetForConnections([FromBody] int videoId)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var videoForConnection = videoService.GetVideoForConnection(videoId, userData.UserId);
                return Ok(videoForConnection);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region MOVE_VIDEO
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult MoveVideo([FromBody] VideoMove data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var videoEdit = videoService.MoveVideo(data, userData.UserId);
                return Ok(videoEdit);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region EXTENSION_VIDEOS
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult AddExtensionVideo([FromBody] ExtentionVideoAddData data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                videoService.AddExtensionVideo(data, userData.UserId);
                return Ok();
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetExtesionVideos")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult GetExtesionVideos()
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var vids = videoService.GetExtesionVideos(userData.UserId);
                return Ok(vids);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult ConvertExtensionVideo([FromBody] ConvertExtensionData data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var videoIndex = videoService.ConvertExtensionVideo(data, userData.UserId);
                return Ok(videoIndex);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region MAKE_PUBLIC
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult MakePublic([FromBody] int videoId)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                videoService.MakePublic(videoId, userData.UserId);
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
        public IActionResult MakePrivate([FromBody] int videoId)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                videoService.MakePrivate(videoId, userData.UserId);
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
