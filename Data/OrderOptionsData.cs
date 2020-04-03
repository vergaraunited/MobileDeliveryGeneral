using System;
using MobileDeliveryGeneral.ExtMethods;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class OrderOptionsData : IMDMMessage, IEquatable<OrderOptionsData>
    {
        public long ORD_NO;
        public short MDL_CNT;
        public byte PAT_POS;
        public long MODEL;                   //4
        public long MDL_NO;                   //4
        public short OPT_TYPE;                 //2
        public short OPT_NUM;
        public string CLR;                      //3
        public string DESC;                     //60

        public eCommand Command { get; set; }
        public Guid RequestId { get; set; }

        public OrderOptionsData()
        { }

        public OrderOptionsData(orderOptions dat)
        {
            Command = dat.command;
            ORD_NO = BitConverter.ToInt32(dat.ORD_NO, 0);
            MDL_CNT = BitConverter.ToInt16(dat.MDL_CNT, 0);
            PAT_POS = dat.PAT_POS;
            MODEL = BitConverter.ToInt32(dat.MODEL, 0);

            MDL_NO = BitConverter.ToInt32(dat.MDL_NO,0);

            OPT_TYPE = BitConverter.ToInt16(dat.OPT_TYPE, 0);
            OPT_NUM = BitConverter.ToInt16(dat.OPT_NUM,0);
            
            CLR = dat.CLR.UMToString(fldsz_DESCOrd);
            DESC = dat.DESC.UMToString(fldsz_DESCOrd);
           
        }

        public bool Equals(OrderOptionsData other)
        {
            return this.ORD_NO == other.ORD_NO && this.MDL_NO == other.MDL_NO && this.PAT_POS == other.PAT_POS;
        }

    }
}
