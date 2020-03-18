using System;
using System.Collections.Generic;
using System.Text;
using UMDGeneral.Interfaces;

namespace UMDGeneral.DataManager.Interfaces
{
    public interface isaUMDData : isaReceiveMessageCallback, isaSendMessageCallback, isaProcessMessage
    {
    }
}
