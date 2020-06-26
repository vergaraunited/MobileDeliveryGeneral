using System;
using MobileDeliveryGeneral.Settings;
using MobileDeliveryGeneral.Utilities;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Interfaces
{
    public interface isaMobileDeliveryClient : isaReceiveMessageCallback, isaSendMessageCallback
    {
        bool Connect();

       void Init(SocketSettings sockSet, ref SendMsgDelegate sm, ReceiveMsgDelegate rcvMsg = null);
        SocketSettings socSet { get; set; }
    }
}
