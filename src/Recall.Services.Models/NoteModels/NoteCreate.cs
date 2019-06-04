namespace Recall.Services.Models.NoteModels
{
    using AutoMapper;
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Core;
    using Recall.Data.Models.Enums;

    public class NoteCreate: IMapFrom<Note>, IMapTo<Note>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int? SeekTo { get; set; }

        //public Formatting Formatting { get; set; }

        public NoteType Type { get; set; }

        public int InPageId { get; set; }

        public int? InPageParentId { get; set; }

        public int BorderThickness { get; set; }

        /// Colors
        public string BorderColor { get; set; }

        public string BackgroundColor { get; set; }

        public string TextColor { get; set; }
        ///...

        /// <summary>
        ///0 means that it is not root and has parent that is not in the db, -1 means root!
        /// </summary>
        public int? ParentDbId { get; set; }

        /// <summary>
        /// 1 is root!
        /// </summary>
        public int Level { get; set; }

        public bool Deleted { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<NoteCreate, Note>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.InPageId));

            configuration.CreateMap<Note, NoteCreate>()
                .ForMember(dest => dest.InPageId, opt => opt.MapFrom(x => x.Order));
        }
    }
}
