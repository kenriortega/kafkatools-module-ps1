namespace KafkaTools.Models
{
    public class CreateTopic
    {
        public string BootstrapServers { get; set; }
        public string Name { get; set; }
        public int NumPartitions { get; set; }
        public short ReplicationFactor { get; set; }

        public string ErrorMessage { get; set; }
    }
}