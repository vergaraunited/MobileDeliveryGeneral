using System;
using System.ComponentModel;
using UMDGeneral.ExtMethods;
using UMDGeneral.Interfaces.DataInterfaces;
using static UMDGeneral.Definitions.MsgTypes;

namespace UMDGeneral.Data
{
    public class OrderDetailsData : IMDMMessage, IEquatable<OrderDetailsData>, INotifyPropertyChanged
    {
        public eCommand Command { get; set; }
        public Guid RequestId { get; set; }
        public int LineNumber { get; set; }
        public long ORD_NO { get; set; }
        public short MDL_NO { get; set; }
        public long DLR_NO { get; set; }
        public short OPT_NUM { get; set; }
        public string DESC { get; set; }
        public string CLR { get; set; }
        public float WIDTH { get; set; }
        public float HEIGHT { get; set; }
        public short MDL_CNT { get; set; }
        public short WIN_CNT { get; set; }
        public string RTE_CDE { get; set; }

        public OrderDetailsData(){}
        public OrderDetailsData(orderDetails od)
        {
            RequestId = new Guid(od.requestId);
            ORD_NO = BitConverter.ToInt32(od.ORD_NO, 0);
            MDL_NO = BitConverter.ToInt16(od.MDL_NO, 0);
            OPT_NUM = BitConverter.ToInt16(od.OPT_NUM, 0);
            DESC = od.DESC.UMToString(fldsz_DESCOrd);
            CLR = od.CLR.UMToString(fldsz_CLR);
            WIDTH = BitConverter.ToSingle(od.WIDTH, 0);
            HEIGHT = BitConverter.ToSingle(od.HEIGHT, 0);
            MDL_CNT = BitConverter.ToInt16(od.MDL_CNT, 0);
            WIN_CNT = BitConverter.ToInt16(od.LINK_WIN_CNT, 0);
            // RTE_CDE = Convert.ToString(mst.TRUCK);
        }
        public bool Equals(OrderDetailsData other)
        {
            return this.Command == other.Command &&
                this.ORD_NO == other.ORD_NO &&
                this.OPT_NUM == other.OPT_NUM &&
                this.DLR_NO == other.DLR_NO &&
                this.WIN_CNT == other.WIN_CNT;

        }
        public event PropertyChangedEventHandler PropertyChanged;
        // List<Item> itList = new List<Item>();
    }
}
