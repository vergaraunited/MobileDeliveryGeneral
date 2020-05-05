using System;
using MobileDeliveryGeneral.Settings;
using MobileDeliveryGeneral.Utilities;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Interfaces
{
    public interface isaMobileDeliveryClient : isaReceiveMessageCallback, isaSendMessageCallback
    {
        bool Connect();

       void Init(string url, ushort port, string name, ref SendMsgDelegate sm, ReceiveMsgDelegate rcvMsg = null);
        string Url { get; set; }
        ushort Port { get; set; }
    }
}
