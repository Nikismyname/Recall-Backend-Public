namespace Recall.Services.Models.VideoModels
{
    using Recall.Services.Models.NoteModels;
    using System.Collections.Generic;

    public class VideoEdit
    {
        public VideoEdit()
        {
            Notes = new List<NoteCreate>();
        }

        public int Id { get; set; }

        public int DirectoryId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int? SeekTo { get; set; }

        public int? Duration { get; set; }

        public string Description { get; set; }

        public bool IsYouTube { get; set; }

        public bool IsVimeo { get; set; }

        public bool IsLocal { get; set; }

        public List<NoteCreate> Notes { get; set; }
    }
}
