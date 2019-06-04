namespace Recall.Data.Models.Core
{
    public class ExtensionAddedVideo
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string  Name { get; set; }

        public string  Type { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
