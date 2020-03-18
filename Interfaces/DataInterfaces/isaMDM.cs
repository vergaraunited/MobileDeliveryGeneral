using System;
using System.Threading.Tasks;
using UMDGeneral.Interfaces;

namespace UMDGeneral.DataManager.Interfaces
{
    //Interface into the Mobile Data Manager
    public interface isaMDM
    {
        ProcessMsgDelegateRXRaw pmRx { get; }
        void HandleClientCmd(byte[] cmd, Func<byte[], Task> cbsend);
    }
}
