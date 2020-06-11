using static MobileDeliveryGeneral.Definitions.MsgTypes;

namespace MobileDeliveryGeneral.Data
{
    public class WalletData : WalletBaseData<WalletData>
    {
        public override eWalletCommand Command { get; set; } = eWalletCommand.GetBalance;
        
    }
}
