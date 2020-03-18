using UMDGeneral.Interfaces.Interfaces;
using static UMDGeneral.Definitions.MsgTypes;

namespace UMDGeneral.Interfaces
{
    public delegate bool SendMsgDelegate(isaCommand cmd);

    public interface isaSendMessageCallback
    {
        //IWebSocketConnection conn { get; set; }
        bool SendMessage(isaCommand cmd);

    }
}
