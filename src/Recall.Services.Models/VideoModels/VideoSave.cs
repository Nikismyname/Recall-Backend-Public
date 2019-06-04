namespace Recall.Services.Models.VideoModels
{
    using Recall.Services.Models.NoteModels;

    public class VideoSave
    {
        public int videoId;

        public int? seekTo;

        public int? duration;

        public string name;

        public string description;

        public string[][] changes;

        public NoteCreate[] newItems;

        public bool finalSave;

        public string url;
    }
}
