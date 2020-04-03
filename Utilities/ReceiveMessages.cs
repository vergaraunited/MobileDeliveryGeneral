using System;
using MobileDeliveryGeneral.Interfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Utilities
{
    public class ReceiveMessages : isaReceiveMessageCallback
    {
        ReceiveMsgDelegate cbViewProcessMsg;
        public ReceiveMessages(ReceiveMsgDelegate pmd)
        {
            cbViewProcessMsg = pmd;
        }

        public isaCommand ReceiveMessage(isaCommand cmd)
        {
            return cbViewProcessMsg(cmd);
        }
    }
}
/*

    using System;
using MobileDeliveryGeneral.Interfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Utilities
{
    public class ReceiveMessages : isaReceiveMessageCallback
    {
        ReceiveMsgDelegate cbReceiveMsg;
        ProcessMessages cbProcessMsg;

        public ReceiveMessages(ReceiveMsgDelegate rm, ProcessMessages pm)
        {
            cbReceiveMsg = rm;
        }

        public isaCommand ReceiveMessage(byte[] cmd_bytes)
        {
            isaCommand cmdOut = Command.FromArray(cmd_bytes)
            switch (cmdOut.command)
            {
                case eCommand.ManifestLoadComplete:
                    cbReceiveMsg()
            }
                return cbViewProcessMsg(cmd);
        }
    }
}
*/
