using System;
using MobileDeliveryGeneral.Definitions;

namespace MobileDeliveryGeneral.Interfaces.DataInterfaces
{
    public interface isaSettings: IMDMMessage
    {
        string LogLevel{ get; set; }
        string Url { get; set; }
        int Port { get; set; }
        string WinsysUrl { get; set; }
        int WinsysPort { get; set; }
        string UMDUrl { get; set;  }
        int UMDPort {get;set;}
       // MsgTypes.eCommand Command { get; set; }
       // Guid RequestId { get; set; }
    }
}
