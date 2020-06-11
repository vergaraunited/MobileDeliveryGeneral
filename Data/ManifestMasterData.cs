using System;
using static MobileDeliveryGeneral.Definitions.MsgTypes;
using MobileDeliveryGeneral.ExtMethods;

namespace MobileDeliveryGeneral.Data
{
    public class ManifestMasterData : BaseData<ManifestMasterData>
    {
        public override bool Equals(ManifestMasterData other)
        {
            return this.RequestId==other.RequestId &&
                this.LINK == other.LINK && 
                this.TRK_CDE == other.TRK_CDE && //SHIP_DTE == other.SHIP_DTE &&
                SHP_QTY == other.SHP_QTY & NOTES == other.NOTES;
        }

        public override int CompareTo(ManifestMasterData mmd)
        {
            if (mmd.LINK == this.LINK && this.TRK_CDE == mmd.TRK_CDE && this.SHIP_DTE == mmd.SHIP_DTE
                && this.NOTES == mmd.NOTES && this.Desc == mmd.Desc && this.SHP_QTY == mmd.SHP_QTY)
                return 0;
            else
                return 1;
        }
        public override string ToString()
        {
            return $"Command:{Enum.GetName(typeof(eCommand), Command) + Environment.NewLine}" + 
                $"\t\t{ManifestId + Environment.NewLine}" +
                $"\t\t{Userid + Environment.NewLine}" +
                $"\t\t{TRK_CDE + Environment.NewLine}" +
                $"\t\t{SHIP_DTE + Environment.NewLine}" +
                $"\t\t{Desc + Environment.NewLine}" +
                $"\t\t{NOTES + Environment.NewLine}" +
                $"\t\t{LINK + Environment.NewLine}" +
                $"\t\t{TRUCKISCLOSED + Environment.NewLine}" +
                $"\t\t{SEAL_DTE + Environment.NewLine}" +
                $"\t\t{SHP_QTY + Environment.NewLine}" +
                $"\t\t{COUNT + Environment.NewLine}" +
                $"\t\t{RequestId + Environment.NewLine}";
        }
        //  public ICollection<ManifestDetailsData> Details { get; set; }

        // Manifest Master Data
        // public Guid RequestId { get; set; }
        public override eCommand Command { get; set; } = eCommand.Manifest;
        public long ManifestId { get; set; }
        public string Userid { get; set; }
        public string TRK_CDE { get; set; }
        public DateTime SHIP_DTE { get; set; }
        public string Desc { get; set; }
        public string NOTES { get; set; }
        public long LINK { get; set; }
        public bool TRUCKISCLOSED { get; set; }
        public DateTime SEAL_DTE { get; set; }
        public short SHP_QTY { get; set; }
        public int COUNT { get; set; }
        public TruckManifestStatus Status { get; set; }
        //public eCommand Command { get; set; }

        public ManifestMasterData() : base() { }

        public ManifestMasterData(ManifestMasterData mmd)
        {
            RequestId = mmd.RequestId;
            ManifestId = mmd.ManifestId;
            Userid = mmd.Userid;
            TRK_CDE = mmd.TRK_CDE;
            IsSelected = mmd.IsSelected;
            SHIP_DTE = mmd.SHIP_DTE;
            Desc = mmd.Desc;
            NOTES = mmd.NOTES;
            LINK = mmd.LINK;
            TRUCKISCLOSED = mmd.TRUCKISCLOSED;
            SEAL_DTE = mmd.SEAL_DTE;
            SHP_QTY = mmd.SHP_QTY;
            COUNT = mmd.COUNT;
            Command = mmd.Command;
            Status = mmd.Status;
        }
        public ManifestMasterData(manifestMaster dat, long id, bool isSelected=false)
        {
            Command = dat.command;
            RequestId = NewGuid(dat.requestId);

            if (dat.id != 0)
                ManifestId = dat.id;
            else
                ManifestId = id;

            Userid = dat.DriverId.ToString();
            
            TRK_CDE = dat.TRK_CDE.UMToString(fldsz_TRK_CDE_Master);
            //SHIP_DTE = DateTime.FromBinary(dat.SHIP_DTE).Date;
            SHIP_DTE = ExtensionMethods.FromJulianToGregorianDT(dat.SHIP_DTE,"yyyy-MM-dd").Date;
            Desc = dat.DESC; //.UMToString(fldsz_DESC);
            NOTES = dat.NOTES; //.UMToString(fldsz_NOTES * sizeof(char)); 
            LINK = dat.LINK;
            TRUCKISCLOSED = Convert.ToBoolean(dat.TRUCKISCLOSED);
            //SEAL_DTE = ;
            SHP_QTY = dat.SHP_QTY;
            IsSelected = isSelected;
            status = Definitions.status.Uploaded;
            Status = TruckManifestStatus.New;
            //if (dat.md != null)
            //{
            //    md = new ManifestDetailsData(dat.md, 2);
            //}
        }
        public bool IsSelected { get; set; }
    }
}