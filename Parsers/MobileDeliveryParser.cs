using System;
using System.Collections.Generic;
using System.Text;
using UMDGeneral.Interfaces;
using static UMDGeneral.Definitions.MsgTypes;

namespace UMDGeneral.Parsers
{
    public class MobileDeliveryParser : isaCommandParser<Command>
    {
        public Command ProcessMessage(byte[] message)
        {
            throw new NotImplementedException();
        }

        public Command PingCommand() { return new Command{ command = Definitions.MsgTypes.eCommand.Pong }; }
    }
}
