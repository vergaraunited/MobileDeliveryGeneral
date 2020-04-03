using System;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class StopData : IMDMMessage, IComparable<StopData>, IEquatable<StopData>
    {
        public eCommand Command { get; set; } = eCommand.Stops;
        public Guid RequestId { get; set; }
        public long ManifestId { get; set; }
        public int DisplaySeq { get; set; }
        public long DealerNo { get; set; }
        public string DealerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string TruckCode { get; set; }
        public int CustomerId { get; set; }
        public bool BillComplete { get; set; }

        public StopData() { }

        public StopData(stops stp)
        {
            this.RequestId = new Guid(stp.requestId);
            this.ManifestId = stp.ManifestId;
            this.DisplaySeq = stp.DisplaySeq;
            this.DealerNo = stp.DealerNo;
            this.DealerName = stp.DealerName;
            this.Address = stp.Address;
            this.PhoneNumber = stp.PhoneNumber;
            this.Description = stp.Description;
            this.Notes = stp.Notes;
            this.TruckCode = stp.TRK_CDE;
            this.CustomerId = stp.CustomerId;
            this.BillComplete = stp.BillComplete;
        }
        public StopData(StopData stp)
        {
            this.RequestId = stp.RequestId;
            this.ManifestId = stp.ManifestId;
            this.DisplaySeq = stp.DisplaySeq;
            this.DealerNo = stp.DealerNo;
            this.DealerName = stp.DealerName;
            this.Address = stp.Address;
            this.PhoneNumber = stp.PhoneNumber;
            this.Description = stp.Description;
            this.Notes = stp.Notes;
            this.TruckCode = stp.TruckCode;
            this.CustomerId = stp.CustomerId;
            this.BillComplete = stp.BillComplete;
        }


        public int CompareTo(StopData other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(StopData sd)
        {
            return this.Address == sd.Address && 
                this.CustomerId == sd.CustomerId &&
               this.DealerName == sd.DealerName &&
               this.DealerNo == sd.DealerNo &&
               this.DisplaySeq == sd.DisplaySeq &&
               this.ManifestId == sd.ManifestId;
        }
    }
}
