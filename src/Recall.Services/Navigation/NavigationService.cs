#region INIT
namespace Recall.Services.Navigation
{
    using GetReady.Services.Mapping;
    using Recall.Data;
    using Recall.Services.Exceptions;
    using Recall.Services.Models.NavigationModels;
    using System.Linq;

    public class NavigationService : INavigationService
    {
        private readonly RecallDbContext context;

        public NavigationService(RecallDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region GET_INDEX
        public DirectoryIndex GetIndex(int? directoryId, int userId, bool isAdmin = false)
        {
            var user = context.Users
                .SingleOrDefault(x => x.Id == userId);

            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            DirectoryIndex dir = null;

            if (directoryId == null || directoryId == -1)
            {
                dir = context.Directories
                    .Where(x => x.UserId == user.Id && x.Id == user.RootDirectoryId)
                    .To<DirectoryIndex>()
                    .SingleOrDefault();

                if (dir == null)
                {
                    throw new ServiceException("Could not find root dir for use!");
                }
            }
            else
            {
                dir = this.context.Directories
                    .Where(x => (x.UserId == userId || isAdmin == true) && x.Id == directoryId)
                    .To<DirectoryIndex>()
                    .SingleOrDefault();

                if (dir == null)
                {
                    throw new ServiceException("The directory you are trying to access eather does not exist or is not yours!");
                }
            }

            return dir;
        }
        #endregion

        #region GET_VIDEO_INDEX
        public VideoIndex GetVideoIndex(int videoId, int userId, bool isAdmin = false)
        {
            var video = this.context.Videos
                .Where(x => (x.UserId == userId || isAdmin) && x.Id == videoId)
                .To<VideoIndex>()
                .SingleOrDefault(); 

            if(video == null)
            {
                throw new ServiceException("Video Not Found Or Does Not Belong To You!");
            }

            return video; 
        }
        #endregion

        #region REORDER

        public void ReorderVideos(ReorderData data, int userId)
        {
            var dir = this.context.Directories
                .SingleOrDefault(x => x.Id == data.DirId);
            if (dir == null)
            {
                throw new ServiceException("Dir to Reorder Does not Exist!");
            }

            var user = this.context.Users
                .SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            if (user.Id != dir.UserId)
            {
                throw new ServiceException("Dir Does Not Belong To You!");
            }

            //TODO: make it so it only updates the the changes again

            var orderings = data.Orderings
                .Select((x, i) => new { id = x[0], newOrder = i, oldOrder = x[1] })
                .OrderBy(x => x.id)
                .ToArray();

            var dbVideos = this.context.Videos
                .Where(x => x.DirectoryId == dir.Id)
                .OrderBy(x => x.Id)
                .ToArray();

            var dbIds = dbVideos.Select(x => x.Id).OrderBy(x => x);
            var orderingsIds = orderings.Select(x => x.id).OrderBy(x => x);

            if (!dbIds.SequenceEqual(orderingsIds)) {
                throw new ServiceException("Reorder Video Data Invalid!");
            }

            for (int i = 0; i < orderings.Length; i++)
            {
                var ordering = orderings[i];
                var dbVideo = dbVideos.Single(x => x.Id == ordering.id);
                dbVideo.Order = ordering.newOrder;
            }
            context.SaveChanges();
        }

        public void ReorderDirectories(ReorderData data, int userId)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            var parentDir = context.Directories
                .SingleOrDefault(x => x.Id == data.DirId);
            if (parentDir == null)
            {
                throw new ServiceException("Dir Parent Sheet Not Found!");
            }

            if (parentDir.UserId != user.Id)
            {
                throw new ServiceException("Dir does not belong to you!");
            }

            this.ReorderDirectories(data.DirId, data.Orderings);
        }

        private void ReorderDirectories(int dirId, int[][] orderings)
        {
            var directories = context.Directories.Where(x => x.ParentDirectoryId == dirId).ToArray();
            var sheetIds = directories.Select(x => x.Id).OrderBy(x => x).ToArray();

            var sentSheetIds = orderings.Select(x => x[0]).OrderBy(x => x).ToArray();

            if (!sheetIds.SequenceEqual(sentSheetIds))
            {
                throw new ServiceException("Bad Dir Reorder Data");
            }

            for (int i = 0; i < orderings.Length; i++)
            {
                var currentId = orderings[i][0];
                var currentOrder = i;
                var currentSheet = directories.Single(x => x.Id == currentId);
                currentSheet.Order = currentOrder;
            }

            context.SaveChanges();
        }

        #endregion
    }
}
