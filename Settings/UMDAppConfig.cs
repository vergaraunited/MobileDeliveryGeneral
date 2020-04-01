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
        SocketSettings _srvSet;
        public SocketSettings srvSet { get { if (_srvSet == null) InitSrvSet(); return _srvSet; } set { if (value!=null) _srvSet = value; } }
        public WinsysFiles winsysFiles {get; set;}

        public void InitSrvSet()
        {
            _srvSet = new SocketSettings()
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
