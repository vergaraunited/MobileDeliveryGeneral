using System;
using System.ComponentModel;
using MobileDeliveryGeneral.ExtMethods;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class OrderDetailsData : BaseData<OrderDetailsData>
    {
        public delegate void cmdFireOnSelected(OrderDetailsData od);
        public cmdFireOnSelected OnSelectionChanged;

        public override eCommand Command { get; set; } = eCommand.OrderDetails;
        public int LineNumber { get; set; }
        public long ORD_NO { get; set; }
        public string MDL_NO { get; set; }
        public long DLR_NO { get; set; }
        public short OPT_NUM { get; set; }
        public string OPT_TYPE { get; set; }             //2
        public string STOCK_ID { get; set; }                 //30
        public string CALL_SIZE { get; set; }                //10
        public string DESC { get; set; }
        public short QTY { get; set; }
        public short SHP_QTY { get; set; }                  //2
        public string CLR { get; set; }
        public decimal WIDTH { get; set; }
        public decimal HEIGHT { get; set; }
        public string CMT1 { get; set; }
        public string CMT2 { get; set; }
        public short MDL_CNT { get; set; }
        public short WIN_CNT { get; set; }
        public string RTE_CDE { get; set; }
        public long SHP_DTE { get; set; }
        public byte PAT_POS { get; set; }
        public DateTime SCAN_DATE_TIME { get; set; }
        public int Count;
        private bool isselected;

        public bool IsSelected { get{return isselected; } set { if ( OnSelectionChanged != null ) { Command = eCommand.OrdersLoadComplete; OnSelectionChanged(this); }  isselected = value; } }
        
        public OrderDetailsData(){}
        public OrderDetailsData(orderDetails od)
        {
            Command = od.command;
            RequestId = NewGuid(od.requestId);
            ORD_NO = od.ORD_NO;
            MDL_NO = od.MDL_NO;
            OPT_NUM = od.OPT_NUM;
            OPT_TYPE = od.OPT_TYPE;
            DESC = od.DESC;
            CLR = od.CLR;
            WIDTH = od.WIDTH;
            HEIGHT = od.HEIGHT;
            MDL_CNT = od.MDL_CNT;
            WIN_CNT = od.LINK_WIN_CNT;
            PAT_POS = od.PAT_POS;
            SCAN_DATE_TIME = od.ScanTime;
            // RTE_CDE = Convert.ToString(mst.TRUCK);
        }
        public override bool Equals(OrderDetailsData other)
        {
            return this.Command == other.Command &&
                this.ORD_NO == other.ORD_NO &&
                this.OPT_NUM == other.OPT_NUM &&
                this.DLR_NO == other.DLR_NO &&
                this.WIN_CNT == other.WIN_CNT &&
                this.PAT_POS == other.PAT_POS;

        }
        public event PropertyChangedEventHandler PropertyChanged;
        // List<Item> itList = new List<Item>();
    }
}
