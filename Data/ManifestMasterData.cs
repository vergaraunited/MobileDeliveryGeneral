using System;
using UMDGeneral.Interfaces.DataInterfaces;
using static UMDGeneral.Definitions.MsgTypes;
using UMDGeneral.ExtMethods;

namespace UMDGeneral.Data
{
    public class ManifestMasterData : BaseData<ManifestMasterData>
    {
        public override bool Equals(ManifestMasterData other)
        {
            return this.RequestId==other.RequestId &&
                this.LINK == other.LINK && 
                this.TRK_CDE == other.TRK_CDE;
        }

        public override int CompareTo(ManifestMasterData mmd)
        {
            if (mmd.LINK == this.LINK && this.TRK_CDE == mmd.TRK_CDE && this.SHIP_DTE == mmd.SHIP_DTE
                && this.NOTES == mmd.NOTES && this.Desc == mmd.Desc)
                return 0;
            else
                return 1;
        }

        //public override string ToString()
        //{
        //    return "ManifestMasterData: " + LINK + ", TC:" + TRK_CDE + ", ID:" + ManifestId + 
        //        ", NOTES:" + NOTES + ", Desc:" + Desc + ", SHPQTY:" + SHP_QTY;
        //}
        //  public ICollection<ManifestDetailsData> Details { get; set; }

        // Manifest Master Data
       // public Guid RequestId { get; set; }
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
        }
        public ManifestMasterData(manifestMaster dat, long id, bool isSelected=false)
        {
            Command = dat.command;
            RequestId = new Guid(dat.requestId);

            if (dat.id != 0)
                ManifestId = dat.id;
            else
                ManifestId = id;

            Userid = dat.DriverId.ToString();
            
            TRK_CDE = dat.TRK_CDE.UMToString(fldsz_TRK_CDE);
            //SHIP_DTE = DateTime.FromBinary(dat.SHIP_DTE).Date;
            SHIP_DTE = ExtensionMethods.FromJulianToGregorianDT(dat.SHIP_DTE,"yyyy-MM-dd").Date;
            Desc = dat.DESC.UMToString(fldsz_DESC);
            NOTES = dat.NOTES.UMToString(fldsz_NOTES * sizeof(char)); 
            LINK = dat.LINK;
            TRUCKISCLOSED = Convert.ToBoolean(dat.TRUCKISCLOSED);
            //SEAL_DTE = ;
            SHP_QTY = dat.SHP_QTY;
            IsSelected = isSelected;

            //if (dat.md != null)
            //{
            //    md = new ManifestDetailsData(dat.md, 2);
            //}
        }
        public bool IsSelected { get; set; }
    }
}