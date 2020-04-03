using System;
using System.Collections.Generic;
using System.Text;
using MobileDeliveryGeneral.Interfaces;

namespace MobileDeliveryGeneral.DataManager.Interfaces
{
    public interface isaUMDData : isaReceiveMessageCallback, isaSendMessageCallback, isaProcessMessage
    {
    }
}
