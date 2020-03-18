using System;
using System.Collections.Generic;
using System.Text;
using static UMDGeneral.Definitions.MsgTypes;

namespace UMDGeneral.Interfaces.DataInterfaces
{
    public interface IMDMMessage
    {
        eCommand Command { get; set; }
        Guid RequestId { get; set; }
    }
}
