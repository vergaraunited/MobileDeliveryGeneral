# MobileDeliveryGeneral
## United Mobile Delivery General Project shared amongst all the services and clients.

## NuGet Package References
#### UMDNuGet - Azure Artifact Repository

#### Configuratrion settings
```    <add key="LogPath" value="C:\apps\logs" />
    <add key="LogLevel" value="Info" />
    <add key="Url" value="localhost" />
    <add key="Port" value="80" />
    <add key="WinsysUrl" value="localhost" />
    <add key="WinsysPort" value="8181" />
    <add key="UMDUrl" value="localhost" />
    <add key="UMDPort" value="80" />
```

##### nuget.config file
```xml
<configuration>
  <packageSources>
    <clear />
    <add key="UMDNuget" value="https://pkgs.dev.azure.com/unitedwindowmfg/1e4fcdac-b7c9-4478-823a-109475434848/_packaging/UMDNuget/nuget/v3/index.json" />
  </packageSources>
</configuration>
```
