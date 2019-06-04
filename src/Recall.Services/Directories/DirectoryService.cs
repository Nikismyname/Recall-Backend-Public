#region INIT
namespace Recall.Services.Directories
{
    using AutoMapper;
    using GetReady.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Recall.Data;
    using Recall.Data.Models.Core;
    using Recall.Services.Exceptions;
    using Recall.Services.Models.DirectoryModels;
    using Recall.Services.Models.NavigationModels;
    using Recall.Services.Utilities;
    using Recall.Services.Videos;
    using System.Linq;

    public class DirectoryService : IDirectoryService
    {
        private readonly RecallDbContext context;
        private readonly IVideoService videoService;

        public DirectoryService(RecallDbContext context, IVideoService videoService)
        {
            this.context = context;
            this.videoService = videoService;
        }
        #endregion

        #region CREATE
        public DirectoryIndex Create(DirectoryCreate create, int userId, bool isAdmin = false)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);

            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            var parentDir = context.Directories.SingleOrDefault(x => x.Id == create.ParentDirectoryId);

            if (parentDir == null)
            {
                throw new ServiceException("The parent directory of the directory you are trying to create does not exist!");
            }

            if (parentDir.UserId != user.Id && isAdmin == false)
            {
                throw new ServiceException("The parent directory does not belong to you!");
            }

            if (string.IsNullOrWhiteSpace(create.Name) || create.Name.Length == 0 || create.Name.ToLower() == "root")
            {
                throw new ServiceException("The directory name is not valid!");
            }

            var order = 0;
            if (parentDir.Subdirectories.Any())
            {
                order = parentDir.Subdirectories.Select(x => x.Order).Max() + 1;
            }

            var dir = new Directory
            {
                Name = create.Name,
                UserId = parentDir.UserId,
                ParentDirectory = parentDir,
                Order = order,
            };

            context.Directories.Add(dir);
            context.SaveChanges();

            var index = Mapper.Instance.Map<DirectoryIndex>(dir);

            return index;
        }
        #endregion

        #region DELETE
        public int Delete(int id, int userId, bool isAdmin = false)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            var dirToDelete = context.Directories.SingleOrDefault(x => x.Id == id);

            if (dirToDelete == null)
            {
                throw new ServiceException("The directory you are trying to delete does not exist!");
            }

            if (dirToDelete.UserId != user.Id && isAdmin == false)
            {
                throw new ServiceException("The directory you are trying to delete does not belong to you!");
            }

            if (dirToDelete.Name == Constants.RootDorectoryName)
            {
                throw new ServiceException("Can not delete Root directory!");
            }

            this.DeleteRecursion(id);

            this.context.SaveChanges();

            return id;
        }

        private void DeleteRecursion(int dirId)
        {
            var directory = context.Directories
                .Include(x => x.Videos)
                    .ThenInclude(x => x.Notes)
                .Include(x => x.Subdirectories)
                .SingleOrDefault(x => x.Id == dirId);

            var childDirIds = directory.Subdirectories.Select(x => x.Id).ToArray();

            foreach (var childDirId in childDirIds)
            {
                this.DeleteRecursion(childDirId);
            }

            foreach (var video in directory.Videos)
            {
                this.videoService.NoteDeleteRecursion(video.Notes.Where(x => x.ParentNote == null).ToArray());
                this.context.Videos.Remove(video);
            }

            this.context.Directories.Remove(directory);
        }
        #endregion

        #region EDIT
        public void Edit(DirectoryEdit data, int userId)
        {
            var directory = this.context.Directories.SingleOrDefault(x => x.Id == data.DirectoryId);
            if (directory == null)
            {
                throw new ServiceException("Directory Not Found!");
            }

            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            if (user.Id != directory.UserId)
            {
                throw new ServiceException("Directory Does Not Belong To You!");
            }

            //TODO: VALIDATE NAME
            directory.Name = data.NewName;
            context.SaveChanges();
        }
        #endregion

        #region GET_ALL 
        public AllFoldersFrontEndNaming[] GetForFolderSelect(int userId)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);

            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            var allDirectories = context.Directories
            .Where(x => x.UserId == user.Id)
            .To<DirectoryForAllFolders>()
            .ToArray();

            var allDirFrontEndNaming = allDirectories.Select(x => new AllFoldersFrontEndNaming
            {
                Id = x.Id,
                Name = x.Name,
                Order = x.Order,
                parentId = x.ParentDirectoryId,
            }).ToArray();

            return allDirFrontEndNaming;
        }

        public AllItemsFrontEndNaming[] GetForItemSelection(int userId)
        {
            var allDirectories = context.Directories
                .Where(x => x.UserId == userId)
                .To<DirectoryForAllItems>()
                .ToArray();

            var allDirectoriesFrontEndNaming = allDirectories
                .Select(x => new AllItemsFrontEndNaming
                {
                    Id = x.Id,
                    Name = x.Name,
                    Order = x.Order,
                    ParentId = x.ParentDirectoryId,
                    Items = x.Videos.ToArray(),
                }).ToArray();

            return allDirectoriesFrontEndNaming; 
        }

        #endregion
    }
}
