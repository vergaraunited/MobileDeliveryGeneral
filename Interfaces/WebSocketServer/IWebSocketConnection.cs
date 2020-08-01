using System;
using System.Threading.Tasks;

namespace MobileDeliveryGeneral.Interfaces.Interfaces
{
    public interface IWebSocketConnection
    {
        Action OnOpen { get; set; }
        Action OnClose { get; set; }
        Action<string> OnMessage { get; set; }
        Action<byte[]> OnBinary { get; set; }
        //Action<byte[]> OnPing { get; set; }
        //Action<byte[]> OnPong { get; set; }
        Action<Exception> OnError { get; set; }
        Task Send(string message);
        Task Send(byte[] message);
        Task SendH(byte[] message, IHandler handler);
        //Task Send(command message);
        //Task SendPing(byte[] message);
        //Task SendPong(byte[] message);
        void Close();
        IWebSocketConnectionInfo ConnectionInfo { get; }
        bool IsAvailable { get; }
    }

}
