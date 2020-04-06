# MobileDeliveryGeneral
## United Mobile Delivery General Project shared amongst all the services and clients.

   
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


## NuGet Package References

![NuGet Package Model](https://github.com/vergaraunited/Docs/blob/master/imgs/MobileDeliveryModel.jpg)

#### UMDNuGet - Azure Artifact Repository

##### nuget.config file
```xml
<configuration>
  <packageSources>
    <clear />
    <add key="UMDNuget" value="https://pkgs.dev.azure.com/unitedwindowmfg/1e4fcdac-b7c9-4478-823a-109475434848/_packaging/UMDNuget/nuget/v3/index.json" />
  </packageSources>
</configuration>
```
### Refernces
Package Name            |  Version  |  Description
--------------------    |  -------  |  -----------
MobileDeliverySettings  |   1.4.3   |  Mobile Delivery Settings base code for all configurable components with Symbols
MobileDeliveryClient    |   1.4.0   |  Mobile Delivery Client base code for all clients with Symbols
MobileDeliveryCaching   |   1.4.2   |  Mobile Delivery Cachong base code for all cacheabale clients with Symbols


SubDependencies         |  Versoin  | Thus included in Packages
----------------------  |  -------- |  -------------------------
MobileDeliveryLogger    |   1.3.0   |  Mobile Delivery Logger base code for all components with Symbols
MobileDeliveryGeneral   |   1.4.3   |  Mobile Delivery General Code with Symbols


**ToDo**<br/>
**_:x: Built into the docker image based on the settings in the app.config_**<br/>
**_:x: Built into the docker image based on the settings in the app.config_**<br/>
**_:heavy_exclamation_mark: Consolidate into MobileDeliverySettings_**<br/>
