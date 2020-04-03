using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MobileDeliveryGeneral.Data;
using MobileDeliveryGeneral.Interfaces.DataInterfaces;
using static MobileDeliveryGeneral.Definitions.enums;
using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.DataManager.Interfaces
{
    public interface isaDataAccess<Connection, Command, Reader> 
        where Connection : DbConnection, ICloneable, new() where Command : DbCommand, ICloneable, new() where Reader : DbDataReader
    {
        //void QueryData(Action<DbDataReader, Func<byte[], Task>> cb, string SQL, Func<byte[], Task> cbsend);
        IMDMMessage QueryData(Func<byte[], Task> cb, isaCommand dat);

        IEnumerable<IMDMMessage> InsertData(SPCmds sql, IMDMMessage msg);
        bool InsertData(string sql, IMDMMessage msg);
    }
}
