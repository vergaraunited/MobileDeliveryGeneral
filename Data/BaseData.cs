﻿using System;
using System.Collections.Generic;
using System.Text;
using MobileDeliveryGeneral.Definitions;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class BaseData<T> : IMDMMessage, IComparable<T>, IEquatable<T>
    {
        public virtual MsgTypes.eCommand Command { get; set; }
        public Guid RequestId { get; set; }
        public status status { get; set; }
        public virtual bool Equals(T other)
        {
            return true;
        }

        public virtual int CompareTo(T mmd) { return 0; }
    }
}
