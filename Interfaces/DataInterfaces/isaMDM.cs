using System;
using System.Threading.Tasks;
using MobileDeliveryGeneral.Interfaces;

namespace MobileDeliveryGeneral.DataManager.Interfaces
{
    //Interface into the Mobile Data Manager
    public interface isaMDM
    {
        ProcessMsgDelegateRXRaw pmRx { get; }
        void HandleClientCmd(byte[] cmd, Func<byte[], Task> cbsend);
    }
}
