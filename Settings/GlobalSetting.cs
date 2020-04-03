using MobileDeliveryGeneral.Settings;

namespace MobileDeliveryGeneral.Settings
{
    public static class GlobalSetting
    {
        static UMDAppConfig _config;
        public static UMDAppConfig Config
        {
            get { return _config; }
            set
            {
                _config = value;
                UpdateConfig(_config);
            }
        }  

        private static void UpdateConfig(UMDAppConfig config)
        {
            if (config.srvSet == null)
            {
                config.srvSet = new SocketSettings()
                {
                    name = "defaultName",
                    port = 81,
                    url = "localhost",
                    umdport=81,
                    umdurl="localhost",
                    WinSysPort = 8181,
                    WinSysUrl = "localhost"
                };
            }
            if (config.winsysFiles == null)
            {
                config.winsysFiles = new WinsysFiles()
                {
                    WinsysDstFile = MobileDeliveryGeneral.Utilities.HomeDirectoryPaths.GetUserHome(config.AppName),
                    WinsysSrcFile = @"\\Fs01\vol1\Winsys32\DATA"
                };
            }

            _config = new UMDAppConfig()
            {
                AppName = config.AppName,
                LogLevel = config.LogLevel,
                LogPath = config.LogPath,
                SQLConn = config.SQLConn,
                srvSet = new SocketSettings()
                {
                    name = config.srvSet.name,
                    port = config.srvSet.port,
                    url = config.srvSet.url,
                    umdurl= config.srvSet.umdurl,
                    umdport=config.srvSet.umdport,
                    WinSysPort = config.srvSet.WinSysPort,
                    WinSysUrl = config.srvSet.WinSysUrl
                },

                Version = config.Version,

                winsysFiles = new WinsysFiles() {  WinsysDstFile= config.winsysFiles.WinsysDstFile, WinsysSrcFile=config.winsysFiles.WinsysSrcFile}
            };
        }
    }
}
