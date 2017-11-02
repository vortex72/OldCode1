using System.Linq;
using System.Web.Mvc;
using EPWI.Components.Models;
using EPWI.Web.Models;
using log4net;

namespace EPWI.Web.Controllers
{
    public class HomeController : EpwiController
    {
        private static readonly ILog Log = LogManager.GetLogger("HomeController");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Blank()
        {
            // Used when we need a blank result where we can show temp messages
            return View();
        }

        public ActionResult ComingSoon()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Menu()
        {
            var kitExists = false;
            var quotesExist = false;
            if (User.Identity.IsAuthenticated)
            {
                var rep = new KitRepository();
                var quoteRep = new QuoteRepository();
                kitExists = CustomerData != null && rep.KitExistsForUser(CustomerData);
                quotesExist = CustomerData != null && quoteRep.QuotesExist(CustomerData);
            }

            return View("Menu", new MenuViewModel {CustomerData = CustomerData, KitExists = kitExists, QuotesExist = quotesExist});
        }

        public ActionResult AS400Check()
        {
            try
            {
                var inventoryItemRep = new InventoryItemRepository();
                var item = inventoryItemRep.GetInventoryItems("Q21", null).First();
                var stockStatus = StockStatusRepository.GetStockStatusByNipc(item.NIPCCode, null, 5,
                    new User {CustomerID = 116, CompanyCode = 'N', UserID = 9616});
            }
            catch (System.Exception ex)
            {
                Log.Error("AS/400 Status Check Failed", ex);
                return Content("DOWN");
            }

            return Content("OK");
        }
    }
}