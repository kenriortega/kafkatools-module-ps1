using System;
using System.Management.Automation;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace KafkaTools
{
    [Cmdlet(VerbsCommon.New, "Topic")]
    [OutputType(typeof(TopicModel))]
    public class CreateTopicCmdlet : PSCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipelineByPropertyName = true)]
        public string BootstrapServers { get; set; } = "localhost:9092";

        [Parameter(
            Position = 1,
            ValueFromPipelineByPropertyName = true)]
        public string TopicName { get; set; } = "myTopic";

        [Parameter(
            Position = 2,
            ValueFromPipelineByPropertyName = true)]
        public int NumPartitions { get; set; } = 1;

        [Parameter(
            Position = 3,
            ValueFromPipelineByPropertyName = true)]
        public short ReplicationFactor { get; set; } = 1;

        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
        protected override async void BeginProcessing(){
            WriteVerbose("Begin!");
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override async void ProcessRecord(){
            var adminClient =
                new AdminClientBuilder(new AdminClientConfig {BootstrapServers = BootstrapServers}).Build();
            try {
                // TODO: get this result
                await adminClient.CreateTopicsAsync(new TopicSpecification[] {
                    new TopicSpecification
                        {Name = TopicName, ReplicationFactor = ReplicationFactor, NumPartitions = NumPartitions}
                });

                WriteObject(new TopicModel {
                    BootstrapServers = BootstrapServers,
                    Name = TopicName,
                    NumPartitions = NumPartitions,
                    ReplicationFactor = ReplicationFactor
                });
            }
            catch (CreateTopicsException e) {
                Console.WriteLine(
                    $"An error occurred creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }
        }

        // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
        protected override void EndProcessing(){
            // todo: close connections...
            WriteVerbose("End!");
        }
    }

    public class TopicModel
    {
        public string BootstrapServers { get; set; }
        public string Name { get; set; }
        public int NumPartitions { get; set; }
        public short ReplicationFactor { get; set; }
    }
}