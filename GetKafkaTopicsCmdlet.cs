using System;
using System.Management.Automation;
using Confluent.Kafka;
using KafkaTools.Models;

namespace KafkaTools
{
    [Cmdlet(VerbsCommon.Get, "KafkaTopics")]
    public class GetKafkaTopics : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipelineByPropertyName = true)]
        public string BootstrapServers { get; set; }


        protected override void EndProcessing()
        {

            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = BootstrapServers }).Build())
            {
                var meta = adminClient.GetMetadata(TimeSpan.FromSeconds(20));

                foreach (var item in meta.Topics)
                {
                    WriteObject(new TopicInfo
                    {
                        Name = item.Topic,
                        NumPartitions = item.Partitions.Count,
                    });
                }
            }
        }
    }

}