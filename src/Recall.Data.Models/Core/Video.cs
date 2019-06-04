namespace Recall.Data.Models.Core
{
    using Recall.Data.Models.Meta;
    using System;
    using System.Collections.Generic;

    public class Video
    {
        public Video()
        {
            this.Notes = new HashSet<Note>();
            this.ConnectedTo = new HashSet<Connection>();
            this.ConnectedFrom = new HashSet<Connection>();
            this.Topics = new HashSet<VideoTopic>();
            this.Public = false;

            this.CreatedOn = DateTime.Now;
            this.TimesAccessed = 0;
            this.TimesPublicAccessed = 0;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public int? SeekTo { get; set; }

        public int? Duration { get; set; }

        public bool IsYouTube { get; set; }
        public bool IsVimeo { get; set; }
        public bool IsLocal { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public int DirectoryId { get; set; }
        public Directory Directiry { get; set; }

        public bool Public { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? LastAccessed { get; set; }
        public DateTime? LastModified { get; set; }
        public int? TimesAccessed { get; set; }
        public int? TimesPublicAccessed { get; set; }

        public ICollection<Note> Notes { get; set; }

        public ICollection<Connection> ConnectedTo { get; set; }
        public ICollection<Connection> ConnectedFrom { get; set; }

        public ICollection<VideoTopic> Topics { get; set; }
    }
}
