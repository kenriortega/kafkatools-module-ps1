# Kafka Tools With PowerShell

Only once create manifiest *powershell* 

`New-ModuleManifest -Path .\PSKafkaTools.psd1 -RootModule KafkaTools.dll`

How to compile this solution for windows | linux | mac select your platform

```bash
dotnet publish -c Release -r win-x64 --self-contained -o ./release/out/KafkaTools
```

Import module in the powershell terminal

```bash
Import-Module  release/out/KafkaTools/KafkaTools.dll
```

Get Information about Module

 `Get-Command -Module KafkaTools`


Remove Module

`Remove-Module KafkaTools`


Resuources

 - Install poweshell on linux or mac