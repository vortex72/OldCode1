using System;
using System.Web.Mvc;
using EPWI.Components.Models;
using EPWI.Components.Proxies;

namespace EPWI.Web.Controllers
{
    public class MillionthPartController : LoggingController
    {
        //
        // GET: /MillionthPart/

        public ActionResult Index(string userID, int customerID, char companyCode, int orderNumber, int validationCode,
            bool? popup)
        {
            ValidateSubmission(customerID, orderNumber, validationCode);

            var model = new MillionthPartViewModel
            {
                UserID = userID,
                CustomerID = customerID,
                CompanyCode = companyCode,
                OrderNumber = orderNumber,
                ValidationCode = validationCode
            };

            if (popup.GetValueOrDefault(false))
                return PartialView("GuessFormFields", model);
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Submit(MillionthPartViewModel model)
        {
            ValidateSubmission(model.CustomerID, model.OrderNumber, model.ValidationCode);

            var guessDateTime =
                DateTime.Parse(model.GuessDate.Value.ToString("MM/dd/yyyy") + " " + model.GuessHour + ":" +
                               model.GuessMinute + model.GuessAmPm);

            var proxy = MillionthPartProxy.Instance;

            if (
                !proxy.SubmitRequest(guessDateTime, model.UserID, model.CustomerID, model.CompanyCode, model.OrderNumber))
                throw new ApplicationException("Error submitting millionth part contest guess.");

            if (Request.IsAjaxRequest())
                return null;
            return View("GuessSubmitted");
        }

        private void ValidateSubmission(int customerID, int orderNumber, int validationCode)
        {
            if ((customerID*2 + orderNumber - 3)*2 != validationCode)
                throw new ApplicationException("Invalid Request.");
        }
    }
}