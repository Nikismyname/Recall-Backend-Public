namespace Recall.Data.Models.Core
{
    using Recall.Data.Models.Meta;
    using Recall.Data.Models.Options;
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
            this.Directories = new HashSet<Directory>();
            this.ExtensionAddedVideos = new HashSet<ExtensionAddedVideo>();
            this.MyTopics = new HashSet<Topic>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string HashedPassword { get; set; }

        public string Salt { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public int? RootDirectoryId { get; set; }

        public int? UserOptionsId { get; set; }
        public UserOptions UserOptions { get; set; }

        public Directory RootDirectory { get; set; }

        public ICollection<Directory> Directories { get; set; } 

        public ICollection<ExtensionAddedVideo> ExtensionAddedVideos { get; set; }

        public ICollection<Topic> MyTopics { get; set; }
    }
}
