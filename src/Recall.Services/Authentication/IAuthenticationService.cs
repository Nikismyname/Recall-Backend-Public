namespace Recall.Services.Authentication
{
    using Recall.Services.Models.Authentication;

    public interface IAuthenticationService
    {
        void Register(RegisterData data, bool admin = false);

        UserWithToken Login(LoginData data);
    }
}
