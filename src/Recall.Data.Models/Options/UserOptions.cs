namespace Recall.Data.Models.Options
{
    using Recall.Data.Models.Core;
    using Recall.Data.Models.Enums;

    public class UserOptions : INavOptions, IThemeOptions, IVideoOptions
    {
        public UserOptions()
        {
            this.VideoShowPercentageWatched = true;
            this.VideoShowNotesCount = true;
            this.Theme = ThemeType.Slate;
        }

        public int Id { get; set; }

        public User User { get; set; }

        ///OPTIONS>
        public bool VideoShowPercentageWatched { get; set; }

        public bool VideoShowNotesCount { get; set; }

        public ThemeType Theme { get; set; }
    }
}
