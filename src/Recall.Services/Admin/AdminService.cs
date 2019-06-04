#region INIT
namespace Recall.Services.Admin
{
    using GetReady.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Recall.Data;
    using Recall.Services.Directories;
    using Recall.Services.Exceptions;
    using Recall.Services.Models.AdminModels;
    using Recall.Services.Models.NavigationModels;
    using Recall.Services.Models.VideoModels;
    using Recall.Services.Videos;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AdminService: IAdminService
    {
        private readonly RecallDbContext context;
        private readonly IDirectoryService directoryService;
        private readonly IVideoService videoService;

        public AdminService(
            RecallDbContext context, 
            IDirectoryService directoryService, 
            IVideoService videoService
            )
        {
            this.context = context;
            this.directoryService = directoryService;
            this.videoService = videoService;
        }
        #endregion

        #region SEED_PUBLIC_VIDEOS
        const string superSecretTestFolder = "testFolder";

        public void SeedPublicVideos(int userId)
        {
            var user = context.Users
                .Include(x => x.RootDirectory)
                    .ThenInclude(x => x.Subdirectories)
                        .ThenInclude(x=>x.Videos)
                .SingleOrDefault(x => x.Id == userId);

            var testDirecotry = user.RootDirectory.Subdirectories
                .SingleOrDefault(x => x.Name == superSecretTestFolder);
            
            if(testDirecotry!= null && testDirecotry.Videos.Count> 0)
            {
                return; 
            }

            int dirId;

            if (testDirecotry == null)
            {
                dirId = this.directoryService.Create(new DirectoryCreate
                {
                    Name = superSecretTestFolder,
                    ParentDirectoryId = user.RootDirectoryId.Value,
                }, user.Id).Id;
            }
            else
            {
                dirId = testDirecotry.Id;
            }

            //var folder = this.context.Directories.SingleOrDefault(x => x.Id == folderId);

            var ids = new List<int>();

            for (int i = 0; i < 30; i++)
            {
                var id = this.videoService.Create(new VideoCreate
                {
                    Description = "test Video " + i,
                    DirectoryId = dirId,
                    IsLocal = false,
                    IsVimeo = false,
                    IsYouTube = true,
                    Name = "test video " + i,
                    Url = "https://www.youtube.com/watch?v=NY86qsWKDt8",
                }, user.Id).Id;
                ids.Add(id);
            }

            for (int i = 30; i < 60; i++)
            {
                var id = this.videoService.Create(new VideoCreate
                {
                    Description = "test Video " + i,
                    DirectoryId = dirId,
                    IsLocal = false,
                    IsVimeo = true,
                    IsYouTube = false,
                    Name = "test video " + i,
                    Url = "https://vimeo.com/335912375",
                }, user.Id).Id;
                ids.Add(id);
            }

            for (int i = 0; i < ids.Count; i += 2)
            {
                var vid1 = this.context.Videos.SingleOrDefault(x => x.Id == ids[i]);
                var vid2 = this.context.Videos.SingleOrDefault(x => x.Id == ids[i + 1]);

                vid1.CreatedOn = DateTime.Now.AddDays(i - 0.1);
                vid2.CreatedOn = DateTime.Now.AddDays(i - 0.2);

                vid1.Public = true;
                vid2.Public = true;
            }

            context.SaveChanges();
        }

        public void DeletePublicTestVideos(int userId)
        {
            var user = context.Users
                .Include(x => x.RootDirectory)
                    .ThenInclude(x => x.Subdirectories)
                        .ThenInclude(x=>x.Videos)
                .SingleOrDefault(x => x.Id == userId);

            var testDirectory = user.RootDirectory.Subdirectories.SingleOrDefault(x => x.Name == superSecretTestFolder);

            if(testDirectory != null)
            {
                var videos = testDirectory.Videos.ToArray(); 

                for (int i = videos.Length - 1; i >= 0; i--)
                {
                    this.videoService.Delete(videos[i].Id, user.Id);
                }
            }
        }
        #endregion

        public UserForAdminView[] GetAllUsers(int userId, bool onlyAdmins = false)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);
            if(user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            if (user.Role != "Admin")
            {
                throw new ServiceException("Only Admin Can Get Users!");
            }

            UserForAdminView[] result;

            if (onlyAdmins)
            {
                result = this.context.Users
                    .Where(x => x.Role == "Admin")
                    .To<UserForAdminView>()
                    .ToArray();
            }
            else
            {
                result = this.context.Users
                   .To<UserForAdminView>()
                   .ToArray();
            }

            return result;
        }

        public void PromoteUser(int promoUserId, int userId)
        {
            var promoUser = this.context.Users.SingleOrDefault(x => x.Id == promoUserId);
            if(promoUser == null)
            {
                throw new ServiceException("User To Promote Not Found!");
            }

            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);
            if(user == null)
            {
                throw new ServiceException("User Not Found!");
            }
            if (user.Role != "Admin")
            {
                throw new ServiceException("Only Admin Can Promote Users!");
            }

            promoUser.Role = "Admin";
            context.SaveChanges(); 
        }

        public void DemoteUser(int demoteUserId, int userId)
        {
            var promoUser = this.context.Users.SingleOrDefault(x => x.Id == demoteUserId);
            if (promoUser == null)
            {
                throw new ServiceException("User To Demote Not Found!");
            }

            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }
            if (user.Role != "Admin")
            {
                throw new ServiceException("Only Admin Can Promote Users!");
            }

            promoUser.Role = "User";
            context.SaveChanges();
        }
    }
}
