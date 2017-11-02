using System;
using System.Linq;

namespace EPWI.Components.Models
{
    public partial class OrderItem
    {
        public string ItemDescription
        {
            get
            {
                string description = string.Empty;

                if (ItemDetail.Any())
                {
                    description = ItemDetail.First().IDESC;
                }


                return description;
            }
        }

        public decimal DiscountedPrice => Price*(1 - DiscountPercent.GetValueOrDefault(0));

        public bool CanSelectShipMethod
        {
            get
            {
                char orderMethod = OrderMethod.GetValueOrDefault(' ');

                return ((orderMethod == 'B' || orderMethod == 'S' || orderMethod == 'C' || orderMethod == 'O') && CoreItem.GetValueOrDefault(' ') != 'C');
            }
        }

        public bool IsKit => LineCode == "KIT";

        // used to determine if item is displayed as a child item on order form
        public bool IsKitComponent
        {
            get
            {
                var kitComponent = KitComponent.GetValueOrDefault(' ');

                return ((kitComponent == 'K' && ParentItemID > 0) || kitComponent == 'C');
            }
        }

        public string KitYear
        {
            get { return getKitData(0); }
        }

        public string KitBoreSize
        {
            get { return getKitData(1); }
        }

        public string KitRodSize
        {
            get { return getKitData(2); }
        }

        public string KitMainBearingSize
        {
            get { return getKitData(3); }
        }

        public string KitThrustWasherSize
        {
            get { return getKitData(4); }
        }

        public static OrderItem CreateFromStockStatusRequest(StockStatusRequest request, string warehouse, int quantity, OrderMethod orderMethod, bool zeroPrice, int? parentItemID, string customerReference)
        {
            OrderItem item = new OrderItem()
            {
                ItemNumber = request.ItemNumber,
                LineCode = request.LineCode,
                NIPCCode = request.NIPCCode,
                Quantity = (short) quantity,
                SizeCode = request.SizeCode == "STD" ? string.Empty : request.SizeCode, // standard size is an empty string on the host
                Price = request.Price,
                ZeroPrice = zeroPrice,
                DiscountPercent = request.DiscountPercent,
                Warehouse = warehouse.ToUpper(),
                DateAdded = DateTime.Now,
                OrderMethod = orderMethod.ToCode()[0],
                InterchangeMethod = null, //TODO: determine if null character actually works here
                CoreItem = request.CorePrice > 0 ? 'Y' : (char?) null,
                KitComponent = request.LineCode == "KIT" ? 'K' : (char?) null,
                CustomerReference = customerReference
            };

            if (parentItemID.HasValue)
            {
                item.ParentItemID = parentItemID.Value;
            }

            return item;
        }

        public static OrderItem CreateCoreItem(StockStatusRequest request, string warehouse, int quantity, string orderMethod, int parentItemID)
        {
            var inventoryItemRep = new InventoryItemRepository();

            int coreNIPC = request.CoreNIPC;

            if (coreNIPC == 0)
            {
                coreNIPC = request.NIPCCode;
            }

            var inventoryItem = inventoryItemRep.GetInventoryItemByNipc(coreNIPC);

            OrderItem item = new OrderItem()
            {
                NIPCCode = inventoryItem.NIPCCode,
                ItemNumber = $"{inventoryItem.ItemNumber}-CORE",
                LineCode = inventoryItem.LineCode,
                Quantity = Convert.ToInt16(quantity),
                SizeCode = request.SizeCode == "STD" ? string.Empty : request.SizeCode,
                Price = request.CorePrice,
                Warehouse = warehouse,
                OrderMethod = orderMethod[0],
                InterchangeMethod = null,
                ParentItemID = parentItemID,
                DateAdded = DateTime.Now
            };

            if (request.LineCode == "KIT")
            {
                item.CoreItem = 'K';
            }
            else
            {
                item.CoreItem = 'C';
            }

            return item;
        }

        private string getKitData(int index)
        {
            if (IsKit)
            {
                var kitData = KitData.Split(',');
                if (kitData.Length > index)
                {
                    return kitData[index] ?? string.Empty;
                }
            }
            return string.Empty;
        }
    }
}