using System;
using System.Collections.Generic;

namespace MobileDeliveryGeneral.Definitions
{
    public enum status { Init, Pending, Uploaded, Released, Releasing, Completed, SystemChange}
    public class Request
    {
        public Guid reqGuid { get; set; }
        public Dictionary<long, status> LIds { get; set; }
        public Dictionary<long, List<long>> LinkMid { get; set; }
        public Dictionary<long, status> ChkIds { get; set; }



    }
}
