using MobileDeliveryGeneral.ExtMethods;
using System;
using System.Collections.Generic;
using System.Text;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class AccountsReceivableData : BaseData<AccountsReceivableData>
    {
        public override eCommand Command { get; set; } = eCommand.Orders;
        public long ORD_NO;
        public long DLR_NO;
        public DateTime SHP_DTE;

        public int DSP_SEQ { get; set; }
        public int CustomerId { get; set; }
        public string CLR { get; set; }
        public int MDL_CNT { get; set; }
        public string MDL_NO { get; set; }
        public int WIN_CNT { get; set; }
        public string DESC { get; set; }
        public OrderStatus Status { get; set; }


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
        public bool IsSelected;
        public long ManifestId;

        public decimal WIDTH { get; set; }
        public decimal HEIGHT { get; set; }
        public byte PAT_POS;
        public string OPT_TYPE;
        public short OPT_NUM;
        public DateTime SCAN_DATE_TIME { get; set; }


        public AccountsReceivableData()
        { }
        public AccountsReceivableData(AccountsReceivableData omd)
        {
            this.Command = omd.Command;
            this.ORD_NO = omd.ORD_NO;
            this.DLR_NO = omd.DLR_NO;
            this.SHP_DTE = omd.SHP_DTE;

            this.DSP_SEQ = omd.DSP_SEQ;
            this.CustomerId = omd.CustomerId;
            this.CLR = omd.CLR;
            this.MDL_CNT = omd.MDL_CNT;
            this.MDL_NO = omd.MDL_NO;
            this.WIN_CNT = omd.WIN_CNT;
            this.DESC = omd.DESC;
            this.Status = omd.Status;


            this.DLR_NME = omd.DLR_NME;
            this.DLR_ADDR = omd.DLR_ADDR;
            this.DLR_ADDR2 = omd.DLR_ADDR2;
            this.DLR_TEL = omd.DLR_TEL;
            this.DLR_CT = omd.DLR_CT;
            this.SHP_NME = omd.SHP_NME;
            this.SHP_ADDR = omd.SHP_ADDR;
            this.SHP_ADDR2 = omd.SHP_ADDR2;
            this.SHP_TEL = omd.SHP_TEL;
            this.SHP_ZIP = omd.SHP_ZIP;
            this.CUS_NME = omd.CUS_NME;
            this.RTE_CDE = omd.RTE_CDE;
            this.SHP_QTY = omd.SHP_QTY;
            this.ManId = omd.ManId;
            this.IsSelected = omd.IsSelected;
            this.ManifestId = omd.ManifestId;

            this.WIDTH = omd.WIDTH;
            this.HEIGHT = omd.HEIGHT;
            this.PAT_POS = omd.PAT_POS;
            this.OPT_TYPE = omd.OPT_TYPE;
            this.OPT_NUM = omd.OPT_NUM;
            this.SCAN_DATE_TIME = omd.SCAN_DATE_TIME;
        }
        public AccountsReceivableData(orderDetails ar)
        {
            Command = ar.command;
            RequestId = NewGuid(ar.requestId);
            //ORD_NO = sd.ORD_NO;
            //DLR_NO = sd.DLR_NO;
            //ManifestId = sd.ManifestId;
            //SHP_DTE = DateTime.Parse(ExtensionMethods.FromJulianToGregorian((long)(sd.SHP_DTE), "MM/dd/yyyy"));

            //DLR_NME = sd.DLR_NME;
            //DLR_ADDR = sd.DLR_ADDR;
            //DLR_ADDR2 = sd.DLR_ADDR2;
            //DLR_TEL = sd.DLR_TEL;
            //DLR_CT = sd.DLR_CT;

            //SHP_NME = sd.SHP_NME;
            //SHP_ADDR = sd.SHP_ADDR;
            //SHP_ADDR2 = sd.SHP_ADDR2;

            //SHP_TEL = sd.SHP_TEL;
            //SHP_ZIP = sd.SHP_ZIP;

            //CUS_NME = sd.CUS_NME;
            //RTE_CDE = sd.RTE_CDE;
            //SHP_QTY = sd.SHP_QTY;
            IsSelected = false;
        }
        public AccountsReceivableData(orderDetails dat, bool isSelected = false)
        {
            Command = dat.command;
            RequestId = NewGuid(dat.requestId);
            ORD_NO = dat.ORD_NO;
            //DLR_NO = dat.DLR_NO;
            //SHP_DTE = DateTime.Parse(ExtensionMethods.FromJulianToGregorian((long)(dat.SHIP_DTE), "MM/dd/yyyy"));

            //DLR_NME = dat.DLR_NME;
            //DLR_ADDR = dat.DLR_ADDR;
            //DLR_ADDR2 = dat.DLR_ADDR2;
            //DLR_TEL = dat.DLR_TEL;
            //DLR_CT = dat.DLR_CT;

            //SHP_NME = dat.SHP_NME;
            //SHP_ADDR = dat.SHP_ADDR;
            //SHP_ADDR2 = dat.SHP_ADDR2;

            //SHP_TEL = dat.SHP_TEL;
            //SHP_ZIP = dat.SHP_ZIP;

            //CUS_NME = dat.CUS_NME;
            //RTE_CDE = dat.RTE_CDE;
            //SHP_QTY = dat.SHP_QTY;
            //Status = dat.Status;
            IsSelected = isSelected;
            //ManId = dat.ManId;
        }

        public bool Equals(AccountsReceivableData other)
        {
            return this.Command == other.Command &&
                this.ORD_NO == other.ORD_NO &&
                this.SHP_DTE == other.SHP_DTE &&
                this.DLR_NO == other.DLR_NO;
        }
    }
}
