namespace Recall.Services.Meta.Topics
{
    using Recall.Services.Models.Meta.TopicModels;

    public interface ITopicService
    {
        TopicFolderFrontEnd Create(TopicCreate data, int userId);
        
        void AddVideo(AddVideoData data, int userId);

        TopicFolderFrontEnd[] GetAllForSelect(bool all, int userId);

        TopicFolderFrontEnd[] GetAllTopicsForVideo(int videoId, int userId);

        void RemoveTopicFromVideo(RmoveTopicFromVideoData data, int userId);
    }
}
