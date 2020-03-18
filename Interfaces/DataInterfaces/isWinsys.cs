using System;
using System.Collections.Generic;
using System.Text;

namespace UMDGeneral.Interfaces.DataInterfaces
{
    public interface isWinsys<T>  where T : struct
    {
        T GetDescription();
    }
}
