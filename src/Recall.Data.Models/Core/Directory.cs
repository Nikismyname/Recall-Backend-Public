namespace Recall.Data.Models.Core
{
    using System.Collections.Generic;

    public class Directory
    {
        public Directory()
        {
            this.Videos = new HashSet<Video>();
            this.Subdirectories = new HashSet<Directory>();
        }
        
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public int NumberOfSubdirectories { get; set; }

        public int NumberOfVideos { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public User RootUser { get; set; }

        public int? ParentDirectoryId { get; set; }
        public Directory ParentDirectory { get; set; }

        public ICollection<Video> Videos { get; set; }

        public ICollection<Directory> Subdirectories { get; set; }
    }
}
