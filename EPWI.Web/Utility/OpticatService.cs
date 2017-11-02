using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using EPWI.Components.Models;
using EPWI.Web.Opticat;

namespace EPWI.Web.Utility
{
    public class OpticatService
    {
        public const int CustomSubcategoryStartingId = 40000;
        private readonly OpticatEpcServiceClient _opticat = new OpticatEpcServiceClient();

        public object GetYears()
        {
            return _opticat.GetYearsExt(null, null);
        }

        public AcesMake[] GetMakes(int year)
        {
            return _opticat.GetMakesExt(null, new OpticatLookupFilter {YearID = year});
        }

        public AcesModel[] GetModels(int year, int? makeId)
        {
            return makeId.HasValue ? _opticat.GetModelsExt(null,
                new OpticatLookupFilter
                {
                    YearID = year,
                    MakeID = makeId.Value
                }) : null;
        }

        public int? GetBaseVehicleId(int year, int? makeId, int? modelId)
        {
            if (makeId.HasValue && modelId.HasValue)
            {
                var baseVehicle = _opticat.GetBaseVehicleExt(null,
                    new OpticatLookupFilter
                    {
                        YearID = year,
                        MakeID = makeId.Value,
                        ModelID = modelId.Value
                    });
                return baseVehicle != null ? baseVehicle.ID : (int?) null;
            }
            return null;
        }

        public AcesCategory[] GetCategories(int? baseVehicleId, int submodelId, int engineConfigId)
        {
            return _opticat.GetCategoriesExt(null,
                new OpticatLookupFilter
                {
                    BaseVehicleID = baseVehicleId.GetValueOrDefault(0),
                    SubmodelID = submodelId,
                    SelectedEngineConfigID = engineConfigId
                });
        }

        public EpwiAcesSubCategory[] GetSubCategories(int? categoryId, int baseVehicleId, int submodelId, int engineConfigId)
        {
            if (!categoryId.HasValue)
            {
                return null;
            }

            var subcategories = _opticat.GetSubCategoriesExt(null,
                new OpticatLookupFilter
                {
                    CategoryID = categoryId.Value,
                    BaseVehicleID = baseVehicleId,
                    SubmodelID = submodelId,
                    SelectedEngineConfigID = engineConfigId
                }).Select(x => new EpwiAcesSubCategory {ID = x.ID, Name = x.Name}).ToList();

            AddEpwiSubcategories(subcategories);

            return subcategories.ToArray();
        }

        private void AddEpwiSubcategories(List<EpwiAcesSubCategory> subcategories)
        {
            IEnumerable<EpwiAcesSubCategory> epwiSubcategories;
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EPWIConnectionString"].ConnectionString))
            {
                epwiSubcategories = connection.Query<EpwiAcesSubCategory>("SELECT DISTINCT lcs.LookupCustomSubcategoryID AS ID, Name FROM LookupCustomSubcategory lcs INNER JOIN LookupSubcategoryMap lsm ON lcs.LookupCustomSubcategoryID = lsm.LookupCustomSubcategoryID WHERE SubcategoryID in @subcategories ORDER BY lcs.Name", new {subcategories = subcategories.Select(x => x.ID)});
            }

            subcategories.InsertRange(0, epwiSubcategories);
        }

        public object[] GetProductLines(int categoryId, int subcategoryId, int baseVehicleId, int submodelId, int engineConfigId)
        {
            var partTypes = MapSubcategoryToPartTypes(subcategoryId).ToArray();

            if (partTypes.Length > 0)
            {
                return _opticat.EPWI_GetProductLinesContainingPartTypeInfoExt(null,
                    new OpticatLookupFilter
                    {
                        CategoryID = categoryId,
                        BaseVehicleID = baseVehicleId,
                        SubmodelID = submodelId,
                        SelectedEngineConfigID = engineConfigId,
                        PartTypeIDList = partTypes.ToArray()
                    }).Select(x => new {x.ProductLineName, ID = x.ProductLineID}).Distinct().ToArray();
            }

            return _opticat.EPWI_GetProductLinesContainingSubCategoryInfoExt(null,
                new OpticatLookupFilter
                {
                    CategoryID = categoryId,
                    BaseVehicleID = baseVehicleId,
                    SubmodelID = submodelId,
                    SelectedEngineConfigID = engineConfigId,
                    SubCategoryID = subcategoryId
                }).Select(x => new {x.ProductLineName, ID = x.ProductLineID}).Distinct().ToArray();
        }

        public AcesPartType[] GetPartTypes(int categoryId, int? subcategoryId, int baseVehicleId, int submodelId, int engineConfigId)
        {
            if (!subcategoryId.HasValue)
            {
                return null;
            }

            var subcategories = MapSubcategoryToSubcategories(subcategoryId.Value);
            var partTypes = MapSubcategoryToPartTypes(subcategoryId.Value);

            return _opticat.EPWI_GetPartTypesExt(null,
                new OpticatLookupFilter
                {
                    BaseVehicleID = baseVehicleId,
                    CategoryID = categoryId,
                    //SubCategoryID = subcategoryId.Value,
                    SubmodelID = submodelId,
                    SelectedEngineConfigID = engineConfigId,
                    SubCategoryIDList = subcategories.ToArray(),
                    PartTypeIDList = partTypes.ToArray()
                });
        }

        public EpcPositionQualifiers GetPositionQualifiers(int baseVehicleId, int categoryId, int subcategoryId, int? partTypeId, int submodelId, int engineConfigId)
        {
            IEnumerable<int> partTypeIds;
            if (partTypeId.GetValueOrDefault() > 0)
            {
                partTypeIds = new[] {partTypeId.Value};
            }
            else if (partTypeId.GetValueOrDefault() == 0 && subcategoryId < CustomSubcategoryStartingId)
            {
                partTypeIds = GetPartTypes(categoryId, subcategoryId, baseVehicleId, submodelId, engineConfigId).Select(x => x.ID).ToArray();
            }
            else
            {
                partTypeIds = MapSubcategoryToPartTypes(subcategoryId);
            }

            var positionQualifiers = _opticat.EPWI_GetPositionsExt(null,
                new OpticatLookupFilter
                {
                    BaseVehicleID = baseVehicleId,
                    SubmodelID = submodelId,
                    SelectedEngineConfigID = engineConfigId,
                    PartTypeIDList = partTypeIds.ToArray()
                });

            return positionQualifiers;
        }

        public AcesSubModel[] GetSubmodels(int baseVehicleId)
        {
            var submodels = _opticat.GetSubmodelsExt(null,
                new OpticatLookupFilter
                {
                    BaseVehicleID = baseVehicleId
                });

            return submodels;
        }

        public AcesEngineConfig[] GetEngineConfigurations(int baseVehicleId, int submodelId)
        {
            var engineConfigurations = _opticat.GetEnginesByOpticatLookupExt(null,
                new OpticatLookupFilter
                {
                    BaseVehicleID = baseVehicleId,
                    SubmodelID = submodelId
                });

            return engineConfigurations;
        }

        public object GetPartApps(int baseVehicleId, int productLineId, int? categoryId, int? subcategoryId, int? partTypeId, short? positionId, int? submodelId, int selectedEngineConfigId, int startItem, int pageSize)
        {
            // opticat uses a 0-based start item
            startItem--;

            ProductInformationContainer productInfoRange;
            EpcProductExt[] parts;
            int partCount;

            if (productLineId > 0)
            {
                var partTypeIds = MapSubcategoryToPartTypes(subcategoryId.GetValueOrDefault(0));
                var subcategories = MapSubcategoryToSubcategories(subcategoryId.GetValueOrDefault(0));
                var isEpwiSubcategory = subcategoryId.GetValueOrDefault(0) >= CustomSubcategoryStartingId;

                if (isEpwiSubcategory)
                {
                    // we're using a EPWI subcategory, so get all the parts that we can
                    pageSize = 1000;
                }

                productInfoRange = _opticat.EPWI_GetVehicleLookupExt(null,
                    new OpticatLookupFilter
                    {
                        BaseVehicleID = baseVehicleId,
                        SubmodelID = submodelId.GetValueOrDefault(0),
                        IncludeProductLineIDs = new[] {productLineId},
                        CategoryID = categoryId.GetValueOrDefault(0),
                        SubCategoryIDList = subcategories.ToArray(),
                        SelectedEngineConfigID = selectedEngineConfigId,
                        LookupStart = startItem,
                        LookupRange = pageSize
                    });
                parts = productInfoRange.AcesProducts.Where(p => !partTypeIds.Any() || partTypeIds.Contains(p.PartTypeID)).ToArray(); // this breaks the paging. Maybe get all matching parts if using an EPWI subcategory?
                if (isEpwiSubcategory)
                {
                    partCount = parts.Length;
                }
                else
                {
                    partCount = productInfoRange.TotalCount;
                }
            }
            else
            {
                IEnumerable<int> partTypeIds;
                if (partTypeId.GetValueOrDefault() > 0)
                {
                    partTypeIds = new[] {partTypeId.GetValueOrDefault(0)};
                }
                else if (partTypeId.GetValueOrDefault() == 0 && subcategoryId.GetValueOrDefault() < CustomSubcategoryStartingId)
                {
                    partTypeIds = GetPartTypes(categoryId.GetValueOrDefault(0), subcategoryId, baseVehicleId, submodelId.GetValueOrDefault(0), selectedEngineConfigId).Select(x => x.ID).ToArray();
                }
                else
                {
                    partTypeIds = MapSubcategoryToPartTypes(subcategoryId.GetValueOrDefault(0));
                }

                productInfoRange = _opticat.EPWI_GetProductInfoRangeExt(null,
                    new OpticatLookupFilter
                    {
                        BaseVehicleID = baseVehicleId,
                        PositionID = positionId.GetValueOrDefault(0),
                        SelectedEngineConfigID = selectedEngineConfigId,
                        SubmodelID = submodelId.GetValueOrDefault(0),
                        PartTypeIDList = partTypeIds.ToArray(),
                        LookupStart = startItem,
                        LookupRange = pageSize
                    });
                parts = productInfoRange.AcesProducts;
                partCount = productInfoRange.TotalCount;
            }

            var results = parts.Select(MapEpcProductExtToPart);

            return new {totalCount = partCount, partList = results};
        }

        public static PartInfo MapEpcProductExtToPart(EpcProductExt part)
        {
            var result = new PartInfo
            {
                ProductID = part.ProductID,
                PartNumber = part.PartNumber,
                AAIABrandID = part.AAIABrandID,
                ProductLineMfrCode = part.ProductLineMfrCode,
                Notes = part.Notes,
                ProductLineName = part.ProductLineName,
                Description = part.Description,
                PositionDescription = part.PositionDescription,
                PartTypeDescription = part.PartTypeDescription,
                Quantity = part.PerCarQty,
                EngineDescriptions = part.EngineDescriptions == null ? new string[0] : part.EngineDescriptions.Distinct().ToArray(),
                // hack. for now include only valid image extensions
                ImageUrls = part.ImageURI == null
                    ? new string[0]
                    : part.ImageURI.Where(y =>
                        y.EndsWith("jpg", StringComparison.InvariantCultureIgnoreCase) ||
                        y.EndsWith("gif", StringComparison.InvariantCultureIgnoreCase) ||
                        y.EndsWith("png", StringComparison.InvariantCultureIgnoreCase)),
                DownloadUrls = part.ImageURI == null ? new string[0] : part.ImageURI.Where(y =>
                    !string.IsNullOrEmpty(y) &&
                    !y.EndsWith("jpg", StringComparison.InvariantCultureIgnoreCase) &&
                    !y.EndsWith("gif", StringComparison.InvariantCultureIgnoreCase) &&
                    !y.EndsWith("png", StringComparison.InvariantCultureIgnoreCase))
            };

            return result;
        }

        public object GetProductDetails(int productId)
        {
            var details = _opticat.GetProductMetaInformationExt(null, productId).First();
            return details;
        }

        public EpcProductExt GetProductInfoByUpc(string upc)
        {
            EpcProductExt product = null;

            if (upc != "0")
            {
                var details = _opticat.GetProductInfoBySKUExt(null, upc);
                product = details.AcesProducts.FirstOrDefault();
            }

            return product != null && product.ProductID != 0 ? product : null;
        }

        public EpcProductExt GetProductInfoByManufacturerCodeAndPartNumber(string mfrCode, string partNumber)
        {
            var details = _opticat.GetProductInfoByBrandAAIAExt(null, mfrCode, partNumber).AcesProducts;

            if (details.FirstOrDefault() == null || details.FirstOrDefault().ProductID == 0)
            {
                return null;
            }

            return details.First();
        }

        public IEnumerable<BuyersGuideEntry> GetBuyersGuide(int productId)
        {
            var results = _opticat.GetBuyersGuideByProductIDExt(null, productId, 0);

            return results;
        }

        private IEnumerable<int> MapSubcategoryToSubcategories(int subcategoryId)
        {
            if (subcategoryId >= CustomSubcategoryStartingId)
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EPWIConnectionString"].ConnectionString))
                {
                    return connection.Query<int>("SELECT SubcategoryID FROM LookupSubcategoryMap WHERE LookupCustomSubcategoryID = @subcategoryId", new {subcategoryId});
                }
            }

            return new List<int> {subcategoryId};
        }

        public IEnumerable<int> MapSubcategoryToPartTypes(int subcategoryId)
        {
            if (subcategoryId >= CustomSubcategoryStartingId)
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EPWIConnectionString"].ConnectionString))
                {
                    return connection.Query<int>("SELECT PartTypeID FROM LookupPartTypeMap WHERE LookupCustomSubcategoryID = @subcategoryId", new {subcategoryId});
                }
            }
            return Enumerable.Empty<int>();
        }
    }

    public class PartInfo
    {
        public int ProductID { get; set; }
        public string PartNumber { get; set; }
        public string AAIABrandID { get; set; }
        public string ProductLineMfrCode { get; set; }
        public string Notes { get; set; }
        public string ProductLineName { get; set; }
        public string PartTypeDescription { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string[] EngineDescriptions { get; set; }
        public IEnumerable<string> ImageUrls { get; set; }
        public IEnumerable<string> DownloadUrls { get; set; }
        public string PositionDescription { get; set; }
    }
}