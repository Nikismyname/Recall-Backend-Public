namespace Recall.Data.Models.Meta
{
    using Recall.Data.Models.Core;

    public class VideoTopic
    {
        public int  TopicId { get; set; }
        public Topic Topic { get; set; }

        public int VideoId { get; set; }
        public Video Video { get; set; }

        /// <summary>
        /// 1 to 10
        /// </summary>
        public int Adherence { get; set; }
    }
}
