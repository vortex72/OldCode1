using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EPWI.Components.Proxies;

namespace EPWI.Components.Models
{
    public class InventoryItemRepository : Repository
    {
        private List<InventoryItem> _inventoryItems;

        public IEnumerable<InventoryItem> GetInventoryItems(string itemNumber, string lineCode, bool forceCrunch = false)
        {
            if (string.IsNullOrEmpty(itemNumber))
                throw new ArgumentException("itemNumber is required", "itemNumber");

            itemNumber = itemNumber.ToUpper();

            if (!string.IsNullOrEmpty(lineCode))
                lineCode = lineCode.ToUpper();

            if (!string.IsNullOrEmpty(lineCode) && !forceCrunch)
            {
                _inventoryItems = getInventoryItems(itemNumber, lineCode, null, 0).ToList();
            }
            else
            {
                // run the request through the cruch program to check for embedded sizes and quantities
                _inventoryItems = new List<InventoryItem>();
                var crunchProxy = CrunchProxy.Instance;

                var crunchResults = crunchProxy.SubmitRequest(itemNumber);

                if (!string.IsNullOrEmpty(lineCode) && forceCrunch && (crunchResults.Rows.Count > 0))
                {
                    var crunchResultsWithMatchingLineCode =
                        crunchResults.AsEnumerable().Where(row => row.Field<string>("C#LINE").Trim() == lineCode);

                    if (crunchResultsWithMatchingLineCode.Any())
                        crunchResults = crunchResultsWithMatchingLineCode.CopyToDataTable();
                }

                foreach (DataRow row in crunchResults.Rows)
                {
                    var applicableSize = row["C#OS"].ToString().Trim();
                    var applicableQuantity = Math.Max(int.Parse(row["C#QTY"].ToString().Trim()), 0);
                    var results = getInventoryItems(row["C#ITEM"].ToString().Trim(), row["C#LINE"].ToString().Trim(),
                        applicableSize, applicableQuantity);

                    if (results.Count() > 1)
                        throw new ApplicationException(
                            $"More than one NIPC Code was found for Crunch result: {row["C#LINE"].ToString().Trim()}, {row["C#ITEM"].ToString().Trim()}");
                    if (results.Count() == 1)
                        _inventoryItems.Add(results.First());
                }
            }

            if (_inventoryItems.Count == 1)
            {
                // only one inventory item, so get the list of sizes for it from the local database
                var sizes = (from s in Db.KitCatSizeUPCs
                    where s.NIPC == _inventoryItems.First().NIPCCode
                    select s.ISIZE).Distinct().ToList();
                // Always offer the Standard ("STD") size if different sizes exist
                if (sizes.Count > 0)
                    sizes.Insert(0, "STD");
                _inventoryItems.First().Sizes = sizes;
            }

            return _inventoryItems;
        }

        public InventoryItem GetInventoryItemByNipc(int nipcCode)
        {
            //TODO lalalal
            var raw = Db.usp_GetInventoryItem(nipcCode);
            return CreateFromInventoryItemUnprocessed(raw /*, string.Empty*/, string.Empty, 0).Single();
        }

        private IEnumerable<InventoryItem> getInventoryItems(string itemNumber, string lineCode, string applicableSize,
            int applicableQuantity)
        {
            IEnumerable<InventoryItemUnprocessed> results = null;

            if (applicableSize == null)
                applicableSize = string.Empty;

            if (!string.IsNullOrEmpty(lineCode))
                results = Db.usp_LookupNIPCCode(itemNumber, lineCode);
            else
                results = Db.usp_LookupNIPCCode(itemNumber);

            return CreateFromInventoryItemUnprocessed(results /*, itemNumber*/, applicableSize, applicableQuantity);
        }

        private IEnumerable<InventoryItem> CreateFromInventoryItemUnprocessed(
            IEnumerable<InventoryItemUnprocessed> items /*, string itemNumber*/, string applicableSize,
            int applicableQuantity)
        {
            return items
                .Select(inventoryItem => new InventoryItem
                {
                    ItemNumber = inventoryItem.ITEM,
                    NIPCCode = inventoryItem.NIPC,
                    ItemDescription = (inventoryItem.IDESC ?? "").Trim(),
                    LineCode = inventoryItem.LINE,
                    LineDescription = (inventoryItem.LINED ?? "").Trim(),
                    Category = inventoryItem.CATGPC ?? 0,
                    Subcategory = inventoryItem.IMCG2 ?? 0,
                    ApplicableSize = applicableSize.Trim().Length > 0 ? applicableSize.Trim() : string.Empty,
                    ApplicableQuantity = applicableQuantity > 1 ? applicableQuantity : 0,
                    IsYearRequired = !(inventoryItem.IMFL09 == 'N'),
                    RequiresSpecialDisplay = inventoryItem.IMFL12 == 'Y',
                    IsKTRACK =
                        (inventoryItem.LINE.Trim() == "KIT") && inventoryItem.IMCG2.HasValue &&
                        ((inventoryItem.IMCG2 == 15) || (inventoryItem.IMCG2 == 16) || (inventoryItem.IMCG2 == 17) ||
                         (inventoryItem.IMCG2 == 18)),
                    // 15 - Rotating Assembly, 16 - Heavy Duty Kit, 17 - Valve Train Kit, 18 - Valve Kit
                    UnitsPerSellMultiple = inventoryItem.IMUQTY ?? 0
                })
                .ToList();
        }
    }
}