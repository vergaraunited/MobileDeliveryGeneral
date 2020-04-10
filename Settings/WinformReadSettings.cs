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
            string srvport = ConfigurationManager.AppSettings["UmdPort"];
            string srvurl = ConfigurationManager.AppSettings["UmdUrl"];
            string winsysport = ConfigurationManager.AppSettings["WinsysPort"];
            string winsysurl = ConfigurationManager.AppSettings["WinsysUrl"];
            string sqlconn = ConfigurationManager.AppSettings["SQLConn"];

            string WinsysSrcFile = ConfigurationManager.AppSettings["WinsysSrcFilePath"];
            string WinsysDstFile = ConfigurationManager.AppSettings["WinsysDstFilePath"];

            ushort wport;
            ushort.TryParse(winsysport, out wport);

            ushort uport;
            ushort.TryParse(port, out uport);

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
                    srvport = uport,
                    srvurl = url,
                    clientport = wport,
                    clienturl = winsysurl
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
