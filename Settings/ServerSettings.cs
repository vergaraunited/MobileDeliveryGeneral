using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Settings
{
    public class SocketSettings
    {
        public string url { get; set; }
        public ushort port { get; set; }
        public string srvurl { get; set; }
        public ushort srvport { get; set; }
        public string clienturl { get; set; }
        public ushort clientport { get; set; }
        public string name { get; set; }

        public delegate isaCommand ReceiveMsgDelegate(isaCommand cmd);
    } 
}
