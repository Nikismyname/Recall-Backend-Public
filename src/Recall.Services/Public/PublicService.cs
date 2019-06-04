namespace Recall.Services.Public
{
    using GetReady.Services.Mapping;
    using Recall.Data;
    using Recall.Services.Models.PublicModels;
    using System.Linq;

    public class PublicService: IPublicService
    {
        const int numberOfVideosToSend = 12;

        private readonly RecallDbContext context;

        public PublicService(RecallDbContext context)
        {
            this.context = context;
        }

        public PublicVideoIndex[] GetLatest(int page)
        {
            return context.Videos
                .Where(x => x.Public == true)
                .OrderByDescending(x=>x.CreatedOn)
                .Skip(page * numberOfVideosToSend)
                .Take(numberOfVideosToSend)
                .To<PublicVideoIndex>()
                .ToArray();
        }
    }
}
