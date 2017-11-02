using System;
using System.Linq;
using System.Web.Mvc;
using EPWI.Components.Models;
using EPWI.Components.Proxies;
using EPWI.Web.Models;
using log4net;

namespace EPWI.Web.Controllers
{
    [Authorize]
    public class QuoteController : LoggingController
    {
        private static readonly ILog log = LogManager.GetLogger(nameof(QuoteController));

        public ActionResult Index()
        {
            var rep = new QuoteRepository();
            var quotes = rep.GetQuotesByCustomer(CustomerData, CurrentUserIsEmployee);

            var order = GetCurrentOrder();

            return
                View(new QuotesViewModel
                {
                    Quotes = quotes,
                    OpenOrderExists = order != null,
                    CustomerData = CustomerData,
                    IsEmployee = CurrentUserIsEmployee
                });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "EMPLOYEE")]
        public ActionResult SearchByCustomer(int? customerID, char? companyCode)
        {
            IQueryable<QuoteDetail> quotes = null;
            var order = GetCurrentOrder();

            if (customerID.HasValue && companyCode.HasValue)
            {
                var rep = new QuoteRepository();
                quotes = rep.GetQuotesByCustomer(new User {CustomerID = customerID.Value, CompanyCode = companyCode},
                    true);
            }

            return View("Index",
                new QuotesViewModel
                {
                    Quotes = quotes,
                    OpenOrderExists = order != null,
                    CustomerData = CustomerData,
                    IsEmployee = CurrentUserIsEmployee
                });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "EMPLOYEE")]
        public ActionResult SearchByQuoteID(int? quoteID)
        {
            if (quoteID.HasValue)
                return RedirectToAction("Load", new {id = quoteID});
            return View("Index");
        }

        public ActionResult Load(int id)
        {
            //load quote needs admin login to db
            var rep = new QuoteRepository(true);
            var quote = rep.GetQuote(id, CustomerData, CurrentUserIsEmployee);

            if (quote == null)
            {
                TempData["message"] = "Quote not found.";
                return RedirectToAction("Index");
            }
            TempData["message"] = "Quote successfully loaded.";

            if (((quote.EPWCompCode != CustomerData.CompanyCode) || (quote.EPWCustID != CustomerData.CustomerID)) &&
                CurrentUserIsEmployee)
            {
                // update the user's customer id to match the quote's customer id
                var userRep = new UserRepository();
                var currentUser = userRep.GetByUserID(CustomerData.UserID);

                currentUser.CompanyCode = quote.EPWCompCode;
                currentUser.CustomerID = quote.EPWCustID;
                userRep.Save();

                TempData["message"] +=
                    $" Your Customer ID has been updated to {quote.EPWCustID} {quote.EPWCompCode} to match the quote's Customer ID.";
            }
            rep.LoadQuote(id, CustomerData);

            TempData["QuoteCreateDate"] = quote.QuoteDate;
            return RedirectToAction("Index", "Order");
        }

        public ActionResult Delete(int id)
        {
            var rep = new QuoteRepository();
            rep.DeleteQuote(id, CustomerData);
            rep.Save();

            TempData["message"] = "Quote deleted.";
            return RedirectToAction("Index");
        }

        public ActionResult SaveOrderAsQuote(string quoteDescription, string shareQuote, string hiddenQuoteNotes,
            string hiddenQuotePO, string saveOption, bool? helpRequested)
        {
            var rep = new OrderRepository();
            var quoteID = 0;
            var order = rep.OpenOrder(CustomerData, false);

            if (order != null)
            {
                order.PONumber = hiddenQuotePO;
                order.OrderNotes = hiddenQuoteNotes;
                rep.Save();

                if (saveOption == "overwriteQuote")
                {
                    var quoteRep = new QuoteRepository();
                    quoteRep.UpdateQuoteFromOrder(order);
                    quoteID = order.QuoteID.GetValueOrDefault(0);
                    TempData["message"] = "Quote successfully updated.";
                }
                else
                {
                    var allowUpdate = shareQuote == "Shared";
                    quoteID = rep.SaveOrderAsQuote(CustomerData, order, quoteDescription,
                        shareQuote.StartsWith("Shared"), !allowUpdate);
                    TempData["message"] = "Order successfully saved as quote.";
                }
            }
            else
            {
                TempData["message"] = "Error saving order as quote. The order no longer exists.";
            }

            if (helpRequested.GetValueOrDefault())
            {
                // if requesting help, won't have the option of processing order, so get rid of it
                rep.DeleteOrder(order);
                rep.Save();
                return PartialView("RequestHelp", GetQuoteHelpRequestViewModel(quoteID, string.Empty, true));
                // eventually  might pass warehouse here if power user is implemented
            }
            return PartialView("PostQuoteSaveOptions");

            //return ClientRedirectResult(Url.Action("Search", "StockStatus"));
        }

        public ActionResult QuoteList()
        {
            var rep = new QuoteRepository();

            var quotes = rep.GetQuotesByUser(CustomerData);

            return PartialView(quotes);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RequestHelp(int id)
        {
            return PartialView("RequestHelp", GetQuoteHelpRequestViewModel(id, string.Empty, false));
            // eventually  might pass warehouse here if power user is implemented
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RequestHelp(string csr, string requestHelpMessage, int quoteID, bool quoteSaved)
        {
            string message =
                $"{CustomerData.FirstName} {CustomerData.LastName} ({CustomerData.UserName}) has requested quote help. ";
            message += $"{Url.Action("Load", "Quote", new {id = quoteID}, "http")} {requestHelpMessage} ";

            //Message is limited to 512 characters on the AS/400
            message = message.Substring(0, Math.Min(message.Length, 512));

            var proxy = EmailCustomerServiceRepProxy.Instance;
            if(!proxy.SubmitRequest(CustomerData, quoteID, csr, message))
            {
                log.Warn($"!! Sending an email failed. CustomerID: {CustomerData.CustomerID}, quoteId: {quoteID}, csr {csr}, message {message}.");
            }

            TempData["message"] = "Email sent successfully.";

            if (quoteSaved)
                return ClientRedirectResult(Url.Action("Search", "StockStatus"));
            return ClientRedirectResult(Url.Action("Index", "Quote"));
        }

        private Order GetCurrentOrder()
        {
            var orderRep = new OrderRepository();
            return orderRep.OpenOrder(CustomerData, false);
        }

        private QuoteHelpRequestViewModel GetQuoteHelpRequestViewModel(int quoteID, string warehouse, bool quoteSaved)
        {
            var proxy = CustomerServiceRepProxy.Instance;
            return new QuoteHelpRequestViewModel
            {
                CustomerServiceReps = proxy.SubmitRequest(CustomerData, warehouse),
                QuoteSaved = quoteSaved,
                QuoteID = quoteID
            };
        }
    }
}