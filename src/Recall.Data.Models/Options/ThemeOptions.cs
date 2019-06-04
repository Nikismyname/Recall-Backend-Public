namespace Recall.Data.Models.Options
{
    using GetReady.Services.Mapping.Contracts;
    using Recall.Data.Models.Enums;

    public class ThemeOptions: IThemeOptions, IMapFrom<UserOptions>
    {
        public ThemeType Theme { get; set; }
    }
}
