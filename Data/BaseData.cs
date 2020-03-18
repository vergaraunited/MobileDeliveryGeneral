using System;
using System.Collections.Generic;
using System.Text;
using UMDGeneral.Definitions;
using UMDGeneral.Interfaces.DataInterfaces;
using static UMDGeneral.Definitions.MsgTypes;

namespace UMDGeneral.Data
{
    public class BaseData<T> : IMDMMessage, IComparable<T>, IEquatable<T>
    {
        public MsgTypes.eCommand Command { get; set; }
        public Guid RequestId { get; set; }

        public virtual bool Equals(T other)
        {
            return true;
        }

        public virtual int CompareTo(T mmd) { return 0; }

        //public BaseData(isaCommand dat)
        //{
        //    Command = dat.command;
        //}
    }
}
