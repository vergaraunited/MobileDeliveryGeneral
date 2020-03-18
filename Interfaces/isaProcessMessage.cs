using System;
using System.Threading.Tasks;

namespace UMDGeneral.Interfaces
{
    public delegate void ProcessMsgDelegateRXRaw(byte[] cmd, Func<byte[], Task> cbsend);

    public interface isaProcessMessage
    {
        void ProcessMessage(byte[] cmd, Func<byte[], Task> cbsend);
    }
}
