using System;

namespace UMDGeneral.Interfaces.Interfaces
{
    public interface IWebSocketServer : IDisposable
    {
        void Start(Action<IWebSocketConnection> config);
    }

}
