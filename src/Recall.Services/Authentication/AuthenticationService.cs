#region INIT
namespace Recall.Services.Authentication
{
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Recall.Data;
    using Recall.Data.Models.Core;
    using Recall.Services.Navigation;
    using Recall.Services.Exceptions;
    using Recall.Services.Models.Authentication;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using Recall.Services.Directories;
    using Recall.Services.Utilities;
    using Recall.Services.Options;

    public class AuthenticationService : IAuthenticationService
    {
        const int HashingIterationsCount = 1000;
        const int SaltNumberOfBytes = 128 / 8;
        const int PasswordNumberOfBytes = 256 / 8;
        const KeyDerivationPrf Algorithm = KeyDerivationPrf.HMACSHA1;

        private readonly RecallDbContext context;
        private readonly IConfiguration configuration;
        private readonly INavigationService navigationService;
        private readonly IDirectoryService dirService;
        private readonly IOptionsService optionsService;

        public AuthenticationService(
            RecallDbContext context, 
            IConfiguration configuration,
            INavigationService navigationService, 
            IDirectoryService dirService, 
            IOptionsService optionsService)
        {
            this.context = context;
            this.configuration = configuration;
            this.navigationService = navigationService;
            this.dirService = dirService;
            this.optionsService = optionsService;
        }

        #endregion

        #region REGISTER
        public void Register(RegisterData data, bool admin = false)
        {
            if (data.Password != data.RepeatPassword)
            {
                throw new ServiceException("Password and Repeate Password must match");
            };

            var existingUser = context.Users
                .SingleOrDefault(x => x.Username == data.Username);

            if (existingUser != null)
            {
                throw new ServiceException("User with the given name already Exists!");
            }

            byte[] salt = new byte[SaltNumberOfBytes];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var saltString = Convert.ToBase64String(salt);

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2
                (
                    password: data.Password,
                    salt: salt,
                    prf: Algorithm,
                    iterationCount: HashingIterationsCount,
                    numBytesRequested: PasswordNumberOfBytes
                 )
            );

            var user = new User()
            {
                Username = data.Username,
                FirstName = data.FirstName,
                LastName = data.LastName,
                HashedPassword = hashedPassword,
                Salt = saltString,
                Role = admin ? "Admin" : "User",
            };
            context.Users.Add(user);
            context.SaveChanges();

            var rootDir = new Directory
            {
                Name = Constants.RootDorectoryName,
                UserId = user.Id,
                RootUser = user,
            };
            context.Directories.Add(rootDir);
            context.SaveChanges();

            user.RootDirectoryId = rootDir.Id;
            context.SaveChanges();
        }
        #endregion

        #region LOGIN
        public UserWithToken Login(LoginData data)
        {
            var user = context.Users.SingleOrDefault(x => x.Username == data.Username);
            if (user == null)
            {
                throw new ServiceException("Invalid username or password!");
            }

            var salt = Convert.FromBase64String(user.Salt);
            var hashedPassword = user.HashedPassword;

            string hashedIncomingPassword = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: data.Password,
                    salt: salt,
                    prf: Algorithm,
                    iterationCount: HashingIterationsCount,
                    numBytesRequested: PasswordNumberOfBytes
                )
            );

            if (hashedPassword != hashedIncomingPassword)
            {
                throw new ServiceException("Invalid username or password!");
            }

            var token = GenerateToken(user);

            var options = this.optionsService.GetAllOptions(user.Id);

            var userWithToken = new UserWithToken
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role == null ? "User" : user.Role,
                Token = token,
                RootDirectoryId = user.RootDirectoryId.Value,
                Options = options,
            };

            return userWithToken;
        }
        #endregion

        #region GENERATE_TOKEN
        private string GenerateToken(User user)
        {
            var signingKey = Convert.FromBase64String(configuration["Jwt:SigningSecret"]);
            var expiryDuration = int.Parse(configuration["Jwt:ExpiryDuration"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,              // Not required as no third-party is involved
                Audience = null,            // Not required as no third-party is involved
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
                Subject = new ClaimsIdentity(new List<Claim> {
                        new Claim("userid", user.Id.ToString()),
                        new Claim("role", user.Role == null? "User": user.Role)
                    }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = jwtTokenHandler.WriteToken(jwtToken);
            return token;
        }
        #endregion
    }
}
