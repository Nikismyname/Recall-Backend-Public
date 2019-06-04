namespace Recall.Services.Models.OptionsModels
{
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Enums;
    using Recall.Data.Models.Options;

    public class AllOptions: INavOptions, IThemeOptions, IVideoOptions, IMapFrom<UserOptions>
    {
        public bool VideoShowPercentageWatched { get; set; }

        public bool VideoShowNotesCount { get; set; }

        public ThemeType Theme { get; set; }
    }
}
