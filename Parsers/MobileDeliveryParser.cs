using System;
using MobileDeliveryGeneral.Interfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Parsers
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
