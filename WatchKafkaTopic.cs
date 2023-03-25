using System;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaTools.Models;
using Newtonsoft.Json;

namespace KafkaTools
{
    [Cmdlet(VerbsCommon.Watch, "KafkaTopic")]
    public class WatchKafkaTopic : PSCmdlet
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
        public string TopicsName { get; set; }
        [Parameter(
            Position = 2,
            ValueFromPipelineByPropertyName = true)]
        public string GroupId { get; set; } = "pwsh-consumer";
        [Parameter(
            Position = 3,
            ValueFromPipelineByPropertyName = true)]
        public int OffsetReset { get; set; } = 1;


        protected override void EndProcessing()
        { 
           
         var config = new ConsumerConfig
            {
                BootstrapServers = BootstrapServers,
                GroupId = GroupId,
                EnableAutoOffsetStore = false,
                EnableAutoCommit = true,
                StatisticsIntervalMs = 20*1000,
                SessionTimeoutMs = 10000,
                AutoOffsetReset = OffsetReset == 1 ? AutoOffsetReset.Earliest : AutoOffsetReset.Latest,
                EnablePartitionEof = true,
                // A good introduction to the CooperativeSticky assignor and incremental rebalancing:
                // https://www.confluent.io/blog/cooperative-rebalancing-in-kafka-streams-consumer-ksqldb/
                PartitionAssignmentStrategy = PartitionAssignmentStrategy.CooperativeSticky
            };

            // Note: If a key or value deserializer is not set (as is the case below), the 
            // deserializer corresponding to the appropriate type from Confluent.Kafka.Deserializers
            // will be used automatically (where available). The default deserializer for string
            // is UTF8. The default deserializer for Ignore returns null for all input data
            // (including non-null data).
            using (var consumer = new ConsumerBuilder<Ignore, string>(config)
                // Note: All handlers are called on the main .Consume thread.
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .SetStatisticsHandler((_, json) => LogKafkaStats(json)).Build())
            {
                consumer.Subscribe(TopicsName);

                while (true)
                {
                    var consumeResult = consumer.Consume();
                    if (consumeResult.IsPartitionEOF)
                    {
                        Console.WriteLine(
                            $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");
                        WriteObject(new
                        {
                            consumeResult.Topic,
                            consumeResult.Partition,
                            consumeResult.Offset
                        });
                        continue;
                    }
                    WriteObject(new
                    {
                        consumeResult.TopicPartitionOffset.Partition,
                        consumeResult.Message.Value,
                        consumeResult.Topic,
                        consumeResult.Message.Timestamp.UtcDateTime,
                        consumeResult.Message.Key,
                        consumeResult.Message.Headers
                    });
                    try
                    {
                        // Store the offset associated with consumeResult to a local cache. Stored offsets are committed to Kafka by a background thread every AutoCommitIntervalMs. 
                        // The offset stored is actually the offset of the consumeResult + 1 since by convention, committed offsets specify the next message to consume. 
                        // If EnableAutoOffsetStore had been set to the default value true, the .NET client would automatically store offsets immediately prior to delivering messages to the application. 
                        // Explicitly storing offsets after processing gives at-least once semantics, the default behavior does not.
                        consumer.StoreOffset(consumeResult);
                    }
                    catch (KafkaException e)
                    {
                        Console.WriteLine($"Store Offset error: {e.Error.Reason}");
                        consumer.Close();
                    }
                }

                
            }   
        }
        
        // TODO: Improve this or more better create a new cmd with only stats.
        private void LogKafkaStats(string kafkaStatistics)
        {
            var stats = JsonConvert.DeserializeObject<KafkaStatistics>(kafkaStatistics);

            if (stats?.topics != null && stats.topics.Count > 0)
            {
                foreach (var topic in stats.topics)
                {
                    foreach (var partition in topic.Value.Partitions)
                    {
                        Console.WriteLine(
                            $"Statistics");

                            WriteObject(new
                            {
                                topic.Value.Topic,
                                partition.Value.Partition,
                                partition.Value.ConsumerLag
                            });
                    }
                }
            }
        }
    }

}