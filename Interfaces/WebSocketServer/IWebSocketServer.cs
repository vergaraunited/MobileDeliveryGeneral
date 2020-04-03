using System;

namespace MobileDeliveryGeneral.Interfaces.Interfaces
{
    public interface IWebSocketServer : IDisposable
    {
        void Start(Action<IWebSocketConnection> config);
    }

}
