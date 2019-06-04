namespace Recall.Services.Models.VideoModels
{
    public class VideoMoveWithOrigin
    {
        public int videoId { get; set; }
        public int newDirectoryId { get; set; }
        public int OriginDirectory { get; set; }
    }
}
