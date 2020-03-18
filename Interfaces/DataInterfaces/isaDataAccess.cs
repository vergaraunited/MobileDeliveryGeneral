using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using UMDGeneral.Data;
using UMDGeneral.Interfaces.DataInterfaces;
using static UMDGeneral.Definitions.enums;
using static UMDGeneral.Definitions.MsgTypes;

namespace UMDGeneral.DataManager.Interfaces
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
