using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPWI.Web.Models.N2CMS;
using N2.Web.Mvc;
using N2.Web;


namespace EPWI.Web.Controllers.N2CMS
{
    [Controls(typeof(LinkItemContainer))]
    public class LinkItemContainerController : ContentController<LinkItemContainer>
    {
        private const int PAGE_SIZE = 15;
        //
        // GET: /LinkItemContainer/
        public override ActionResult Index()
        {
            var items = getItems(1, null);

            return View("Index", new ContainerViewModel<LinkItemContainer, LinkItem> { Container = CurrentItem, Items = items });
        }

        public ActionResult List(string id, int? p)
        {
            var items = getItems(p.GetValueOrDefault(1), id);

            return View("Index", new ContainerViewModel<LinkItemContainer, LinkItem> { Container = CurrentItem, Items = items });
        }

        private IEnumerable<LinkItem> getItems(int page, string startsWith)
        {
            IEnumerable<LinkItem> items;

            if (CurrentItem.ShowAlphabetically)
            {
                items = CurrentItem.GetItems().OrderBy(i => i.Title);
            }
            else
            {
                items = CurrentItem.GetItems();
            }

            if (!string.IsNullOrEmpty(startsWith) && startsWith != "ALL")
            {
                items = items.Where(i => i.Title.StartsWith(startsWith, StringComparison.CurrentCultureIgnoreCase));
            }

            var itemCount = items.Count();

            if (CurrentItem.EnablePaging)
            {
                ViewData["TotalPages"] = (int)Math.Ceiling((double)itemCount / PAGE_SIZE);
                ViewData["CurrentPage"] = page;
                ViewData["filter"] = startsWith;
                return items.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            }
            else
            {
                return items;
            }
        }
    }
}
