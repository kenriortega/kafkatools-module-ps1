using System;
using System.Management.Automation;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace KafkaTools
{
    [Cmdlet(VerbsCommon.New, "Topic")]
    public class CreateTopicCmdlet : PSCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipelineByPropertyName = true)]
        public string BootstrapServers { get; set; }

        [Parameter(
            Position = 1,
            ValueFromPipelineByPropertyName = true)]
        public string TopicName { get; set; }

        [Parameter(
            Position = 2,
            ValueFromPipelineByPropertyName = true)]
        public int NumPartitions { get; set; }

        [Parameter(
            Position = 3,
            ValueFromPipelineByPropertyName = true)]
        public short ReplicationFactor { get; set; }


        protected override void EndProcessing()
        {

            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = BootstrapServers }).Build())
            {
                try
                {
                    adminClient.CreateTopicsAsync(new TopicSpecification[] {
                        new TopicSpecification { Name = TopicName, ReplicationFactor = ReplicationFactor, NumPartitions = NumPartitions } });

                    WriteObject(new TopicModel
                    {
                        BootstrapServers = BootstrapServers,
                        Name = TopicName,
                        NumPartitions = NumPartitions,
                        ReplicationFactor = ReplicationFactor,
                        TopicCreated = true,
                        ErrorMessage = ""
                    });
                }
                catch (CreateTopicsException e)
                {
                    Console.WriteLine($"An error occurred creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");

                    WriteObject(new TopicModel
                    {
                        BootstrapServers = BootstrapServers,
                        Name = TopicName,
                        NumPartitions = NumPartitions,
                        ReplicationFactor = ReplicationFactor,
                        TopicCreated = false,
                        ErrorMessage = e.ToString()
                    });

                }
            }

        }
    }

    public class TopicModel
    {
        public string BootstrapServers { get; set; }
        public string Name { get; set; }
        public int NumPartitions { get; set; }
        public short ReplicationFactor { get; set; }

        public bool TopicCreated { get; set; }
        public string ErrorMessage { get; set; }
    }
}