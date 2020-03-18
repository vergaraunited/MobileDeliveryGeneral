using UMDGeneral.Interfaces;
using UMDGeneral.Interfaces.Interfaces;
using static UMDGeneral.Definitions.MsgTypes;

namespace UMDGeneral.Utilities
{
    public class SendMessages : isaSendMessageCallback
    {
        SendMsgDelegate cbSendMsg;

        public SendMessages(SendMsgDelegate smd)
        {
            cbSendMsg = smd;
        }

        //public IWebSocketConnection conn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public bool SendMessage(isaCommand cmd)
        {
            return cbSendMsg(cmd);
        }

    }
}
