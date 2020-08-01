namespace MobileDeliveryGeneral.Interfaces
{
    public interface isaCommandParser<C>
    {
        C ProcessMessage(byte[] message);
    }
}
