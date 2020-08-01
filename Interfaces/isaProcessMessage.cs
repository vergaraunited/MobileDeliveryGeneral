using MobileDeliveryGeneral.Interfaces.Interfaces;
using System;
using System.Threading.Tasks;

namespace MobileDeliveryGeneral.Interfaces
{
    public delegate void ProcessMsgDelegateRXRaw(byte[] cmd, Func<byte[], Task> cbsend);

    public interface isaProcessMessage
    {
        void ProcessMessage(byte[] cmd, Func<byte[], Task> cbsend);
    }
}
