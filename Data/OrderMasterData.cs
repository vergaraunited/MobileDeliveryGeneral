using System;
using MobileDeliveryGeneral.ExtMethods;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class OrderMasterData : IMDMMessage, IEquatable<OrderMasterData>
    {
        public eCommand Command { get; set; } = eCommand.Orders;
        public Guid RequestId { get; set; }
        public long ORD_NO;
        public long DLR_NO;
        public DateTime SHP_DTE;
        public string DLR_NME;
        public string DLR_ADDR;
        public string DLR_ADDR2;
        public string DLR_TEL;
        public short DLR_CT;
        public string SHP_NME;
        public string SHP_ADDR;
        public string SHP_ADDR2;
        public string SHP_TEL;
        public string SHP_ZIP;
        public string CUS_NME;
        public string RTE_CDE;
        public int SHP_QTY;
        public long ManId;

        public OrderMasterData()
        { }

        public OrderMasterData(orderMaster dat)
        {
            Command = dat.command;
            RequestId = new Guid(dat.requestId);
            ORD_NO = BitConverter.ToInt32(dat.ORD_NO, 0);
            DLR_NO = BitConverter.ToInt32(dat.DLR_NO, 0);
            SHP_DTE = DateTime.Parse(ExtensionMethods.FromJulianToGregorian((long)(dat.SHIP_DTE),"MM/dd/yyyy"));

            DLR_NME = dat.DLR_NME.UMToString(fldsz_SHP_NME);
            DLR_ADDR = dat.DLR_ADDR.UMToString(fldsz_SHP_NME);
            DLR_ADDR2 = dat.DLR_ADDR2.UMToString(fldsz_SHP_NME);          
            DLR_TEL = dat.DLR_TEL.UMToString(fldsz_SHP_TEL);
            DLR_CT = dat.DLR_CT;

            SHP_NME = dat.SHP_NME.UMToString(fldsz_SHP_NME);
            SHP_ADDR = dat.SHP_ADDR.UMToString(fldsz_SHP_NME);
            SHP_ADDR2 = dat.SHP_ADDR2.UMToString(fldsz_SHP_NME);

            SHP_TEL = dat.SHP_TEL.UMToString(fldsz_SHP_TEL);
            SHP_ZIP = dat.SHP_ZIP.UMToString(fldsz_SHP_ZIP);

            CUS_NME = dat.CUS_NME.UMToString(fldsz_CUS_NME);
            RTE_CDE = dat.RTE_CDE.UMToString(fldsz_RTE_CDE);
            SHP_QTY = BitConverter.ToInt16(dat.SHP_QTY, 0);
        }

        public bool Equals(OrderMasterData other)
        {
            return this.Command == other.Command && 
                this.ORD_NO == other.ORD_NO && 
                this.SHP_DTE == other.SHP_DTE && 
                this.DLR_NO == other.DLR_NO;
        }
    }
}
