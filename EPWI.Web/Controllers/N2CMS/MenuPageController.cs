using System.Web.Mvc;
using EPWI.Web.Models.N2CMS;
using N2.Web.Mvc;
using N2.Web;

namespace EPWI.Web.Controllers.N2CMS
{
    [Controls(typeof(MenuPage))]
    public class MenuPageController : ContentController<MenuPage>
    {
        public override ActionResult Index()
        {
            return View("Index", CurrentItem);
        }
    }
}
