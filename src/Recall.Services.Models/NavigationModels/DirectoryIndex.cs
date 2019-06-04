namespace Recall.Services.Models.NavigationModels
{
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Core;
    using System.Collections.Generic;

    public class DirectoryIndex : IMapFrom<Directory>
    {
        public DirectoryIndex()
        {
            this.Videos = new HashSet<VideoIndex>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public int? ParentDirectoryId { get; set; }

        public ICollection<DirectoryChildIndex> Subdirectories { get; set; }

        public ICollection<VideoIndex> Videos { get; set; }
    }
}
