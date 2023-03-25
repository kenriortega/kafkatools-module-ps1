using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaTools.Models;

namespace KafkaTools
{
    [Cmdlet(VerbsCommon.New, "KafkaTopic")]
    public class NewKafkaTopic : PSCmdlet
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
        public int NumPartitions { get; set; }
        [Parameter(
            Position = 3,
            ValueFromPipelineByPropertyName = true)]
        public short ReplicationFactor { get; set; }

        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {

        }

        protected override void EndProcessing()
        {

            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = BootstrapServers }).Build())
            {
                adminClient.CreateTopicsAsync(new TopicSpecification[] {
                        new TopicSpecification { Name = TopicName, ReplicationFactor = ReplicationFactor, NumPartitions = NumPartitions } });

                WriteObject(new CreateTopic
                {
                    BootstrapServers = BootstrapServers,
                    Name = TopicName,
                    NumPartitions = NumPartitions,
                    ReplicationFactor = ReplicationFactor,
                    ErrorMessage = ""
                });
            }

        }
    }

}