using System;
using System.IO;

namespace UMDGeneral.Utilities
{
    public static class HomeDirectoryPaths
    {
        public static string GetUserHome(string appName="")
        {
            var homeDrive = Environment.GetEnvironmentVariable("HOMEDRIVE");
            if (!string.IsNullOrWhiteSpace(homeDrive))
            {
                var homePath = Environment.GetEnvironmentVariable("HOMEPATH");
                if (!string.IsNullOrWhiteSpace(homePath))
                {
                    var fullHomePath = homeDrive + Path.DirectorySeparatorChar + homePath;
                    if (appName.Length == 0)
                        appName = "UMDnoname";
                    return Path.Combine(fullHomePath, appName);
                }
                else
                {
                    throw new Exception("Environment variable error, there is no 'HOMEPATH'");
                }
            }
            else
            {
                throw new Exception("Environment variable error, there is no 'HOMEDRIVE'");
            }
        }
    }
}
