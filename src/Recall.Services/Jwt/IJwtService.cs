namespace Recall.Services.Jwt
{
    using Recall.Services.Models.AuthenticationModels;
    using System.Security.Claims;

    public interface IJwtService
    {
        JwtUserData ParseData(ClaimsPrincipal claimsPrincipal);
    }
}
