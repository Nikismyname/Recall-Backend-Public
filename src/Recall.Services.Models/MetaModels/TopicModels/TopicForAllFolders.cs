namespace Recall.Services.Models.Meta.TopicModels
{
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Meta;

    public class TopicForAllFolders: IMapFrom<Topic>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public int? ParentTopicId { get; set; }

        public string Description { get; set; }
        
        public string CriteriaForBelonging { get; set; }
    }
}
