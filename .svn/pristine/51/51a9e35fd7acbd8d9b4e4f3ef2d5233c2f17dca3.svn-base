using System.Web.Mvc;
using EPWI.Web.Models.N2CMS;
using N2.Web.Mvc;
using N2.Web;

namespace EPWI.Web.Controllers.N2CMS
{
    [Controls(typeof(ListItemContainer))]
    public class ListItemContainerController : ContentController<ListItemContainer>
    {
        public override ActionResult Index()
        {
            return View("Index", new ContainerViewModel<ListItemContainer, ListItemPage> { Container = CurrentItem, Items = CurrentItem.GetItems() });
        }

    }
}
