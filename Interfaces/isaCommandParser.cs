using System;
using System.Collections.Generic;
using System.Text;

namespace MobileDeliveryGeneral.Interfaces
{
    public interface isaCommandParser<C>
    {
        C ProcessMessage(byte[] message);
    }
}
