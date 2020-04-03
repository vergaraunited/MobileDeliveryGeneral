using System.Collections.Generic;
using System.IO;

namespace MobileDeliveryGeneral.Data
{
    public class WinSysDBFile
    {
        List<string> WinSysFileNames = new List<string>();
        public string WinSysSrcFilePath { get; set; }
        public string WinsysDstFilePath { get; set; }

        public WinSysDBFile(string srcPath, string dstPath)
        {
            WinSysSrcFilePath = srcPath;
            WinsysDstFilePath = dstPath;
        }

        public override string ToString()
        {
            return WinSysSrcFilePath;
        }
        public string SrcFileName()
        {
            return Path.GetFileName(WinSysSrcFilePath);
        }

        public string SrcPath()
        {
            return Path.GetFullPath(WinSysSrcFilePath);
        }



    }
}
