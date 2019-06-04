namespace Recall.Services.Models.NavigationModels
{
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Core;

    public class DirectoryCreate: IMapTo<Directory>
    {
        public int ParentDirectoryId { get; set; }

        public string Name { get; set; }
    }
}
