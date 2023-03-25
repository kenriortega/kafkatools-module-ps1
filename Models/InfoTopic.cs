using System.Collections.Generic;

namespace KafkaTools.Models
{
    public class TopicInfo
    {
        public  string Name{ get; set; }
        public int NumPartitions { get; set; }
    }
}