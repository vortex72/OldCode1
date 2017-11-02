using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using EPWI.Components.Models;
using EPWI.Components.Proxies;
using EPWI.Components.Services;
using EPWI.Web.Exception;
using EPWI.Web.Models;
using log4net;
using xVal.ServerSide;

namespace EPWI.Web.Controllers
{
    [Authorize]
    public class OrderController : LoggingController
    {
        private static readonly ILog log = LogManager.GetLogger("OrderController");

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index()
        {
            var orderRepository = new OrderRepository();
            var kitRepository = new KitRepository();
            var quoteCreateDate = (DateTime?) TempData["QuoteCreateDate"];
            var order = orderRepository.OpenOrder(CustomerData, true);
            var model = new OrderViewModel
            {
                CustomerData = CustomerData,
                Order = order,
                SavedKitConfigurationID = kitRepository.GetSavedConfigurationID(CustomerData.UserID),
                QuoteCreateDate = quoteCreateDate,
                UserIsEmployee = CurrentUserIsEmployee,
                ShippingMethods = GetOrderShipMethods(order)
            };

            return View("OrderForm", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddItem(StockStatusRequest request, bool? localPickup, OrderMethod orderMethod1,
            string warehouse1, int quantity1, OrderMethod? orderMethod2, string warehouse2, int? quantity2,
            string customerReference, SessionStore sessionStore)
        {
            if (request.ItemNumber == null)
            {
                log.WarnFormat(
                    "Attempted to add an item to order from an empty stock status request. Session probably timed out. User name: {0}",
                    User.Identity.Name);
                TempData["message"] =
                    "Sorry, your Stock Status Request has timed out. Please repeat the search and add the item to your order again.";
                return RedirectToAction("Search", "StockStatus");
            }
            int? parentItemID = null;
            var zeroPrice = false;

            var orderRepository = new OrderRepository();
            var order = orderRepository.OpenOrder(CustomerData, true);
            OrderItem newItem;

            // if a local pickup was requested, then add an additional entry before the normal warehouse requests
            // indicating a method of "L" and with the total requested quantity, a warehouse code of "LPU", and set
            // zeroPrice to try so that the remainder of the items added will have their price zeroed out when displayed, but they
            // will still have their price tracked so that it can be passed to the 400
            if (localPickup.GetValueOrDefault(false))
            {
                var localPickupItem = OrderItem.CreateFromStockStatusRequest(request, "XXX",
                    quantity1 + quantity2.GetValueOrDefault(0), OrderMethod.LocalPickup, false, parentItemID,
                    customerReference);
                order.OrderItems.Add(localPickupItem);
                orderRepository.Save();
                zeroPrice = true;
                parentItemID = localPickupItem.OrderItemID;
            }

            newItem = OrderItem.CreateFromStockStatusRequest(request, warehouse1, quantity1, orderMethod1, zeroPrice,
                parentItemID, customerReference);
            order.OrderItems.Add(newItem);
            orderRepository.Save();
            if (request.CorePrice > 0)
                order.OrderItems.Add(OrderItem.CreateCoreItem(request, warehouse1, quantity1, orderMethod1.ToCode(),
                    newItem.OrderItemID));

            //add request for other warehouse if it exists
            if (orderMethod2.HasValue && quantity2.HasValue)
            {
                newItem = OrderItem.CreateFromStockStatusRequest(request, warehouse2, quantity2.Value,
                    orderMethod2.Value, zeroPrice, parentItemID, customerReference);
                order.OrderItems.Add(newItem);
                orderRepository.Save();
                if (request.CorePrice > 0)
                    order.OrderItems.Add(OrderItem.CreateCoreItem(request, warehouse2, quantity2.Value,
                        orderMethod2.Value.ToCode(), newItem.OrderItemID));
            }

            orderRepository.Save();

            // reset the stock status request
            request = new StockStatusRequest();

            TempData["unencodedmessage"] =
                $"Item added to {HtmlHelper.GenerateLink(ControllerContext.RequestContext, RouteTable.Routes, "order", string.Empty, "Index", "Order", null, null)}.";

            return sessionStore.ReturnToLookup
                ? RedirectToAction("Index", "Lookup")
                : RedirectToAction("Search", "StockStatus");
        }

        public ActionResult OrderWidget()
        {
            try
            {
                var rep = new OrderRepository();

                var order = rep.OpenOrder(CustomerData, false);

                return PartialView("OrderWidget", order);
            }
            catch (System.Exception ex)
            {
                if (ex is UserInvalidException)
                    throw;

                log.Error("Error in OrderWidget", ex);
                ViewData["error"] = true;
                return PartialView("OrderWidget");
            }
        }

        public ActionResult OrderPowerUserStatus()
        {
            var rep = new OrderRepository();

            var order = rep.OpenOrder(CustomerData, false);

            return PartialView(order);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetPurchaseOrderNumber(string poNumber)
        {
            var orderRepository = new OrderRepository();
            var order = orderRepository.OpenOrder(CustomerData, false);

            if (order == null)
                throw new ApplicationException("Order does not exist. It may have been deleted.");

            order.PONumber = poNumber;
            orderRepository.Save();

            return new EmptyResult();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetOrderShipMethod(string orderShipMethod)
        {
            var orderRepository = new OrderRepository();
            var order = orderRepository.OpenOrder(CustomerData, false);

            if (order == null)
                throw new ApplicationException("Order does not exist. It may have been deleted.");

            order.RequestedShipMethod = orderShipMethod;
            orderRepository.Save();

            return new EmptyResult();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetItemShipMethod(int id, string itemShipMethod)
        {
            var orderRepository = new OrderRepository();
            var order = orderRepository.OpenOrder(CustomerData, false);

            if (order == null)
                throw new ApplicationException("Order does not exist. It may have been deleted.");

            var orderItem = order.OrderItems.SingleOrDefault(oi => oi.OrderItemID == id);

            if (orderItem == null)
                throw new ApplicationException("Order item does not exist. It may have been deleted.");

            orderItem.ShipMethod = itemShipMethod;
            orderRepository.Save();

            return new EmptyResult();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteItem(int id)
        {
            var orderRepository = new OrderRepository();
            var order = orderRepository.OpenOrder(CustomerData, false);

            if (order == null)
                throw new ApplicationException("Order does not exist. It may have been deleted.");

            var orderItem = order.OrderItems.SingleOrDefault(oi => oi.OrderItemID == id);

            if (orderItem != null)
            {
                orderRepository.DeleteOrderItem(orderItem);
                orderRepository.Save();
            }
            //return View("OrderForm", model);

            return PartialView("OrderDetails",
                new OrderViewModel
                {
                    CustomerData = CustomerData,
                    Order = order,
                    ShippingMethods = GetOrderShipMethods(order)
                });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeItem(int id)
        {
            var orderRepository = new OrderRepository();
            var order = orderRepository.OpenOrder(CustomerData, false);

            if (order == null)
                throw new ApplicationException("Order does not exist. It may have been deleted.");

            var orderItem = order.OrderItems.SingleOrDefault(oi => oi.OrderItemID == id);

            if (orderItem == null)
            {
                TempData["Message"] = "Order Item not found.";
                return RedirectToAction("Index");
            }
            orderRepository.DeleteOrderItem(orderItem);
            orderRepository.Save();

            if (orderItem.IsKit)
            {
                var kitRep = new KitRepository();
                kitRep.LoadKitFromXml(orderItem.KitXml, CustomerData);
                kitRep.Save();
                return RedirectToAction("Edit", "Kit");
            }

            return RedirectToAction("Search", "StockStatus",
                new
                {
                    RequestedItemNumber = orderItem.ItemNumber,
                    RequestedLineCode = orderItem.LineCode,
                    RequestedQuantity = orderItem.Quantity
                });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveNotes(string orderNotes)
        {
            var orderRepository = new OrderRepository();
            var order = orderRepository.OpenOrder(CustomerData, false);

            if (order == null)
                throw new ApplicationException("Order does not exist. It may have been deleted.");

            order.OrderNotes = orderNotes;

            orderRepository.Save();

            return new EmptyResult();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateCustomerReference(int id, string customerReference)
        {
            var orderRepository = new OrderRepository();
            var order = orderRepository.OpenOrder(CustomerData, false);

            if (order == null)
                throw new ApplicationException("Order does not exist. It may have been deleted.");

            var orderItem = order.OrderItems.SingleOrDefault(oi => oi.OrderItemID == id);

            if (orderItem == null)
                throw new ApplicationException("Order item does not exist. It may have been deleted.");

            orderItem.CustomerReference = customerReference;
            orderRepository.Save();

            return PartialView("EditCustomerReference", orderItem);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Process(string hiddenPO, string hiddenNotes)
        {
            var success = false;
            var orderSubmitted = false;
            var orderRepository = new OrderRepository();
            var order = orderRepository.OpenOrder(CustomerData, false);

            order.SendEmail = !CurrentUserIsEmployee;

            var systemStatusProxy = SystemStatusProxy.Instance;

            if (!systemStatusProxy.SubmitRequest())
                TempData["message"] =
                    "The system is currently unable to accept your order due to maintenance. Please try again in 15 minutes.";
            else
                try
                {
                    order.PONumber = hiddenPO;
                    order.OrderNotes = hiddenNotes;
                    success = orderRepository.ProcessOrder(order, false, false, null, CustomerData);
                    orderSubmitted = true;
                }
                catch (RulesException ex)
                {
                    ex.AddModelStateErrors(ModelState, null);
                }

            var model = new OrderViewModel
            {
                CustomerData = CustomerData,
                Order = order,
                OrderSubmitted = orderSubmitted,
                OrderAccepted = success,
                UserIsEmployee = CurrentUserIsEmployee
            };

            ViewData["MillionthPartEnabled"] = (ConfigurationManager.AppSettings["MillionthPartEnabled"] != null) &&
                                               bool.Parse(ConfigurationManager.AppSettings["MillionthPartEnabled"]);

            // delete the order if it has been processed successfully
            if (success)
            {
                orderRepository.DeleteOrder(order);
                orderRepository.Save();
            }
            else
            {
                // we need to add shipping methods to the model for re-display
                model.ShippingMethods = GetOrderShipMethods(order);
            }

            return View("OrderForm", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete()
        {
            var orderRepository = new OrderRepository();
            var order = orderRepository.OpenOrder(CustomerData, false);

            if (order != null)
            {
                orderRepository.DeleteOrder(order);
                orderRepository.Save();
            }

            return RedirectToAction("Index", "StockStatus");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateShipToAddress(Address address, bool useSoldToAddress)
        {
            var orderRepository = new OrderRepository();
            var order = orderRepository.OpenOrder(CustomerData, false);

            if (order == null)
                throw new ApplicationException("Order does not exist. It may have been deleted.");

            if (useSoldToAddress)
                address = new Address
                {
                    // couldn't just null out the address here because the webservice chokes if it doesn't receive these specific values
                    Name = string.Empty,
                    StreetAddress1 = string.Empty,
                    StreetAddress2 = null,
                    City = string.Empty,
                    State = string.Empty,
                    Zip = "0",
                    Zip4 = "0",
                    Phone = "0"
                };

            order.ShipToAddress = address;
            orderRepository.Save();

            return View("Address", order.HasShipToAddress ? order.ShipToAddress : order.SoldToAddress);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "POWER_USER")]
        public ActionResult OpenPowerUserOrder()
        {
            var kitRepository = new KitRepository();

            ViewData["KitInProgress"] = kitRepository.KitExistsForUser(CustomerData);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "POWER_USER")]
        public ActionResult OpenPowerUserOrder(Address address)
        {
            var kitRepository = new KitRepository();

            kitRepository.DeleteConfiguredKit(CustomerData);
            kitRepository.Save();

            var powerUserService = new PowerUserService();

            var order = powerUserService.OpenPowerUserOrder(address, CustomerData);

            TempData["message"] =
                $"Power User Order opened. Primary warehouse: {order.PrimaryWarehouse} Secondary warehouse: {order.SecondaryWarehouse}";

            return RedirectToAction("Search", "StockStatus");
        }

        private IEnumerable<ShipMethodDetail> GetOrderShipMethods(Order order)
        {
            IEnumerable<ShipMethodDetail> shippingMethods;

            if (order.IsPowerUserOrder)
            {
                var powerUserService = new PowerUserService();
                shippingMethods = powerUserService.GetPowerUserShipMethods();
            }
            else
            {
                var shipMethodsProxy = AvailableShipMethodsProxy.Instance;
                shippingMethods = shipMethodsProxy.SubmitRequest(CustomerData, order.AssignedWhse, string.Empty);
            }

            return shippingMethods;
        }
    }
}