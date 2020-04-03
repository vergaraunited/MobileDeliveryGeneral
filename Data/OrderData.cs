using System;
using MobileDeliveryGeneral.Definitions;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class OrderData : IMDMMessage, IComparable<OrderData>, IEquatable<OrderData>
    {
        public eCommand Command { get; set; }

        public Guid RequestId { get; set; }
        public long ManifestId { get; set; }
        public int DSP_SEQ { get; set; }
        public int CustomerId { get; set; }
        public long DLR_NO { get; set; }
        public long ORD_NO { get; set; }
        public string CLR { get; set; }
        public int MDL_CNT { get; set; }
        public int MDL_NO { get; set; }
        public int WIN_CNT { get; set; }
        public string Status { get; set; }

        public OrderData()
        { }
        public OrderData(OrderData ord)
        {
            this.Command = ord.Command;
            this.RequestId = ord.RequestId;
            this.ManifestId = ord.ManifestId;
            this.DSP_SEQ = ord.DSP_SEQ;
            this.CustomerId = ord.CustomerId;
            this.DLR_NO = ord.DLR_NO;
            this.ORD_NO = ord.ORD_NO;
            this.CLR = ord.CLR;
            this.MDL_CNT = ord.MDL_CNT;
            this.MDL_NO = ord.MDL_NO;
            this.WIN_CNT = ord.WIN_CNT;
            this.Status = ord.Status;
        }
        public OrderData(orders ord)
        {
            this.Command = ord.command;
            this.RequestId = new Guid(ord.requestId);
            this.ManifestId = ord.ManifestId;
            this.DSP_SEQ = ord.DSP_SEQ;
            this.CustomerId = ord.CustomerId;
            this.DLR_NO = ord.DLR_NO;
            this.ORD_NO = ord.ORD_NO;
            this.CLR = ord.CLR;
            this.MDL_CNT = ord.MDL_CNT;
            this.MDL_NO = ord.MDL_NO;
            this.WIN_CNT = ord.WIN_CNT;
            this.Status = ord.Status;
        }
        public int CompareTo(OrderData other)
        {
            if (this.ManifestId.CompareTo(other.ManifestId) == 0 &&
            this.DSP_SEQ.CompareTo(other.DSP_SEQ) == 0 &&
            this.CustomerId.CompareTo(other.CustomerId) == 0 &&
            this.DLR_NO.CompareTo(other.DLR_NO) == 0 &&
            this.ORD_NO.CompareTo(other.ORD_NO) == 0 &&
            this.CLR.CompareTo(other.CLR) == 0 &&
            this.MDL_CNT.CompareTo(other.MDL_CNT) == 0 &&
            this.MDL_NO.CompareTo(other.MDL_NO) == 0 &&
            this.WIN_CNT.CompareTo(other.WIN_CNT) == 0 &&
            this.Status.CompareTo(other.Status) == 0)
                return 0;
            else return 1;
        }

        public bool Equals(OrderData other)
        {
            return (this.ManifestId == other.ManifestId &&
                        this.DSP_SEQ == other.DSP_SEQ &&
                        this.CustomerId == other.CustomerId &&
                        this.DLR_NO == other.DLR_NO &&
                        this.ORD_NO == other.ORD_NO &&
                        this.CLR == other.CLR &&
                        this.MDL_CNT == other.MDL_CNT &&
                        this.MDL_NO == other.MDL_CNT &&
                        this.WIN_CNT == other.MDL_CNT &&
                        this.Status == other.Status);
        }
    }
}
