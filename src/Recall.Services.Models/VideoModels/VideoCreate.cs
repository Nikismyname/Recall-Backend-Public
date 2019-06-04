namespace Recall.Services.Models.VideoModels
{
    using System.ComponentModel.DataAnnotations;

    public class VideoCreate
    {
        public int DirectoryId { get; set; }

        //[StringLength(40, MinimumLength = 3, ErrorMessage = "The Video Notes length must be between 3 and 40 characters!")]
        public string Name { get; set; }

        public string Url { get; set; }

        //Only One Should Be Selected
        public bool IsYouTube { get; set; }

        public bool IsVimeo { get; set; }

        public bool IsLocal { get; set; }

        public string Description { get; set; }
    }
}
