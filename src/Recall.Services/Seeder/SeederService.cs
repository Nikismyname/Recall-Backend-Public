namespace Recall.Services.Seeder
{
    using Recall.Data;
    using Recall.Services.Authentication;
    using Recall.Services.Models.Authentication;

    using System.Linq;

    public class SeederService : ISeederService
    {
        private readonly RecallDbContext context;
        private readonly IAuthenticationService authService;

        public SeederService(RecallDbContext context, IAuthenticationService authService)
        {
            this.context = context;
            this.authService = authService;
        }

        public void Seed123()
        {
            var usersCount = context.Users.Count();

            if (usersCount == 0)
            {
                this.authService.Register(new RegisterData
                {
                    Username = "123",
                    FirstName = "123",
                    LastName = "123",
                    Password = "123",
                    RepeatPassword = "123",
                });
            }
        }

        public void SeedAdmin()
        {
            var adminUsersCount = context.Users.Where(x => x.Role == "Admin").Count();

            if (adminUsersCount == 0)
            {
                this.authService.Register(new RegisterData
                {
                    Username = "admin",
                    FirstName = "Admin",
                    LastName = "Adminov",
                    Password = "admin",
                    RepeatPassword = "admin",
                }, true);
            }
        }
    }
}
