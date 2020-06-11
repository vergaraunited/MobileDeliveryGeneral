using MobileDeliveryLogger;

namespace MobileDeliveryGeneral.Settings
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
                srvport = 81,
                srvurl = "localhost",
                clientport = 8181,
                clienturl = "localhost",
                keepalive = 600,
                retry = 6000
            };            
        }
    }
}
