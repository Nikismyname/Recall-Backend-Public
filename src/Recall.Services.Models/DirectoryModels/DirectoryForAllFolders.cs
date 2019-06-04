namespace Recall.Services.Models.DirectoryModels
{
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Core;

    public class DirectoryForAllFolders: IMapFrom<Directory>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public int? ParentDirectoryId { get; set; }
    }
}
