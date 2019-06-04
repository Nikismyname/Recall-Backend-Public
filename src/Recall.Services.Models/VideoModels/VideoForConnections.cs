namespace Recall.Services.Models.VideoModels
{
    using AutoMapper;
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Core;
    using Recall.Services.Models.Meta.TopicModels;
    using System.Collections.Generic;
    using System.Linq;

    public class VideoForConnections : IMapFrom<Video>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public int? SeekTo { get; set; }
        public int? Duration { get; set; }

        public bool IsYouTube { get; set; }
        public bool IsVimeo { get; set; }
        public bool IsLocal { get; set; }

        public string  Username { get; set; }

        public int NoteCount { get; set; }

        //TODO: Add Connections

        public TopicFolderFrontEnd[] Topics { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Video, VideoForConnections>()
                .ForMember(
                    dest => dest.Topics,
                    src => src.MapFrom(x => x.Topics.Select(y => y.Topic))
                )
                .ForMember(
                    dest => dest.NoteCount,
                    src => src.MapFrom(x => x.Notes.Count())
                )
                .ForMember(
                    dest => dest.Username,
                    src => src.MapFrom(x => x.User.Username)
                );
        }
    }
}
