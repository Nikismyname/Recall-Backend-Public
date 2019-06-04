namespace Recall.Services.Models.Authentication
{
    using Recall.Services.Models.OptionsModels;

    public class UserWithToken
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }

        public int  RootDirectoryId { get; set; }

        public AllOptions Options { get; set; } 
    }
}
