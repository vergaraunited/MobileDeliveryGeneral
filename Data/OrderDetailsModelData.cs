using MobileDeliveryGeneral.DataManager.Interfaces;
using SQLite;
using static MobileDeliveryGeneral.Definitions.MsgTypes;
using MobileDeliveryGeneral.Definitions;
using System;

namespace MobileDeliveryGeneral.Data
{
    public class OrderDetailsModelData : BaseData<OrderDetailsModelData>, isaCacheItem<OrderDetailsModelData>
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ORD_NO { get; set; }

        public short MDL_CNT { get; set; }
        public string MDL_NO { get; set; }


        public string CLR { get; set; }

        public short OPT_NUM { get; set; }
        public string OPT_TYPE { get; set; }
        public byte PAT_POS { get; set; }
        public DateTime SCAN_DATE_TIME { get; set; }
        public Int16 BIN_NO { get; set; }

        public string DESC { get; set; }
        public decimal WIDTH { get; set; }
        public decimal HEIGHT { get; set; }
        public string ScanTime { get; set; }
        public OrderStatus Status { get; set; }
        private bool isselected;
        public bool prevstate;
        public bool IsSelected { get { return isselected; } set { isselected = value; prevstate = !isselected; if ((((status == status.Uploaded || status == status.Init || status == status.Completed) && Status == OrderStatus.New) || ((status == status.Uploaded || status == status.Init || status == status.Completed) && Status == OrderStatus.Delivered) || ((status == status.Releasing || status == status.Completed || status == status.Uploaded) && Status == OrderStatus.Shipped)) && prevstate != isselected && OnSelectionChanged != null) { OnSelectionChanged(this); } } }

        public delegate void cmdFireOnSelected(OrderDetailsModelData od);
        public cmdFireOnSelected OnSelectionChanged;
    }
}
