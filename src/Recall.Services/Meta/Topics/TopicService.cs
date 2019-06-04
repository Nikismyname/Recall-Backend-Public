#region INIT
namespace Recall.Services.Meta.Topics
{
    using GetReady.Services.Mapping;
    using Recall.Data;
    using Recall.Data.Models.Meta;
    using Recall.Services.Exceptions;
    using Recall.Services.Models.Meta.TopicModels;
    using System.Linq;

    public class TopicService : ITopicService
    {
        private readonly RecallDbContext context;

        public TopicService(RecallDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region CREATE
        public TopicFolderFrontEnd Create(TopicCreate data, int userId)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found");
            }

            var topic = new Topic
            {
                Approved = user.Role == "Admin" ? true : false,
                CriteriaForBelonging = data.CriteriaForBelonging,
                Description = data.Description,
                Name = data.Name,
                ParentTopicId = data.ParentTopicId,
                UserId = user.Id,
            };

            this.context.Topics.Add(topic);
            this.context.SaveChanges();

            var result = new TopicFolderFrontEnd
            {
                Id = topic.Id,
                Name = topic.Name,
                Order = 0,
                parentId = topic.ParentTopicId,
                CriteriaForBelonging = topic.CriteriaForBelonging,
                Description = topic.Description,
            };

            return result;
        }
        #endregion

        #region ADD_VIDEO_TO_TOPIC
        public void AddVideo(AddVideoData data, int userId)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            var videoTopic = new VideoTopic
            {
                Adherence = data.Adherence,
                TopicId = data.TopicId,
                VideoId = data.VideoId,
            };

            try
            {
                this.context.VideosTopics.Add(videoTopic);
                this.context.SaveChanges();
            }
            catch
            {
                throw new ServiceException("Addng Video to Topic failed. Pobably Video already belongs to Topic!");
            }
        }
        #endregion

        #region REMOVE_TOPIC_VIDEO
        public void RemoveTopicFromVideo(RmoveTopicFromVideoData data, int userId)
        {
            var topicId = data.TopicId;
            var videoId = data.VideoId; 

            var connection = this.context.VideosTopics
                .SingleOrDefault(x => x.TopicId == topicId && x.VideoId == videoId);

            if (connection != null)
            {
                this.context.VideosTopics.Remove(connection);
                this.context.SaveChanges();
            }
        }
        #endregion

        #region GET_ALL
        public TopicFolderFrontEnd[] GetAllForSelect(bool all, int userId)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            TopicForAllFolders[] topics;
            if (all)
            {
                topics = this.context.Topics.To<TopicForAllFolders>().ToArray();
            }
            else
            {
                topics = this.context.Topics
                    .Where(x => x.Approved == true || (x.UserId != null && x.UserId == user.Id))
                    .To<TopicForAllFolders>()
                    .ToArray();
            }

            var result = topics.Select(x => new TopicFolderFrontEnd
            {
                Id = x.Id,
                Name = x.Name,
                Order = 0, // NOT USED AT THE MOMENT
                parentId = x.ParentTopicId,
                CriteriaForBelonging = x.CriteriaForBelonging,
                Description = x.Description,
            }).ToArray();

            return result;
        }
        #endregion

        #region GET_ALL_FOR_VIDEO
        public TopicFolderFrontEnd[] GetAllTopicsForVideo(int videoId, int userId)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            var video = this.context.Videos
                .Select(x => new
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Topics = x.Topics.Select(y => y.Topic).AsQueryable().To<TopicFolderFrontEnd>().ToArray(),
                })
                .SingleOrDefault(x => x.Id == videoId);

            //TODO: Some sort of validation

            return video.Topics; 
        }
        #endregion
    }
}
