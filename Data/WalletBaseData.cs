using MobileDeliveryGeneral.Definitions;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using System;

namespace MobileDeliveryGeneral.Data
{
    public class WalletBaseData<T> : IMDMMessage, IComparable<T>, IEquatable<T>
    {
        public virtual MsgTypes.eWalletCommand Command { get; set; }
        public Guid RequestId { get; set; }
        public status status { get; set; }
        MsgTypes.eCommand IMDMMessage.Command { get { return MsgTypes.eCommand.WalletCommand; } set { } }

        public virtual bool Equals(T other)
        {
            return true;
        }

        public virtual int CompareTo(T mmd) { return 0; }
    }
}
