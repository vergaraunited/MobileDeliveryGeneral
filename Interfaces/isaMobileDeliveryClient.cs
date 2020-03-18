using System;
using UMDGeneral.Settings;
using UMDGeneral.Utilities;
using static UMDGeneral.Definitions.MsgTypes;

namespace UMDGeneral.Interfaces
{
    public interface isaMobileDeliveryClient : isaReceiveMessageCallback, isaSendMessageCallback
    {
        bool Connect();

       void Init(SocketSettings settings, ref SendMsgDelegate sm, ReceiveMsgDelegate rcvMsg = null);
        string Url { get; set; }
        ushort Port { get; set; }
    }
}
