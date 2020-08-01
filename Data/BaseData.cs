using System;
using MobileDeliveryGeneral.Definitions;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;

namespace MobileDeliveryGeneral.Data
{
    public abstract class BaseData<T> : IMDMMessage, IComparable<T>, IEquatable<T>
    {
        public virtual MsgTypes.eCommand Command { get; set; }
        public virtual Guid RequestId { get; set; }
        public virtual status status { get; set; }
        public virtual bool Equals(T other)
        {
            return true;
        }

        public virtual int CompareTo(T mmd) { return 0; }
    }
}
