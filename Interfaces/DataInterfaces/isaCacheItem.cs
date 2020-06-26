using System;

namespace MobileDeliveryGeneral.DataManager.Interfaces
{
    public interface isaCacheItem<O> :  IComparable<O>
    {
        int Id { get; set; }
    }
}
