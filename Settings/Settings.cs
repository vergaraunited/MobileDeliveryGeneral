// Helpers/Settings.cs
using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using MobileDeliveryGeneral.Definitions;
using MobileDeliveryGeneral.Events;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;

//namespace MobileDeliveryGeneral.Settings
//{

/*   Settings Plugin Readme
 *   See more at: https://github.com/jamesmontemagno/SettingsPlugin/blob/master/CHANGELOG.md
 *   ## News
 *   - Plugins have moved to.NET Standard and have some important changes! Please read my blog:
 *   http://motzcod.es/post/162402194007/plugins-for-xamarin-go-dotnet-standard

    -New changes in Settings API: GetValueOrDefault and AddOrUpdateValue are no longer generic and you must pass in only supported types.

    ### Important
    Ensure that you install NuGet into ALL projects.
    Create a new file called Settings.cs or whatever you want and copy this code in to get started:
*/
/*
public class Settings : Notification, isaSettings, IMDMMessage
{
    static ISettings AppSettings
    {
        get
        {
            if (CrossSettings.IsSupported)
                return CrossSettings.Current;
            return null; // or your custom implementation
        }
    }

    static Settings settings;
    public static Settings Current
    {
        get { return settings ?? (settings = new Settings()); }
    }

    public void ClearAll()
    {
        AppSettings.Clear();
    }

    #region Setting Constants

    private const string idLogPath = "LogPath";
    private static readonly string LogPathDefault = string.Empty;

    private const string idLogLevel = "LogLevel";
    private static readonly string LogLevelDefault = string.Empty;

    private const string idUrl = "Url";
    private static readonly string UrlDefault = string.Empty;

    private const string idPort = "Port";
    private static readonly int PortDefault = 0;

    private const string idWinsysUrl = "WinsysUrl";
    private static readonly string WinsysUrlDefault = string.Empty;

    private const string idWinsysPort = "WinsysPort";
    private static readonly int WinsysPortDefault = 8181;

    private const string idUMDUrl = "UMDUrl";
    private static readonly string UMDUrlDefault = GlobalSetting.Config.srvSet.srvurl;

    private const string idUMDPort = "UMDPort";
    private static readonly int UMDPortDefault = GlobalSetting.Config.srvSet.srvport;

    #endregion

    public string LogLevel
    {
        get
        {
            return AppSettings.GetValueOrDefault(idLogLevel, LogLevelDefault);
        }
        set
        {
            if (AppSettings.AddOrUpdateValue(idLogLevel, value))
                OnPropertyChanged();
        }
    }

    public string Url
    {
        get
        {
            return AppSettings.GetValueOrDefault(idUrl, UrlDefault);
        }
        set
        {
            AppSettings.AddOrUpdateValue(idUrl, value);
        }
    }
    public int Port
    {
        get
        {
            return AppSettings.GetValueOrDefault(idPort, PortDefault);
        }
        set
        {
            AppSettings.AddOrUpdateValue(idPort, value);
        }
    }


    public string WinsysUrl
    {
        get
        {
            return AppSettings.GetValueOrDefault(idWinsysUrl, WinsysUrlDefault);
        }
        set
        {
            AppSettings.AddOrUpdateValue(idWinsysUrl, value);
        }
    }

    public int WinsysPort
    {
        get
        {
            return AppSettings.GetValueOrDefault(idWinsysPort, WinsysPortDefault);
        }
        set
        {
            AppSettings.AddOrUpdateValue(idWinsysPort, value);
        }
    }

    public string UMDUrl
    {
        get
        {
            return AppSettings.GetValueOrDefault(idUMDUrl, UMDUrlDefault);
        }
        set
        {
            AppSettings.AddOrUpdateValue(idUMDUrl, value);
        }
    }

    public int UMDPort
    {
        get
        {
            return AppSettings.GetValueOrDefault(idUMDPort, UMDPortDefault);
        }
        set
        {
            AppSettings.AddOrUpdateValue(idUMDPort, value);
        }
    }

    public MsgTypes.eCommand Command { get; set; }
    public Guid RequestId { get; set; }
}
*/
//}
