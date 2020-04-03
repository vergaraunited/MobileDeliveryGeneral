using System;
using System.Collections.Generic;
using System.Text;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Settings
{
    public class SocketSettings
    {
        public string url { get; set; }
        public ushort port { get; set; }
        public string umdurl { get; set; }
        public ushort umdport { get; set; }
        public string WinSysUrl { get; set; }
        public ushort WinSysPort { get; set; }
        public string name { get; set; }

        public delegate isaCommand ReceiveMsgDelegate(isaCommand cmd);
    } 
}
