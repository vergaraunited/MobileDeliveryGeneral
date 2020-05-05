using Plugin.Settings.Abstractions;
using System;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class SettingsData : BaseData<SettingsData>
    {
        public override eCommand Command { get; set; } = eCommand.LoadSettings;
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

        public SettingsData() { }

        public SettingsData(ISettings stp)
        {
            //this.RequestId = NewGuid(stp.requestId);
            //this.ManifestId = stp.ManifestId;
            //this.DisplaySeq = stp.DisplaySeq;
            //this.DealerNo = stp.DealerNo;
            //this.DealerName = stp.DealerName;
            //this.Address = stp.Address;
            //this.PhoneNumber = stp.PhoneNumber;
            //this.Description = stp.Description;
            //this.Notes = stp.Notes;
            //this.TruckCode = stp.TRK_CDE;
            //this.CustomerId = stp.CustomerId;
            //this.BillComplete = stp.BillComplete;
        }
        public override int CompareTo(SettingsData other)
        {
            throw new NotImplementedException();
        }

        //public bool Equals(SettingsData sd)
        //{
        //    return this.Address == sd.Address &&
        //        this.CustomerId == sd.CustomerId &&
        //       this.DealerName == sd.DealerName &&
        //       this.DealerNo == sd.DealerNo &&
        //       this.DisplaySeq == sd.DisplaySeq &&
        //       this.ManifestId == sd.ManifestId;
        //}
    }
}
