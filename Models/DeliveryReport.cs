namespace KafkaTools.Models
{
    public class DeliveryReport
    {
        public string TopicName { get; set; }
        public Confluent.Kafka.Partition Partition { get; set; }
        public Confluent.Kafka.Offset Offset { get; set; }
        public Confluent.Kafka.PersistenceStatus Status { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public System.DateTime Timestamp { get; set; }
    }
}