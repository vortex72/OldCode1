using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using EPWI.Components.Exceptions;
using EPWI.Components.Models;
using EPWI.Components.Proxies;
using EPWI.Components.Utility;
using EPWI.Web.HtmlHelpers;
using EPWI.Web.Models;
using log4net;
using MvcReCaptcha;
using xVal.ServerSide;

namespace EPWI.Web.Controllers
{
    public class AccountController : LoggingController
    {
        private readonly ILog log = LogManager.GetLogger("AccountController");

        [Authorize]
        public ActionResult CustomerInfo()
        {
            return PartialView(CustomerData);
        }

        [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None, VaryByParam = "*")]
        public ActionResult LogOn()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None, VaryByParam = "*")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
             Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userName, string password, bool rememberMe, string returnUrl)
        {
            if (!ValidateLogOn(userName, password))
                return View();

            FormsAuthentication.SetAuthCookie(userName, rememberMe);
            Roles.DeleteCookie();

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None, VaryByParam = "*")]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View(new UserProfile());
        }

        [CaptchaValidator]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(UserProfile profile, bool captchaValid)
        {
            var rep = new UserRepository();
            var user = new User();

            try
            {
                if (!captchaValid)
                {
                    ViewData["SuppressDialog"] = true;
                    ViewData["recaptchaMessage"] = "You did not type the verification word correctly. Please try again.";
                    return View(profile);
                }
                user.CreateDate = DateTime.Now;
                rep.UpdateUserProfile(user, profile);
                rep.Add(user);
                rep.Save();

                try
                {
                    ViewData["userid"] = user.UserID;
                    var messageBody = this.CaptureActionHtml(this,
                        c => (ViewResult) c.UserRegistrationNotification(profile));
                    var message = new MailMessage(ConfigurationManager.AppSettings["FromAddress"],
                        ConfigurationManager.AppSettings["AdminAddress"])
                    {
                        IsBodyHtml = true,
                        Subject = "New Customer Registration",
                        Body = messageBody
                    };
                    MailUtility.SendEmail(message);
                }
                catch (System.Exception ex)
                {
                    log.Error("Error sending notification of new user registration.", ex);
                }
            }
            catch (RulesException ex)
            {
                ex.AddModelStateErrors(ModelState, "profile");
                ViewData["SuppressDialog"] = true;
                return View(profile);
            }

            return View("RegistrationComplete");
        }

        [Authorize]
        public ActionResult RequestDataDownloadAccess()
        {
            try
            {
                var messageBody = this.CaptureActionHtml(this,
                    c => (ViewResult) c.DataDownloadAccessRequestNotification());
                var message = new MailMessage(ConfigurationManager.AppSettings["FromAddress"],
                    ConfigurationManager.AppSettings["AdminAddress"])
                {
                    IsBodyHtml = true,
                    Subject = "Data Download Access Request",
                    Body = messageBody
                };
                MailUtility.SendEmail(message);
            }
            catch (System.Exception ex)
            {
                log.Error("Error sending data download request notification.", ex);
                TempData["message"] = "There was an error processing your request. Please try again.";
                return RedirectToAction("Blank", "Home");
            }

            return View("RequestDataDownloadAccess");
        }

        public ActionResult UserRegistrationNotification(UserProfile profile)
        {
            return View("UserRegistrationNotification", profile);
        }

        public ActionResult DataDownloadAccessRequestNotification()
        {
            return View("DataDownloadAccessRequestNotification", CustomerData);
        }

        [Authorize(Roles = "ADMIN")]
        public ActionResult Create()
        {
            var rep = new UserRepository();
            var user = new User();

            var userProfile = new UserProfile(user);
            var userAdminSettings = new UserAdminSettings(user, rep.GetUserRoleMembership(user));

            return View(new UserEditViewModel {UserProfile = userProfile, UserAdminSettings = userAdminSettings});
        }

        [Authorize(Roles = "ADMIN")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(UserProfile profile, UserAdminSettings admin)
        {
            var rep = new UserRepository();
            var user = new User
            {
                CreateDate = DateTime.Now
            };

            try
            {
                rep.UpdateUserProfile(user, profile);
                rep.Add(user);
                // need to persist the user before we can update user admin settings
                rep.Save();
                rep.UpdateUserAdminSettings(user, admin);
                rep.Save();
            }
            catch (RulesException ex)
            {
                ex.AddModelStateErrors(ModelState, "profile");
                return View(new UserEditViewModel {UserProfile = profile, UserAdminSettings = admin});
            }

            TempData["unencodedmessage"] = "User successfully created. <a href=\"" + Url.Action("Create") +
                                           "\">Add another user</a>";
            TempData["UserAdded"] = true;
            return RedirectToAction("Edit", new {id = user.UserID});
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            var rep = new UserRepository();
            var user = GetUser(id, rep, false);

            var userProfile = new UserProfile(user);
            var userAdminSettings = new UserAdminSettings(user, rep.GetUserRoleMembership(user));

            return
                View(new UserEditViewModel
                {
                    UserProfile = userProfile,
                    UserAdminSettings = userAdminSettings,
                    ShowReturnToUserList = id.HasValue
                });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(UserProfile profile, UserAdminSettings admin, int? id, string submitButton)
        {
            var rep = new UserRepository();

            var user = GetUser(id, rep, false);

            var changingSelf = user.UserName == User.Identity.Name;

            if (submitButton == "Delete User")
            {
                rep.Delete(user);
                TempData["message"] = "User deleted.";
                return RedirectToAction("UserList", "Admin");
            }
            // only other option is Save
            try
            {
                rep.UpdateUserProfile(user, profile);

                // only update admin settings if user in ADMIN role
                if (User.IsInRole("ADMIN"))
                {
                    log.Warn(
                        $"Updating user admin settings for userid {user.UserID} customer id from: {user.CustomerID} to {admin.CustomerID}");

                    rep.UpdateUserAdminSettings(user, admin);
                }

                rep.Save();
                log.Warn("Updated.");
                // if user has changed his own username , re-login the user
                if (changingSelf && (profile.UserName != User.Identity.Name))
                {
                    FormsAuthentication.SignOut();
                    FormsAuthentication.SetAuthCookie(profile.UserName, false);
                    Roles.DeleteCookie();
                }
            }
            catch (RulesException ex)
            {
                ex.AddModelStateErrors(ModelState, "profile");
                return View(new UserEditViewModel {UserProfile = profile, UserAdminSettings = admin});
            }
            TempData["message"] = "User profile updated.";

            if (id.HasValue)
                return RedirectToAction("UserList", "Admin");
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var rep = new UserRepository();

            try
            {
                var newPassword = rep.ResetPassword(model);
                var user = rep.GetByUserName(model.Username);

                var messageBody = this.CaptureActionHtml(this, c => (ViewResult) c.ResetPasswordTemplate(newPassword));

                MailUtility.SendEmail(ConfigurationManager.AppSettings["FromAddress"], user.EmailAddress,
                    "EPWI Password Reset Request", messageBody);
            }
            catch (RulesException ex)
            {
                ex.AddModelStateErrors(ModelState, null);
            }

            if (ModelState.IsValid)
            {
                TempData["message"] =
                    "Your password has been reset and the new password has been sent to your e-mail address.";
                return RedirectToAction("LogOn");
            }
            return View();
        }

        public ActionResult ResetPasswordTemplate(string newPassword)
        {
            ViewData["newpassword"] = newPassword;
            return View("ResetPasswordTemplate");
        }

        [Authorize(Roles = "ADMIN,ACCESS_ACCOUNT_SETTINGS")]
        public ActionResult Settings(int? id)
        {
            var rep = new UserRepository();
            var user = GetUser(id, rep, false);

            return View(new AccountSettings(user));
        }

        [Authorize(Roles = "ADMIN,ACCESS_ACCOUNT_SETTINGS")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Settings(int? id, AccountSettings accountSettings)
        {
            var rep = new UserRepository();

            var user = GetUser(id, rep, false);
            try
            {
                rep.UpdateAccountSettings(user, accountSettings);
                rep.Save();
            }
            catch (RulesException ex)
            {
                ex.AddModelStateErrors(ModelState, null);
                return View(new AccountSettings(user));
            }
            TempData["message"] = "Account Settings successfully saved.";
            return RedirectToAction("Edit", new {id});
        }

        [Authorize(Roles = "ACCESS_ACCOUNT_STATUS,ADMIN,EMPLOYEE")]
        public ActionResult Status(int? id, int? customerID, char? companyCode)
        {
            var userRepository = new UserRepository();
            var rep = new AccountRepository();
            User user;
            if (companyCode.HasValue && (User.IsInRole("ADMIN") || CurrentUserIsEmployee))
                user = new User {CompanyCode = companyCode, CustomerID = customerID};
            else
                user = GetUser(id, userRepository, true);

            var model = rep.GetAccountStatus(user);
            model.CurrentUser = CustomerData;

            //TODO: Remove
            if (model.CustomerID != CustomerData.CustomerID) log.Warn($"!! Customer id is different requested: {user.CustomerID}, received: {model.CustomerID}");

            if (model.ErrorOccurred)
            {
                if (User.IsInRole("ADMIN"))
                    ViewData["ErrorDescription"] = model.ErrorDescription;

                return View("AccountInfoUnavailable");
            }

            return View(model);
        }

        [Authorize(Roles = "EMPLOYEE,ADMIN")]
        public ActionResult InvoiceByNumber(string invoiceNumber)
        {
            var proxy = CustomerNumberByInvoiceProxy.Instance;
            var result = proxy.SubmitRequest(invoiceNumber);

            //Invoice number is not being returned from eSynth - can't check

            if (result.DeletedInvoice)
            {
                TempData["message"] = $"Invoice #{invoiceNumber} has been deleted.";
                return RedirectToAction("Blank", "Home");
            }
            if (result.OpenInvoice)
            {
                TempData["message"] = $"Invoice #{invoiceNumber} is currently open.";
                return RedirectToAction("Blank", "Home");
            }
            if (result.InvoiceNotFound)
            {
                TempData["message"] = $"Invoice #{invoiceNumber} could not be found.";
                return RedirectToAction("Blank", "Home");
            }

            return RedirectToAction("Invoice",
                new {customerID = result.CustomerID, companyCode = result.CompanyCode, invoiceNumber});
        }

        [Authorize(Roles = "ACCESS_ACCOUNT_STATUS,EMPLOYEE,ADMIN,CUSTOMER")]
        public ActionResult Invoice(int customerID, char companyCode, string invoiceNumber)
        {
            validateUserAccess(customerID, companyCode);

            var rep = new AccountRepository();
            var user = new User {CustomerID = customerID, CompanyCode = companyCode};

            var invoice = rep.GetInvoice(user, invoiceNumber);

            if (invoice.Error)
            {
                TempData["message"] = $"Invoice {invoiceNumber} not found for Customer #{customerID} ({companyCode})";
                return RedirectToAction("Status", "Account", new {customerID, companyCode});
            }

            return View(new InvoiceViewModel {Invoice = invoice, CustomerData = CustomerData});
        }

        [Authorize(Roles = "ACCESS_ACCOUNT_STATUS,EMPLOYEE,ADMIN,CUSTOMER")]
        public ActionResult InvoiceSearch(string searchSelection, string invoiceDateDirection, DateTime? invoiceDate,
            string partNum, string invNum, int customerID, char companyCode)
        {
            validateUserAccess(customerID, companyCode);

            var rep = new AccountRepository();
            var user = new User {CustomerID = customerID, CompanyCode = companyCode, UserID = CustomerData.UserID};

            switch (searchSelection)
            {
                case "number":
                    return RedirectToAction("Invoice", new {customerID, companyCode, invoiceNumber = invNum});
                case "date":
                    var dateSearchResults = rep.GetInvoiceListByDate(user, invoiceDate.GetValueOrDefault(DateTime.Now),
                        invoiceDateDirection);
                    return View("InvoiceDateSearch", dateSearchResults);
                case "part":
                    var partSearchResults = rep.GetInvoiceListByPartNumber(user, partNum);
                    return View("InvoicePartSearch", partSearchResults);
            }

            throw new ApplicationException("Invalid search request received by InvoiceSearch action method");
        }

        [Authorize(Roles = "ACCESS_ACCOUNT_STATUS,EMPLOYEE,ADMIN,CUSTOMER")]
        public ActionResult MultipleInvoices(List<InvoiceSelections> invoiceSelections, int customerID, char companyCode)
        {
            validateUserAccess(customerID, companyCode);

            var rep = new AccountRepository();
            var user = new User {CustomerID = customerID, CompanyCode = companyCode, UserID = CustomerData.UserID};

            var invoices = rep.GetMultipleInvoices(user, invoiceSelections);

            return View(new MultipleInvoiceViewModel {Invoices = invoices, CustomerData = CustomerData});
        }

        [Authorize(Roles = "ACCESS_ACCOUNT_STATUS,EMPLOYEE,ADMIN,CUSTOMER")]
        public ActionResult Statement(int customerID, char companyCode, int year, int month)
        {
            validateUserAccess(customerID, companyCode);
            var statementDate = new DateTime(year, month, 1);

            var rep = new AccountRepository();
            var user = new User {CustomerID = customerID, CompanyCode = companyCode};
            var statement = rep.GetAccountStatement(user, new DateTime(year, month, 1));

            if (statement.Error)
            {
                TempData["message"] = "Error retrieving statement.";
                return RedirectToAction("Status", "Account", new {customerID, companyCode});
            }

            statement.CurrentUser = CustomerData;

            return View(statement);
        }

        [Authorize(Roles = "EMPLOYEE")]
        public ActionResult UpdateCustomerID(int customerID, char companyCode)
        {
            var userRep = new UserRepository();
            var orderRep = new OrderRepository();
            var order = orderRep.OpenOrder(CustomerData, false);

            // if an order exists for the user, delete is so we don't have an order associated with the wrong customer
            if (order != null)
            {
                orderRep.DeleteOrder(order);
                orderRep.Save();
            }
            log.Warn($"Updating customer id from {CustomerData.UserID} to {customerID}");

            var user = userRep.GetByUserID(CustomerData.UserID);

            user.CompanyCode = companyCode;
            user.CustomerID = customerID;

            userRep.Save();

            TempData["message"] = $"Customer ID updated to {customerID} {companyCode}";

            return RedirectToAction("Search", "StockStatus");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
                throw new InvalidOperationException("Windows authentication is not supported.");
        }

        /// <summary>
        ///     Verifies that the current user can access data for the supplied account info. Throws an exception if not.
        /// </summary>
        private void validateUserAccess(int customerID, char companyCode)
        {
            // admins and employees can access any account
            if (!User.IsInRole("ADMIN") && !CurrentUserIsEmployee)
                if ((customerID != CustomerData.CustomerID.Value) ||
                    (companyCode != CustomerData.CompanyCode.GetValueOrDefault(' ')))
                    throw new AccessDeniedException("Access denied. Only the current user's account data can be viewed.");
        }

        #region Validation Methods

        private bool ValidateLogOn(string userName, string password)
        {
            var rep = new UserRepository();
            if (string.IsNullOrEmpty(userName))
                ModelState.AddModelError("username", "You must specify a username.");
            if (string.IsNullOrEmpty(password))
                ModelState.AddModelError("password", "You must specify a password.");
            if (!rep.ValidateAccount(userName, password))
                ModelState.AddModelError("_FORM", "The username or password provided is incorrect.");

            return ModelState.IsValid;
        }

        /// <summary>
        ///     Gets the specified user (if an admin), otherwise gets the current user
        /// </summary>
        /// <param name="userid">userid of user to get</param>
        /// <returns>User object</returns>
        private User GetUser(int? userid, UserRepository rep, bool allowEmployees)
        {
            User user;

            if (userid.HasValue)
                if (User.IsInRole("ADMIN") || (CurrentUserIsEmployee && allowEmployees) ||
                    (userid == rep.GetByUserName(User.Identity.Name).UserID))
                    user = rep.GetByUserID(userid.Value);
                else
                    throw new AccessDeniedException("Only administrators and employees can access specific users.");
            else
                user = rep.GetByUserName(User.Identity.Name);

            return user;
        }

        #endregion
    }
}