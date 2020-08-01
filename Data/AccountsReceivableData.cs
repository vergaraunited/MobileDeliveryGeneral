using System;
using System.IO;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class AccountsReceivableData : BaseData<AccountsReceivableData>
    {
        public override eCommand Command { get; set; } = eCommand.AccountReceivable;
        public int ORD_NO;
        public int DLR_NO;
        public DateTime SHP_DTE;

        public int InvoiceNum { get; set; }
        public string DLR_NME { get; set; }

        public string RTE_CDE { get; set; }
        public string CustomerName { get; set; }
        public int MDL_CNT { get; set; }
        public byte [] Signature { get; set; }
        public string DealerPO { get; set; }
        public string NOTES { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public DateTime BillDateTime { get; set; }
        public int WIN_CNT { get; set; }
       // public Image POD { get; set; }

        public AccountsReceivableData()
        { RequestId = NewGuid(); }
        public AccountsReceivableData(AccountsReceivableData omd)
        {
            this.Command = omd.Command;
            this.RequestId = omd.RequestId;
            this.ORD_NO = omd.ORD_NO;
            this.DLR_NO = omd.DLR_NO;
            this.SHP_DTE = omd.SHP_DTE;

            
            this.DLR_NME = omd.DLR_NME;
            this.RTE_CDE = omd.RTE_CDE;
            this.CustomerName = omd.CustomerName;
            this.MDL_CNT = omd.MDL_CNT;
            this.Signature = omd.Signature;
            using (var mem = new MemoryStream(Signature))
            {
                //Now you can
                //img_result.Source = ImageSource.FromStream(() => mstream);
                //    POD = Image.FromStream(mem);
                //  Image image = Image.FromStream(new MemoryStream(bitmap));
            }
                this.Status = omd.Status;

                this.DeliveryDateTime = omd.DeliveryDateTime;
                this.BillDateTime = omd.BillDateTime;
            }
        public AccountsReceivableData(accountReceivable ar)
        {
            Command = ar.command;
            RequestId = NewGuid(ar.requestId);
            ORD_NO = ar.ORD_NO;
            CustomerName = ar.CUS_NME;
            //MDL_CNT = ar.MDL;
            Signature = ar.POD;

            Status = (OrderStatus)ar.Status_OD;
            DeliveryDateTime = ar.ScanDateTime_OD;
            BillDateTime = ar.Timestamp;
            SHP_DTE = ar.SHIP_DTE;
            DLR_NME = ar.DLR_NME;
            RTE_CDE = ar.RTE_CDE;

            //DLR_ADDR = ar.DLR_ADDR;
            //DLR_ADDR2 = sd.DLR_ADDR2;
            //DLR_TEL = sd.DLR_TEL;
            //DLR_CT = sd.DLR_CT;

            //SHP_NME = sd.SHP_NME;
            //SHP_ADDR = sd.SHP_ADDR;
            //SHP_ADDR2 = sd.SHP_ADDR2;

            //SHP_TEL = sd.SHP_TEL;
            //SHP_ZIP = sd.SHP_ZIP;

            //  SHP_QTY = ar.SHP_QTY;
            // IsSelected = false;
        }
        public AccountsReceivableData(accountReceivable dat, bool isSelected = false)
        {
            Command = dat.command;
            RequestId = NewGuid(dat.requestId);
            ORD_NO = dat.ORD_NO;
            //DLR_NO = da;
            DeliveryDateTime = dat.SHIP_DTE;
            Signature = dat.POD;
            
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
           // IsSelected = isSelected;
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
