using System.Web.Mvc;
using EPWI.Components.Models;
using EPWI.Web.Opticat;
using EPWI.Web.Utility;
using log4net;

namespace EPWI.Web.Controllers
{
    [Authorize]
    public class LookupController : LoggingController
    {
        private static readonly ILog Log = LogManager.GetLogger("LookupController");
        private readonly OpticatService _opticatService;

        public LookupController()
        {
            _opticatService = new OpticatService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Years()
        {
            var years = _opticatService.GetYears();
            return Json(years, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Makes(int year)
        {
            var makes = _opticatService.GetMakes(year);
            return Json(makes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Models(int year, int makeId)
        {
            var models = _opticatService.GetModels(year, makeId);
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Submodels(int year, int makeId, int modelId)
        {
            var baseVehicleId = _opticatService.GetBaseVehicleId(year, makeId, modelId);
            var submodels = _opticatService.GetSubmodels(baseVehicleId.GetValueOrDefault(0));
            return Json(new {baseVehicleId, submodels}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Engines(int baseVehicleId, int? submodelId)
        {
            var engines = _opticatService.GetEngineConfigurations(baseVehicleId, submodelId.GetValueOrDefault(0));
            return Json(engines, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Categories(int baseVehicleId, int? submodelId, int? engineConfigId)
        {
            var categories = _opticatService.GetCategories(baseVehicleId, submodelId.GetValueOrDefault(0), engineConfigId.GetValueOrDefault(0));
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Subcategories(int categoryId, int baseVehicleId, int? submodelId, int? engineConfigId)
        {
            var subcategories = _opticatService.GetSubCategories(categoryId, baseVehicleId, submodelId.GetValueOrDefault(0), engineConfigId.GetValueOrDefault(0));
            return Json(subcategories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductLines(int categoryId, int subcategoryId, int baseVehicleId, int? submodelId, int? engineConfigId)
        {
            var productLines = _opticatService.GetProductLines(categoryId, subcategoryId, baseVehicleId, submodelId.GetValueOrDefault(0), engineConfigId.GetValueOrDefault(0));
            return Json(productLines, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PartTypes(int categoryId, int subcategoryId, int baseVehicleId, int? submodelId, int? engineConfigId)
        {
            var partTypes = _opticatService.GetPartTypes(categoryId, subcategoryId, baseVehicleId, submodelId.GetValueOrDefault(0), engineConfigId.GetValueOrDefault(0));
            return Json(partTypes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PositionQualifiers(int baseVehicleId, int partTypeId, int categoryId, int subcategoryId, int? submodelId, int? engineConfigId)
        {
            var positionQualifierResult = _opticatService.GetPositionQualifiers(baseVehicleId, categoryId, subcategoryId, partTypeId, submodelId.GetValueOrDefault(0), engineConfigId.GetValueOrDefault(0));
            return Json(positionQualifierResult, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Parts(int baseVehicleId, int productLineId, int? categoryId, int? subcategoryId, int? partTypeId, short? positionId, int? submodelId, int? engineConfigId, int startItem, int pageSize)
        {
            var partApps = _opticatService.GetPartApps(baseVehicleId, productLineId, categoryId, subcategoryId, partTypeId, positionId, submodelId, engineConfigId.GetValueOrDefault(0), startItem, pageSize);
            return Json(partApps, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RestoreSearch(int selectedYear, int? selectedMake, int? selectedModel, int? selectedCategory, int? selectedSubcategory, int? selectedProductLine,
            int? selectedPartType, int? selectedSubmodel, short? selectedPosition, int? selectedEngineConfig)
        {
            try
            {
                // given the selections passed in, go get all of the available values for each of the dropdowns so we can repopulate them on the client
                var makes = _opticatService.GetMakes(selectedYear);
                var models = _opticatService.GetModels(selectedYear, selectedMake);
                var baseVehicleId = _opticatService.GetBaseVehicleId(selectedYear, selectedMake, selectedModel);
                AcesCategory[] categories = null;
                EpwiAcesSubCategory[] subcategories = null;
                object[] productLines = null;
                AcesPartType[] partTypes = null;
                EpcPositionQualifiers positionQualifiers = null;
                AcesSubModel[] submodels = null;
                AcesEngineConfig[] engineConfigurations = null;

                if (baseVehicleId.HasValue)
                {
                    // the following are only populated if there is a base vehicle id
                    submodels = _opticatService.GetSubmodels(baseVehicleId.Value);
                    engineConfigurations = _opticatService.GetEngineConfigurations(baseVehicleId.Value, selectedSubmodel.GetValueOrDefault(0));

                    if (selectedEngineConfig.HasValue)
                    {
                        categories = _opticatService.GetCategories(baseVehicleId, selectedSubmodel.GetValueOrDefault(0), selectedEngineConfig.Value);
                    }

                    if (selectedCategory.HasValue)
                    {
                        subcategories = _opticatService.GetSubCategories(selectedCategory, baseVehicleId.Value, selectedSubmodel.GetValueOrDefault(0), selectedEngineConfig.GetValueOrDefault(0));
                    }

                    if (selectedSubcategory.HasValue)
                    {
                        productLines = _opticatService.GetProductLines(selectedCategory.Value, selectedSubcategory.Value, baseVehicleId.Value, selectedSubmodel.GetValueOrDefault(0), selectedEngineConfig.GetValueOrDefault(0));
                    }

                    if (selectedProductLine.HasValue && selectedProductLine == -1)
                    {
                        partTypes = _opticatService.GetPartTypes(selectedCategory.Value, selectedSubcategory, baseVehicleId.Value, selectedSubmodel.GetValueOrDefault(0), selectedEngineConfig.GetValueOrDefault(0));
                    }

                    if (selectedPartType.HasValue || selectedSubcategory.GetValueOrDefault(0) >= OpticatService.CustomSubcategoryStartingId)
                    {
                        positionQualifiers = _opticatService.GetPositionQualifiers(baseVehicleId.Value, selectedCategory.Value, selectedSubcategory.Value, selectedPartType, selectedSubmodel.GetValueOrDefault(0), selectedEngineConfig.GetValueOrDefault(0));
                    }
                }
                return Json(new {makes, models, baseVehicleId, categories, subcategories, productLines, partTypes, positionQualifiers, submodels, engineConfigurations}, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                Log.Error("Error restoring a saved Year/Make/Model search", ex);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetProductDetails(int id)
        {
            return Json(_opticatService.GetProductDetails(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBuyersGuide(int id)
        {
            return Json(_opticatService.GetBuyersGuide(id), JsonRequestBehavior.AllowGet);
        }
    }
}