using System;
using System.Collections.Generic;
using System.Text;

namespace UMDGeneral.Interfaces
{
    public interface isaCommandParser<C>
    {
        C ProcessMessage(byte[] message);
    }
}
