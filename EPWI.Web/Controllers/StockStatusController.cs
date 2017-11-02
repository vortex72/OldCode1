using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPWI.Components.Models;
using EPWI.Components.Services;
using EPWI.Web.Models;
//using EPWI.Web.Opticat;
using EPWI.Web.Utility;
using log4net;
using Newtonsoft.Json;
using xVal.ServerSide;
using EPWI.Components.Utility;
using EPWI.Web.Opticat;


namespace EPWI.Web.Controllers
{
    [Authorize]
    public class StockStatusController : LoggingController
    {
        private static readonly ILog log = LogManager.GetLogger("StockStatusController");

        // GET: /StockStatus/
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index(SessionStore sessionStore)
        {
            int currentView = GetViewCookie();
            sessionStore.ReturnToLookup = false;

            var model = new StockStatusViewModel
            {
                CustomerData = CustomerData,
                CurrentView = currentView,
                RequestedQuantity = 1,
                LastViewedKit = sessionStore.LastViewedKit
            };

            return View("Search", model);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Search(StockStatusRequest stockStatusRequest, int? requestedQuantity,
            string requestedItemNumber, string requestedLineCode, bool? lookup, SessionStore sessionStore)
        {
            int currentView = GetViewCookie();
            sessionStore.ReturnToLookup = lookup.GetValueOrDefault();

            var model = new StockStatusViewModel
            {
                CustomerData = CustomerData,
                CurrentView = currentView,
                LastViewedKit = sessionStore.LastViewedKit
            };

            model.RequestedQuantity = requestedQuantity.GetValueOrDefault(1);

            if (!string.IsNullOrEmpty(requestedItemNumber))
            {
                // if we have an item number, do the search
                model.RequestedItemNumber = requestedItemNumber;

                model.RequestedLineCode = requestedLineCode;
                processModel(model, stockStatusRequest, lookup: lookup.GetValueOrDefault());
            }

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(StockStatusRequest stockStatusRequest, StockStatusViewModel model, string switchView,
            bool? lookup, SessionStore sessionStore)
        {
            if (lookup.HasValue && !lookup.Value)
            {
                sessionStore.ReturnToLookup = false;
            }

            if (model.CurrentView == 0)
            {
                model.CurrentView = GetViewCookie();
            }

            if (switchView != null)
            {
                model.CurrentView = model.CurrentView == 1 ? 2 : 1;
                WriteViewCookie(model.CurrentView);
            }

            model.LastViewedKit = sessionStore.LastViewedKit;
            processModel(model, stockStatusRequest, lookup: lookup.GetValueOrDefault());

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "KIT_BUILDER")]
        public ActionResult KitSearch(StockStatusRequest stockStatusRequest, int? requestedQuantity,
            string requestedItemNumber, string requestedLineCode)
        {
            int currentView = GetViewCookie();

            var model = new KitStockStatusViewModel() {CustomerData = CustomerData, CurrentView = currentView};

            var inventoryRep = new InventoryItemRepository();

            model.RequestedQuantity = requestedQuantity.GetValueOrDefault(1);

            if (!string.IsNullOrEmpty(requestedItemNumber))
            {
                // if we have an item number, do the search
                model.RequestedItemNumber = requestedItemNumber;

                model.RequestedLineCode = requestedLineCode;
                processModel(model, stockStatusRequest);
            }

            return PartialView("KitSearch", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "KIT_BUILDER")]
        public ActionResult KitSearch(StockStatusRequest stockStatusRequest, KitStockStatusViewModel model)
        {
            model.CurrentView = GetViewCookie();

            var inventoryRep = new InventoryItemRepository();

            InventoryItem originalInventoryItem = null;

            if (!string.IsNullOrEmpty(model.OriginalPartUniqueIdentifier))
            {
                originalInventoryItem = inventoryRep.GetInventoryItemByNipc(model.OriginalPartNIPC);
            }

            if (model.RequestedSize == string.Empty)
            {
                model.RequestedSize = "STD";
            }

            model.CustomerData = CustomerData;
            model.OriginalPartInventoryItem = originalInventoryItem;

            if (!string.IsNullOrEmpty(model.RequestedItemNumber))
            {
                // if we have an item number, do the search; if for interchange, supply kit nipc to get kit pricing
                processModel(model, stockStatusRequest, model.IsForInterchange ? model.KitNipc : 0);
            }

            return PartialView("KitSearch", model);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ProductImage(int id, bool? download)
        {
            string filePath = Server.MapPath($"~/Content/Products/{id}.jpg");
            if (System.IO.File.Exists(filePath))
            {
                if (download.GetValueOrDefault(false))
                {
                    Response.AddHeader("content-disposition", $"attachment; filename={id}.jpg");
                }

                return File(FileUtility.ResizeImage(filePath, 300, 300, true), "image/jpeg");
            }

            Response.StatusCode = 404;
            return new EmptyResult();
        }

        private void processModel(StockStatusViewModel model, StockStatusRequest stockStatusRequest,
            int applicableKitNipc = 0, bool lookup = false)
        {
            model.CustomerData = CustomerData;

            if (ModelState.IsValid)
            {
                // if line code was supplied, parse it for the line code and the actual part number (to account for hyphenated part numbers)
                if (!string.IsNullOrEmpty(model.RequestedLineCode))
                {
                    var partData = model.RequestedLineCode.Split('|');

                    model.RequestedLineCode = partData[0];
                    if (partData.Length > 1)
                    {
                        model.RequestedItemNumber = partData[1];
                    }
                }

                var inventoryRep = new InventoryItemRepository();

                IEnumerable<InventoryItem> inventoryItems;

                try
                {
                    inventoryItems = inventoryRep.GetInventoryItems(model.RequestedItemNumber, model.RequestedLineCode,
                        lookup);
                }
                catch (System.Net.WebException ex)
                {
                    // this is a work-around for now. If we get a web error and the item number is greater than 20 characters, need to enforce this limitation
                    // have to do it this way because crunch might take a part number up to 25 characters, but it will error out if it is not valid and greater
                    // than 20 characters
                    if (model.RequestedItemNumber.Length > 20)
                    {
                        ModelState.AddModelError("RequestedItemNumber", "Item Number must be 20 characters or less.");
                        return;
                    }

                    // must be some other problem
                    throw;
                }

                try
                {
                    model.AddInventoryItems(inventoryItems);
                }
                catch (RulesException ex)
                {
                    ex.AddModelStateErrors(ModelState, string.Empty);
                }

                // if we have all the information we need to get stock status, go get it
                if (model.RequestComplete)
                {
                    try
                    {
                        var powerUserService = new PowerUserService();
                        var powerUserWarehouses = powerUserService.GetPowerUserWarehouseOverrides(CustomerData);

                        var stockStatus = StockStatusRepository.GetStockStatusByNipc(model.InventoryItem.NIPCCode,
                            applicableKitNipc, model.RequestedSize, model.RequestedQuantity.Value, CustomerData,
                            powerUserWarehouses);

                        model.AddStockStatus(stockStatus);

                        if (model.InventoryItem.IsKit)
                        {
                            var kitRep = new KitRepository();
                            model.SavedKitConfigurationID = kitRep.GetSavedConfigurationID(CustomerData.UserID);
                        }

                        // searches from the kit screen don't need interchanges
                        if (applicableKitNipc == 0)
                        {
                            model.AddInterchanges(InterchangeRepository.GetInterchangeData(CustomerData,
                                model.InventoryItem.NIPCCode, model.RequestedSize, model.RequestedQuantity.Value,
                                model.StockStatus.Cylinders, model.StockStatus.CustomerDefaultWarehouse));
                        }

                        if (User.IsInRole("LOOKUP"))
                        {

                            //TODO: Opticat
                            AddOpticatPartInfoJson(model);
                        }

                        // update the user's current stock status request to be the current request
                        model.UpdateStockStatusRequest(stockStatusRequest);
                        if (model is KitStockStatusViewModel)
                        {
                            // for kit adds, check to see if the part already exists in the kit and get category family information
                            var tempModel = model as KitStockStatusViewModel;
                            if (!tempModel.IsForInterchange)
                            {
                                var kitRepository = new KitRepository();
                                var kit = kitRepository.LoadConfiguredKit(CustomerData);
                                if (
                                    kit.MasterKitParts.Where(
                                        kp => kp.NIPCCode == model.InventoryItem.NIPCCode && kp.SequenceNumber == 999)
                                        .Count() > 0)
                                {
                                    ModelState.AddModelError("RequestedItemNumber",
                                        "The part you have requested has already been added to the kit. Duplicates cannot be added.");
                                    model.CanPlaceOrder = false;
                                }
                            }

                            if (stockStatusRequest.CorePrice > 0)
                            {
                                ModelState.AddModelError("RequestedItemNumber",
                                    "Items requiring cores cannot be added to kits.");
                                model.CanPlaceOrder = false;
                            }
                        }
                    }
                    catch (RulesException ex)
                    {
                        ex.AddModelStateErrors(ModelState, string.Empty);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                //hack to work around MVC bug
                ViewData.ModelState.Clear();
            }
        }

        private static void AddOpticatPartInfoJson(StockStatusViewModel model)
        {
            try
            {
                var opticatService = new OpticatService();

                EpcProductExt partInfo = null;

                if (!string.IsNullOrEmpty(model.StockStatus.Upc))
                {
                    partInfo = opticatService.GetProductInfoByUpc(model.StockStatus.Upc);
                }

                if (partInfo == null && !string.IsNullOrEmpty(model.StockStatus.ManufacturerPartNumber))
                {
                    partInfo = opticatService.GetProductInfoByManufacturerCodeAndPartNumber(model.InventoryItem.LineCode, model.StockStatus.ManufacturerPartNumber);
                }

                if (partInfo != null)
                {
                    model.OpticatPartInfoJson = JsonConvert.SerializeObject(OpticatService.MapEpcProductExtToPart(partInfo));
                }
            }
            catch (System.Exception ex)
            {
                log.Warn(
                    $"Error getting Opticat part info during a Stock Status Inquiry. UPC: {model.StockStatus.Upc}; " +
                    $"Manufacturer Part Number: {model.StockStatus.ManufacturerPartNumber}; Line Code: " +
                    $"{model.InventoryItem.LineCode}",
                    ex);
            }

        }
    }
}
