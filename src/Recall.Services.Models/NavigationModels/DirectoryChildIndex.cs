namespace Recall.Services.Models.NavigationModels
{
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Core;

    public class DirectoryChildIndex: IMapFrom<Directory> 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberOfSubdirectories { get; set; }

        public int NumberOfVideos { get; set; }

        public int Order { get; set; }
    }
}
