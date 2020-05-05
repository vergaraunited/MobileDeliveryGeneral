using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class OrderOptionsData : BaseData<OrderOptionsData>
    {
        public override eCommand Command { get; set; } = eCommand.OrderOptions;
        public long ORD_NO;
        public short MDL_CNT;
        public byte PAT_POS;
        public string MDL_NO;                   
        public string OPT_TYPE;                 //2
        public short OPT_NUM;
        public string CLR;                      //3
        public string DESC;                     //60
        public int Count;
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
           
        }

        public bool Equals(OrderOptionsData other)
        {
            return this.ORD_NO == other.ORD_NO && this.MDL_NO == other.MDL_NO && this.PAT_POS == other.PAT_POS;
        }

    }
}
