using MobileDeliveryGeneral.DataManager.Interfaces;
using MobileDeliveryGeneral.ExtMethods;
using SQLite;
using System;
using System.Collections.Generic;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class OrderModelData : BaseData<OrderModelData>, isaCacheItem<OrderModelData>
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public List<OrderDetailsModelData> ordDetails { get; set; }
        public int OrderSort
        {
            get
            {
                return ORD_NO;
            }
        }

        public string SHP_ADDR { get; set; }
        public string SHP_ADDR2 { get; set; }
        public string SHP_CSZ { get; set; }
        public string DLR_ADDR { get; set; }
        public string DLR_ADDR2 { get; set; }
        public string DLR_CSZ { get; set; }
        public string DLR_TEL { get; set; }
        public int ORD_NO { get; set; }
        public DateTime SHP_DTE { get; set; }
        public string CMT1 { get; set; }
        public string CMT2 { get; set; }
        public DateTime ORD_DTE { get; set; }
        public string DLR_NME { get; set; }
        public int DLR_NO { get; set; }

        public override eCommand Command { get; set; } = eCommand.OrderModel;

        public Boolean IsVisible { get; set; }

        public OrderModelData()
        {
            ordDetails = new List<OrderDetailsModelData>();
        }
        public OrderModelData(orders sd, bool isSelected = false)
        {
            Command = sd.command;
            RequestId = NewGuid(sd.requestId);

            ORD_NO = sd.ORD_NO;
            DLR_NO = sd.DLR_NO;

            ORD_DTE = ExtensionMethods.FromJulianToGregorianDT(sd.ORD_DTE, "yyyy-MM-dd").Date;
            SHP_DTE = ExtensionMethods.FromJulianToGregorianDT(sd.SHP_DTE, "yyyy-MM-dd").Date;

            CMT1 = sd.CMNT1;
            CMT2 = sd.CMNT2;
            DLR_NME = sd.DLR_NME;
            DLR_ADDR = sd.DLR_ADDR;
            DLR_ADDR2 = sd.DLR_ADDR2;
            DLR_CSZ = sd.DLR_CSZ;
            //DLR_TEL = sd.DLR_TEL;
            SHP_ADDR = sd.SHP_ADDR;
            SHP_ADDR2 = sd.SHP_ADDR2;
            SHP_CSZ = sd.SHP_CSZ;
            //foreach(var s in sd.ordDetails)
            if (ordDetails == null)
                ordDetails = new List<OrderDetailsModelData>();
            ordDetails.Add(new OrderDetailsModelData()
            {
                PAT_POS = sd.ordDetails.PAT_POS,
                BIN_NO = sd.ordDetails.BIN_NO,
                OPT_NUM = sd.ordDetails.OPT_NUM,
                OPT_TYPE = sd.ordDetails.OPT_TYPE,
                DESC = sd.ordDetails.DESC,
                HEIGHT = sd.ordDetails.HEIGHT,
                WIDTH = sd.ordDetails.WIDTH,
                MDL_CNT = sd.ordDetails.MDL_CNT,
                MDL_NO = sd.ordDetails.MDL_NO,
                ORD_NO = sd.ordDetails.ORD_NO
            });
        }

        public OrderModelData(orderMaster sd, bool isSelected = false)
        {
            Command = sd.command;
            RequestId = NewGuid(sd.requestId);

            ORD_NO = sd.ORD_NO;
            DLR_NO = sd.DLR_NO;
           
            ORD_DTE = ExtensionMethods.FromJulianToGregorianDT(sd.ORD_DTE, "yyyy-MM-dd").Date;
            SHP_DTE = ExtensionMethods.FromJulianToGregorianDT(sd.SHP_DTE, "yyyy-MM-dd").Date;
          
            CMT1 = sd.CMNT1;
            CMT2 = sd.CMNT2;
            DLR_NME = sd.DLR_NME;
            DLR_ADDR = sd.DLR_ADDR;
            DLR_ADDR2 = sd.DLR_ADDR2;
            DLR_CSZ = sd.DLR_CSZ;
            DLR_TEL = sd.DLR_TEL;
            SHP_ADDR = sd.SHP_ADDR;
            SHP_ADDR2 = sd.SHP_ADDR2;
            SHP_CSZ = sd.SHP_CSZ;
        }
        public OrderModelData(OrderMasterData sd, bool isSelected = false)
        {
            Command = sd.Command;
            RequestId = sd.RequestId;

            ORD_NO = sd.ORD_NO;
            DLR_NO = sd.DLR_NO;

            ORD_DTE = ExtensionMethods.FromJulianToGregorianDT(sd.ORD_DTE, "yyyy-MM-dd").Date;
            SHP_DTE = ExtensionMethods.FromJulianToGregorianDT(sd.SHP_DTE, "yyyy-MM-dd").Date;

            CMT1 = sd.CMNT1;
            CMT2 = sd.CMNT2;
            DLR_NME = sd.DLR_NME;
            DLR_ADDR = sd.DLR_ADDR;
            DLR_ADDR2 = sd.DLR_ADDR2;
            DLR_CSZ = sd.DLR_CSZ;
            DLR_TEL = sd.DLR_TEL;
            SHP_ADDR = sd.SHP_ADDR;
            SHP_ADDR2 = sd.SHP_ADDR2;
            SHP_CSZ = sd.SHP_CSZ;
        }
        public override string ToString()
        {
            return $"Command:{Enum.GetName(typeof(eCommand), Command) + Environment.NewLine}" +
                $"\t\t{RequestId + Environment.NewLine}" +
                $"\t\t{ORD_NO + Environment.NewLine}" +
                $"\t\t{DLR_NO + Environment.NewLine}" +
                $"\t\t{ORD_DTE + Environment.NewLine}" +
                $"\t\t{SHP_DTE + Environment.NewLine}" +
                $"\t\t{CMT1 + Environment.NewLine}" +
                $"\t\t{CMT2 + Environment.NewLine}" +
                $"\t\t{DLR_NME + Environment.NewLine}" +
                $"\t\t{DLR_ADDR + Environment.NewLine}" +
                $"\t\t{DLR_ADDR2 + Environment.NewLine}" +
                $"\t\t{DLR_CSZ + Environment.NewLine}" +
                $"\t\t{DLR_TEL + Environment.NewLine}" +
                $"\t\t{SHP_ADDR + Environment.NewLine}" +
                $"\t\t{SHP_ADDR2 + Environment.NewLine}" +
                $"\t\t{SHP_CSZ + Environment.NewLine}";
        }

        public override int GetHashCode()
        {
            return ORD_NO.GetHashCode();
        }
        public override bool Equals(OrderModelData other)
        {
            return this.Command == other.Command &&
                this.ORD_NO == other.ORD_NO;
        }
    }
}
