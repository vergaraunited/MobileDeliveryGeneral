using System;
using MobileDeliveryGeneral.ExtMethods;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class TruckData : IMDMMessage, IComparable<TruckData>, IEquatable<TruckData>
    {
        public eCommand Command { get; set; } = eCommand.Trucks;

        public Guid RequestId { get; set; }
        public long Id { get; set; }
        public long ManifestId { get; set; }
        public int DriverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TRK_CDE { get; set; }
        public DateTime SHIP_DTE { get; set; }
        public string Desc { get; set; }
        public string NOTES { get; set; }
        public bool IsClosed { get; set; }

        public TruckData() { }
        public TruckData(TruckData td)
        {
            RequestId = td.RequestId;
            ManifestId = td.ManifestId;
            DriverId = td.DriverId;
            FirstName = td.FirstName;
            LastName = td.LastName;
            TRK_CDE = td.TRK_CDE;
            SHIP_DTE = td.SHIP_DTE;
            Desc = td.Desc;
            NOTES = td.NOTES;
            IsClosed = td.IsClosed;
        }
        public TruckData(trucks trk)
        {
            RequestId = new Guid(trk.requestId);
            ManifestId = trk.ManifestId;
            DriverId = trk.DriverId;
            FirstName = trk.FirstName;
            LastName = trk.LastName;
            TRK_CDE = trk.TruckCode;
            SHIP_DTE = ExtensionMethods.FromJulianToGregorianDT(trk.ShipDate, "yyyy-MM-dd").Date;
            NOTES = trk.Notes;
            Desc = trk.Description;
            NOTES = trk.Notes;
            IsClosed = trk.IsClosed;
        }

        public int CompareTo(TruckData other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(TruckData other)
        {
            return ((this.ManifestId == other.ManifestId) &&
                (this.SHIP_DTE.CompareTo(other.SHIP_DTE)==0) &&
                (this.TRK_CDE == other.TRK_CDE) &&
                (this.NOTES == other.NOTES) &&
                (this.Desc == other.Desc) &&
                (this.IsClosed == other.IsClosed));
        }
    }
}
