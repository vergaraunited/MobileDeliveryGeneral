using System;
using UMDGeneral.Interfaces;
using static UMDGeneral.Definitions.MsgTypes;

namespace UMDGeneral.Utilities
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
using UMDGeneral.Interfaces;
using static UMDGeneral.Definitions.MsgTypes;

namespace UMDGeneral.Utilities
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
