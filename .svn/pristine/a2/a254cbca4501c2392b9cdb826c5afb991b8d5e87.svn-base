using System.Web.Mvc;
using EPWI.Web.Models.N2CMS;
using N2.Web.Mvc;
using N2.Web;

namespace EPWI.Web.Controllers.N2CMS
{
    [Controls(typeof(ListItemPage))]
    public class ListItemController : ContentController<ListItemPage>
    {
        public override ActionResult Index()
        {
            var vm = new ListItemViewModel
            {
                Back = CurrentItem.Parent,
                Item = CurrentItem
            };

            return View("index", vm);
        }

    }
}
