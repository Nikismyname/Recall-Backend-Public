namespace Recall.Data.Models.Meta
{
    using Recall.Data.Models.Core;
    using System.Collections.Generic;

    public class Topic
    {
        public Topic()
        {
            this.Videos = new HashSet<VideoTopic>();
            this.Subtopics = new HashSet<Topic>();
            this.Approved = false;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CriteriaForBelonging { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public bool Approved { get; set; }

        public int? ParentTopicId { get; set; }
        public Topic ParentTopic { get; set; }

        public ICollection<Topic> Subtopics { get; set; }
        public ICollection<VideoTopic> Videos { get; set; }
    }
}
