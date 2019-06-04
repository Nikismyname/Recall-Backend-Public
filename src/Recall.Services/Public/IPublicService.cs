namespace Recall.Services.Public
{
    using Recall.Services.Models.PublicModels;

    public interface IPublicService
    {
        PublicVideoIndex[] GetLatest(int page);
    }
}
