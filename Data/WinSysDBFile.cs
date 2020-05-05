using System.Collections.Generic;
using System.IO;

namespace MobileDeliveryGeneral.Data
{
    public class WinsysDBFile
    {
        public List<string> WinsysFileNames = new List<string>();
        public string WinsysSrcFilePath { get; set; }
        public string WinsysDstFilePath { get; set; }

        public WinsysDBFile(string srcPath, string dstPath)
        {
            WinsysSrcFilePath = srcPath;
            WinsysDstFilePath = dstPath;
        }

        public override string ToString()
        {
            return WinsysSrcFilePath;
        }
        public string SrcFileName()
        {
            return Path.GetFileName(WinsysSrcFilePath);
        }

        public string SrcPath()
        {
            return Path.GetFullPath(WinsysSrcFilePath);
        }
    }
}
