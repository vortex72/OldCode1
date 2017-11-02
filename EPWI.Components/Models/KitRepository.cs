using EPWI.Components.Exceptions;
using EPWI.Components.Proxies;
using EPWI.Components.Utility;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace EPWI.Components.Models
{
    public class KitRepository : Repository
    {
        private static readonly ILog log = LogManager.GetLogger("KitRepository");

        public Kit GetAcesKit(string id)
        {
            Kit kit;

            var kitHeaderData = getAcesKitHeader(id);

            if (kitHeaderData == null)
                return null;

            var baseKitIdentifier = kitHeaderData.KXEG.Trim() + "MK";
            var inventoryItemRepository = new InventoryItemRepository();
            var inventoryItem = inventoryItemRepository.GetInventoryItems(baseKitIdentifier, "KIT").SingleOrDefault();

            if (inventoryItem == null)
                throw new ApplicationException("No NIPC codes were found for the selected kit");

            if (kitHeaderData == null)
                throw new ApplicationException("Kit header data not found.");

            kit = new Kit();
            kit.KitIdentifier = new KitIdentifier
            {
                KitID = kitHeaderData.KXEG.Trim(),
                KitType = "MK",
                AcesKitIdentifier = id
            };
            PopulateStandardKitData(kit, kitHeaderData, inventoryItem);
            kit.SelectedYear = kitHeaderData.KSYY.GetValueOrDefault(0);
            kit.MasterKitParts = getMasterKitParts(kit, true);
            //this.GetPartPricingAndAvailability(kit, customerData);

            return kit;
        }

        public Kit GetKit(KitIdentifier kitIdentifier, int? year)
        {
            Kit kit;

            // get the kit header data by passing in the KitID for the generic kit (without the type)
            var kitHeaderData = Db.usp_GetKitCatalogHeaderData(kitIdentifier.KitID).SingleOrDefault();

            var inventoryItemRepository = new InventoryItemRepository();
            var inventoryItem =
                inventoryItemRepository.GetInventoryItems(kitIdentifier.KitPartNumber, "KIT").SingleOrDefault();

            if (inventoryItem == null)
                throw new KitNoNipcCodeFoundException(
                    $"No NIPC codes were found for the specified kit. A friendly error message was displayed to the customer. Kit Part Number: {kitIdentifier.KitPartNumber}");

            if (kitHeaderData == null)
                kitHeaderData = Db.usp_GetKitCatalogHeaderData(null, inventoryItem.NIPCCode).SingleOrDefault();

            //if (kitHeaderData == null)
            //{
            //  throw new ApplicationException("Kit header data not found.");
            //}

            kit = new Kit
            {
                KitIdentifier = kitIdentifier,
                SelectedYear = year.GetValueOrDefault(0)
            };
            PopulateStandardKitData(kit, kitHeaderData, inventoryItem);
            kit.MasterKitParts = getMasterKitParts(kit);
            //this.GetPartPricingAndAvailability(kit, customerData);

            return kit;
        }

        public Kit GetPartPricingAndAvailability(Kit kit, ICustomerData customerData, string primaryWarehouse)
        {
            var proxy = KitPricingRequestProxy.Instance;
            return proxy.SubmitRequest(kit, customerData, primaryWarehouse);
        }

        public Kit LoadConfiguredKit(ICustomerData customerData)
        {
            var kitConfig = getKitConfiguration(customerData);

            // if no kit configuration, then there is no saved kit configuration, return null
            return kitConfig == null ? null : PopulateConfiguredKit(kitConfig);
        }

        public bool KitExistsForUser(ICustomerData customerData)
        {
            return (from kc in Db.KitConfigurations
                    where kc.UserID == customerData.UserID
                    select kc).Count() > 0;
        }

        public string SerializeKitConfiguration(ICustomerData customerData)
        {
            var kitConfig = getKitConfiguration(customerData);

            var ms = new MemoryStream();
            var writer = new XmlTextWriter(ms, new UTF8Encoding());

            var serializer = new XmlSerializer(typeof(KitConfiguration));

            try
            {
                serializer.Serialize(writer, kitConfig);
            }
            catch (Exception e)
            {
                throw;
            }
            var result = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(result, 0, (int)ms.Length);

            return Encoding.UTF8.GetString(result, 0, (int)ms.Length);
        }

        public void DeleteConfiguredKit(ICustomerData customerData)
        {
            var kitConfig = getKitConfiguration(customerData);

            if (kitConfig != null)
                Db.KitConfigurations.DeleteOnSubmit(kitConfig);
        }

        public void GetStandardPricing(Kit kit, ICustomerData customerData)
        {
            var rep = new StockStatusRepository();

            var stockStatus = StockStatusRepository.GetStockStatusByNipc(kit.NIPCCode, 0, null, 1, customerData);
            kit.Cylinders = stockStatus.Cylinders;
            kit.StandardPrice = stockStatus.Price[PriceType.P3];
        }

        public FulfillmentProcessingResult RunFulfillmentProcessing(Kit kit, ICustomerData customerData,
            string requestedWarehouse)
        {
            var unavailablePart = false;
            var processedNICodes = new List<string>();
            var processedWarehouses = new List<string>();
            string foundLocation = null;

            saveKitConfigurationSnapshot(customerData.UserID);


            var orderRepository = new OrderRepository();
            var order = orderRepository.OpenOrder(customerData, false);


            var fulfillmentProcessingProxy = FulfillmentProcessingProxy.Instance;
            string primaryWarehouse = string.Empty, secondaryWarehouse = string.Empty;
            if ((order != null) && order.IsPowerUserOrder)
            {
                primaryWarehouse = order.PrimaryWarehouse;
                secondaryWarehouse = order.SecondaryWarehouse;
            }
            var result = fulfillmentProcessingProxy.SubmitRequest(kit, customerData, primaryWarehouse,
                secondaryWarehouse);

            if (result.ResultCode == "N")
                return result;

            switch (result.ResultCode)
            {
                case "P": // combined in primary and secondary
                    processedNICodes = ParseFulfillmentParts(kit, result.FulfillmentProcessingRecords, "1",
                        result.ResultCode, out unavailablePart, customerData);
                    break;
                case "S":
                case "C": // avaiable in secondary or combined in primary and secondary
                    processedNICodes = ParseFulfillmentParts(kit, result.FulfillmentProcessingRecords, "2",
                        result.ResultCode, out unavailablePart, customerData);
                    break;
                case "O": // other than primary or secondary
                    for (var i = 3; i < 16; i++)
                        if (
                            !string.IsNullOrEmpty(
                                result.FulfillmentProcessingRecords.Rows[0]["ZKWHS" + i.ToString("X")].ToString().Trim()))
                            foundLocation = i.ToString("X");
                    processedNICodes = ParseFulfillmentParts(kit, result.FulfillmentProcessingRecords, foundLocation,
                        result.ResultCode, out unavailablePart, customerData);
                    break;
                case "M":
                    if (!string.IsNullOrEmpty(requestedWarehouse))
                    {
                        // if the user has selected a warehouse, process the fulfillment request based on that selected warehouse.
                        // find the column set that contains that warehouse code (third set and greater)
                        for (var i = 3; i < 16; i++)
                            if (
                                result.FulfillmentProcessingRecords.Rows[0]["ZKWHS" + i.ToString("X")].ToString().Trim() ==
                                requestedWarehouse)
                            {
                                foundLocation = i.ToString("X");
                                break;
                            }
                        processedNICodes = ParseFulfillmentParts(kit, result.FulfillmentProcessingRecords, foundLocation,
                            result.ResultCode, out unavailablePart, customerData);
                        // set the processing result to "O" so that the function return and the same functionality is established just as if
                        // only one solution was found in an outside warehouse
                        result.ResultCode = "O";
                    }
                    else
                    {
                        // if the user hasn't selected a warehouse (meaning that this is the first time that the fulfillment processing is called)
                        // then loop through all columns (except the 1st and 2nd which would mean that the solution was found just in the primary or secondary)
                        // and determine warehouse that can solve the fulfillment request
                        for (var i = 3; i < 16; i++)
                            if (
                                !string.IsNullOrEmpty(
                                    result.FulfillmentProcessingRecords.Rows[0]["ZKWHS" + i.ToString("X")].ToString()
                                        .Trim()))
                                processedWarehouses.Add(
                                    result.FulfillmentProcessingRecords.Rows[0]["ZKWHS" + i.ToString("X")].ToString()
                                        .Trim());

                        // loop through all the parts and get the original part number
                        for (var i = 1; i < result.FulfillmentProcessingRecords.Rows.Count; i++)
                            processedNICodes.Add(result.FulfillmentProcessingRecords.Rows[i]["ZKPNI"].ToString());
                    }
                    break;
            }

            if (unavailablePart)
                result.ResultCode += "X";

            result.ProcessedNipcCodes = processedNICodes;
            result.ProcessedWarehouses = processedWarehouses;

            return result;
        }

        private List<string> ParseFulfillmentParts(Kit kit, DataTable fulfillmentRecords, string columnSet,
            string resultCode, out bool unavailablePart, ICustomerData customerData)
        {
            unavailablePart = false;
            var fulfillmentParts = new List<string>();
            // get the user's primary warehouse from the first record
            var usersPrimaryWarehouse = fulfillmentRecords.Rows[0]["ZKWHS1"].ToString();

            for (var i = 1; i < fulfillmentRecords.Rows.Count; i++)
            {
                var record = fulfillmentRecords.Rows[i];
                var fulfillmentPart = new FulfillmentPart
                {
                    OriginalPartNipc = record["ZKPNI"].ToString().Trim(),
                    OriginalQuantity = int.Parse(record["ZKQTY"].ToString()),
                    OriginalSequenceNumber = int.Parse(record["ZKSEQ"].ToString()),
                    InterchangePartNipc = record["ZKPNI" + columnSet].ToString().Trim(),
                    InterchangeQuantity = int.Parse(record["ZKQTY" + columnSet].ToString()),
                    InterchangeWarehouse = record["ZKWHS" + columnSet].ToString().Trim(),
                    InterchangePrice = decimal.Parse(record["ZKPRC" + columnSet].ToString()),
                    InterchangeType = record["ZKITP" + columnSet].ToString().Trim(),
                    InterchangeCategory = int.Parse(record["ZKCTG" + columnSet].ToString())
                };

                // clear out the warehouse code if shipping from the user's primary warehouse
                if (fulfillmentPart.InterchangeWarehouse == usersPrimaryWarehouse)
                    fulfillmentPart.InterchangeWarehouse = string.Empty;

                if (fulfillmentPart.InterchangeType != "X")
                    interchangeFulfillmentPart(fulfillmentPart, kit, customerData);
                else
                    unavailablePart = true;

                fulfillmentParts.Add($"{fulfillmentPart.InterchangePartNipc}-{fulfillmentPart.OriginalSequenceNumber}");
            }

            return fulfillmentParts;
        }

        private void interchangeFulfillmentPart(FulfillmentPart fulfillmentPart, Kit kit, ICustomerData customerData)
        {
            if (fulfillmentPart.OriginalPartNipc == fulfillmentPart.InterchangePartNipc)
                updateShipmentWarehouse(kit, fulfillmentPart.OriginalPartNipc, fulfillmentPart.OriginalSequenceNumber,
                    fulfillmentPart.InterchangeWarehouse, OrderMethod.OtherWarehouse, customerData);
            else
                InterchangePart(kit, $"{fulfillmentPart.OriginalPartNipc}-{fulfillmentPart.OriginalSequenceNumber}",
                    fulfillmentPart.InterchangePartNipc, customerData, fulfillmentPart.InterchangeWarehouse,
                    fulfillmentPart.InterchangeQuantity,
                    string.IsNullOrEmpty(fulfillmentPart.InterchangeWarehouse)
                        ? OrderMethod.MainWarehouse
                        : OrderMethod.OtherWarehouse, "F", false, fulfillmentPart.InterchangePrice);
        }

        private void updateShipmentWarehouse(Kit kit, string partNipc, int partSequenceNumber, string warehouse,
            OrderMethod orderMethod, ICustomerData customerData)
        {
            var partToUpdate = (from kp in kit.MasterKitParts
                                where (kp.NIPCCode == int.Parse(partNipc)) && (kp.SequenceNumber == partSequenceNumber)
                                select kp).Single();

            // only update the warehouse if it has actually changed
            if (partToUpdate.ShipWarehouse != warehouse)
            {
                partToUpdate.ShipWarehouse = warehouse;
                partToUpdate.OrderMethod = orderMethod.ToCode();
                UpdateKitConfiguration(kit, customerData);
            }
        }

        public void UpdateKitConfiguration(Kit kit, ICustomerData customerData)
        {
            var kitConfig = getKitConfiguration(customerData);

            updateKitConfigurationFromKit(kitConfig, kit, customerData);

            foreach (var kitConfigurationPart in kitConfig.KitConfigurationParts)
            {
                var kitPart = (from kp in kit.MasterKitParts
                               where
                               (kp.NIPCCode == kitConfigurationPart.PartNIPC) &&
                               (kp.SequenceNumber == kitConfigurationPart.SequenceNumber)
                               select kp).SingleOrDefault();
                if (kitPart != null)
                    updateKitConfigurationPartFromKitPart(kitConfigurationPart, kitPart);
            }
        }

        private void updateKitConfigurationPartFromKitPart(KitConfigurationPart kitConfigurationPart, KitPart kitPart)
        {
            //Disabled... this causes weird side effects
            //if (kitPart.InterchangeMethod == "I" || kitPart.InterchangeMethod == "K")
            //{ // always insure that the kitPart is selected on a user commanded interchange
            //  kitPart.Selected = true;
            //}

            kitConfigurationPart.PartNIPC = kitPart.NIPCCode;
            kitConfigurationPart.SizeCode = kitPart.SizeCode;
            kitConfigurationPart.Selected = kitPart.Selected;
            kitConfigurationPart.SequenceNumber = kitPart.SequenceNumber;
            kitConfigurationPart.Category = kitPart.CategoryID;
            kitConfigurationPart.JoinQualifier = string.IsNullOrEmpty(kitPart.JoinQualifier)
                ? ' '
                : kitPart.JoinQualifier[0];
            kitConfigurationPart.PreJoinQualifier = string.IsNullOrEmpty(kitPart.PreJoinQualifier)
                ? ' '
                : kitPart.PreJoinQualifier[0];
            kitConfigurationPart.GroupingMain = kitPart.GroupingMain;
            kitConfigurationPart.GroupingOr = kitPart.GroupingOr;
            kitConfigurationPart.GroupingAnd = kitPart.GroupingAnd;
            kitConfigurationPart.PartsToGroup = kitPart.PartsToGroup;
            kitConfigurationPart.PartsToDeselect = kitPart.PartsToDeselect;
            kitConfigurationPart.PartsToSelect = kitPart.PartsToSelect;
            kitConfigurationPart.QuantityReq = kitPart.QuantityRequired;
            kitConfigurationPart.QuantitySel = kitPart.QuantitySelected;
            kitConfigurationPart.Price = kitPart.Price;
            kitConfigurationPart.PricingPercentage = kitPart.PricingPercentage;
            kitConfigurationPart.ShipWarehouse = kitPart.ShipWarehouse;
            kitConfigurationPart.InterchangeMethod = string.IsNullOrEmpty(kitPart.InterchangeMethod)
                ? ' '
                : kitPart.InterchangeMethod[0];
            kitConfigurationPart.OrderMethod = string.IsNullOrEmpty(kitPart.OrderMethod) ? ' ' : kitPart.OrderMethod[0];
            kitConfigurationPart.OriginalPartUniqueID = kitPart.OriginalPartUniqueID != string.Empty
                ? kitPart.OriginalPartUniqueID
                : null;
            kitConfigurationPart.Available = kitPart.IsAvailable;
            kitConfigurationPart.StartYear = kitPart.StartYear;
            kitConfigurationPart.EndYear = kitPart.EndYear;
            kitConfigurationPart.PartNote = kitPart.Note;
        }

        /// <summary>
        ///     Updates a KitConfiguration object from a Kit record
        /// </summary>
        private void updateKitConfigurationFromKit(KitConfiguration kitConfiguration, Kit kit,
            ICustomerData customerData)
        {
            kitConfiguration.UserID = customerData.UserID;
            kitConfiguration.KitNIPC = kit.NIPCCode;
            kitConfiguration.SelectedYear = kit.SelectedYear;
            kitConfiguration.BoreSize = kit.SelectedBoreSize;
            kitConfiguration.RodBearingSize = kit.SelectedRodBearingSize;
            kitConfiguration.MainBearingSize = kit.SelectedMainBearingSize;
            kitConfiguration.ThrustWasherSize = kit.SelectedThrustWasherSize;
            kitConfiguration.Cylinders = kit.Cylinders;
            //TODO: Order method
            kitConfiguration.StandardPrice = kit.StandardPrice;
            kitConfiguration.CrankKitNIPC = kit.SelectedCrankKitNIPC;
            kitConfiguration.CrankKitPrice = kit.SelectedCrankKitPrice;
            kitConfiguration.CrankKitCoreNIPC = kit.SelectedCrankKitCoreNIPC;
            kitConfiguration.CrankKitCorePrice = kit.SelectedCrankKitCorePrice;
            kitConfiguration.AcesID = kit.AcesID;
            //TODO: Order Item ID
            //TODO: Grouping list??
        }

        public void SaveKitConfiguration(Kit kit, ICustomerData customerData)
        {
            // delete any existing configuration
            DeleteConfiguredKit(customerData);
            Save();

            var kitConfig = new KitConfiguration();
            updateKitConfigurationFromKit(kitConfig, kit, customerData);

            foreach (var part in kit.MasterKitParts)
            {
                var partConfig = new KitConfigurationPart();
                updateKitConfigurationPartFromKitPart(partConfig, part);
                kitConfig.KitConfigurationParts.Add(partConfig);
            }
            kitConfig.CreateDate = DateTime.Now;
            Db.KitConfigurations.InsertOnSubmit(kitConfig);
        }

        public void SelectCrankKit(Kit kit, ICustomerData customerData, int crankKitNipc)
        {
            var rep = new StockStatusRepository();

            var stockStatus = StockStatusRepository.GetStockStatusByNipc(crankKitNipc, 0, null, 1, customerData);

            kit.SelectedCrankKitNIPC = crankKitNipc;
            kit.SelectedCrankKitPrice = stockStatus.Price[PriceType.P3];
            kit.SelectedCrankKitCoreNIPC = stockStatus.CoreNIPC;
            kit.SelectedCrankKitCorePrice = stockStatus.CorePrice;

            // loop through each part in the kit and change the selected property 
            // for the rod bearings, main bearings, and thrust washers to false
            kit.MasterKitParts.Where(kp => new[] { 20, 21, 23 }.Contains(kp.CategoryID))
                .ToList()
                .ForEach(kp => kp.Selected = false);
        }

        public long GetSavedConfigurationID(int userId)
        {
            var id = from kc in Db.KitConfigurations
                     where kc.UserID == userId
                     select kc.KitConfigurationID;

            return id.FirstOrDefault();
        }

        private Kit PopulateConfiguredKit(KitConfiguration kitConfig)
        {
            var kit = new Kit();

            usp_GetKitCatalogHeaderDataResult kitHeaderData;

            if (!string.IsNullOrEmpty(kitConfig.AcesID))
                kitHeaderData = getAcesKitHeader(kitConfig.AcesID);
            else
                kitHeaderData = Db.usp_GetKitCatalogHeaderData(null, kitConfig.KitNIPC).SingleOrDefault();

            var inventoryItemRepository = new InventoryItemRepository();
            var inventoryItem = inventoryItemRepository.GetInventoryItemByNipc(kitConfig.KitNIPC);

            if (inventoryItem == null)
                throw new ApplicationException(
                    $"Unable to find matching inventory item for saved kit nipc {kitConfig.KitNIPC}");

            if (kitHeaderData == null)
                throw new ApplicationException("Kit header data not found.");

            var kitID = kitHeaderData.KXEG.Trim();
            // populate the kit identifier
            kit.KitIdentifier = new KitIdentifier
            {
                KitID = kitID,
                KitType =
                    inventoryItem.ItemNumber.Substring(kitID.Length, inventoryItem.ItemNumber.Length - kitID.Length)
            };

            kit.SelectedYear = kitConfig.SelectedYear;
            kit.SelectedBoreSize = kitConfig.BoreSize;
            kit.SelectedMainBearingSize = kitConfig.MainBearingSize;
            kit.SelectedRodBearingSize = kitConfig.RodBearingSize;
            kit.SelectedThrustWasherSize = kitConfig.ThrustWasherSize;
            kit.OrderMethod = kitConfig.OrderMethod.GetValueOrDefault(' ').ToString();
            kit.GroupingList = kitConfig.GroupingList;
            kit.StandardPrice = kitConfig.StandardPrice;
            kit.ConfiguredPrice = kitConfig.ConfiguredPrice.GetValueOrDefault(0);
            kit.TypicallyConfiguredPrice = kitConfig.TypicallyConfiguredPrice.GetValueOrDefault(0);
            kit.SelectedCrankKitNIPC = kitConfig.CrankKitNIPC.GetValueOrDefault(0);
            kit.SelectedCrankKitPrice = kitConfig.CrankKitPrice.GetValueOrDefault(0);
            kit.SelectedCrankKitCoreNIPC = kitConfig.CrankKitCoreNIPC.GetValueOrDefault(0);
            kit.SelectedCrankKitCorePrice = kitConfig.CrankKitCorePrice.GetValueOrDefault(0);
            kit.MasterKitParts = getConfiguredKitParts(kit, kitConfig.UserID);
            kit.AcesID = kitConfig.AcesID;
            kit.Cylinders = kitConfig.Cylinders;
            PopulateStandardKitData(kit, kitHeaderData, inventoryItem);

            return kit;
        }

        private IEnumerable<KitPart> getConfiguredKitParts(Kit kit, int userId)
        {
            return
            (from kp in getConfiguredKitParts(userId)
             select new KitPart
             {
                 NIPCCode = kp.NIPC,
                 ItemNumber = kp.PartNum,
                 ItemDescription = kp.PartDesc,
                 LineCode = kp.MfrCode,
                 LineDescription = kp.MfrName,
                 JoinQualifier = kp.JoinQualifier.GetValueOrDefault(' ').ToString(),
                 PreJoinQualifier = kp.PreJoinQualifier.GetValueOrDefault(' ').ToString(),
                 GroupingMain = kp.GroupingMain.GetValueOrDefault(0),
                 GroupingOr = kp.GroupingOr.GetValueOrDefault(0),
                 GroupingAnd = kp.GroupingAnd.GetValueOrDefault(0),
                 PartsToGroup = kp.PartsToGroup,
                 PartsToDeselect = kp.PartsToDeselect,
                 PartsToSelect = kp.PartsToSelect,
                 CategoryID = kp.Category.GetValueOrDefault(0),
                 SequenceNumber = kp.SequenceNumber.GetValueOrDefault(0),
                 QuantityRequired = kp.QuantityReq.GetValueOrDefault(0),
                 QuantitySelected = kp.QuantitySel.GetValueOrDefault(0),
                 StartYear = kp.StartYear.GetValueOrDefault(0),
                 EndYear = kp.EndYear.GetValueOrDefault(0),
                 Note = kp.PartNote,
                 Selected = kp.InterchangeMethod.GetValueOrDefault(' ') == 'K' ? true : kp.Selected,
                 SizeCode = kp.SizeCode,
                 Price = kp.Price.GetValueOrDefault(0),
                 PricingPercentage = kp.PricingPercentage.GetValueOrDefault(0),
                 OrderMethod = kp.OrderMethod.GetValueOrDefault(' ').ToString(),
                 ShipWarehouse = kp.ShipWarehouse,
                 InterchangeMethod = kp.InterchangeMethod.GetValueOrDefault(' ').ToString(),
                 OriginalPartUniqueID =
                     kp.InterchangeMethod.GetValueOrDefault(' ') != 'S' ? kp.OriginalPartUniqueID : null,
                 Kit = kit
             }).ToList();
        }

        private IEnumerable<usp_GetKitConfigurationPartsResult> getConfiguredKitParts(int userId)
        {
            return from kp in Db.usp_GetKitConfigurationParts(userId)
                   select kp;
        }

        private void PopulateStandardKitData(Kit kit, usp_GetKitCatalogHeaderDataResult kitHeaderData,
            InventoryItem inventoryItem)
        {
            if (kitHeaderData != null)
                kit.PopulateHeaderData(kitHeaderData);
            kit.PopulateInventoryItemData(inventoryItem);
            kit.RelatedKitParts = getRelatedKitParts(kit);
            kit.CategoryNotes = getCategoryNotes(kit);

            // add sizes if we have a year
            if (kit.SelectedYear > 0)
            {
                kit.BoreSizes = getCategorySizes(kit.NIPCCode, kit.SelectedYear, "bore");
                kit.RodBearingSizes = getCategorySizes(kit.NIPCCode, kit.SelectedYear, "rods");
                kit.MainBearingSizes = getCategorySizes(kit.NIPCCode, kit.SelectedYear, "mains");
                kit.ThrustWasherSizes = getCategorySizes(kit.NIPCCode, kit.SelectedYear, "twash");
            }
        }

        private IEnumerable<KitCategoryNote> getCategoryNotes(Kit kit)
        {
            var categoryNotes = new List<KitCategoryNote>();

            var notesResult = Db.usp_GetKitCatalogMasterKitPartNotes(kit.KitIdentifier.KitID, 0);

            foreach (var note in notesResult)
                for (var i = 1; i <= 4; i++)
                {
                    var type = note.GetType();
                    var noteText = (string)type.GetProperty("KZNT" + i).GetValue(note, null);
                    if (!string.IsNullOrEmpty(noteText))
                        categoryNotes.Add(new KitCategoryNote
                        {
                            CategoryCode = note.CATGPC.GetValueOrDefault(0),
                            Note = noteText
                        });
                }

            return categoryNotes;
        }

        private IEnumerable<KitPart> getRelatedKitParts(Kit kit)
        {
            var kitPartsResult = Db.usp_GetKitCatalogRelatedParts_YearFiltered(kit.KitIdentifier.KitID, kit.SelectedYear);

            var kitParts = from kp in kitPartsResult
                           select new KitPart
                           {
                               Kit = kit,
                               NIPCCode = kp.NIPC.GetValueOrDefault(0),
                               ItemNumber = kp.PartNum.TrimAndConvertNulls(),
                               ItemDescription = kp.PartDesc.TrimAndConvertNulls(),
                               LineCode = kp.MfrCode.TrimAndConvertNulls(),
                               LineDescription = kp.MfrName.TrimAndConvertNulls(),
                               JoinQualifier = string.Empty,
                               PreJoinQualifier = string.Empty,
                               CategoryID = kp.Category,
                               LineType = kp.LineType,
                               SequenceNumber = kp.SequenceNumber,
                               QuantityRequired = kp.QuantityReq,
                               QuantitySelected = kp.QuantityReq,
                               // for all standard kit parts, quantity can't deviate from the required values
                               StartYear = kp.StartYear.GetValueOrDefault(Convert.ToInt16(kit.StartYear)),
                               EndYear = kp.EndYear.GetValueOrDefault(Convert.ToInt16(kit.EndYear)),
                               Note = kp.PartNote.TrimAndConvertNulls(),
                               IsMasterKitPart = false
                           };

            return kitParts.ToList();
        }

        private IEnumerable<KitPart> getMasterKitParts(Kit kit)
        {
            return getMasterKitParts(kit, false);
        }

        private IEnumerable<KitPart> getMasterKitParts(Kit kit, bool aces)
        {
            IEnumerable<usp_GetKitCatalogMasterKitPartsResult> kitPartsResult;

            if (aces)
            {
                var acesKitParts = Db.usp_GetKitCatalogAcesMasterKitParts(kit.KitIdentifier.AcesKitIdentifier,
                    string.Empty, kit.SelectedYear, 0);

                kitPartsResult = from kp in acesKitParts
                                 select new usp_GetKitCatalogMasterKitPartsResult
                                 {
                                     Category = kp.Category,
                                     EndYear = kp.EndYear.GetValueOrDefault(Convert.ToInt16(kit.EndYear)),
                                     GroupingAnd = kp.GroupingAnd,
                                     GroupingOr = kp.GroupingOr,
                                     GroupingMain = kp.GroupingMain,
                                     JoinQualifier = kp.JoinQualifier,
                                     MfrCode = kp.MfrCode,
                                     MfrName = kp.MfrName,
                                     NIPC = kp.NIPC,
                                     PartDesc = kp.PartDesc,
                                     PartNote = kp.PartNote,
                                     PartNum = kp.PartNum,
                                     PartsToDeselect = kp.PartsToDeselect,
                                     PartsToGroup = kp.PartsToGroup,
                                     PartsToSelect = kp.PartsToSelect,
                                     PricingPercentage = kp.PricingPercentage,
                                     QuantityReq = kp.QuantityReq,
                                     SequenceNumber = kp.SequenceNumber,
                                     StartYear = kp.StartYear.GetValueOrDefault(Convert.ToInt16(kit.StartYear)),
                                     PreJoinQualifier = kp.PreJoinQualifier
                                 };
            }
            else
            {
                kitPartsResult = Db.usp_GetKitCatalogMasterKitParts(kit.KitIdentifier.KitID, kit.KitIdentifier.KitType,
                    kit.SelectedYear, 0);
            }

            var kitParts = from kp in kitPartsResult
                           select new KitPart
                           {
                               Kit = kit,
                               NIPCCode = kp.NIPC,
                               ItemNumber = kp.PartNum.TrimAndConvertNulls(),
                               ItemDescription = kp.PartDesc.TrimAndConvertNulls(),
                               LineCode = kp.MfrCode.TrimAndConvertNulls(),
                               LineDescription = kp.MfrName.TrimAndConvertNulls(),
                               PricingPercentage = kp.PricingPercentage.GetValueOrDefault(0),
                               JoinQualifier = kp.JoinQualifier.ToString(),
                               PreJoinQualifier = kp.PreJoinQualifier.ToString(),
                               GroupingMain = kp.GroupingMain,
                               GroupingOr = kp.GroupingOr,
                               GroupingAnd = kp.GroupingAnd,
                               PartsToGroup = kp.PartsToGroup.TrimAndConvertNulls(),
                               PartsToDeselect = kp.PartsToDeselect.TrimAndConvertNulls(),
                               PartsToSelect = kp.PartsToSelect.TrimAndConvertNulls(),
                               CategoryID = kp.Category.GetValueOrDefault(0),
                               SequenceNumber = kp.SequenceNumber,
                               QuantityRequired = kp.QuantityReq,
                               QuantitySelected = kp.QuantityReq,
                               // for all standard kit parts, quantity can't deviate from the required values
                               StartYear = kp.StartYear.GetValueOrDefault(0),
                               EndYear = kp.EndYear.GetValueOrDefault(0),
                               Note = kp.PartNote.TrimAndConvertNulls(),
                               OrderMethod = "M",
                               InterchangeMethod = "S", // default all parts to "Standard" selection
                               Selected = string.IsNullOrEmpty(kp.PartsToDeselect),
                               IsMasterKitPart = true
                           };
            return kitParts.ToList();
        }

        private IEnumerable<string> getCategorySizes(int nipcCode, int year, string category)
        {
            return (from size in Db.GetAvailableKitGroupSizes(category, nipcCode, year)
                    select size.ISIZE.Trim()).ToList();
        }

        public void InterchangePart(Kit kit, string originalPartUniqueIdentifier, string interchangeNIPC,
            ICustomerData customerData, string warehouse, int interchangedQuantity, OrderMethod partOrderMethod,
            string interchangeMethod, bool confirmingAvailability)
        {
            InterchangePart(kit, originalPartUniqueIdentifier, interchangeNIPC, customerData, warehouse,
                interchangedQuantity, partOrderMethod, interchangeMethod, confirmingAvailability, null);
        }

        public void InterchangePart(Kit kit, string originalPartUniqueIdentifier, string interchangeNIPC,
            ICustomerData customerData, string warehouse, int interchangedQuantity, OrderMethod partOrderMethod,
            string interchangeMethod, bool confirmingAvailability, decimal? interchangePrice)
        {
            var originalPart =
                kit.MasterKitParts.Where(kp => kp.UniqueKitPartIdentifier == originalPartUniqueIdentifier)
                    .SingleOrDefault();

            if (originalPart.NIPCCode == int.Parse(interchangeNIPC))
            {
                // same part, so simply update the shipping location
                updateShipmentWarehouse(kit, interchangeNIPC, originalPart.SequenceNumber, warehouse, partOrderMethod,
                    customerData);
                return;
            }

            var interchangeStockStatus = StockStatusRepository.GetStockStatusByNipc(int.Parse(interchangeNIPC),
                kit.NIPCCode, originalPart.SizeCode, interchangedQuantity, customerData);

            if (originalPart == null)
                throw new ApplicationException("Part to be interchanged not found in kit.");

            // if the original part has an interchange method other than "S", then it has already been interchanged, so revert the first interchange and interchange the original part
            // with this one
            if (originalPart.InterchangeMethod != "S")
            {
                log.InfoFormat(
                    "Attempting to interchange a part that has already been interchanged. Original Part Identifier: {0}; Original Part NIPC: {1}; Original Part Interchange Method: {2}; New part NIPC: {3}; User: {4}",
                    originalPartUniqueIdentifier, originalPart.NIPCCode, originalPart.InterchangeMethod, interchangeNIPC,
                    customerData.UserName);
                RevertInterchange(kit, originalPart.UniqueKitPartIdentifier, customerData);
                InterchangePart(kit, originalPart.OriginalPartUniqueID, interchangeNIPC, customerData, warehouse,
                    interchangedQuantity, partOrderMethod, interchangeMethod, confirmingAvailability);
                return;
            }
            // set the interchange method and deselect the part
            originalPart.InterchangeMethod = "X";
            originalPart.Selected = false;
            //TODO: Capture original part price? - doesn't appear to be needed

            var inventoryItemRepository = new InventoryItemRepository();
            var interchangeItem = inventoryItemRepository.GetInventoryItemByNipc(int.Parse(interchangeNIPC));

            var interchangePart = new KitPart
            {
                NIPCCode = interchangeItem.NIPCCode,
                ItemNumber = interchangeItem.ItemNumber,
                ItemDescription = interchangeItem.ItemDescription,
                LineCode = interchangeItem.LineCode,
                LineDescription = interchangeItem.LineDescription,
                CategoryID = interchangeItem.Category,
                SizeCode = originalPart.SizeCode,
                SequenceNumber = originalPart.SequenceNumber,
                JoinQualifier = originalPart.JoinQualifier,
                PreJoinQualifier = originalPart.PreJoinQualifier,
                GroupingMain = originalPart.GroupingMain,
                GroupingOr = originalPart.GroupingOr,
                GroupingAnd = originalPart.GroupingAnd,
                PartsToGroup = originalPart.PartsToGroup,
                PartsToDeselect = originalPart.PartsToDeselect,
                PartsToSelect = originalPart.PartsToSelect,
                ShipWarehouse = warehouse.Trim(),
                QuantityRequired = interchangedQuantity > 0 ? interchangedQuantity : originalPart.QuantityRequired,
                QuantitySelected = interchangedQuantity > 0 ? interchangedQuantity : originalPart.QuantitySelected,
                OrderMethod = partOrderMethod.ToCode(),
                Selected = true,
                IsAvailable = true,
                InterchangeMethod = string.IsNullOrEmpty(interchangeMethod) ? "I" : interchangeMethod
                // default to standard interchange if none defined
            };

            // if a price is passed in, use it instead of the other pricing rules
            if (interchangePrice.HasValue)
            {
                interchangePart.Price = interchangePrice.Value;
            }
            else
            {
                // at the final stage, if the interchange is an exact interchange and the price difference is less than 200% of the original part
                // then there is no price change
                if (confirmingAvailability && (interchangeMethod == "E") &&
                    ((interchangeStockStatus.Price[PriceType.Invoice] - originalPart.Price) / originalPart.Price < 2))
                {
                    interchangePart.Price = originalPart.Price;
                }
                else
                {
                    if (interchangeStockStatus.Price[PriceType.KitPrice] > 0)
                        interchangePart.Price = interchangeStockStatus.Price[PriceType.KitPrice];
                    else
                        interchangePart.Price = interchangeStockStatus.Price[PriceType.P3] *
                                                (1 - interchangeStockStatus.DiscountPercent);
                }
            }

            if (originalPart.InterchangeMethod != "I")
                interchangePart.OriginalPartUniqueID = originalPart.UniqueKitPartIdentifier;

            // update the part lists by appending the select part to any list that contained the original part
            foreach (var part in kit.MasterKitParts)
            {
                part.PartsToGroup = appendPart(part.PartsToGroup, originalPart.UniqueKitPartIdentifier,
                    interchangePart.UniqueKitPartIdentifier);
                part.PartsToDeselect = appendPart(part.PartsToDeselect, originalPart.UniqueKitPartIdentifier,
                    interchangePart.UniqueKitPartIdentifier);
                part.PartsToSelect = appendPart(part.PartsToSelect, originalPart.UniqueKitPartIdentifier,
                    interchangePart.UniqueKitPartIdentifier);
            }

            UpdateKitConfiguration(kit, customerData);
            AddKitPart(kit, interchangePart, customerData);
        }

        public void RevertInterchange(Kit kit, string id, ICustomerData customerData)
        {
            var partToRevert = kit.MasterKitParts.Where(kp => kp.UniqueKitPartIdentifier == id).SingleOrDefault();
            var partToRevertIsSelected = partToRevert.Selected;
            if (partToRevert == null)
                throw new ApplicationException("Part to be reverted not found.");

            var originalPartUniqueID = partToRevert.OriginalPartUniqueID;

            // don't remove the part if it was just interchanged as the alternate OR item, simply deselect it
            if (partToRevert.InterchangeMethod == "O")
                partToRevert.Selected = false;

            // loop through the parts and adjust the grouping fields, reselect the original part, and delete the part to revert
            foreach (var part in kit.MasterKitParts)
            {
                if (part.PartsToGroup != null)
                    part.PartsToGroup = part.PartsToGroup.Replace($",{partToRevert.UniqueKitPartIdentifier}",
                        string.Empty);
                if (part.PartsToDeselect != null)
                    part.PartsToDeselect = part.PartsToDeselect.Replace($",{partToRevert.UniqueKitPartIdentifier}",
                        string.Empty);
                if (part.PartsToSelect != null)
                    part.PartsToSelect = part.PartsToSelect.Replace($",{partToRevert.UniqueKitPartIdentifier}",
                        string.Empty);

                // for the originally selected part, reset the interchange flag back to "S"
                if (part.UniqueKitPartIdentifier == originalPartUniqueID)
                {
                    part.Selected = partToRevert.Selected;
                    part.InterchangeMethod = "S";
                }

                if ((part.UniqueKitPartIdentifier == partToRevert.UniqueKitPartIdentifier) &&
                    (partToRevert.InterchangeMethod != "O"))
                    DeleteKitPart(part, customerData);
            }

            UpdateKitConfiguration(kit, customerData);
        }

        public List<Warranty> GetKitWarranties(int kitNipc, decimal kitPrice, ICustomerData customerData,
            int orderItemID)
        {
            var proxy = KitWarrantyProxy.Instance;
            var inventoryItemRepository = new InventoryItemRepository();

            var warranties = proxy.SubmitRequest(kitNipc, kitPrice, customerData);

            foreach (var warranty in warranties)
            {
                warranty.Description = inventoryItemRepository.GetInventoryItemByNipc(warranty.Nipc).ItemDescription;
                warranty.OrderItemID = orderItemID;
            }

            return warranties;
        }

        public void AddKitPartFromStockStatus(Kit kit, StockStatusRequest stockStatus, ICustomerData customerData,
            string warehouse, OrderMethod orderMethod)
        {
            var inventoryItemRepository = new InventoryItemRepository();
            var item = inventoryItemRepository.GetInventoryItemByNipc(stockStatus.NIPCCode);

            var kitPart = new KitPart
            {
                NIPCCode = item.NIPCCode,
                ItemNumber = item.ItemNumber,
                ItemDescription = item.ItemDescription,
                LineCode = item.LineCode,
                LineDescription = item.LineDescription,
                CategoryID = 0,
                SizeCode = stockStatus.SizeCode == "STD" ? null : stockStatus.SizeCode,
                SequenceNumber = 999,
                Price = stockStatus.Price * (1 - stockStatus.DiscountPercent),
                ShipWarehouse = warehouse,
                QuantityRequired = stockStatus.Quantity,
                QuantitySelected = stockStatus.Quantity,
                OrderMethod = orderMethod.ToCode(),
                Selected = true,
                InterchangeMethod = "K" // K = Keyed (added) additional part
            };

            AddKitPart(kit, kitPart, customerData);
        }

        public void AddKitPart(Kit kit, KitPart kitPart, ICustomerData customerData)
        {
            var kitConfig = getKitConfiguration(customerData);
            var kitConfigurationPart = new KitConfigurationPart();
            updateKitConfigurationPartFromKitPart(kitConfigurationPart, kitPart);
            kitConfig.KitConfigurationParts.Add(kitConfigurationPart);
        }

        public void DeleteKitPart(KitPart kitPart, ICustomerData customerData)
        {
            var kitConfig = getKitConfiguration(customerData);
            kitConfig.KitConfigurationParts.Remove(
                kitConfig.KitConfigurationParts.Single(
                    kp => (kitPart.NIPCCode == kp.PartNIPC) && (kitPart.SequenceNumber == kp.SequenceNumber)));
        }

        public bool SaveAcesKit(Kit kit, string notes, string kitNumber, ICustomerData customerData)
        {
            var kitAcesHeaderData = Db.usp_GetKitCatalogAcesHeaderData(kit.AcesID, 0).SingleOrDefault();

            // get the parts that the user selected
            var selectedParts = (from kp in kit.MasterKitParts
                                 where
                                 (kp.Selected && (kp.SequenceNumber < 900)) ||
                                 // crank kits are always selected in Aces kit builder, so exclude them here
                                 ((kp.NIPCCode == kit.SelectedCrankKitNIPC) && (kp.SequenceNumber >= 900))
                                 // handle crank kits separately
                                 select kp).ToList();

            var proxy = AcesProxy.Instance;

            return proxy.SubmitRequest(kit.AcesID, kitNumber, kitAcesHeaderData?.KBNI.GetValueOrDefault(0) ?? 0,
                selectedParts, notes, customerData.UserName);
        }

        public void ReloadSnapshot(ICustomerData customerData)
        {
            Db.usp_ReloadKitConfigurationSnapshot(customerData.UserID);
        }


        public void LoadKitFromXml(string kitXml, ICustomerData customerData)
        {
            DeleteConfiguredKit(customerData);
            Save();

            var serializer = new XmlSerializer(typeof(KitConfiguration));

            var reader = new StringReader(kitXml);

            var kitConfiguration = (KitConfiguration)serializer.Deserialize(reader);

            // need to override user id now that users can share quotes
            kitConfiguration.UserID = customerData.UserID;

            //FB597 test: change how drop-shipped items are handled when reloading a kit
            foreach (
                var part in
                kitConfiguration.KitConfigurationParts.Where(kp => kp.OrderMethod.GetValueOrDefault(' ') != 'M'))
            {
                part.OrderMethod = 'M';
                part.ShipWarehouse = null;
            }

            Db.KitConfigurations.InsertOnSubmit(kitConfiguration);
        }

        private void saveKitConfigurationSnapshot(int userID)
        {
            Db.usp_SaveKitConfigurationSnapshot(userID);
        }

        private string appendPart(string partList, string oldPart, string newPart)
        {
            if (!string.IsNullOrEmpty(partList))
                if (partList.Split(',').Contains(oldPart))
                    partList += $",{newPart}";
            return partList;
        }

        private KitConfiguration getKitConfiguration(ICustomerData customerData)
        {
            return (from kc in Db.KitConfigurations
                    where kc.UserID == customerData.UserID
                    select kc).SingleOrDefault();
        }

        private usp_GetKitCatalogHeaderDataResult getAcesKitHeader(string id)
        {
            // get the kit header data by passing in the KitID for the generic kit (without the type)
            var kitAcesHeaderData = Db.usp_GetKitCatalogAcesHeaderData(id, 0).SingleOrDefault();

            if (kitAcesHeaderData == null)
                return null;

            var kitHeaderData = new usp_GetKitCatalogHeaderDataResult
            {
                KEYY = Convert.ToInt16(kitAcesHeaderData.KEYY),
                KSYY = Convert.ToInt16(kitAcesHeaderData.KSYY),
                KXEG = kitAcesHeaderData.KXEG,
                KZAP1 = kitAcesHeaderData.KZAP1,
                KZAP2 = kitAcesHeaderData.KZAP2,
                KZAP3 = kitAcesHeaderData.KZAP3,
                KZAP4 = kitAcesHeaderData.KZAP4,
                KZAP5 = kitAcesHeaderData.KZAP5,
                KZAP6 = kitAcesHeaderData.KZAP6,
                KZMD = kitAcesHeaderData.KZMD,
                KZRD = kitAcesHeaderData.KZRD,
                OBORE = kitAcesHeaderData.OBORE,
                OCYL = kitAcesHeaderData.OCYL,
                OSTRK = kitAcesHeaderData.OSTRK
            };

            return kitHeaderData;
        }
    }
}