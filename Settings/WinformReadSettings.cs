using MobileDeliveryLogger;
using Plugin.Settings.Abstractions;
using System;
using System.Configuration;

namespace MobileDeliveryGeneral.Settings
{
    public static class WinformReadSettings
    {
        public static UMDAppConfig GetSettings(Type type)
        {
            string logfilepath = ConfigurationManager.AppSettings["LogPath"];
            string loglevel = ConfigurationManager.AppSettings["LogLevel"];
            string port = ConfigurationManager.AppSettings["Port"];
            string url = ConfigurationManager.AppSettings["Url"];
            string srvport = ConfigurationManager.AppSettings["SrvPort"];
            string srvurl = ConfigurationManager.AppSettings["SrvUrl"];
            string clientport = ConfigurationManager.AppSettings["ClientPort"];
            string clienturl = ConfigurationManager.AppSettings["ClientUrl"];
            string sqlconn = ConfigurationManager.AppSettings["SQLConn"];

            string keepAlive = ConfigurationManager.AppSettings["KeepAliveInterval"];
            string retry = ConfigurationManager.AppSettings["RetryInterval"];
            string recontimeout = ConfigurationManager.AppSettings["ReconTimeOut"];
            string errrecontimeout = ConfigurationManager.AppSettings["ErrReconTimeOut"];
            
            string WinsysSrcFile = ConfigurationManager.AppSettings["WinsysSrcFilePath"];
            string WinsysDstFile = ConfigurationManager.AppSettings["WinsysDstFilePath"];
            string TPSFileNamesToCopy = ConfigurationManager.AppSettings["WinsysTPSFiles "];

            if (WinsysSrcFile == null || WinsysSrcFile.Length == 0)
                WinsysSrcFile = @"\\Fs01\vol1\Winsys32\DATA";
            
            if (WinsysDstFile == null || WinsysDstFile.Length == 0)
                WinsysDstFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (TPSFileNamesToCopy == null || TPSFileNamesToCopy.Length == 0)
                TPSFileNamesToCopy = @"";

            ushort uport;
            ushort.TryParse(port, out uport);

            ushort usrvport;
            ushort.TryParse(srvport, out usrvport);

            ushort uclientport;
            ushort.TryParse(clientport, out uclientport);


            ushort ukeepalive;
            ushort.TryParse(keepAlive, out ukeepalive);
            ushort uretry;
            ushort.TryParse(retry, out uretry);
            ushort urecontimeout;
            ushort.TryParse(recontimeout, out urecontimeout);
            ushort uerrrecontimeout;
            ushort.TryParse(errrecontimeout, out uerrrecontimeout);
            if (ukeepalive == 0)
                ukeepalive = 60000;
            if (uretry == 0)
                uretry = 60000;
            if (urecontimeout == 0)
                urecontimeout = 60000;
            if (uerrrecontimeout == 0)
                uerrrecontimeout = 60000;

            LogLevel eloglevel;

            if (!Enum.TryParse<LogLevel>(loglevel, out eloglevel))
                eloglevel = LogLevel.Info;

            UMDAppConfig config = new UMDAppConfig()
            {
                AppName = type.Assembly.GetName().Name,
                Version = type.Assembly.GetName().Version.ToString(),
                srvSet = new SocketSettings()
                {
                    name = type.Assembly.GetName().Name,
                    port = uport,
                    url = url,
                    srvport = usrvport,
                    srvurl = srvurl,
                    clientport = uclientport,
                    clienturl = clienturl,
                    keepalive = ukeepalive,
                    retry = uretry,
                    recontimeout = urecontimeout,
                    errrecontimeout = uerrrecontimeout
                },
                SQLConn = sqlconn,
                winsysFiles = new WinsysFiles() { WinsysDstFile = WinsysDstFile, WinsysSrcFile = WinsysSrcFile },
                LogPath = logfilepath,
                LogLevel = eloglevel
            };
            return config;
        }
    }

}
