using System;
using System.Collections.Generic;
using System.Text;

namespace UMDGeneral.Definitions
{
    public enum status { Pending, Uploaded, Released, Init, Releasing, Completed}
    public class Request
    {
        public Guid reqGuid { get; set; }
        public Dictionary<long, status> LIds { get; set; }
        public Dictionary<long, status> MIds { get; set; }
    }
}
