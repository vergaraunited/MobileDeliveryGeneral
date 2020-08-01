using System;
using MobileDeliveryGeneral.Definitions;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class OrderData : BaseData<OrderData>
    {
        public delegate void cmdFireOnSelected(OrderData od);
        public cmdFireOnSelected OnSelectionChanged;

        public override eCommand Command { get; set; } = eCommand.OrdersLoad;
        public long ManifestId { get; set; }

        public Int32 ORD_NO { get; set; }
        public Int32 DLR_NO { get; set; }
        public String DLR_PO { get; set; }
        public Int32 ORD_DTE { get; set; }
        public Int32 SHP_DTE { get; set; }
        public Int32 SHIP_DTE { get; set; }
        public String CMNT1 { get; set; }
        public String CMNT2 { get; set; }
        public String DLR_NME { get; set; }
        public String DLR_ADDR { get; set; }
        public String DLR_ADDR2 { get; set; }
        public String SHP_NME { get; set; }
        public String SHP_ADDR { get; set; }
        public String SHP_ADDR2 { get; set; }
        public String SHP_CSZ { get; set; }
        public String SHP_TEL { get; set; }
        public Int16 SHP_CT { get; set; }
        public String SHP_ZIP { get; set; }
        public String CUS_NME { get; set; }
        public String CUS_ADDR { get; set; }
        public String CUS_CSZ { get; set; }
        public String CUS_TEL { get; set; }
        public String RTE_CDE { get; set; }
        public String ORD_TYPE { get; set; }
        public String ENT_BY { get; set; }
        public Decimal ORD_AMT { get; set; }
        public Int16 WIN_QTY { get; set; }
        public Int16 PAR_QTY { get; set; }
        public Int16 STK_QTY { get; set; }
        public Int16 CMP_QTY { get; set; }
        public Int16 SHP_QTY { get; set; }
        public Decimal SHP_AMT { get; set; }
        public String MISC_TEXT { get; set; }

        private bool isselected;
        public bool prevstate;
        public bool IsSelected { get{return isselected; } set { isselected = value; prevstate = !isselected; if ((((status == status.Uploaded || status == status.Init || status==status.Completed) && Status == OrderStatus.New) || ((status == status.Uploaded || status==status.Init  || status == status.Completed) && Status == OrderStatus.Delivered) || ((status == status.Releasing || status == status.Completed || status == status.Uploaded) && Status == OrderStatus.Shipped)) && prevstate != isselected && OnSelectionChanged != null ) { OnSelectionChanged(this); } } }

        public OrderStatus Status { get; set; }


        public OrderData()
        { status = status.Init; }
        public OrderData(OrderData ord)
        {
            this.Command = ord.Command;
            this.RequestId = ord.RequestId;
            this.ManifestId = ord.ManifestId;
            // this.DSP_SEQ = ord.DSP_SEQ;
            //this.CustomerId = ord.CustomerId;

            this.ORD_NO = ord.ORD_NO;
            this.DLR_NO = ord.DLR_NO;

            this.SHP_DTE = ord.SHP_DTE;
            //this.CLR = ord.CLR;
            //this.MDL_CNT = ord.MDL_CNT;
            //this.MDL_NO = ord.MDL_NO;
            //this.WIN_CNT = ord.WIN_CNT;
            //this.DESC = ord.DESC;
            this.Status = ord.Status;
            this.IsSelected = (this.Status == OrderStatus.Delivered) ? true : false;
            status = ord.status;
            //this.WIDTH = ord.WIDTH;
            //this.HEIGHT = ord.HEIGHT;
        }
        public OrderData(orders ord)
        {
            this.Command = ord.command;
            this.RequestId = NewGuid(ord.requestId);
            this.ManifestId = ord.ManifestId;

            ORD_NO = ord.ORD_NO;            
            DLR_NO = (int)ord.DLR_NO;
            SHP_DTE = ord.SHP_DTE;
            SHIP_DTE = ord.SHIP_DTE;
            CMNT1 = ord.CMNT1;
            CMNT2 = ord.CMNT2;
            DLR_NME = ord.DLR_NME;
            DLR_ADDR = ord.DLR_ADDR;
            DLR_ADDR2 = ord.DLR_ADDR2;
            SHP_NME = ord.SHP_NME;
            SHP_ADDR = ord.SHP_ADDR;
            SHP_ADDR2 = ord.SHP_ADDR2;
            SHP_CSZ = ord.SHP_CSZ;
            SHP_TEL = ord.SHP_TEL;
            SHP_CT = ord.SHP_CT;
            SHP_ZIP = ord.SHP_ZIP;
            CUS_NME = ord.CUS_NME;
            CUS_ADDR = ord.CUS_ADDR;
            CUS_CSZ = ord.CUS_CSZ;
            CUS_TEL = ord.CUS_TEL;
            RTE_CDE = ord.RTE_CDE;
            ORD_TYPE = ord.ORD_TYPE;
            ENT_BY = ord.ENT_BY;
            ORD_AMT = ord.ORD_AMT;
            WIN_QTY = ord.WIN_QTY;
            STK_QTY = ord.STK_QTY;
            CMP_QTY = ord.CMP_QTY;
            SHP_QTY = ord.SHP_QTY;
            SHP_AMT = ord.SHP_AMT;
            MISC_TEXT = ord.MISC_TEXT;
            DLR_NO = (int)ord.DLR_NO;
            DLR_PO = ord.DLR_PO;
            ORD_DTE = ord.ORD_DTE;
            SHIP_DTE = ord.SHIP_DTE;
            CMNT1 = ord.CMNT1;
            CMNT2 = ord.CMNT2;
            DLR_NME = ord.DLR_NME;
            DLR_ADDR = ord.DLR_ADDR;
            DLR_ADDR2 = ord.DLR_ADDR2;
            SHP_NME = ord.SHP_NME;
            SHP_ADDR = ord.SHP_ADDR;
            SHP_ADDR2 = ord.SHP_ADDR2;
            SHP_CSZ = ord.SHP_CSZ;
            SHP_TEL = ord.SHP_TEL;
            SHP_CT = ord.SHP_CT;
            SHP_ZIP = ord.SHP_ZIP;
            CUS_NME = ord.CUS_NME;
            CUS_ADDR = ord.CUS_ADDR;
            CUS_CSZ = ord.CUS_CSZ;
            CUS_TEL = ord.CUS_TEL;
            RTE_CDE = ord.RTE_CDE;
            ENT_BY = ord.ENT_BY;
            WIN_QTY = ord.WIN_QTY;
            STK_QTY = ord.STK_QTY;
            CMP_QTY = ord.CMP_QTY;
            SHP_QTY = ord.SHP_QTY;
            SHP_AMT = ord.SHP_AMT;
            MISC_TEXT = ord.MISC_TEXT;


            this.Status = (OrderStatus)ord.Status;
            this.IsSelected = (this.Status == OrderStatus.Delivered) ? true : false;

            //this.DSP_SEQ = ord.DSP_SEQ;
            //this.CustomerId = ord.CustomerId;

            //this.CLR = ord.CLR;
            //this.MDL_CNT = ord.MDL_CNT;
            //this.MDL_NO = ord.MDL_NO;
            //this.WIN_CNT = ord.WIN_CNT;
            //this.DESC = ord.DESC;
            //this.WIDTH = ord.WIDTH;
            //this.HEIGHT = ord.HEIGHT;


        }
        public override string ToString()
        {
            return $"Command:{Enum.GetName(typeof(eCommand), Command) + Environment.NewLine}" +
                $"\t\t{RequestId + Environment.NewLine}" +
                $"\t\t{ManifestId + Environment.NewLine}" +
                $"\t\t{ORD_NO + Environment.NewLine}" +
                $"\t\t{DLR_NO + Environment.NewLine}" +
                $"\t\t{DLR_PO + Environment.NewLine}" +
                $"\t\t{ORD_DTE + Environment.NewLine}" +
                $"\t\t{SHP_DTE + Environment.NewLine}" +
                $"\t\t{SHIP_DTE + Environment.NewLine}" +
                $"\t\t{CMNT1 + Environment.NewLine}" +
                $"\t\t{CMNT2 + Environment.NewLine}" +
                $"\t\t{DLR_NME + Environment.NewLine}" +
                $"\t\t{DLR_ADDR + Environment.NewLine}" +
                $"\t\t{DLR_ADDR2 + Environment.NewLine}" +
                $"\t\t{SHP_NME + Environment.NewLine}" +
                $"\t\t{SHP_ADDR + Environment.NewLine}" +
                $"\t\t{SHP_ADDR2 + Environment.NewLine}" +
                $"\t\t{SHP_CSZ + Environment.NewLine}" +
                $"\t\t{SHP_TEL + Environment.NewLine}" +
                $"\t\t{SHP_CT + Environment.NewLine}" +
                $"\t\t{SHP_ZIP + Environment.NewLine}" +
                $"\t\t{CUS_NME + Environment.NewLine}" +
                $"\t\t{CUS_ADDR + Environment.NewLine}" +
                $"\t\t{CUS_CSZ + Environment.NewLine}" +
                $"\t\t{CUS_TEL + Environment.NewLine}" +
                $"\t\t{RTE_CDE + Environment.NewLine}" +
                $"\t\t{ORD_TYPE + Environment.NewLine}" +
                $"\t\t{ENT_BY + Environment.NewLine}" +
                $"\t\t{ORD_AMT + Environment.NewLine}" +
                $"\t\t{WIN_QTY + Environment.NewLine}" +
                $"\t\t{STK_QTY + Environment.NewLine}" +
                $"\t\t{CMP_QTY + Environment.NewLine}" +
                $"\t\t{SHP_QTY + Environment.NewLine}" +
                $"\t\t{SHP_AMT + Environment.NewLine}" +
                $"\t\t{MISC_TEXT + Environment.NewLine}";
                //$"\t\t{DSP_SEQ + Environment.NewLine}" +
                //$"\t\t{CustomerId + Environment.NewLine}" +
                //$"\t\t{DLR_NO + Environment.NewLine}" +
                //$"\t\t{ORD_NO + Environment.NewLine}" +
                //$"\t\t{CLR + Environment.NewLine}" +
                //$"\t\t{MDL_CNT + Environment.NewLine}" +
                //$"\t\t{MDL_NO + Environment.NewLine}" +
                //$"\t\t{WIN_CNT + Environment.NewLine}" +
                //$"\t\t{DESC + Environment.NewLine}" +
                //$"\t\t{Status + Environment.NewLine}" +
                //$"\t\t{IsSelected + Environment.NewLine}" +
                //$"\t\t{WIDTH + Environment.NewLine}" +
                //$"\t\t{WIN_CNT + Environment.NewLine}" +
                //$"\t\t{HEIGHT + Environment.NewLine}";
        }
        public override int CompareTo(OrderData other)
        {
            if (this.ManifestId.CompareTo(other.ManifestId) == 0 &&
            //this.DSP_SEQ.CompareTo(other.DSP_SEQ) == 0 &&
            //this.CustomerId.CompareTo(other.CustomerId) == 0 &&
            this.DLR_NO.CompareTo(other.DLR_NO) == 0 &&
            this.ORD_NO.CompareTo(other.ORD_NO) == 0 &&
            this.SHP_DTE.CompareTo(other.SHP_DTE) == 0 

            //this.CLR.CompareTo(other.CLR) == 0 &&
            //this.MDL_CNT.CompareTo(other.MDL_CNT) == 0 &&
            //this.MDL_NO.CompareTo(other.MDL_NO) == 0 &&
            //this.WIN_CNT.CompareTo(other.WIN_CNT) == 0 && 
            //this.WIDTH.CompareTo(other.WIDTH) == 0 &&
            //this.HEIGHT.CompareTo(other.HEIGHT) == 0
            )
                return 0;
            else return 1;
        }

        public override bool Equals(OrderData other)
        {
            return (this.ManifestId == other.ManifestId &&

                        this.DLR_NO == other.DLR_NO &&
                        this.ORD_NO == other.ORD_NO //&&
                                               
                        //this.DSP_SEQ == other.DSP_SEQ &&
                        //this.CustomerId == other.CustomerId && this.CLR == other.CLR &&
                        //this.MDL_CNT == other.MDL_CNT &&
                        //this.MDL_NO == other.MDL_NO &&
                        //this.WIN_CNT == other.WIN_CNT &&
                        //this.WIDTH == other.WIDTH &&
                        //this.HEIGHT == other.HEIGHT
                       );
        }
    }
}
