using System;

namespace EPWI.Components.Models
{
    [Flags]
    public enum PriceType
    {
        Undefined = 0,
        Customer = 1,       // AKA Retail
        Jobber = 2,
        Invoice = 4,
        Elite = 8,
        P3 = 64,
        Market = 16,
        Margin = 32,
        KitPrice = 128      // The price of a part if in the given kit (identified by ApplicableKitNIPC)
    }
}
