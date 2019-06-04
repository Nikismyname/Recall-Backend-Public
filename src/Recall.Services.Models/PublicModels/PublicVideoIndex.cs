namespace Recall.Services.Models.PublicModels
{
    using AutoMapper;
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Core;
    using System.Linq;

    public class PublicVideoIndex: IMapFrom<Video>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public int? Duration { get; set; }

        public bool IsYouTube { get; set; }
        public bool IsVimeo { get; set; }
        public bool IsLocal { get; set; }

        public string Username { get; set; }

        public string CreatedOn { get; set; }
        public string LastAccessed { get; set; }
        public string LastModified { get; set; }
        public int? TimesAccessed { get; set; }
        public int? TimesPublicAccessed { get; set; }
        public int NoteCount { get; set; }

        public string[] Topics { get; set; }

        //public ICollection<Connection> ConnectedTo { get; set; }
        //public ICollection<Connection> ConnectedFrom { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Video, PublicVideoIndex>()
                .ForMember(dest => dest.Topics, src => src.MapFrom(x=>x.Topics.Select(y=>y.Topic.Name).ToArray()))
                .ForMember(dest => dest.Username, src => src.MapFrom(x => x.User.Username))
                .ForMember(dest => dest.NoteCount, src => src.MapFrom(x=>x.Notes.Count))
                .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => x.CreatedOn == null? "None": x.CreatedOn.Value.ToString("yyyy/MM/dd HH:mm")))
                .ForMember(dest => dest.LastAccessed, src => src.MapFrom(x => x.LastAccessed == null ? "None" : x.LastAccessed.Value.ToString("yyyy/MM/dd HH:mm")))
                .ForMember(dest => dest.LastModified, src => src.MapFrom(x => x.LastModified == null ? "None" : x.LastModified.Value.ToString("yyyy/MM/dd HH:mm")));
        }
    }
}
