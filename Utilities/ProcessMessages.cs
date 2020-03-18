using System;
using System.Threading.Tasks;
using UMDGeneral.Interfaces;

namespace UMDGeneral.Utilities
{
    public class ProcessMessages
    {
        ProcessMsgDelegateRXRaw cbMsgProcessorRxRaw;
        Func<byte[], Task> cbsend;

        public ProcessMessages(ProcessMsgDelegateRXRaw pmd)
        {
            cbMsgProcessorRxRaw = pmd;
        }

        public ProcessMessages(ProcessMsgDelegateRXRaw pmd, Func<byte[], Task> cbsend) { 
            cbMsgProcessorRxRaw = pmd;
            this.cbsend = cbsend;
        }

        public void ProcessMessage(byte[] byte_cmd, Func<byte[], Task> cbsend)
        {
            cbMsgProcessorRxRaw(byte_cmd, cbsend);
        }

        public void ProcessMessage(byte[] byte_cmd)
        {
            if (cbsend != null)
                cbMsgProcessorRxRaw(byte_cmd, cbsend);
            else
                throw new Exception("cbsend is null in ProcessMessage");
        }
    }
}
