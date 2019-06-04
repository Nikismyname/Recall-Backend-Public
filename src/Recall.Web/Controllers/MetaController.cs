#region INIT
namespace Recall.Web.Controllers
{
    using GetReady.Web.Middleware;
    using Microsoft.AspNetCore.Mvc;
    using Recall.Services.Exceptions;
    using Recall.Services.Jwt;
    using Recall.Services.Meta.Connections;
    using Recall.Services.Meta.Topics;
    using Recall.Services.Models.Meta.TopicModels;
    using Recall.Services.Models.OptionsModels;
    using Recall.Services.Options;
    using Recall.Services.Utilities;

    [Route("api/[controller]")]
    [ApiController]
    public class MetaController : ControllerBase
    {
        private readonly ITopicService topicService;
        private readonly IConnectionService connectionService;
        private readonly IJwtService jwtService;
        private readonly IOptionsService optionsService;

        public MetaController(ITopicService topicService, IConnectionService connectionService, IJwtService jwtService, IOptionsService optionsService)
        {
            this.topicService = topicService;
            this.connectionService = connectionService;
            this.jwtService = jwtService;
            this.optionsService = optionsService;
        }


        #endregion

        #region CREATE_TOPIC
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult CreateTopic([FromBody] TopicCreate data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var result = this.topicService.Create(data, userData.UserId);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region ADD_VIDEO_TO_TOPIC
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult AddVideoToTopic([FromBody] AddVideoData data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                this.topicService.AddVideo(data, userData.UserId);
                return Ok();
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region REMOVE_VIDEO_FROM_TOPIC
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult RemoveVideoFromTopic([FromBody] RmoveTopicFromVideoData data)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                this.topicService.RemoveTopicFromVideo(data, userData.UserId);
                return Ok();
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region GET_ALL_TOPICS
        [HttpGet("GetAllTopicsForSelect/{all}")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult GetAllTopicsForSelect(bool all)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var result = this.topicService.GetAllForSelect(all, userData.UserId);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region GET_ALL_TOPICS_FOR_VIDEO
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult GetAllTopicsForVideo([FromBody] int videoId)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                var result = this.topicService.GetAllTopicsForVideo(videoId, userData.UserId);
                return Ok(result);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region OPTIONS
        [HttpPost]
        [Route("[action]")]
        [ClaimRequirement(Constants.RoleType, "User")]
        public IActionResult SaveOptions ([FromBody] AllOptions options)
        {
            try
            {
                var userData = jwtService.ParseData(this.User);
                this.optionsService.SaveOptions(options, userData.UserId);
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