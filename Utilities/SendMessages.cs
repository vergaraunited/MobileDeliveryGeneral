using MobileDeliveryGeneral.Interfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Utilities
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
