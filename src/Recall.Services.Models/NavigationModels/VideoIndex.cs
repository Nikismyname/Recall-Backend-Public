namespace Recall.Services.Models.NavigationModels
{
    using AutoMapper;
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Core;

    public class VideoIndex: IMapFrom<Video>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int NoteCount { get; set; }

        public int Order { get; set; }

        public int? SeekTo { get; set; }

        public int? Duration { get; set; }

        public bool Public { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Video, VideoIndex>()
                .ForMember(dest => dest.NoteCount, src => src.MapFrom(x => x.Notes.Count));
        }
    }
}
