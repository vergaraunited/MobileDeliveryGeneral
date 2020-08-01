using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Interfaces
{
    public delegate bool SendMsgDelegate(isaCommand cmd);

    public interface isaSendMessageCallback
    {
        //IWebSocketConnection conn { get; set; }
        bool SendMessage(isaCommand cmd);

    }
}
