namespace Recall.Data.Models.Meta
{
    using Recall.Data.Models.Core;
    using Recall.Data.Models.Enums;

    public class Connection
    {
        public int VideoOneId { get; set; }
        public Video VideoOne { get; set; }

        public int VideoTwoId { get; set; }
        public Video VideoTwo { get; set; }

        public string Name { get; set; }

        public ConnectionType Type { get; set; }

        /// <summary>
        /// 1 to 10
        /// </summary>
        public int Strength { get; set; }
    }
}
