using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using EPWI.Components.Proxies;
using log4net;
using xVal.ServerSide;

namespace EPWI.Components.Models
{
    public class OrderRepository : Repository, IOrderRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrderRepository));


        public Order OpenOrder(ICustomerData customerData, bool createIfNeeded)
        {
            return OpenOrder(customerData, createIfNeeded, true);
        }

        public Order OpenOrder(ICustomerData customerData, bool createIfNeeded, bool retryOnDuplicateKey)
        {
            var order = (from o in Db.Orders
                where o.UserID == customerData.UserID
                select o).SingleOrDefault();

            if ((order == null) && createIfNeeded)
                try
                {
                    order = GetOrderFromHost(customerData);
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("UX_Orders_UserID") && retryOnDuplicateKey)
                    {
                        log.Warn("Attempt to insert a duplicate order for a user", ex);
                        order = OpenOrder(customerData, createIfNeeded, false);
                    }
                    else
                    {
                        throw ex;
                    }
                }

            return order;
        }

        public int AddKitToOrder(Kit kit, ICustomerData customerData, string kitXml, string customerReference,
            string powerUserPrimaryWarehouse)
        {
            var order = OpenOrder(customerData, true);
            var orderShippingWarehouse = powerUserPrimaryWarehouse ?? order.AssignedWhse;

            kit.OrderMethod = kit.OrderMethod ?? string.Empty;

            // set to order from main warehouse (M) for the base kit if no other method has been provided
            if (string.IsNullOrEmpty(kit.OrderMethod.Trim()))
                kit.OrderMethod = "M";

            // add the kit as line item to the order;

            var kitItem = new OrderItem
            {
                OrderID = order.OrderID,
                ItemNumber = kit.KitIdentifier.KitPartNumber,
                LineCode = "KIT",
                NIPCCode = kit.NIPCCode,
                Quantity = 1,
                Warehouse = orderShippingWarehouse,
                OrderMethod = kit.OrderMethod[0],
                // for non-ktracks, add the year, bore, rods, main, and thrust washer sizes as a CSV value to the KitData field
                KitData = kit.IsKTRACK
                    ? null
                    : $"{kit.SelectedYear},{getSizeString(kit.SelectedBoreSize)},{getSizeString(kit.SelectedRodBearingSize)},{getSizeString(kit.SelectedMainBearingSize)},{getSizeString(kit.SelectedThrustWasherSize)}",
                Price = kit.ConfiguredPrice,
                KitComponent = 'K',
                KitXml = kitXml,
                CustomerReference = customerReference,
                DateAdded = DateTime.Now
            };
            order.OrderItems.Add(kitItem);
            Save();

            foreach (var part in kit.MasterKitParts)
            {
                var item = new OrderItem
                {
                    OrderID = order.OrderID,
                    ItemNumber = part.ItemNumber,
                    LineCode = part.LineCode,
                    NIPCCode = part.NIPCCode,
                    Quantity = part.Selected ? (short) part.QuantityRequired : (short) 0,
                    SizeCode = part.SizeCode,
                    OrderMethod = part.OrderMethod[0],
                    InterchangeMethod = part.InterchangeMethod[0],
                    Warehouse = string.IsNullOrEmpty(part.ShipWarehouse) ? orderShippingWarehouse : part.ShipWarehouse,
                    KitComponent = 'C',
                    ParentItemID = kitItem.OrderItemID,
                    Price = part.Price,
                    PricingPercentage = part.PricingPercentage,
                    ZeroPrice = true,
                    DateAdded = DateTime.Now
                };
                order.OrderItems.Add(item);
            }

            // if a crank kit was selected, then add it and the associated core to the order
            if (kit.SelectedCrankKitNIPC > 0)
            {
                var inventoryItemRepository = new InventoryItemRepository();
                var crankKitInventoryItem = inventoryItemRepository.GetInventoryItemByNipc(kit.SelectedCrankKitNIPC);

                var crankKitItem = new OrderItem();
                crankKitItem.OrderID = order.OrderID;
                crankKitItem.ItemNumber = crankKitInventoryItem.ItemNumber;
                crankKitItem.LineCode = crankKitInventoryItem.LineCode;
                crankKitItem.NIPCCode = crankKitInventoryItem.NIPCCode;
                crankKitItem.Quantity = 1;
                crankKitItem.Warehouse = orderShippingWarehouse;
                crankKitItem.OrderMethod = kit.OrderMethod[0];
                crankKitItem.Price = kit.SelectedCrankKitPrice;
                crankKitItem.DateAdded = DateTime.Now;
                if (crankKitInventoryItem.IsKit)
                {
                    crankKitItem.KitComponent = 'K';
                    crankKitItem.CustomerReference = customerReference;
                }
                else
                {
                    crankKitItem.KitComponent = 'C';
                    crankKitItem.ParentItemID = kitItem.OrderItemID;

                    // Per FB #220, if the crank kit is not a "kit" then the crank kit price needs to be added to the price of the whole kit and not displayed
                    // seperately because it causes problems on the AS/400
                    kitItem.Price += crankKitItem.Price;
                    Save();

                    crankKitItem.ZeroPrice = true;
                }

                if (kit.SelectedCrankKitCorePrice > 0)
                    crankKitItem.CoreItem = 'Y';

                crankKitItem.KitData = "CRANK";
                order.OrderItems.Add(crankKitItem);
                Save();

                // add the core if required
                if (kit.SelectedCrankKitCorePrice > 0)
                {
                    var coreInventoryItem = inventoryItemRepository.GetInventoryItemByNipc(kit.SelectedCrankKitCoreNIPC);
                    var coreItem = new OrderItem();
                    coreItem.OrderID = order.OrderID;
                    coreItem.NIPCCode = coreInventoryItem.NIPCCode;
                    coreItem.ItemNumber = $"{coreInventoryItem.ItemNumber}-CORE";
                    coreItem.LineCode = coreInventoryItem.LineCode;
                    coreItem.Quantity = 1;
                    coreItem.Price = kit.SelectedCrankKitCorePrice;
                    coreItem.Warehouse = orderShippingWarehouse;
                    coreItem.OrderMethod = kit.OrderMethod[0];
                    coreItem.DateAdded = DateTime.Now;
                    if (crankKitInventoryItem.IsKit)
                    {
                        coreItem.ParentItemID = crankKitItem.OrderItemID;
                    }
                    else
                    {
                        coreItem.ParentItemID = kitItem.OrderItemID;
                        coreItem.KitComponent = 'C';
                    }
                    coreItem.CoreItem = 'K';
                    // K = it is a core item, but with a kit (either crank kit or a normal kit). These need to be handled differently than normal cores. See FB #198
                    order.OrderItems.Add(coreItem);
                }
            }
            return kitItem.OrderItemID;
        }

        public int AddWarrantyToKitOrderItem(int kitOrderItemID, int warrantyNipc, decimal warrantyPrice)
        {
            return Db.usp_AddKitWarranty(kitOrderItemID, warrantyNipc, warrantyPrice);
        }

        public Order GetOrderFromHost(ICustomerData customerData)
        {
            var order = new Order
            {
                UserID = customerData.UserID,
                EPWCustID = customerData.CustomerID.Value,
                EPWCompCode = customerData.CompanyCode.Value,
                UserName = customerData.UserName,
                CreateDate = DateTime.Now
            };
            order = UpdateOrderFromHost(order, customerData, true);
            Db.Orders.InsertOnSubmit(order);
            Db.SubmitChanges();

            return order;
        }

        public Order UpdateOrderFromHost(Order order, ICustomerData customerData, bool flushData)
        {
            var proxy = OrderHeaderProxy.Instance;
            var dataTable = proxy.SubmitRequest(customerData.CompanyCode, customerData.CustomerID, flushData);
            order.PopulateFromHost(dataTable.Rows[0], customerData);

            return order;
        }

        public Order GetOrderFromDatabase(int orderId)
        {
            var order = (from o in Db.Orders where o.OrderID == orderId select o).SingleOrDefault();

            return order;
        }

        public void DeleteOrder(Order order)
        {
            Db.Orders.DeleteOnSubmit(order);
        }

        public void DeleteOrderItem(OrderItem orderItem)
        {
            var childItems = from ci in Db.OrderItems
                where ci.ParentItemID == orderItem.OrderItemID
                select ci;
            Db.OrderItems.DeleteAllOnSubmit(childItems);
            Db.OrderItems.DeleteOnSubmit(orderItem);
        }

        public bool ProcessOrder(Order order, bool saveAsQuote, bool perpetualQuote, string quoteNumber,
            ICustomerData customerData)
        {
            if (quoteNumber == string.Empty)
                quoteNumber = null;

            var errors = getProcessOrderErrors(order);

            if (errors.Count() > 0)
                throw new RulesException(errors);

            order.SetManualFlags();
            Db.SubmitChanges();

            var orderHeaderUpdateProxy = OrderHeaderUpdateProxy.Instance;
            var headerUpdateSuccess = orderHeaderUpdateProxy.SubmitRequest(order, saveAsQuote);

            if (!headerUpdateSuccess)
            {
                order = UpdateOrderFromHost(order, customerData, false);
                Db.SubmitChanges();
                return false;
            }

            // reset the ErrorFlag for the order's order items so that 
            // it is not being marked falsely and so that the host can mark the field
            foreach (var item in order.OrderItems)
                item.ErrorFlag = null;
            Db.SubmitChanges();

            var orderItemUpdateProxy = OrderItemUpdateProxy.Instance;

            var orderItemUpdateSuccess = orderItemUpdateProxy.SubmitRequest(order, saveAsQuote, perpetualQuote,
                quoteNumber);

            if (!orderItemUpdateSuccess && !saveAsQuote)
            {
                // requery the host to get updated error codes for header and items
                // don't flush the item data so that any error codes are available
                order = UpdateOrderFromHost(order, customerData, false);
                updateOrderItemErrorFlagsFromHost(order);
                // since there was a problem with an order item, run the order wizard
                runOrderWizard(order, customerData);
                Db.SubmitChanges();

                return false;
            }

            // requery the host to get updated error codes for the header as well as the invoice number
            // don't update the item data here because it might show errors, even though the order was processed successfully
            order = UpdateOrderFromHost(order, customerData, false);
            Db.SubmitChanges();

            return true;
        }

        public int SaveOrderAsQuote(ICustomerData customerData, Order order, string quoteDescription, bool shareQuote,
            bool readOnly)
        {
            //using (var transaction = new TransactionScope())
            //{
            var quoteRep = new QuoteRepository();

            //automatically overwrite existing quote if it has the same description
            var existingQuote =
                quoteRep.GetQuotesByUser(customerData)
                    .Where(q => q.QuoteDescription == quoteDescription)
                    .SingleOrDefault();

            if (existingQuote != null)
            {
                quoteRep.DeleteQuote(existingQuote.QuoteID, customerData);
                quoteRep.Save();
            }

            Db.SubmitChanges();

            return Db.SaveOrderAsQuote(order.OrderID, quoteDescription, shareQuote, readOnly);
            //}
        }

        private void runOrderWizard(Order order, ICustomerData customerData)
        {
            // if there is a pricing error flag, perform a stock status to get the correct price and update the order item with it
            var pricingErrorItems = from oi in order.OrderItems
                where oi.ErrorFlag == 'P'
                select oi;

            foreach (var item in pricingErrorItems)
            {
                var stockStatus = StockStatusRepository.GetStockStatusByNipc(item.NIPCCode, item.SizeCode, item.Quantity,
                    customerData);
                log.InfoFormat(
                    "Order Wizard is adjusting price for NIPC: {0}, User: {1}, Original Price: {2}, New Price: {3}",
                    item.NIPCCode, customerData.UserName, item.Price, stockStatus.Price[PriceType.P3]);
                item.Price = stockStatus.Price[PriceType.P3];
            }
        }

        private IEnumerable<ErrorInfo> getProcessOrderErrors(Order order)
        {
            var errors = new List<ErrorInfo>();
            if ((order.PORequired.GetValueOrDefault(' ') == 'Y') &&
                (string.IsNullOrEmpty(order.PONumber) || (order.PONumber.Trim().Length <= 1)))
                errors.Add(new ErrorInfo("PONumber", "A PO Number of greater than 1 character is required."));

            if (order.OrderedOrderItems.Any(oi => string.IsNullOrEmpty(oi.ShipMethod) && oi.CanSelectShipMethod))
                errors.Add(new ErrorInfo(null,
                    "You need to select a ship method for items that do not originate from your primary warehouse"));

            return errors;
        }

        private void updateOrderItemErrorFlagsFromHost(Order order)
        {
            var proxy = OrderItemProxy.Instance;

            var hostOrderItems = proxy.SubmitRequest(order.OrderID);

            foreach (DataRow hostOrderItem in hostOrderItems.Rows)
            {
                var webOrderItem = (from oi in order.OrderItems
                    where oi.OrderItemID.ToString() == hostOrderItem["ZOI#"].ToString()
                    select oi).SingleOrDefault();

                if (webOrderItem == null)
                    throw new ApplicationException(
                        "Order Item from host does not contain a matching Order Item in the local database.");

                if (hostOrderItem["ZOEMF"].ToString() != " ")
                    webOrderItem.ErrorFlag = hostOrderItem["ZOEMF"].ToString()[0];
            }
        }

        private string getSizeString(string sizeCode)
        {
            return string.IsNullOrEmpty(sizeCode == null ? sizeCode : sizeCode.Trim()) ? "STD" : sizeCode;
        }
    }
}