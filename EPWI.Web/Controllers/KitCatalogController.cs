using System.Web.Mvc;
using EPWI.Components.Models;
using EPWI.Web.Models;

namespace EPWI.Web.Controllers
{
  [Authorize(Roles="CUSTOMER,ADMIN,EMPLOYEE")]
  public class KitCatalogController : LoggingController
  {
    //
    // GET: /KitCatalog/
    public ActionResult Index()
    {
      var rep = new KitCatalogRepository();

      return View(new KitCatalogIndexViewModel { Manufacturers = rep.GetManufacturerList() });
    }

    public ActionResult Make(string id)
    {
      if (string.IsNullOrEmpty(id))
      {
        return Content("Manufacturer ID is missing.");
      }

      var rep = new KitCatalogRepository();

      return View(new KitCatalogListingViewModel { Kits = rep.GetManufacturerKits(id), ManufacturerName = rep.GetManufacturerNameByMake(id) });
    }

  }
}
