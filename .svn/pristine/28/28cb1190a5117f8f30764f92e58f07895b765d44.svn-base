using System;
using System.Collections.Generic;
using System.Linq;
using EPWI.Components.Models;
using System.Web.Mvc;

namespace EPWI.Web.Models
{
    public class OrderViewModel
    {

        public Order Order { get; set; }
        public ICustomerData CustomerData { get; set; }
        public bool OrderAccepted { get; set; }
        public bool OrderSubmitted { get; set; }
        public bool UserIsEmployee { get; set; }
        public long SavedKitConfigurationID { get; set; }
        public DateTime? QuoteCreateDate { get; set; }
        public IEnumerable<ShipMethodDetail> ShippingMethods { get; set; }

        public bool JustLoadedFromQuote
        {
            get
            {
                return QuoteCreateDate.HasValue;
            }
        }

        public bool OrderError
        {
            get
            {
                return OrderSubmitted && !OrderAccepted;
            }
        }

        public bool OrderErrorNotHandledByOrderWizard
        {
            get
            { // right now, only pricing errors are handled by the order wizard
                return Order.OrderItems.Any(oi => oi.ErrorFlag.HasValue && oi.ErrorFlag != ' ' && oi.ErrorFlag != 'P');
            }
        }

        public IEnumerable<SelectListItem> GetShippingMethods(bool forHeader, string currentShipCode, string fromWarehouse)
        {
            var list = new List<SelectListItem>();

            // For power user orders, always use ShippingMethods to populate the list
            if (Order.IsPowerUserOrder)
            {
                if (!forHeader && string.IsNullOrEmpty(currentShipCode))
                {
                    list.Add(getShipMethodItem(string.Empty, "---SELECT---", string.Empty));
                }

                // if for the header and no from warehouse is specified, use the order's primary warehouse
                if (forHeader && fromWarehouse == null)
                {
                    fromWarehouse = Order.PrimaryWarehouse;
                }
                foreach (var shipMethod in ShippingMethods.Where(sm => sm.From == fromWarehouse))
                {
                    var description = shipMethod.Description;
                    list.Add(getShipMethodItem(shipMethod.ShipMethod, description, currentShipCode));
                }
            }
            else if (forHeader)
            {
                list.Add(getShipMethodItem(Order.DefaultShipMethod, Order.DefaultShipMethodName, Order.DefaultShipMethod));
                // Display Will Call if it is not the user's default method
                if (Order.DefaultShipMethod != "WC")
                {
                    list.Add(getShipMethodItem("WW", "Will Call", currentShipCode));
                }
                list.Add(getShipMethodItem("WG", "Ground", currentShipCode));
                list.Add(getShipMethodItem("W3", "3-Day Ground", currentShipCode));
                list.Add(getShipMethodItem("W2", "2-Day Air", currentShipCode));
                list.Add(getShipMethodItem("W1", "Overnight Air", currentShipCode));
                list.Add(getShipMethodItem("WP", "Priority Overnight", currentShipCode));
                list.Add(getShipMethodItem("WO", "Other**", currentShipCode));
            }
            else
            { // item level
                if (string.IsNullOrEmpty(currentShipCode))
                {
                    list.Add(getShipMethodItem(string.Empty, "---SELECT---", string.Empty));
                }

                foreach (var shipMethod in ShippingMethods.Where(sm => sm.From == fromWarehouse || sm.From == string.Empty))
                {
                    var description = shipMethod.Description;
                    if (shipMethod.Charge > 0)
                    {
                        description = $"{shipMethod.Description} ({shipMethod.Charge:C2})";
                    }
                    list.Add(getShipMethodItem(shipMethod.ShipMethod, description, currentShipCode));
                }
            }

            return list;
        }

        public string ShipMethodCodeToName(string code)
        {
            string name;

            if (code == Order.DefaultShipMethod && !string.IsNullOrEmpty(Order.DefaultShipMethodName))
            {
                name = Order.DefaultShipMethodName;
            }
            else
            {
                switch (code)
                {
                    case "D":
                        name = "Deliver";
                        break;
                    case "WW":
                        name = "Will Call";
                        break;
                    case "WG":
                        name = "Ground";
                        break;
                    case "W3":
                        name = "3-Day Ground";
                        break;
                    case "W2":
                        name = "2-Day Air";
                        break;
                    case "W1":
                        name = "Overnight Air";
                        break;
                    case "WP":
                        name = "Priority Overnight";
                        break;
                    case "WD":
                        name = $"Pool Ship Ground to {Order.AssignedWhse}*";
                        break;
                    case "WN":
                        name = $"Pool Ship Next Day to {Order.AssignedWhse}*";
                        break;
                    case "WT":
                        name = $"Pool Ship Two Day to {Order.AssignedWhse}*";
                        break;
                    case "WO":
                        name = "Other***";
                        break;
                    default:
                        name = code;
                        break;
                }
            }

            return name;
        }

        public bool LoadedFromQuote
        {
            get
            {
                return Order.Quote != null;
            }
        }

        public bool UserOwnsQuote
        {
            get
            {
                return LoadedFromQuote && CustomerData.UserID == Order.Quote.UserID;
            }
        }

        public bool CanOverwriteQuote
        {
            get
            {
                return LoadedFromQuote && (UserOwnsQuote || !Order.Quote.ReadOnly || UserIsEmployee);
            }
        }

        private SelectListItem getShipMethodItem(string value, string text, string currentValue)
        {
            return new SelectListItem { Value = value, Text = text, Selected = (currentValue == value) };
        }
    }
}
