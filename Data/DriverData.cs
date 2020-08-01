using System;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class DriverData : BaseData<DriverData>
    {
        public override eCommand Command { get; set; } = eCommand.Drivers;
        public int DriverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public override String ToString() {
            return  "(" + DriverId + ") " + LastName + ", " + FirstName;
        }
        public DriverData()
        { }

        //public DriverData(DriverData dat)
        //{
        //    Command = dat.Command;
        //    DriverId = dat.DriverId;
        //    FirstName = dat.FirstName;
        //    LastName = dat.LastName;
        //}

        public DriverData(drivers dat)
        {
            Command = dat.command;
            DriverId = dat.DriverId;
            FirstName = dat.FirstName;
            LastName = dat.LastName;
        }
        public bool Equals(DriverData other)
        {
            return DriverId == other.DriverId && FirstName == other.FirstName && LastName == other.LastName ? true:false;
        }
    }
}
