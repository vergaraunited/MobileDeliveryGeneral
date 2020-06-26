using MobileDeliveryGeneral.Settings;

namespace MobileDeliveryGeneral.Settings
{
    public static class GlobalSetting
    {
        static UMDAppConfigOld _config;
        public static UMDAppConfigOld Config
        {
            get { return _config; }
            set
            {
                _config = value;
                UpdateConfig(_config);
            }
        }  

        private static void UpdateConfig(UMDAppConfigOld config)
        {
            if (config.srvSet == null)
            {
                config.srvSet = new SocketSettingsOld()
                {
                    name = "defaultName",
                    port = 81,
                    url = "localhost",
                    srvport=81,
                    srvurl="localhost",
                    clientport = 8181,
                    clienturl = "localhost"
                    //keepalive =
                    //rety =   
                };
            }
            if (config.winsysFiles == null)
            {
                config.winsysFiles = new WinsysFilesOld()
                {
                    WinsysDstFile = MobileDeliveryGeneral.Utilities.HomeDirectoryPaths.GetUserHome(config.AppName),
                    WinsysSrcFile = @"\\Fs01\vol1\Winsys32\DATA"
                };
            }

            _config = new UMDAppConfigOld()
            {
                AppName = config.AppName,
                LogLevel = config.LogLevel,
                LogPath = config.LogPath,
                SQLConn = config.SQLConn,
                srvSet = new SocketSettingsOld()
                {
                    name = config.srvSet.name,
                    port = config.srvSet.port,
                    url = config.srvSet.url,
                    srvurl= config.srvSet.srvurl,
                    srvport=config.srvSet.srvport,
                    clientport = config.srvSet.clientport,
                    clienturl = config.srvSet.clienturl
                },

                Version = config.Version,

                winsysFiles = new WinsysFilesOld() {  WinsysDstFile= config.winsysFiles.WinsysDstFile, WinsysSrcFile=config.winsysFiles.WinsysSrcFile}
            };
        }
    }
}
