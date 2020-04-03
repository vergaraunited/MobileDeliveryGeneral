using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Interfaces
{
    public delegate isaCommand ReceiveMsgDelegate(isaCommand cmd);
        
    public interface isaReceiveMessageCallback
    {
        isaCommand ReceiveMessage(isaCommand cmd);
    }
}
