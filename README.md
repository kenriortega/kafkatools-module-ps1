# Kafka Tools With PowerShell


> dotnet build

> Import-Module ./bin/Debug/netstandard2.0/KafkaTools.dll


> Get-Command -Module KafkaTools


>  New-Topic -? 

> Remove-Module KafkaTools

> New-ModuleManifest -Path .\PSKafkaTools.psd1 -RootModule KafkaTools.dll