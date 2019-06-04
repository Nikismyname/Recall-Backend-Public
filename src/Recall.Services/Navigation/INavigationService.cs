namespace Recall.Services.Navigation
{
    using Recall.Services.Models.NavigationModels;

    public interface INavigationService
    {
        DirectoryIndex GetIndex(int? directoryId, int userId, bool isAdmin = false);

        void ReorderVideos(ReorderData data, int userId);

        void ReorderDirectories(ReorderData data, int userId);

        VideoIndex GetVideoIndex(int videoId, int userId, bool isAdmin = false);
    }
}
