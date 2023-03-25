using System;
using System.Management.Automation;
using Confluent.Kafka;
using KafkaTools.Models;

namespace KafkaTools
{
    [Cmdlet(VerbsCommunications.Write, "KafkaMsg")]
    public class WriteKafkaMsg : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipelineByPropertyName = true)]
        public string BootstrapServers { get; set; }
        [Parameter(
             Mandatory = true,
             Position = 1,
             ValueFromPipelineByPropertyName = true)]
        public string TopicName { get; set; }
        [Parameter(
            Position = 2,
            ValueFromPipelineByPropertyName = true)]
        public string Key { get; set; } = "";
        [Parameter(
        Mandatory = true,
        Position = 3,
        ValueFromPipelineByPropertyName = true)]
        public string Value { get; set; }
        protected override void EndProcessing()
        {

            var config = new ProducerConfig { BootstrapServers = BootstrapServers };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                var deliveryReport = producer.ProduceAsync(
                    TopicName, new Message<string, string> { Key = Key, Value = Value });
                WriteObject(new DeliveryReport
                {
                    TopicName = deliveryReport.Result.Topic,
                    Partition = deliveryReport.Result.Partition,
                    Offset = deliveryReport.Result.Offset,
                    Status = deliveryReport.Result.Status,
                    Key = deliveryReport.Result.Key,
                    Value = deliveryReport.Result.Value,
                    Timestamp = deliveryReport.Result.Timestamp.UtcDateTime
                });
            }
        }
    }

}