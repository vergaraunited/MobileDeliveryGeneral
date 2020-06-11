using System;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class OrderOptionsData : BaseData<OrderOptionsData>
    {
        public override eCommand Command { get; set; } = eCommand.OrderOptions;

        public Int32 ORD_NO { get; set; }
        public Int16 MDL_CNT { get; set; }
        public Byte PAT_POS { get; set; }
        public String MODEL { get; set; }
        public String MDL_NO { get; set; }
        public String OPT_TYPE { get; set; }
        public Int16 OPT_NUM { get; set; }
        public String STOCK_ID { get; set; }
        public String CALL_SIZE { get; set; }
        public String CLR { get; set; }
        public String DESC { get; set;}
        public Int16 QTY { get; set; }
        public Int16 CMP_QTY { get; set; }
        public Int16 PAT_ID { get; set; }
        public Decimal WIDTH { get; set; }
        public Decimal HEIGHT { get; set; }
        public Decimal COM { get; set; }
        public Decimal PRICE { get; set; }
        public Decimal DIS_PER { get; set; }
        public Decimal DIS_AMT { get; set; }
        public Decimal DIS_UNT { get; set; }
        public Decimal NET_AMT { get; set; }
        public String CMT1 { get; set; }
        public String CMT2 { get; set; }
        public String NOTES { get; set; }
        public String SHIPPING { get; set; }
        public Int32 SHP_DTE { get; set; }
        public String TRUCK { get; set; }
        public Int16 SHP_SEQUENCE { get; set; }
        public String TYPE { get; set; }
        public String PROD_DESC { get; set; }
        public Decimal EXP_SZE { get; set; }
        public Int32 LOT_NO { get; set; }
        public Int16 ORTIDX { get; set; }
        public Int32 LOT_DTE { get; set; }
        public Int16 LOT_SEQ { get; set; }
        public Int16 BIN { get; set; }
        public Int16 BGN_BIN { get; set; }
        public Int16 END_BIN { get; set; }
        public Byte EMAILED { get; set; }
        public Int16 ADD_DAYS { get; set; }
        public Int32 DTE_ADDED { get; set; }
    
    //public long ORD_NO;
    //public short MDL_CNT;
    //public byte PAT_POS;
    //public string MDL_NO;
    //public string OPT_TYPE;                 //2
    //public short OPT_NUM;
    //public string CLR;                      //3
    //public string DESC;                     //60
    public int Count;
    //public int WIN_CNT;
    //public decimal WIDTH;
    //public decimal HEIGHT;
    public OrderOptionsData()
        { }

        public OrderOptionsData(orderOptions dat)
        {
            Command = dat.command;
            RequestId = NewGuid(dat.requestId);
            ORD_NO = dat.ORD_NO;
            MDL_CNT = dat.MDL_CNT;
            PAT_POS = dat.PAT_POS;

            MDL_NO = dat.MDL_NO;
            OPT_TYPE = dat.OPT_TYPE;
            OPT_NUM = dat.OPT_NUM;
            
            CLR = dat.CLR;
            DESC = dat.DESC;
           // WIN_CNT=dat.WIN_CNT;
            WIDTH=dat.WIDTH;
            HEIGHT=dat.HEIGHT;
        }

        public override string ToString()
        {
            return $"Command:{Enum.GetName(typeof(eCommand), Command) + Environment.NewLine}" +
                $"\t\t{RequestId + Environment.NewLine}" +
                $"\t\t{ORD_NO + Environment.NewLine}" +
                $"\t\t{MDL_CNT + Environment.NewLine}" +
                $"\t\t{PAT_POS + Environment.NewLine}" +
                $"\t\t{MDL_NO + Environment.NewLine}" +
                $"\t\t{OPT_TYPE + Environment.NewLine}" +
                $"\t\t{OPT_NUM + Environment.NewLine}" +
                $"\t\t{CLR + Environment.NewLine}" +
                $"\t\t{DESC + Environment.NewLine}" + 
               // $"\t\t{WIN_CNT + Environment.NewLine}" +                
                $"\t\t{WIDTH + Environment.NewLine}" +
                $"\t\t{HEIGHT + Environment.NewLine}";
        }

        public override bool Equals(OrderOptionsData other)
        {
            return this.ORD_NO == other.ORD_NO && this.MDL_CNT == other.MDL_CNT &&
                this.MDL_NO == other.MDL_NO && this.PAT_POS == other.PAT_POS &&
                this.OPT_NUM == other.OPT_NUM && this.OPT_TYPE == other.OPT_TYPE;
        }

    }
}
