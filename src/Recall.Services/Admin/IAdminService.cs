namespace Recall.Services.Admin
{
    using Recall.Services.Models.AdminModels;

    public interface IAdminService
    {
        void SeedPublicVideos(int userId);

        void DeletePublicTestVideos(int userId);

        UserForAdminView[] GetAllUsers(int userId, bool onlyAdmins = false);

        void PromoteUser(int promoUserId, int userId);

        void DemoteUser(int demoteUserId, int userId);
    }
}
