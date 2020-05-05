using MobileDeliveryGeneral.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Interfaces.DataInterfaces
{
    public interface IMDMMessage
    {
        eCommand Command { get; set; }
        Guid RequestId { get; set; }
        status status { get; set; }
    }
}
