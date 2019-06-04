namespace Recall.Services.Videos
{
    using Recall.Data.Models.Core;
    using Recall.Services.Models.NavigationModels;
    using Recall.Services.Models.NoteModels;
    using Recall.Services.Models.VideoModels;
    using System.Collections.Generic;

    public interface IVideoService
    {
        int Delete(int id, int userId, bool isAdmin = false);

        void NoteDeleteRecursion(ICollection<Note> notes);

        VideoIndex Create(VideoCreate videoCreate, int userId, bool isAdmin = false);

        VideoEdit GetVideoForEdit(int videoId, int userId, bool isAdmin = false);

        int[][] Save(int videoId, int? seekTo, int? duration,
            string name, string desctiption, string url, string[][] changes,
            NoteCreate[] newNotes, bool finalSave, int userId, bool isAdmin = false);

        VideoMoveWithOrigin MoveVideo(VideoMove data, int userId);

        VideoForConnections GetVideoForConnection(int videoId, int userId);


        void AddExtensionVideo(ExtentionVideoAddData data, int userId);

        ExtentionVideoAddData[] GetExtesionVideos(int userId);

        VideoIndex ConvertExtensionVideo(ConvertExtensionData data, int userId);

        void MakePublic(int videoId, int userId);

        void MakePrivate(int videoId, int userId);

        VideoEdit GetVideoForView(int videoId, int userId, bool isAdmin = false);
    }
}
