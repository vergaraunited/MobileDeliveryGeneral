using MobileDeliveryLogger;

namespace UMDGeneral.Settings
{
   
    public class UMDAppConfig
    {
        public string AppName { get; set; }
        public string Version { get; set; }
        public string LogPath { get; set; }
        public LogLevel LogLevel {get; set;}
        public string SQLConn { get; set; }
        public SocketSettings srvSet { get; set; }
        public WinsysFiles winsysFiles {get; set;}

        public void InitSrvSet()
        {
            srvSet = new SocketSettings()
            {
                name = AppName,
                port = 81,
                url = "localhost",
                umdport = 81,
                umdurl = "localhost",
                WinSysPort = 8181,
                WinSysUrl = "localhost"
            };            
        }
    }
}
