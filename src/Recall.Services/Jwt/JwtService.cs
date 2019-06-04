namespace Recall.Services.Jwt
{
    using Recall.Services.Models.AuthenticationModels;
    using Recall.Services.Utilities;
    using System.Linq;
    using System.Security.Claims;

    public class JwtService: IJwtService
    {
        public JwtUserData ParseData(ClaimsPrincipal claimsPrincipal)
        {
            if (!claimsPrincipal.Identity.IsAuthenticated)
            {
                return null;
            }

            var claims = claimsPrincipal.Claims;

            var result = new JwtUserData
            {
                Role = claims.ToArray().SingleOrDefault(x => x.Type == Constants.RoleType).Value,
                UserId = int.Parse(claims.ToArray().SingleOrDefault(x => x.Type == Constants.UserIdType).Value),
            };

            return result;
        }
    }
}
