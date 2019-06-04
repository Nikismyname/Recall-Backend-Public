namespace Recall.Services.Models.DirectoryModels
{
    public class AllItemsFrontEndNaming
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public int Order { get; set; }

        public bool Selected { get; set; }

        public ItemForSelect[] Items { get; set; }
    }
}
