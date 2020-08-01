using System;
using System.Collections.Generic;
using MobileDeliveryGeneral.DataManager.Interfaces;
using SQLite;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class StopData : BaseData<StopData>, isaCacheItem<StopData>
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

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
        public byte[] POD;
        public OrderStatus Status { get; set; }
        public DateTime ScanDateTime { get; set; }

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
            this.ScanDateTime = stp.ScanDateTime;
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
            this.POD = stp.POD;
            this.Status = stp.Status;
            this.ScanDateTime = stp.ScanDateTime;
        }

        public override string ToString()
        {
            return $"Command:{Enum.GetName(typeof(eCommand), Command) + Environment.NewLine}" +
                $"\t\t{RequestId + Environment.NewLine}" +
                $"\t\t{Address + Environment.NewLine}" +
                $"\t\t{CustomerId + Environment.NewLine}" +
                $"\t\t{DealerName + Environment.NewLine}" +
                $"\t\t{DealerNo + Environment.NewLine}" +
                $"\t\t{DisplaySeq + Environment.NewLine}" +
                $"\t\t{ManifestId + Environment.NewLine}" +
                $"\t\t{ScanDateTime + Environment.NewLine}";
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
               this.ManifestId == sd.ManifestId &&
               this.ScanDateTime==sd.ScanDateTime;
        }
    }
}
