#region INIT
using AutoMapper;
using GetReady.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Recall.Data;
using Recall.Data.Models.Core;
using Recall.Data.Models.Enums;
using Recall.Services.Exceptions;
using Recall.Services.Models.Meta.TopicModels;
using Recall.Services.Models.NavigationModels;
using Recall.Services.Models.NoteModels;
using Recall.Services.Models.VideoModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Recall.Services.Videos
{
    public class VideoService : IVideoService
    {
        private readonly RecallDbContext context;

        public VideoService(RecallDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region Create
        public VideoIndex Create(VideoCreate videoCreate, int userId, bool isAdmin = false)
        {
            var dirId = videoCreate.DirectoryId;

            var user = context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            //-1 means add it to the root;
            if (dirId == -1)
            {
                dirId = this.context.Directories
                    .Where(x => x.UserId == user.Id && x.ParentDirectoryId == null)
                    .Single()
                    .Id;
            }

            var directory = context.Directories.SingleOrDefault(x => x.Id == dirId);
            if (directory == null)
            {
                throw new ServiceException("The Directory you selected for creating the new video notes in, does not exist!");
            }

            if (user.Id != directory.UserId && isAdmin == false)
            {
                throw new ServiceException("The directory you are trying to create a video on does note belong to you!");
            }

            var typeBools = new bool[] { videoCreate.IsLocal, videoCreate.IsYouTube, videoCreate.IsVimeo };
            if (typeBools.Where(x => x).Count() != 1)
            {
                throw new ServiceException("You must select one type of video!");
            }

            var ordersInfo = context.Directories
                .Select(x => new { id = x.Id, orders = x.Videos.Select(y => y.Order).ToArray() })
                .SingleOrDefault(x => x.id == dirId);
            var order = ordersInfo.orders.Length == 0 ? 0 : ordersInfo.orders.Max() + 1;

            var video = new Video
            {
                DirectoryId = dirId,
                Order = order,
                UserId = user.Id,
                Name = videoCreate.Name,
                Description = videoCreate.Description,
                Url = videoCreate.Url,
                IsLocal = videoCreate.IsLocal,
                IsYouTube = videoCreate.IsYouTube,
                IsVimeo = videoCreate.IsVimeo,
                SeekTo = 0,
            };

            context.Videos.Add(video);
            context.SaveChanges();

            var index = Mapper.Instance.Map<VideoIndex>(video);

            return index;
        }
        #endregion

        #region Delete
        public int Delete(int id, int userId, bool isAdmin = false)
        {
            var video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == id);

            if (video == null)
            {
                throw new ServiceException("The video you are trying to delete does not exist!");
            }

            var user = context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            if (video.UserId != user.Id && isAdmin == false)
            {
                throw new ServiceException("The video you are trying to delete does not belong to you!");
            }

            this.NoteDeleteRecursion(video.Notes.Where(x => x.ParentNote == null).ToArray());
            context.Videos.Remove(video);

            context.SaveChanges();

            return id;
        }

        public void NoteDeleteRecursion(ICollection<Note> notes)
        {
            foreach (var note in notes)
            {
                var childNotes = note.ChildNotes;
                this.NoteDeleteRecursion(childNotes);
            }

            foreach (var note in notes)
            {
                this.context.Notes.Remove(note);
            }
        }

        private void NoteDeleteRecSingle(int noteId)
        {
            var note = this.context.Notes
                .Select(x => new { note = x, childIds = x.ChildNotes.Select(y => y.Id).ToArray() })
                .SingleOrDefault(x => x.note.Id == noteId);
            if (note == null)
            {
                return;
            }

            for (int i = 0; i < note.childIds.Length; i++)
            {
                this.NoteDeleteRecSingle(note.childIds[i]);
            }

            this.context.Notes.Remove(note.note);
        }
        #endregion

        #region GET_FOR_EDIT
        public VideoEdit GetVideoForEdit(int videoId, int userId, bool isAdmin = false)
        {
            var video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == videoId);

            if (video == null)
            {
                throw new ServiceException("The video you are trying to edit does not exist!");
            }

            var user = context.Users.SingleOrDefault(x => x.Id == userId);

            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            if (video.UserId != user.Id && isAdmin == false)
            {
                throw new ServiceException("You can note edit video that does not belong to you!");
            }

            ///Reordering the notes so that when there are deletions the numbers are sequential
            var dbNotes = video.Notes.OrderBy(x => x.Order).ToArray();
            for (int i = 0; i < dbNotes.Length; i++)
            {
                dbNotes[i].Order = i;
            }
            context.SaveChanges();

            var map = new Dictionary<int, int>();
            var pageNotes = new NoteCreate[dbNotes.Length];

            for (int i = 0; i < dbNotes.Length; i++)
            {
                var dbNote = dbNotes[i];
                ///The inPageId is mapped from the order
                pageNotes[i] = Mapper.Instance.Map<NoteCreate>(dbNote);
                pageNotes[i].Level = dbNote.Level;
                map.Add(dbNote.Id, i);
            }

            for (int i = 0; i < dbNotes.Length; i++)
            {
                var dbNote = dbNotes[i];
                var pageNote = pageNotes[i];

                if (dbNote.ParentNoteId != null)
                {
                    pageNote.InPageParentId = map[(int)dbNote.ParentNoteId];
                }
            }

            var result = new VideoEdit
            {
                Id = video.Id,
                Description = video.Description,
                Url = video.Url,
                Name = video.Name,
                DirectoryId = video.DirectoryId,
                SeekTo = video.SeekTo,
                Duration = video.Duration,
                Notes = pageNotes.ToList(),
                IsYouTube = video.IsYouTube,
                IsLocal = video.IsLocal,
                IsVimeo = video.IsVimeo,
            };

            video.LastAccessed = DateTime.Now;
            video.TimesAccessed += 1;
            context.SaveChanges();

            return result;
        }

        public VideoEdit GetVideoForView(int videoId, int userId, bool isAdmin = false)
        {
            var video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == videoId);

            if (video == null)
            {
                throw new ServiceException("The video you are trying to edit does not exist!");
            }

            var user = context.Users.SingleOrDefault(x => x.Id == userId);

            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            if (video.Public == false && video.UserId != user.Id)
            {
                throw new ServiceException("You can note edit video that does not belong to you!");
            }

            var publicViewer = false;
            if(video.UserId != user.Id)
            {
                publicViewer = true;
            }

            ///Reordering the notes so that when there are deletions the numbers are sequential
            var dbNotes = video.Notes.OrderBy(x => x.Order).ToArray();
            for (int i = 0; i < dbNotes.Length; i++)
            {
                dbNotes[i].Order = i;
            }
            context.SaveChanges();

            var map = new Dictionary<int, int>();
            var pageNotes = new NoteCreate[dbNotes.Length];

            for (int i = 0; i < dbNotes.Length; i++)
            {
                var dbNote = dbNotes[i];
                ///The inPageId is mapped from the order
                pageNotes[i] = Mapper.Instance.Map<NoteCreate>(dbNote);
                pageNotes[i].Level = dbNote.Level;
                map.Add(dbNote.Id, i);
            }

            for (int i = 0; i < dbNotes.Length; i++)
            {
                var dbNote = dbNotes[i];
                var pageNote = pageNotes[i];

                if (dbNote.ParentNoteId != null)
                {
                    pageNote.InPageParentId = map[(int)dbNote.ParentNoteId];
                }
            }

            var result = new VideoEdit
            {
                Id = video.Id,
                Description = video.Description,
                Url = video.Url,
                Name = video.Name,
                DirectoryId = video.DirectoryId,
                SeekTo = video.SeekTo,
                Duration = video.Duration,
                Notes = pageNotes.ToList(),
                IsYouTube = video.IsYouTube,
                IsLocal = video.IsLocal,
                IsVimeo = video.IsVimeo,
            };

            if (publicViewer)
            {
                video.TimesPublicAccessed++;
            }
            else
            {
                video.LastAccessed = DateTime.Now;
                video.TimesAccessed += 1;
            }
            context.SaveChanges();
            return result;
        }
        #endregion

        #region GET_FOR_CONNECTION
        public VideoForConnections GetVideoForConnection (int videoId, int userId)
        {
            //TODO: ADD VALIDATION
            var user = context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            var video = this.context.Videos
                .Include(x => x.Topics)
                    .ThenInclude(x => x.Topic)
                .To<VideoForConnections>()
                .SingleOrDefault(x => x.Id == videoId);

            return video;
        }
        #endregion

        #region SAVE

        public int[][] Save(int videoId, int? seekTo, int? duration,
            string name, string desctiption, string url, string[][] changes,
            NoteCreate[] newNotes, bool finalSave, int userId, bool isAdmin = false)
        {
            this.ValidateSave(videoId, changes, userId, finalSave, isAdmin);
            ///Appy changes to the video fields
            this.SaveVideoFields(videoId, name, desctiption, seekTo, url, duration);
            ///Appy changes to the existing Notes
            this.SaveChangesToExistingNotes(changes);
            ///Create the new notes and return their IDs
            var newNoteDbId = this.SaveNewNotes(newNotes, videoId);
            return newNoteDbId;
        }

        ///Tested except the integration with the trackable service
        private void ValidateSave(
            int videoId, string[][] changes, int userId, bool finalSave, bool isAdmin)
        {
            ///chech if video exists 
            var video = context.Videos.SingleOrDefault(x => x.Id == videoId);
            if (video == null)
            {
                throw new ServiceException("The video you are working on does not exists in the database");
            }

            var user = context.Users.
                Include(x => x.Directories)
                .SingleOrDefault(x => x.Id == userId);

            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            ///First check is beacause some legacy vidoes do not have user id
            if (video.UserId != null && video.UserId != user.Id && isAdmin == false)
            {
                throw new ServiceException("The video you are trying to modify does not belong to you!");
            }
            ///check if all the notes being changed belong the video they are coming for;
            var videoNotesIds = context
            .Videos
            .Include(x => x.Notes)
            .SingleOrDefault(x => x.Id == videoId)
            .Notes.Select(x => x.Id).ToArray();
            var sentVideoNoteIds = changes.Select(x => int.Parse(x[0])).ToArray();

            if (sentVideoNoteIds.Any(x => !videoNotesIds.Contains(x)))
            {
                throw new ServiceException("The video notes you are trying to modify does not belong the the current video");
            }
        }

        ///Tested
        private void SaveVideoFields(
            int videoId,
            string name,
            string description,
            int? seekTo,
            string url,
            int? duration
            )
        {
            var video = context.Videos.SingleOrDefault(x => x.Id == videoId);

            if (name != null)
            {
                video.Name = name;
            }
            if (description != null)
            {
                video.Description = description;
            }
            if (seekTo != null)
            {
                video.SeekTo = seekTo;
            }
            if (url != null)
            {
                video.Url = url;
            }
            if (duration != null)
            {
                video.Duration = duration;
            }
        }

        ///Tested
        private bool SaveChangesToExistingNotes(string[][] changes)
        {
            if (changes.Length == 0)
            {
                return true;
            }

            var videoIds = changes.Select(x => int.Parse(x[0])).ToArray();
            var notesToChange = context.Notes.Where(x => videoIds.Contains(x.Id)).ToArray();

            foreach (var change in changes)
            {
                var noteId = int.Parse(change[0]);
                var prop = change[1];
                var newVal = change[2];
                Note currentNote = notesToChange.SingleOrDefault(x => x.Id == noteId);

                switch (prop.ToUpper())
                {
                    case "DELETED":
                        this.NoteDeleteRecSingle(noteId);
                        break;
                    case "CONTENT":
                        currentNote.Content = newVal;
                        break;
                    //case "FORMATTING":
                    //    currentNote.Formatting = (Formatting)int.Parse(newVal);
                    //    break;
                    case "SEEKTO":
                        currentNote.SeekTo = int.Parse(newVal);
                        break;
                    case "TYPE":
                        currentNote.Type = (NoteType)int.Parse(newVal);
                        break;
                    case "BORDERCOLOR":
                        currentNote.BorderColor = newVal;
                        break;
                    case "BORDERTHICKNESS":
                        currentNote.BorderThickness = int.Parse(newVal);
                        break;
                    case "BACKGROUNDCOLOR":
                        currentNote.BackgroundColor = newVal;
                        break;
                    case "TEXTCOLOR":
                        currentNote.TextColor = newVal;
                        break;
                }
            }
            return true;
        }

        ///Tested
        private int[][] SaveNewNotes(NoteCreate[] newPageNotes, int videoId)
        {
            ///removing items which are created and deleted in the same save window
            newPageNotes = newPageNotes.Where(x => x.Deleted == false).ToArray();

            var existingParentNotesIds = newPageNotes
                .Select(x => x.ParentDbId)
                .Where(x => x > 0)
                .ToArray();

            var existingParentNotes = context.Notes.
                Where(x => existingParentNotesIds.Contains(x.Id)).
                ToList();

            newPageNotes = newPageNotes.OrderBy(x => x.InPageId).ToArray();
            var dbNotesToBe = new List<Note>();
            for (int i = 0; i < newPageNotes.Length; i++)
            {
                var pageNote = newPageNotes[i];
                var newNote = Mapper.Instance.Map<Note>(pageNote);
                newNote.VideoId = videoId;

                dbNotesToBe.Add(newNote);
            }

            for (int i = 0; i < dbNotesToBe.Count; i++)
            {
                var dbNoteToBe = dbNotesToBe[i];
                var pageNote = newPageNotes[i];

                if (pageNote.ParentDbId > 0)
                {
                    existingParentNotes.SingleOrDefault(x => x.Id == pageNote.ParentDbId)
                        .ChildNotes.Add(dbNoteToBe);
                }
                ///0 means that it is not root and has parent that is not in the db, -1 means root
                else if (pageNote.ParentDbId == 0)
                {
                    var inPageParentId = pageNote.InPageParentId;
                    var pageParent = newPageNotes.FirstOrDefault(x => x.InPageId == inPageParentId);

                    var indexOfPageParent = Array.IndexOf(newPageNotes, pageParent);
                    var dbParent = dbNotesToBe[indexOfPageParent];
                    dbParent.ChildNotes.Add(dbNoteToBe);
                }
            }

            this.CheckTheNestringLevel(dbNotesToBe, existingParentNotes);

            context.Notes.AddRange(dbNotesToBe);

            ///Final SaveChanges for the Save
            context.SaveChanges();

            ///inPageId and Order are the same thing,
            ///realigning them after putting them in 
            ///the db in case they got reaordered
            dbNotesToBe = dbNotesToBe.OrderBy(x => x.Order).ToList();
            newPageNotes = newPageNotes.OrderBy(x => x.InPageId).ToArray();

            var resultList = new List<int[]>();
            ///mapping the new Ids to the in-page ids and 
            ///sending them to the JS so the dbId can be send 
            ///back in case of changes
            for (int i = 0; i < dbNotesToBe.Count; i++)
            {
                resultList.Add(new int[] { newPageNotes[i].InPageId, dbNotesToBe[i].Id });
            }

            return resultList.ToArray();
        }

        ///TODO: find more efficent way to check for level
        private void CheckTheNestringLevel(List<Note> dbNotesToBe, List<Note> existingParentNotes)
        {
            var dict = new Dictionary<Note, int>();
            var allNotes = dbNotesToBe.Concat(existingParentNotes).ToArray();
            foreach (var note in allNotes)
            {
                if (FindDeepestNesting(note, dict) > 4)
                {
                    throw new ServiceException("The notes you are trying to save are nested deeper the four levels!");
                }
            }
        }

        private int FindDeepestNesting(Note note, Dictionary<Note, int> dict)
        {
            if (dict.ContainsKey(note))
            {
                return dict[note];
            }

            var deepestNesting = 0;
            foreach (var child in note.ChildNotes)
            {
                var tempDeepest = FindDeepestNesting(child, dict);
                if (tempDeepest > deepestNesting)
                {
                    deepestNesting = tempDeepest;
                }
            }

            var depth = deepestNesting + 1;
            dict[note] = depth;

            return depth;
        }
        #endregion

        #region VIDEO_MOVE
        public VideoMoveWithOrigin MoveVideo(VideoMove data, int userId)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            var video = this.context.Videos.SingleOrDefault(x => x.Id == data.videoId);
            if (video == null)
            {
                throw new ServiceException("Video Not Found!");
            }

            var origin = video.DirectoryId;

            if (video.DirectoryId == data.newDirectoryId)
            {
                return null;
            }

            if (video.UserId != userId)
            {
                throw new ServiceException("Video Does Not Belong To You!");
            }

            var newDir = context.Directories.SingleOrDefault(x => x.Id == data.newDirectoryId);
            if (newDir == null)
            {
                throw new ServiceException("New Dir Not Found!");
            }

            if (newDir.UserId != userId)
            {
                throw new ServiceException("New Dir Does Not Belong To You!");
            }

            video.DirectoryId = data.newDirectoryId;
            context.SaveChanges();

            return new VideoMoveWithOrigin
            {
                newDirectoryId = data.newDirectoryId,
                OriginDirectory = origin,
                videoId = data.videoId,
            };
        }
        #endregion

        #region EXTESION
        public void AddExtensionVideo(ExtentionVideoAddData data, int userId)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User not found!");
            }

            this.context.ExtensionAddedVideos.Add(new ExtensionAddedVideo
            {
                UserId = user.Id,
                Name = data.Name,
                Url = data.Url,
                Type = data.Type,
            });

            this.context.SaveChanges();
        }

        public ExtentionVideoAddData[] GetExtesionVideos(int userId)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User not found!");
            }

            return this.context.ExtensionAddedVideos
                .Where(x => x.UserId == userId)
                .Select(x => new ExtentionVideoAddData
                {
                    Url = x.Url,
                    Name = x.Name,
                    Id = x.Id,
                    Type = x.Type,
                })
                .ToArray();
        }

        public VideoIndex ConvertExtensionVideo(ConvertExtensionData data, int userId)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            var extensionVid = this.context.ExtensionAddedVideos
                .SingleOrDefault(x => x.Id == data.ExtensionVideoId);
            if (extensionVid == null)
            {
                throw new ServiceException("Extension Video Not Found!");
            }

            if (extensionVid.UserId != user.Id)
            {
                throw new ServiceException("Extension Vid Does Not Belong To You!");
            }

            if (data.ShouldAdd)
            {
                var newDirectory = context.Directories.SingleOrDefault(x => x.Id == data.ParentDirId);
                if (newDirectory == null)
                {
                    throw new ServiceException("New Directory Not Found!");
                }

                if (newDirectory.UserId != user.Id)
                {
                    throw new ServiceException("New Directory Does Not Belong To You!");
                }

                var ordersInfo = context.Directories
                    .Select(x => new { id = x.Id, orders = x.Videos.Select(y => y.Order).ToArray() })
                    .SingleOrDefault(x => x.id == newDirectory.Id);
                var order = ordersInfo.orders.Length == 0 ? 0 : ordersInfo.orders.Max() + 1;

                var desctiption = extensionVid.Name;
                if (extensionVid.Name.Length > 40)
                {
                    extensionVid.Name = extensionVid.Name.Substring(0,37) + "...";
                }

                var isYouTube = extensionVid.Type == "youtube";
                var isVimeo = extensionVid.Type == "vimeo";

                var video = new Video
                {
                    DirectoryId = newDirectory.Id,
                    Order = order,
                    UserId = user.Id,
                    Name = extensionVid.Name,
                    Description = desctiption,
                    Url = extensionVid.Url,
                    IsLocal = false,
                    IsYouTube = isYouTube,
                    IsVimeo = isVimeo,
                    SeekTo = 0,
                };

                context.Videos.Add(video);
                context.ExtensionAddedVideos.Remove(extensionVid);
                context.SaveChanges();
                var index = Mapper.Instance.Map<VideoIndex>(video);
                return index;
            }
            else
            {
                context.ExtensionAddedVideos.Remove(extensionVid);
                context.SaveChanges();
                return null;
            }
        }
        #endregion

        #region MAKE_PUBLIC
        public void MakePublic(int videoId, int userId)
        {
            var user = this.context.Users.SingleOrDefault(x=>x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            var video = this.context.Videos.SingleOrDefault(x => x.Id == videoId);
            if(video == null)
            {
                throw new ServiceException("Video Not Found!");
            }

            if(video.UserId != user.Id)
            {
                throw new ServiceException("This is not your video!");
            }

            video.Public = true;
            this.context.SaveChanges();
        }

        public void MakePrivate(int videoId, int userId)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            var video = this.context.Videos.SingleOrDefault(x => x.Id == videoId);
            if (video == null)
            {
                throw new ServiceException("Video Not Found!");
            }

            if (video.UserId != user.Id)
            {
                throw new ServiceException("This is not your video!");
            }

            video.Public = false;
            this.context.SaveChanges();
        }
        #endregion;
    }
}
