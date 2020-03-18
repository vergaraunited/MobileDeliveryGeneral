using static UMDGeneral.Definitions.MsgTypes;

namespace UMDGeneral.Interfaces
{
    public delegate isaCommand ReceiveMsgDelegate(isaCommand cmd);
        
    public interface isaReceiveMessageCallback
    {
        isaCommand ReceiveMessage(isaCommand cmd);
    }
}
