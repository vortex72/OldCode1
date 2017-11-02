using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EPWI.Components.Proxies;

namespace EPWI.Components.Models
{
    public class StockStatus
    {
        private string[] orderCodes = new string[5];
        private Dictionary<OrderMethod, bool> orderMethodAvailability = new Dictionary<OrderMethod, bool>();
        private Dictionary<PriceType, decimal> price = new Dictionary<PriceType, decimal>();
        private Dictionary<string, int> warehouseAvailability = new Dictionary<string, int>();

        public StockStatus()
        {
        }

        /// <summary>
        /// Constructor for unit testing of order method availabilities
        /// </summary>		
        public StockStatus(string defaultWarehouse, string secondaryWarehouse, Dictionary<string, int> warehouseAvailability, int requestedQuantity, string[] orderCodes)
        {
            CustomerDefaultWarehouse = defaultWarehouse;
            CustomerSecondaryWarehouse = secondaryWarehouse;
            this.warehouseAvailability = warehouseAvailability;
            QuantityRequested = requestedQuantity;
            this.orderCodes = orderCodes;
            setOrderMethodAvailabilities();
        }

        public int QuantityRequested { get; set; }
        public int ApplicableKitNipc { get; set; }
        public int NIPCCode { get; set; }
        public string CustomerDefaultWarehouse { get; set; }
        public string CustomerSecondaryWarehouse { get; set; }
        public bool IsObsolete { get; set; }
        public bool IsSuperseded { get; set; }
        public string SupersededPartNumber { get; set; }
        public string SupersededPartLine { get; set; }
        public decimal DiscountPercent { get; set; }
        public int CoreNIPC { get; set; }
        public decimal CorePrice { get; set; }
        public int Cylinders { get; set; }
        public bool IsCrankKit { get; set; }
        public string DenyPurchaseFlag { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public string Upc { get; set; }

        public bool DenyPurchase
        {
            get { return DenyPurchaseFlag == "001"; }
        }

        public string[] OrderCodes
        {
            get { return orderCodes; }
        }

        public string AlternateWarehouseB { get; set; }
        public string AlternateWarehouseF { get; set; }

        public Dictionary<string, int> WarehouseAvailabilityDictionary
        {
            get { return warehouseAvailability; }
        }

        public Dictionary<PriceType, decimal> Price
        {
            get { return price; }
        }

        public bool IsQuantityAvailableCombined
        {
            get { return (warehouseAvailability.Select(wa => wa.Value).Sum() >= QuantityRequested); }
        }

        public bool IsQuantityAvailableAnywhere
        {
            get { return (warehouseAvailability.Any(wa => wa.Value >= QuantityRequested)); }
        }

        public int WarehouseAvailability(string warehouse)
        {
            int availability = 0;

            if (warehouseAvailability.ContainsKey(warehouse))
            {
                availability = warehouseAvailability[warehouse];
            }

            return availability;
        }

        public bool OrderMethodAvailability(OrderMethod orderMethod)
        {
            bool availability = false;

            if (orderMethodAvailability.ContainsKey(orderMethod))
            {
                availability = orderMethodAvailability[orderMethod];
            }

            return availability;
        }

        public void PopulateFromHost(DataTable stockStatusTable, int nipcCode, int quantityRequested, ICustomerData customerData)
        {
            PopulateFromHost(stockStatusTable, nipcCode, 0, quantityRequested, customerData, null);
        }

        /// <summary>
        /// Returns a hydrated StockStatus object based on a dataset from the AS/400
        /// </summary>
        /// <param name="dsStockStatus">The DataSet returned by the AS/400</param>
        /// <returns>A hydrated StockStatus object</returns>
        public void PopulateFromHost(DataTable stockStatusTable, int nipcCode, int applicableKitNipc, int quantityRequested, ICustomerData customerData, PowerUserWarehouseResult powerUserWarehouses)
        {
            if (stockStatusTable.Rows.Count == 0)
            {
                throw new ApplicationException("No records returned from Stock Status Inquiry. Possible invalid Customer ID/Company Code");
            }
            DataRow parentRow = stockStatusTable.Rows[0];

            NIPCCode = nipcCode;
            QuantityRequested = quantityRequested;
            ApplicableKitNipc = applicableKitNipc;
            CustomerDefaultWarehouse = powerUserWarehouses != null ? powerUserWarehouses.PrimaryWarehouse : parentRow["SSAWH"].ToString();
            CustomerSecondaryWarehouse = powerUserWarehouses != null ? powerUserWarehouses.SecondaryWarehouse : parentRow["SSAWH2"].ToString();
            Price[PriceType.Jobber] = decimal.Parse(parentRow["SSP1"].ToString());
            Price[PriceType.Invoice] = decimal.Parse(parentRow["SSP3N"].ToString());
            Price[PriceType.Elite] = Price[PriceType.Invoice]*0.9M; //TODO: Should this be configurable?
            Price[PriceType.P3] = decimal.Parse(parentRow["SSP3"].ToString());
            Price[PriceType.Market] = decimal.Parse(parentRow["SSPMKT"].ToString());

            //Adjust prices based on customer pricing basis
            Price[PriceType.Customer] = Price[customerData.CustomerPricingBasis]*customerData.PricingFactor;

            //Set Margins based on customer margin pricing basis
            if (Price[customerData.MarginPricingBasis] > 0)
            {
                Price[PriceType.Margin] = (Price[PriceType.Market] - Price[customerData.MarginPricingBasis])/
                                          Price[customerData.MarginPricingBasis];
            }
            else
            {
                Price[PriceType.Margin] = 0;
            }

            if (ApplicableKitNipc > 0)
            {
                // get pricing for the part when in the specified kit
                // create a dummy kit with just the current part 
                Kit kit = new Kit {NIPCCode = ApplicableKitNipc};
                var kitParts = new List<KitPart>();
                kitParts.Add(new KitPart {NIPCCode = NIPCCode});
                kit.MasterKitParts = kitParts;

                KitPricingRequestProxy proxy = KitPricingRequestProxy.Instance;
                kit = proxy.SubmitRequest(kit, customerData);

                Price[PriceType.KitPrice] = kit.MasterKitParts.Single().Price;
            }
            else
            {
                Price[PriceType.KitPrice] = 0;
            }

            IsObsolete = (parentRow["SSSO"].ToString() == "O");
            IsSuperseded = (parentRow["SSSO"].ToString() == "S");

            if (IsSuperseded)
            {
                SupersededPartNumber = parentRow["SSSIT"].ToString().Trim();
                SupersededPartLine = parentRow["SSSLI"].ToString().Trim();
            }

            Cylinders = int.Parse(parentRow["SS#CYL"].ToString());
            DiscountPercent = decimal.Parse(parentRow["SSDI"].ToString());
            IsCrankKit = (parentRow["SSCRKT"].ToString() == "Y");
            CorePrice = decimal.Parse(parentRow["SSPCR"].ToString());

            // if the SSCRNI field has a value, then the part is a crank kit and the core
            // has a different NI code which is located in this field. If it does not have
            // a value, but there is still a core price, then the core is the same as the
            // part being searched

            if (int.Parse(parentRow["SSCRNI"].ToString()) != 0)
            {
                CoreNIPC = int.Parse(parentRow["SSCRNI"].ToString());
            }
            else
            {
                if (CorePrice > 0)
                {
                    CoreNIPC = NIPCCode;
                }
            }

            DenyPurchaseFlag = parentRow["SSLLMT"].ToString().Trim();

            for (int i = 0; i < 4; i++)
            {
                orderCodes[i] = parentRow[$"OS{i + 1}"].ToString().Trim();
            }

            AlternateWarehouseB = parentRow["OSBW"].ToString().Trim();
            AlternateWarehouseF = parentRow["OSFW"].ToString().Trim();

            ManufacturerPartNumber = parentRow["SSMFGITM"].ToString().Trim();
            Upc = parentRow["SSUPC"].ToString().Trim();

            processInventory(stockStatusTable);

            // Add Units Per Sell Multiple
            var rep = new InventoryItemRepository();
            var result = rep.GetInventoryItemByNipc(nipcCode);
            //this.UnitsPerSellMultiple = result.UnitsPerSellMultiple;

            setOrderMethodAvailabilities();
        }

        private void processInventory(DataTable stockStatusTable)
        {
            foreach (DataRow row in stockStatusTable.Rows)
            {
                warehouseAvailability[row["SSWH"].ToString()] = int.Parse(row["SSAV"].ToString());
            }
        }

        private void setOrderMethodAvailabilities()
        {
            // If the requested quantity is available in the user's default warehouse
            // then enable the order method of "M" and do not enable any other methods
            if (WarehouseAvailability(CustomerDefaultWarehouse) >= QuantityRequested)
            {
                orderMethodAvailability[OrderMethod.MainWarehouse] = true;
                return;
            }

            // As long as quantity is not available in the user's default warehouse,
            // then all other alternatives will include the option to request a factory drop ship
            // and, if the AS/400 indicates so, an option for local pickup
            orderMethodAvailability[OrderMethod.DropShip] = true;

            // if any of the order status codes is "L" then enable order method of "L"
            orderMethodAvailability[OrderMethod.LocalPickup] = orderCodes.Any(o => o == "L");

            // Now, start checking alternate order methods
            // If there is 1 or more of the requested items in the user's default warehouse
            // and the remainder can be made up from the user's secondary warehouse,
            // then enable the order method of "B"
            orderMethodAvailability[OrderMethod.MainAndSecondaryWarehouse] = WarehouseAvailability(CustomerDefaultWarehouse) > 0 && (WarehouseAvailability(CustomerDefaultWarehouse) + WarehouseAvailability(CustomerSecondaryWarehouse)) >= QuantityRequested;

            // If there is sufficient quantity in the user's secondary warehouse,
            // then enable the order method of "S"
            orderMethodAvailability[OrderMethod.SecondaryWarehouse] = WarehouseAvailability(CustomerSecondaryWarehouse) >= QuantityRequested;

            // At this point, if both the "B" and "S" methods are true, then quit checking for any more alternatives
            if (OrderMethodAvailability(OrderMethod.MainAndSecondaryWarehouse) && OrderMethodAvailability(OrderMethod.SecondaryWarehouse))
            {
                return;
            }

            // If the full quantity is available anywhere other than the customer's main or secondary warehouse, then enable the "O" method
            orderMethodAvailability[OrderMethod.OtherWarehouse] = warehouseAvailability.Any(wa => wa.Value >= QuantityRequested && wa.Key != CustomerDefaultWarehouse && wa.Key != CustomerSecondaryWarehouse);

            // If there is quantity when combined with any other warehouse, excluding the user's secondary warehouse,
            // then enable order method "C"
            if (!OrderMethodAvailability(OrderMethod.MainAndSecondaryWarehouse) && WarehouseAvailability(CustomerDefaultWarehouse) > 0)
            {
                orderMethodAvailability[OrderMethod.MainAndOtherWarehouse] = warehouseAvailability.Where(wa => wa.Key != CustomerDefaultWarehouse).Any(wa => wa.Value + WarehouseAvailability(CustomerDefaultWarehouse) >= QuantityRequested);
            }

            // If order method "B", "S", "O", or "C" is available, then exit so that the manual option is not displayed
            if (OrderMethodAvailability(OrderMethod.MainAndSecondaryWarehouse) || OrderMethodAvailability(OrderMethod.SecondaryWarehouse) || OrderMethodAvailability(OrderMethod.OtherWarehouse) || OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse))
            {
                return;
            }

            // If we have gotten to this point, then we need to enable the option to allow for manual processing
            // This is because quantity either does not exist anywhere or that quantity only exists
            // when combined with multiple other (not the default or secondary) warehouse
            orderMethodAvailability[OrderMethod.Manual] = true;
        }
    }
}