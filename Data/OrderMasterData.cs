using System;
using System.Collections.Generic;
using System.Linq;
using MobileDeliveryGeneral.ExtMethods;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class OrderMasterData : BaseData<OrderMasterData>
    {
        public override eCommand Command { get; set; } = eCommand.OrdersLoad;
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
        public Decimal DEPOSIT { get; set; }
        public String ORD_TYPE { get; set; }
        public String ENT_BY { get; set; }
        public Decimal ORD_AMT { get; set; }
        public Int16 WIN_QTY { get; set; }
        public Int16 STK_QTY { get; set; }
        public Int16 CMP_QTY { get; set; }
        public Int16 SHP_QTY { get; set; }
        public Decimal SHP_AMT { get; set; }
        public String MISC_TEXT { get; set; }

        public long ManId;
        public bool IsSelected;
        public DateTime SCAN_DATE_TIME { get; set; }
        public OrderStatus Status { get; set; }

        //public List<ScanFileData> scanFileData = new List<ScanFileData>();

        public OrderMasterData()
        { }
        public OrderMasterData(OrderMasterData omd)
        {
            this.Command= omd.Command;

            ORD_NO = omd.ORD_NO;
            DLR_NO = omd.DLR_NO;
            DLR_PO = omd.DLR_PO;
            ORD_DTE = omd.ORD_DTE;
            SHP_DTE = omd.SHP_DTE;
            SHIP_DTE = omd.SHIP_DTE;
            CMNT1 = omd.CMNT1;
            CMNT2 = omd.CMNT2;
            DLR_NME = omd.DLR_NME;
            DLR_ADDR = omd.DLR_ADDR;
            DLR_ADDR2 = omd.DLR_ADDR2;
            SHP_NME = omd.SHP_NME;
            SHP_ADDR = omd.SHP_ADDR;
            SHP_ADDR2 = omd.SHP_ADDR2;
            SHP_CSZ = omd.SHP_CSZ;
            SHP_TEL = omd.SHP_TEL;
            SHP_CT = omd.SHP_CT;
            SHP_ZIP = omd.SHP_ZIP;
            CUS_NME = omd.CUS_NME;
            CUS_ADDR = omd.CUS_ADDR;
            CUS_CSZ = omd.CUS_CSZ;
            CUS_TEL = omd.CUS_TEL;
            RTE_CDE = omd.RTE_CDE;
            ORD_TYPE = omd.ORD_TYPE;
            ENT_BY = omd.ENT_BY;
            ORD_AMT = omd.ORD_AMT;
            WIN_QTY = omd.WIN_QTY;
            STK_QTY = omd.STK_QTY;
            CMP_QTY = omd.CMP_QTY;
            SHP_QTY = omd.SHP_QTY;
            SHP_AMT = omd.SHP_AMT;
            MISC_TEXT = omd.MISC_TEXT;
            
            this.ManId=omd.ManId;
            this.IsSelected=omd.IsSelected;
            this.SCAN_DATE_TIME= omd.SCAN_DATE_TIME;

            //scanFileData.AddRange(omd.scanFileData);
        }

        public OrderMasterData(orders sd)
        {
            Command = sd.command;
            RequestId = NewGuid(sd.requestId);
            ManId = sd.ManifestId;


            ORD_NO = sd.ORD_NO;
            DLR_NO = (int)sd.DLR_NO;
            DLR_PO = sd.DLR_PO;
            ORD_DTE = sd.ORD_DTE;
            SHP_DTE = sd.SHP_DTE;
            SHIP_DTE = sd.SHIP_DTE;
            CMNT1 = sd.CMNT1;
            CMNT2 = sd.CMNT2;
            DLR_NME = sd.DLR_NME;
            DLR_ADDR = sd.DLR_ADDR;
            DLR_ADDR2 = sd.DLR_ADDR2;
            SHP_NME = sd.SHP_NME;
            SHP_ADDR = sd.SHP_ADDR;
            SHP_ADDR2 = sd.SHP_ADDR2;
            SHP_CSZ = sd.SHP_CSZ;
            SHP_TEL = sd.SHP_TEL;
            SHP_CT = sd.SHP_CT;
            SHP_ZIP = sd.SHP_ZIP;
            CUS_NME = sd.CUS_NME;
            CUS_ADDR = sd.CUS_ADDR;
            CUS_CSZ = sd.CUS_CSZ;
            CUS_TEL = sd.CUS_TEL;
            RTE_CDE = sd.RTE_CDE;
            ORD_TYPE = sd.ORD_TYPE;
            ENT_BY = sd.ENT_BY;
            ORD_AMT = sd.ORD_AMT;
            ENT_BY = sd.ENT_BY;
            WIN_QTY = sd.WIN_QTY;
            STK_QTY = sd.STK_QTY;
            CMP_QTY = sd.CMP_QTY;
            SHP_QTY = sd.SHP_QTY;
            SHP_AMT = sd.SHP_AMT;
            MISC_TEXT = sd.MISC_TEXT;
            IsSelected = false;
        }
        public OrderData GetOrderData()
        {
            return new OrderData()
            {
                //CLR = this.CLR,
                Command = this.Command,
                SHP_DTE = this.SHP_DTE,
                //CustomerId = this.CustomerId,
                //DESC = this.DESC,
                //DLR_NO = this.DLR_NO,
                //DSP_SEQ = this.DSP_SEQ,
                //HEIGHT = this.HEIGHT,
                IsSelected = this.IsSelected,
                ManifestId = this.ManId,
                //MDL_CNT = this.MDL_CNT,
                //MDL_NO = this.MDL_NO,
                ORD_NO = (int)this.ORD_NO,
                RequestId = this.RequestId,
                Status = this.Status,
                status = this.status
                //WIDTH = this.WIDTH,
                //WIN_CNT = this.WIN_CNT

            };
        }
        public OrderMasterData(orderMaster sd, bool isSelected=false)
        {
            Command = sd.command;
            RequestId = NewGuid(sd.requestId);
            ManId = sd.ManId;

            ORD_NO = sd.ORD_NO;
            DLR_NO = sd.DLR_NO;
            DLR_PO = sd.DLR_PO;
            ORD_DTE = sd.ORD_DTE;
            SHP_DTE = sd.SHP_DTE;
            SHIP_DTE = sd.SHIP_DTE;
            CMNT1 = sd.CMNT1;
            CMNT2 = sd.CMNT2;
            DLR_NME = sd.DLR_NME;
            DLR_ADDR = sd.DLR_ADDR;
            DLR_ADDR2 = sd.DLR_ADDR2;
            SHP_NME = sd.SHP_NME;
            SHP_ADDR = sd.SHP_ADDR;
            SHP_ADDR2 = sd.SHP_ADDR2;
            SHP_CSZ = sd.SHP_CSZ;
            SHP_TEL = sd.SHP_TEL;
            SHP_CT = sd.SHP_CT;
            SHP_ZIP = sd.SHP_ZIP;
            CUS_NME = sd.CUS_NME;
            CUS_ADDR = sd.CUS_ADDR;
            CUS_CSZ = sd.CUS_CSZ;
            CUS_TEL = sd.CUS_TEL;
            RTE_CDE = sd.RTE_CDE;
            ENT_BY = sd.ENT_BY;
            ORD_AMT = sd.ORD_AMT;
            WIN_QTY = sd.WIN_QTY;
            STK_QTY = sd.STK_QTY;
            CMP_QTY = sd.CMP_QTY;
            SHP_QTY = sd.SHP_QTY;
            SHP_AMT = sd.SHP_AMT;
            MISC_TEXT = sd.MISC_TEXT;

            //scanFileData.AddRange(sd.ScanFile.Select(s => new ScanFileData(s)).ToList());
        }
        public override string ToString()
        {
            return $"Command:{Enum.GetName(typeof(eCommand), Command) + Environment.NewLine}" +
                $"\t\t{RequestId + Environment.NewLine}" +
                $"\t\t{ManId + Environment.NewLine}" +
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
        }

        public override int GetHashCode()
        {
            return ORD_NO.GetHashCode();
        }
        public override bool Equals(OrderMasterData other)
        {
            return this.Command == other.Command &&
                this.ORD_NO == other.ORD_NO && this.ManId == other.ManId;
                //this.SHIP_DTE == other.SHIP_DTE && 
                //this.DLR_NO == other.DLR_NO;
        }
    }
}
