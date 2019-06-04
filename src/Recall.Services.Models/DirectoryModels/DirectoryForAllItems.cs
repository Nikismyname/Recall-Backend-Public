namespace Recall.Services.Models.DirectoryModels
{
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Core;

    public class DirectoryForAllItems: IMapFrom<Directory>
    {
        public DirectoryForAllItems()
        {
            this.Selected = false;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentDirectoryId { get; set; }

        public int Order { get; set; }

        public ItemForSelect[] Videos { get; set; }

        public bool Selected { get; set; }
    }
}
