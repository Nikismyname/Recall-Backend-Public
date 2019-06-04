namespace Recall.Services.Models.AdminModels
{
    using AutoMapper;
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Core;
    using System.Linq;

    public class UserForAdminView : IMapFrom<User>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public int? RootDirectoryId { get; set; }

        public int DirectoriesCount { get; set; }

        public int VideosCount { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<User, UserForAdminView>()
                .ForMember(dest => dest.DirectoriesCount, src => src.MapFrom(x => x.Directories.Count))
                .ForMember(dest => dest.VideosCount, src => src.MapFrom(x => x.Directories.SelectMany(y => y.Videos).Count()));
        }
    }
}
