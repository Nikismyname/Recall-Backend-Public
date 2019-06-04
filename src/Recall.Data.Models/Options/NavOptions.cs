namespace Recall.Data.Models.Options
{
    using GetReady.Services.Mapping.Contracts;

    public class NavOptions: INavOptions, IMapFrom<UserOptions>
    {
        public bool VideoShowPercentageWatched { get; set; }

        public bool VideoShowNotesCount { get; set; }
    }
}
