using MobileDeliveryLogger;

namespace MobileDeliveryGeneral.Settings
{
   
    public class UMDAppConfigOld
    {
        public string AppName { get; set; }
        public string Version { get; set; }
        public string LogPath { get; set; }
        public LogLevel LogLevel {get; set;}
        public string SQLConn { get; set; }
        SocketSettingsOld _srvSet;
        public SocketSettingsOld srvSet { get { if (_srvSet == null) InitSrvSet(); return _srvSet; } set { if (value!=null) _srvSet = value; } }
        public WinsysFilesOld winsysFiles {get; set;}

        public void InitSrvSet()
        {
            _srvSet = new SocketSettingsOld()
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
