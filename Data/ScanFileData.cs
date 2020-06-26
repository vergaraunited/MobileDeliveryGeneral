using System;
using MobileDeliveryGeneral.DataManager.Interfaces;
using MobileDeliveryGeneral.Definitions;
using SQLite;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class ScanFileData : BaseData<ScanFileData>, isaCacheItem<ScanFileData>
    {
        public delegate void cmdFireOnSelected(ScanFileData od);
        public cmdFireOnSelected OnSelectionChanged;
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public override eCommand Command { get; set; } = eCommand.ScanFile;

        public Int32 ORD_NO { get; set; }
        public Int16 MDL_CNT { get; set; }
        public Int16 PAT_POS { get; set; }
        public Int32 LOT_NO { get; set; }
        public Int16 BIN_NO { get; set; }
        public String MDL_NO { get; set; }
        public Byte INVOICE_FLAG { get; set; }
        public Int32 INVOICE_NO { get; set; }
        public Byte DSP_SEQ { get; set; }
        public String TRK_CDE { get; set; }
        public Int32 TRK_DTE { get; set; }
        public Int16 WIN_CNT { get; set; }
        //public Byte SHP_SCN_FLG { get; set; }
        public Int32 SHP_DTE { get; set; }
        public Int32 SHP_TME { get; set; }
        public String SHP_BY { get; set; }
        public String LOCATION { get; set; }
        public String REASON { get; set; }
        public Int64 MAN_ID { get; set; }
        private bool isselected;
        public OrderStatus Status { get; set; }

        //public List<ScanFileData> scanFileData = new List<ScanFileData>();
        public bool prevstate;
        public bool IsSelected
        {
            get { return isselected; }
            set
            {
                isselected = value; prevstate = !isselected;
                if ((((status == status.Uploaded || status == status.Init || status == status.Completed) && Status == OrderStatus.New) || ((status == status.Uploaded || status == status.Init || status == status.Completed) && Status == OrderStatus.Delivered) || ((status == status.Releasing || status == status.Completed || status == status.Uploaded) && Status == OrderStatus.Shipped)) && prevstate != isselected && OnSelectionChanged != null)
                { OnSelectionChanged(this); }
            }
        }


        public ScanFileData() { }
        public ScanFileData(scanFile sf)
        {
            ORD_NO = (int)sf.ORD_NO;
            MDL_CNT = sf.MDL_CNT;
            PAT_POS = sf.PAT_POS;
            LOT_NO = sf.LOT_NO;
            BIN_NO = sf.BIN_NO;
            MDL_NO = sf.MDL_NO;
            INVOICE_FLAG = sf.INVOICE_FLAG;
            INVOICE_NO = sf.INVOICE_NO;
            //TYPE = sf.TYPE;
            DSP_SEQ = sf.DSP_SEQ;
            TRK_CDE = sf.TRK_CDE;
            TRK_DTE = sf.TRK_DTE;
            WIN_CNT = sf.WIN_CNT;
//            SHP_SCN_FLG = sf.SHP_SCN_FLG;
            SHP_DTE = sf.SHP_DTE;
            SHP_TME = sf.SHP_TME;
            SHP_BY = sf.SHP_BY;
            LOCATION = sf.LOCATION;
            REASON = sf.REASON;
            MAN_ID = sf.MAN_ID;
        }

        public scanFile Toscanfile()
        {
            scanFile sf = new scanFile();

            sf.ORD_NO = ORD_NO;
            sf.MDL_CNT = MDL_CNT;
            sf.PAT_POS = PAT_POS;
            sf.LOT_NO = LOT_NO;
            sf.BIN_NO = BIN_NO;
            sf.MDL_NO = MDL_NO;
            sf.INVOICE_FLAG = INVOICE_FLAG;
            sf.INVOICE_NO = INVOICE_NO;
           // sf.TYPE = TYPE;
            sf.DSP_SEQ = DSP_SEQ;
            sf.TRK_CDE = TRK_CDE;
            sf.TRK_DTE = TRK_DTE;
            sf.WIN_CNT = WIN_CNT;
            //sf.SHP_SCN_FLG = SHP_SCN_FLG;
            sf.SHP_DTE = SHP_DTE;
            sf.SHP_TME = SHP_TME;
            sf.SHP_BY = SHP_BY;
            sf.LOCATION = LOCATION;
            sf.REASON = REASON;
            sf.MAN_ID = MAN_ID;
            return sf;

        }

        public override string ToString()
        {
            return $"Command:{Enum.GetName(typeof(eCommand), Command) + Environment.NewLine}" +
               $"\t\t{RequestId + Environment.NewLine}" +
               $"\t\t{ORD_NO + Environment.NewLine}" +
                $"\t\t{MDL_CNT + Environment.NewLine}" +
                $"\t\t{PAT_POS + Environment.NewLine}" +
                $"\t\t{LOT_NO + Environment.NewLine}" +
                $"\t\t{BIN_NO + Environment.NewLine}" +
                $"\t\t{MDL_NO + Environment.NewLine}" +
                $"\t\t{INVOICE_FLAG + Environment.NewLine}" +
                $"\t\t{INVOICE_NO + Environment.NewLine}" +
                //$"\t\t{TYPE + Environment.NewLine}" +
                $"\t\t{DSP_SEQ + Environment.NewLine}" +
                $"\t\t{TRK_CDE + Environment.NewLine}" +
                $"\t\t{TRK_DTE + Environment.NewLine}" +
                $"\t\t{WIN_CNT + Environment.NewLine}" +
                $"\t\t{SHP_DTE + Environment.NewLine}" +
                $"\t\t{SHP_TME + Environment.NewLine}" +
                $"\t\t{SHP_BY + Environment.NewLine}" +
                $"\t\t{LOCATION + Environment.NewLine}" +
                $"\t\t{REASON + Environment.NewLine}" + 
                $"\t\t{MAN_ID + Environment.NewLine}";
        }
        public override bool Equals(ScanFileData other)
        {
            return this.Command == other.Command &&
                this.ORD_NO == other.ORD_NO &&
                this.BIN_NO == other.BIN_NO &&
                this.MDL_CNT == other.MDL_CNT &&
                this.PAT_POS == other.PAT_POS;
        }

    }
}
   