namespace Recall.Services.Options
{
    using Recall.Services.Models.OptionsModels;

    public interface IOptionsService
    {
        void CreateOptionsForUser(int userId);

        AllOptions GetAllOptions(int userId);

        void SaveOptions(AllOptions incOptions, int userId);
    }
}
