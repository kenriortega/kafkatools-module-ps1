# Kafka Tools With PowerShell

Install Module Go to PSGallery

Import module in the powershell terminal

```bash
Install-Module -Name KafkaTools

# or

Find-Module -Name KafkaTools | Install-Module -Force
```
Then import this module

```bash
Import-Module Kafkatools
```

Get Information about Module

 ```bash
 Get-Command -Module KafkaTools
 ```

Remove Module

 ```bash
 Remove-Module -Module KafkaTools
 ```

 ### Usage

 Create Topic
 ```bash
 New-KafkaTopic -BootstrapServers localhost:9092 -TopicName "name-topic"
 ```

Get Topics
 ```bash
 Get-KafkaTopics -BootstrapServers localhost:9092
 ```

Write Msg to an specific Topic

 ```bash
 Write-KafkaMsg -BootstrapServers localhost:9092 -TopicName "topic-name" -Key "sd" -Value "value"
 ```

Watch Topic 

 ```bash
  Watch-KafkaTopic -BootstrapServers localhost:9092 -TopicName "topic-name"
 ```
 

Watch Stats by Topic 

 ```bash
 Watch-KafkaStatsTopic -BootstrapServers localhost:9092 -TopicName "topic-name"
 ```
