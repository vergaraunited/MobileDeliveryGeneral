using System;
using System.Collections.Generic;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class StopData : BaseData<StopData>
    {
        public override eCommand Command { get; set; } = eCommand.Stops;
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
        public List<OrderMasterData> Orders { get; set; }

        public StopData() { }

        public StopData(stops stp)
        {
            Command = stp.command;
            this.RequestId = NewGuid(stp.requestId);
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
            Command = stp.Command;
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


        public override int CompareTo(StopData other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(StopData sd)
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
