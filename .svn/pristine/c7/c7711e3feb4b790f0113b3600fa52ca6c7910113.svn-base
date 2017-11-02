using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using EPWI.Components.Models;
using EPWI.Web.Models.N2CMS;
using N2.Web;

namespace EPWI.Web.Controllers.N2CMS
{
    [Controls(typeof(CatalogPage))]
    public class CatalogController : ContentController
    {
        //
        // GET: /Catalog/

        public override ActionResult Index()
        {
            return View("Index", new CatalogViewModel {Back = CurrentItem.Parent, Item = (CatalogPage) CurrentItem});
        }

        public ActionResult GetCatalogItems(int startIndex, int results, string sort, string dir, string searchCriteria)
        {
            // persuade IE not to cache the data
            Response.AddHeader("Cache-Control", "no-store, no-cache, must-revalidate");
            Response.AddHeader("Cache-Control", "post-check=0, pre-check=0");
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -6000;
            var rep = new CatalogRepository();

            var records = from c in rep.GetAll().OrderBy($"{sort} {dir}") select c;

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                records = from c in records
                    where c.ItemName.Contains(searchCriteria) || c.Line.Contains(searchCriteria)
                    select c;
            }

            var count = records.Count();

            var filteredRecords = from c in records.Skip(startIndex).Take(results)
                select new {c.ItemName, c.Line, Url = Url.Content("~" + c.Url), c.IsNew};

            return Json(new {data = filteredRecords, recordCount = count}, JsonRequestBehavior.AllowGet);
        }
    }
}