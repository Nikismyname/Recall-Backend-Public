namespace Recall.Services.Models.DirectoryModels
{
    public class AllFoldersFrontEndNaming
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? parentId { get; set; }

        public int Order { get; set; }
    }
}
