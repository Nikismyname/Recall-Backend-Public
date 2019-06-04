namespace Recall.Services.Models.Meta.TopicModels
{
    public class TopicCreate
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string CriteriaForBelonging { get; set; }

        public int? ParentTopicId { get; set; }
    }
}
