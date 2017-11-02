using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using EPWI.Components.Models;
using EPWI.Components.Proxies;
using EPWI.Components.Services;
using EPWI.Components.Utility;
using EPWI.Web.Models;

namespace EPWI.Web.Controllers
{
    [Authorize]
    public class KitController : LoggingController
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult View(string id, SessionStore sessionStore)
        {
            var currentView = GetViewCookie();

            var kitIdentifier = getKitIdentifier(id, string.Empty);
            var kitRepository = new KitRepository();

            var kit = kitRepository.GetKit(kitIdentifier, null);

            if (kit.StartYear == 0 && kit.EndYear == 0 && !kit.IsKTRACK)
            {
                // hack -- if the kit is not in a valid state, we must have parsed the kit identifier wrong (Ex: CH305LRMK should be a master kit), try again
                kitIdentifier = getKitIdentifier(id, string.Empty, true);
                kit = kitRepository.GetKit(kitIdentifier, null);
            }

            sessionStore.LastViewedKit = id;

            return
                View(new KitViewViewModel
                {
                    Kit = kit,
                    CustomerData = CustomerData,
                    CurrentView = currentView,
                    SavedConfigurationID = kitRepository.GetSavedConfigurationID(CustomerData.UserID),
                    AllowKitConfiguration = User.IsInRole("KIT_BUILDER")
                });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "KIT_BUILDER")]
        public ActionResult View(string id, bool deleteSavedConfiguration)
        {
            if (deleteSavedConfiguration)
            {
                // redirect to the build action, it will implicitly deleted any saved configuration
                return RedirectToAction("Build", new {id = id});
            }
            else
            {
                // redirect to the edit action, which will edit the saved configuration
                var kitRepository = new KitRepository();
                var kit = kitRepository.LoadConfiguredKit(CustomerData);

                if (!string.IsNullOrEmpty(kit.AcesID))
                {
                    // redirect to aces editor if it is an aces kit
                    return RedirectToAction("AcesEdit");
                }
                else
                {
                    return RedirectToAction("Edit");
                }
            }
        }

        [Authorize(Roles = "KIT_BUILDER")]
        public ActionResult Build(string id, int? selectedYear, string selectedKitType, string selectedBoreSize,
            string selectedRodBearingSize, string selectedMainBearingSize, string selectedThrustWasherSize,
            string selectedCrankKit, bool? sizesSelected, SessionStore sessionStore)
        {
            TempData.Remove("FulfillmentProcessingResult");
            var currentView = GetViewCookie();
            var kitIdentifier = getKitIdentifier(id, selectedKitType);
            var kitRepository = new KitRepository();

            sessionStore.LastViewedKit = kitIdentifier.KitPartNumber;

            kitRepository.DeleteConfiguredKit(CustomerData);
            kitRepository.Save();

            //for KTRAKs (for now), make the start year the start year of the kit, since it is believed there is no year specific part filtering
            var tempKit = kitRepository.GetKit(kitIdentifier, 0);
            if (tempKit.IsKTRACK)
            {
                selectedYear = tempKit.StartYear;
            }

            var kit = kitRepository.GetKit(kitIdentifier, selectedYear);

            if (kit.StartYear == 0 && kit.EndYear == 0 && !kit.IsKTRACK)
            {
                // hack -- if the kit is not in a valid state, we must have parsed the kit identifier wrong (Ex: CH305LRMK should be a master kit), try again
                kitIdentifier = getKitIdentifier(id, selectedKitType, true);
                kit = kitRepository.GetKit(kitIdentifier, selectedYear);
            }

            kit.SelectedBoreSize = selectedBoreSize;
            kit.SelectedRodBearingSize = selectedRodBearingSize;
            kit.SelectedMainBearingSize = selectedMainBearingSize;
            kit.SelectedThrustWasherSize = selectedThrustWasherSize ?? "";

            if (sizesSelected.GetValueOrDefault(false) || kit.IsKTRACK ||
                (kit.KitIdentifier.KitType == "CK" && selectedYear.GetValueOrDefault(0) > 0))
                // automatically go to editing for KTRACKs. For Cam Kits, skip size and crank kit selected after year is selected
            {
                var powerUserService = new PowerUserService();
                var powerUserWarehouses = powerUserService.GetPowerUserWarehouseOverrides(CustomerData);

                kit = kitRepository.GetPartPricingAndAvailability(kit, CustomerData,
                    powerUserWarehouses == null ? string.Empty : powerUserWarehouses.PrimaryWarehouse);

                // determine if parts aren't included for the kit's year by comparing the price before selecting a crank kit to the standard price of the kit
                var priceBeforeSelectingCrankKit =
                    (from kp in kit.MasterKitParts
                        // (we don't use kit.TypicallyConfiguredPrice because it is calculated by the DB and
                        where kp.GroupingOr == 1
                        //  we haven't persisted the record to the db yet)
                        select kp.Price*kp.QuantityRequired).Sum();
                kitRepository.GetStandardPricing(kit, CustomerData);
                TempData["PartsNotIncludedForYear"] = Math.Abs(priceBeforeSelectingCrankKit - kit.StandardPrice) >
                                                      (kit.StandardPrice*0.0025M);

                if (!string.IsNullOrEmpty(selectedCrankKit))
                {
                    kitRepository.SelectCrankKit(kit, CustomerData, int.Parse(selectedCrankKit));
                }


                kitRepository.SaveKitConfiguration(kit, CustomerData);
                kitRepository.Save();

                TempData["NewConfiguration"] = true;
                return RedirectToAction("Edit");
            }

            return
                View(new KitBuilderViewModel
                {
                    Kit = kit,
                    CustomerData = CustomerData,
                    Editing = false,
                    SizesSelected = sizesSelected.GetValueOrDefault(false),
                    CurrentView = currentView
                });
        }

        [Authorize(Roles = "ACES_KIT_BUILDER")]
        public ActionResult Aces(string id)
        {
            ViewData["id"] = id.ToUpper();
            return View();
        }

        [Authorize(Roles = "ACES_KIT_BUILDER")]
        public ActionResult AcesBuild(string id, string selectedCrankKit, bool? crankKitSelected)
        {
            id = id.ToUpper();
            var kitRepository = new KitRepository();

            var kit = kitRepository.GetAcesKit(id);

            if (kit == null)
            {
                TempData["Message"] = $"ACES Kit '{id}' not found.";
                return RedirectToAction("Aces");
            }

            kit.SelectedBoreSize = string.Empty;
            kit.SelectedRodBearingSize = string.Empty;
            kit.SelectedMainBearingSize = string.Empty;
            kit.SelectedThrustWasherSize = string.Empty;
            kit.AcesID = id;

            if (crankKitSelected.GetValueOrDefault(false))
            {
                if (!string.IsNullOrEmpty(selectedCrankKit))
                {
                    kitRepository.SelectCrankKit(kit, CustomerData, int.Parse(selectedCrankKit));
                }
                kitRepository.SaveKitConfiguration(kit, CustomerData);
                kitRepository.Save();

                TempData["NewConfiguration"] = true;
                return RedirectToAction("AcesEdit");
            }

            // default view to 1--not needed for Aces
            return
                View(new KitBuilderViewModel
                {
                    Kit = kit,
                    CustomerData = CustomerData,
                    Editing = false,
                    CurrentView = 1,
                    AcesMode = true
                });
        }

        [Authorize(Roles = "ACES_KIT_BUILDER")]
        public ActionResult AcesEdit()
        {
            var newConfiguration = (bool?) TempData["NewConfiguration"] ?? false;
            var kitRepository = new KitRepository();

            var kit = kitRepository.LoadConfiguredKit(CustomerData);
            var acesKitNumberProxy = AcesKitNumberProxy.Instance;
            var newKitNumber = acesKitNumberProxy.SubmitRequest(kit.AcesID);
            
            if (kit == null)
            {
                TempData["message"] = "Unable to edit kit. There is no saved kit configuration.";
                return RedirectToAction("Index", "Home");
            }

            ViewData["NewKitNumber"] = newKitNumber;

            // default view to 1--not needed for Aces
            return
                View(new KitBuilderViewModel
                {
                    Kit = kit,
                    Editing = true,
                    CustomerData = CustomerData,
                    CurrentView = 1,
                    NewConfiguration = newConfiguration,
                    AcesMode = true
                });
        }

        public ActionResult AcesSave(string notes, string newKitNumber)
        {
            var kitRepository = new KitRepository();
            var kit = kitRepository.LoadConfiguredKit(CustomerData);

            if (kitRepository.SaveAcesKit(kit, notes, newKitNumber, CustomerData))
            {
                TempData["message"] = $"ACES Kit '{newKitNumber}' saved successfully.";
                kitRepository.DeleteConfiguredKit(CustomerData);
                kitRepository.Save();
                return RedirectToAction("Aces", new {id = kit.AcesID});
            }
            else
            {
                TempData["message"] = "Error saving kit.";
                return RedirectToAction("AcesEdit");
            }
        }

        [Authorize(Roles = "KIT_BUILDER")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Edit(string selectedBoreSize, string selectedRodBearingSize, string selectedMainBearingSize,
            string selectedThrustWasherSize, bool? sizesSelected, string switchView, SessionStore sessionStore)
        {
            var result = (FulfillmentProcessingResult) TempData["FulfillmentProcessingResult"];
            if(result != null) TempData.Remove("FulfillmentProcessingResult");
            var currentView = GetViewCookie();

            if (switchView != null)
            {
                currentView = currentView == 1 ? 2 : 1;
                WriteViewCookie(currentView);
            }

            var newConfiguration = (bool?) TempData["NewConfiguration"] ?? false;
            var kitRepository = new KitRepository();

            var kit = kitRepository.LoadConfiguredKit(CustomerData);

            if (kit == null)
            {
                TempData["message"] = "Unable to edit kit. There is no saved kit configuration.";
                return RedirectToAction("Index", "Home");
            }

            sessionStore.LastViewedKit = kit.KitIdentifier.KitPartNumber;

            // if this is an ACES kit, redirect to the Aces editor
            if (!string.IsNullOrEmpty(kit.AcesID))
            {
                return RedirectToAction("AcesEdit");
            }

            if (sizesSelected.GetValueOrDefault(false))
            {
                kit.SelectedBoreSize = selectedBoreSize;
                kit.SelectedRodBearingSize = selectedRodBearingSize;
                kit.SelectedMainBearingSize = selectedMainBearingSize;
                kit.SelectedThrustWasherSize = selectedThrustWasherSize ?? "";
                //kitRepository.SaveKitConfiguration(kit, CustomerData);
                //kitRepository.Save();
            }

            var deselectionLimit = User.IsInRole("UNLIMITED_DESELECTION") ? 99 : kit.DeselectionLimit;
            var powerUserService = new PowerUserService();
            var powerUserWarehouses = powerUserService.GetPowerUserWarehouseOverrides(CustomerData);
            kit = kitRepository.GetPartPricingAndAvailability(kit, CustomerData,
                powerUserWarehouses == null ? string.Empty : powerUserWarehouses.PrimaryWarehouse);
            kitRepository.SaveKitConfiguration(kit, CustomerData);
            kitRepository.Save();

            return
                View(new KitBuilderViewModel
                {
                    Kit = kit,
                    Editing = true,
                    FulfillmentProcessingResult = result,
                    CustomerData = CustomerData,
                    CurrentView = currentView,
                    NewConfiguration = newConfiguration,
                    DeselectionLimit = deselectionLimit
                });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "KIT_BUILDER")]
        public ActionResult InterchangePart(string id)
        {
            var kitInterchangeViewModel = new KitInterchangeViewModel()
            {
                CurrentView = GetViewCookie(),
                CustomerData = CustomerData
            };
            var partData = parsePartUniqueIdentifier(id);
            var nipcCode = int.Parse(partData[0]);
            var sequenceNumber = partData[1];

            var kitRepository = new KitRepository();
            var kit = kitRepository.LoadConfiguredKit(CustomerData);

            var part = kit.GetKitPartByUniqueIdentifier(id);

            if (part == null)
            {
                throw new ApplicationException("Kit part not found in kit.");
            }

            var powerUserService = new PowerUserService();
            var powerUserWarehouses = powerUserService.GetPowerUserWarehouseOverrides(CustomerData);

            var stockStatus = StockStatusRepository.GetStockStatusByNipc(nipcCode, part.SizeCode, part.QuantitySelected,
                CustomerData, powerUserWarehouses);

            kitInterchangeViewModel.Kit = kit;
            kitInterchangeViewModel.Interchanges = InterchangeRepository.GetInterchangeData(CustomerData, nipcCode,
                part.SizeCode, part.QuantitySelected, kit.Cylinders, stockStatus.CustomerDefaultWarehouse, true);
            kitInterchangeViewModel.OriginalPart = part;
            // if not available in default warehouse, show the original part so the user can be presented with a list of shipment options
            kitInterchangeViewModel.ShowOriginalPart = !stockStatus.OrderMethodAvailability(OrderMethod.MainWarehouse);
            kitInterchangeViewModel.DefaultWarehouse = stockStatus.CustomerDefaultWarehouse;
            return PartialView(kitInterchangeViewModel);
        }

        [Authorize(Roles = "KIT_BUILDER")]
        public ActionResult InterchangeShipmentOptions(string id, int interchangeNIPC, int interchangeQuantity,
            string sizeCode, string interchangeCode)
        {
            var kitInterchangeViewModel = new KitInterchangeViewModel()
            {
                CurrentView = GetViewCookie(),
                CustomerData = CustomerData,
                InterchangeCode = interchangeCode
            };

            var partData = parsePartUniqueIdentifier(id);
            var nipcCode = int.Parse(partData[0]);
            var sequenceNumber = partData[1];

            var kitRepository = new KitRepository();
            var kit = kitRepository.LoadConfiguredKit(CustomerData);

            var part = kit.GetKitPartByUniqueIdentifier(id);

            if (part == null)
            {
                throw new ApplicationException("Kit part not found in kit.");
            }

            var inventoryRep = new InventoryItemRepository();
            var interchangeInventoryItem = inventoryRep.GetInventoryItemByNipc(interchangeNIPC);

            var interchangeStockStatus = StockStatusRepository.GetStockStatusByNipc(interchangeNIPC, sizeCode,
                interchangeQuantity, CustomerData);
            kitInterchangeViewModel.OriginalPart = part;
            kitInterchangeViewModel.InterchangeInventoryItem = interchangeInventoryItem;
            kitInterchangeViewModel.InterchangeStockStatus = interchangeStockStatus;

            return PartialView(kitInterchangeViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "KIT_BUILDER")]
        public ActionResult InterchangePart(StockStatusRequest stockStatusRequest, string id, string interchangeNIPC,
            int interchangeQuantity, OrderMethod orderMethod, string warehouse, bool? confirmingAvailability,
            string interchangeType)
        {
            if (!confirmingAvailability.GetValueOrDefault(false))
            {
                // only track the actual interchange type during the confirming availability phase, so we can price accordingly. otherwise set it to null so it defaults to "I"
                interchangeType = null;
            }

            var kitRepository = new KitRepository();

            var kit = kitRepository.LoadConfiguredKit(CustomerData);

            // don't save the warehouse if shipping from customer's main warehouse
            if (orderMethod == OrderMethod.MainWarehouse)
            {
                warehouse = string.Empty;
            }

            // No original part, so simply add the part
            if (string.IsNullOrEmpty(id))
            {
                kitRepository.AddKitPartFromStockStatus(kit, stockStatusRequest, CustomerData, warehouse, orderMethod);
            }
            else
            {
                // otherwise, do the interchange
                kitRepository.InterchangePart(kit, id, interchangeNIPC, CustomerData, warehouse, interchangeQuantity,
                    orderMethod, interchangeType, confirmingAvailability.GetValueOrDefault(false));
            }

            kitRepository.Save();

            // reinitialize repository to eliminate caching issues
            kitRepository = new KitRepository();
            //reload the kit to get updated pricing. this might need to be optimized at some point
            kit = kitRepository.LoadConfiguredKit(CustomerData);

            // return new price for updating on the client
            return Content(adjustedKitPrice(kit));
        }

        [Authorize(Roles = "KIT_BUILDER")]
        public ActionResult RevertInterchange(string id)
        {
            var kitRepository = new KitRepository();
            var kit = kitRepository.LoadConfiguredKit(CustomerData);

            kitRepository.RevertInterchange(kit, id, CustomerData);
            kitRepository.Save();

            // reinitialize repository to eliminate caching issues
            kitRepository = new KitRepository();
            //reload the kit to get updated pricing. this might need to be optimized at some point
            kit = kitRepository.LoadConfiguredKit(CustomerData);

            // return new price for updating on the client
            return Content(adjustedKitPrice(kit));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "KIT_BUILDER")]
        public ActionResult AddKitToOrder(string selectedWarehouse, string resultCode, bool? revert, bool? force,
            string customerReference)
        {
            // if previous result code was P,S,C,O, or X no need to re-run fulfillment processing. add kit to order
            // X is okay because the client side validation won't allow submission of the order until the out of stock item is fixed
            if (!string.IsNullOrEmpty(resultCode))
            {
                // only interested in the first character of the result code
                resultCode = resultCode.Substring(0, 1);
            }

            var bypassFulfillmentProcessing = (resultCode == "P" || resultCode == "S" || resultCode == "C" ||
                                                resultCode == "O" || resultCode == "X") ||
                                               force.GetValueOrDefault(false);
            if (selectedWarehouse == "null")
            {
                selectedWarehouse = null;
            }
            var orderID = 0;
            var kitRepository = new KitRepository();
            var orderRepository = new OrderRepository();

            if (revert.GetValueOrDefault(false))
            {
                kitRepository.ReloadSnapshot(CustomerData);
            }

            var kit = kitRepository.LoadConfiguredKit(CustomerData);
            var powerUserService = new PowerUserService();
            var powerUserWarehouses = powerUserService.GetPowerUserWarehouseOverrides(CustomerData);
            kit = kitRepository.GetPartPricingAndAvailability(kit, CustomerData,
                powerUserWarehouses == null ? string.Empty : powerUserWarehouses.PrimaryWarehouse);

            if (kit == null)
            {
                TempData["message"] = "Unable to add kit to order. Kit configuration not found.";
                return RedirectToAction("Index", "Home");
            }

            FulfillmentProcessingResult result = null;

            if (bypassFulfillmentProcessing)
            {
                result = new FulfillmentProcessingResult {ResultCode = "N"};
            }
            else
            {
                result = kitRepository.RunFulfillmentProcessing(kit, CustomerData, selectedWarehouse);
                kitRepository.Save();
            }

            if (force.GetValueOrDefault(false))
            {
                // add force flag
                kit.OrderMethod = "F";
            }

            if (bypassFulfillmentProcessing || result.ResultCode == "N")
            {
                orderID = orderRepository.AddKitToOrder(kit, CustomerData,
                    kitRepository.SerializeKitConfiguration(CustomerData), customerReference,
                    powerUserWarehouses == null ? null : powerUserWarehouses.PrimaryWarehouse);
                orderRepository.Save();
                kitRepository.DeleteConfiguredKit(CustomerData);
                kitRepository.Save();
            }

            TempData["FulfillmentProcessingResult"] = result;
            TempData["CustomerReference"] = customerReference;

            return Json(new {resultCode = result.ResultCode, orderID = orderID, kitNipc = kit.NIPCCode},
                JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "KIT_BUILDER")]
        public ActionResult ReloadSnapshot()
        {
            var kitRepository = new KitRepository();
            kitRepository.ReloadSnapshot(CustomerData);

            return RedirectToAction("Edit");
        }

        [Authorize(Roles = "KIT_BUILDER,ACES_KIT_BUILDER")]
        public ActionResult GetKitClientData()
        {
            var kitRepository = new KitRepository();
            var kit = kitRepository.LoadConfiguredKit(CustomerData);

            var clientData = from kp in kit.MasterKitParts
                select new
                {
                    kp.CategoryID,
                    kp.UniqueKitPartIdentifier,
                    kp.GroupName,
                    kp.PartsToGroup,
                    kp.PartsToSelect,
                    kp.PartsToDeselect
                };

            return Json(clientData, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "KIT_BUILDER")]
        public ActionResult UpdateCategory(int id)
        {
            KitCategory category;
            var kitRepository = new KitRepository();
            var kit = kitRepository.LoadConfiguredKit(CustomerData);

            if (kit.IsKTRACK)
            {
                category = KitCategory.Ktrack;
            }
            else
            {
                category = (from m in KitCategoryMapping.Mappings
                    where m.Value.CategoryID.Contains(id)
                    select m.Key).Single();
            }

            return PartialView("KitCategory",
                new KitCategoryViewModel()
                {
                    CategoryParts = kit.GetCategoryParts(category),
                    Editing = true,
                    Kit = kit,
                    MasterKitCategory = id != 0
                });
        }

        [Authorize(Roles = "KIT_BUILDER,ACES_KIT_BUILDER")]
        public ActionResult UpdatePartSelections(string group, string [] partSelections)
        {
            if (partSelections == null)
            {
                partSelections = new string[0];
            }

            var kitRepository = new KitRepository();

            var kit = kitRepository.LoadConfiguredKit(CustomerData);

            foreach (var part in kit.MasterKitParts.Where(kp => kp.GroupName == group))
            {
                part.Selected = partSelections.Contains(part.UniqueKitPartIdentifier);
            }
            kitRepository.UpdateKitConfiguration(kit, CustomerData);
            kitRepository.Save();

            //reload the kit to get updated pricing. this might need to be optimized at some point
            kitRepository = new KitRepository();
            kit = kitRepository.LoadConfiguredKit(CustomerData);

            // return new price for updating on the client
            return Content(adjustedKitPrice(kit));
        }

        [Authorize(Roles = "KIT_BUILDER")]
        public ActionResult GetWarranties(int id, int orderItemID, List<Warranty> warranties)
        {
            var orderRepository = new OrderRepository();
            var kitRepository = new KitRepository();
            var order = orderRepository.OpenOrder(CustomerData, false);

            if (order == null)
            {
                throw new ApplicationException("Can't display warranties. No order found for current user.");
            }

            var kitItem = order.OrderItems.Where(oi => oi.OrderItemID == orderItemID).SingleOrDefault();

            if (kitItem == null)
            {
                throw new ApplicationException("Can't display warranties. Kit item not found in order.");
            }

            warranties.Clear();
            warranties.AddRange(kitRepository.GetKitWarranties(id, kitItem.Price, CustomerData, orderItemID));

            addKitMessage();

            return PartialView("Warranty",
                new WarrantyViewModel
                {
                    OrderItemID = orderItemID,
                    KitPrice = kitItem.Price,
                    Warranties = warranties,
                    CustomerData = CustomerData,
                    View = GetViewCookie()
                });
        }

        [Authorize(Roles = "KIT_BUILDER")]
        public ActionResult AddWarranty(int id, int warranty, List<Warranty> warranties)
        {
            if (warranty > 0)
            {
                var orderRepository = new OrderRepository();
                orderRepository.AddWarrantyToKitOrderItem(id, warranty,
                    warranties.Where(w => w.Nipc == warranty && w.OrderItemID == id).Single().Price);
            }
            addKitMessage();
            return RedirectToAction("Search", "StockStatus");
        }

        private KitIdentifier getKitIdentifier(string kitID)
        {
            return getKitIdentifier(kitID, string.Empty);
        }

        private KitIdentifier getKitIdentifier(string kitID, string kitType)
        {
            return getKitIdentifier(kitID, kitType, false);
        }

        /// <summary>
        /// Splits a kit id into a kit identifier and kit type
        /// </summary>
        /// <param name="kitID">Full kit id string</param>
        /// <param name="kitType">Default kit type to use if one isn't found</param>
        /// <returns>KitIdentifier object with kit identifier and kit type</returns>
        private KitIdentifier getKitIdentifier(string kitID, string kitType, bool ignoreThreeCharacterKitType)
        {
            if (string.IsNullOrEmpty(kitType))
            {
                kitType = /*"MK"*/ string.Empty;
            }

            kitID = kitID.ToUpper();

            if (kitID == "CH305LRMK")
            {
                /* This is a one off for handling this kit which has an ambiguous identifier */
                kitID = "CH305LR";
                kitType = "MK";
            }
            else
            {
                if (!ignoreThreeCharacterKitType)
                {
                    switch (kitID.Substring(kitID.Length - 3, 3))
                    {
                        case "RRP":
                            kitID = kitID.Substring(0, kitID.Length - 3);
                            kitType = "RRP";
                            break;
                        case "RMK":
                            kitID = kitID.Substring(0, kitID.Length - 3);
                            kitType = "RMK";
                            break;
                    }
                }

                switch (kitID.Substring(kitID.Length - 2, 2))
                {
                    case "MK":
                        kitID = kitID.Substring(0, kitID.Length - 2);
                        kitType = "MK";
                        break;
                    case "RR":
                        kitID = kitID.Substring(0, kitID.Length - 2);
                        kitType = "RR";
                        break;
                    case "EK":
                        kitID = kitID.Substring(0, kitID.Length - 2);
                        kitType = "EK";
                        break;
                    case "CK":
                        kitID = kitID.Substring(0, kitID.Length - 2);
                        kitType = "CK";
                        break;
                }
            }

            if (kitID.Length < 3)
            {
                throw new ArgumentException("Kit ID must be at least 3 characters");
            }

            return new KitIdentifier {KitID = kitID, KitType = kitType};
        }

        private string adjustedKitPrice(Kit kit)
        {
            var currentView = GetViewCookie();

            return kit.GetConfiguredPrice(false).Adjusted(currentView, CustomerData).ToString("C");
        }

        private string [] parsePartUniqueIdentifier(string uniqueIdentifier)
        {
            return uniqueIdentifier.Split('-');
        }

        private void addKitMessage()
        {
            TempData["unencodedmessage"] =
                $"Kit successfully added to {HtmlHelper.GenerateLink(ControllerContext.RequestContext, RouteTable.Routes, "order", string.Empty, "Index", "Order", null, null)}.";
        }
    }
}