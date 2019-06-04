namespace Recall.Services.Models.Meta.TopicModels
{
    using AutoMapper;
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Meta;

    public class TopicFolderFrontEnd : IMapFrom<Topic>, IHaveCustomMappings
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public string CriteriaForBelonging { get; set; }

        public int? parentId { get; set; }

        public int Order { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Topic, TopicFolderFrontEnd>().ForMember(
                dest => dest.parentId,
                src => src.MapFrom(x => x.ParentTopicId)
            );
        }
    }
}
