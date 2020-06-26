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
        public string OPT_CLASS { get; set; }
        public string OPT_SOURCE { get; set; }
        public string DESC { get; set; }
        public string CLR { get; set; }
        public short MDL_CNT { get; set; }
        public byte PAT_POS { get; set; }
        public DateTime SCAN_DATE_TIME { get; set; }

        public int Count;
        private bool isselected;
        public OrderStatus Status { get; set; }

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
           // BIN_NO = od.BIN;
            MDL_CNT = od.MDL_CNT;
            PAT_POS = Convert.ToByte(od.PAT_POS);

            SCAN_DATE_TIME = od.ScanTime;
        }

        String Index() {
            return ORD_NO.ToString() + MDL_CNT.ToString() + PAT_POS.ToString() + MDL_CNT.ToString();
        }

        public override bool Equals(OrderDetailsData other)
        {
            return this.Command == other.Command &&
                this.ORD_NO == other.ORD_NO &&
                this.OPT_NUM == other.OPT_NUM &&
                this.DLR_NO == other.DLR_NO &&
                this.MDL_CNT == other.MDL_CNT &&
                this.PAT_POS == other.PAT_POS &&
                this.MDL_NO == other.MDL_NO;

        }
        public override string ToString()
        {
            return $"Command:{Enum.GetName(typeof(eCommand), Command) + Environment.NewLine}" +
                $"\t\t{RequestId + Environment.NewLine}" +
                $"\t\t{ORD_NO + Environment.NewLine}" +
                $"\t\t{OPT_NUM + Environment.NewLine}" +
                $"\t\t{DLR_NO + Environment.NewLine}" +
                $"\t\t{MDL_CNT + Environment.NewLine}" +
                $"\t\t{PAT_POS + Environment.NewLine}" +
                $"\t\t{OPT_SOURCE + Environment.NewLine}" +
                $"\t\t{MDL_NO + Environment.NewLine}";

        }
        public override int GetHashCode()
        {
            return Index().GetHashCode();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        // List<Item> itList = new List<Item>();
    }
}
