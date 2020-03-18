using System;
using System.Collections.Generic;
using System.Text;

namespace UMDGeneral.Interfaces.Interfaces
{
    public interface IHandler
    {
        byte[] CreateHandshake(string subProtocol = null);
        void Receive(IEnumerable<byte> data);
        byte[] FrameText(string text);
        byte[] FrameBinary(byte[] bytes);
        byte[] FramePing(byte[] bytes);
        byte[] FramePong(byte[] bytes);
        byte[] FrameClose(int code);

    }
}
