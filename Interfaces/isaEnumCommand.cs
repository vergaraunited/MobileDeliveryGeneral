using System;
using System.Collections.Generic;
using System.Text;

namespace UMDGeneral.Interfaces
{
    public interface isaEnumCommand<E>
    {
        E Command { get; set; }
    }
}
