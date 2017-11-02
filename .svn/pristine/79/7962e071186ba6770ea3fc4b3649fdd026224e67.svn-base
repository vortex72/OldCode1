using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web.DynamicData;
using System.Web.Mvc;
using EPWI.Components.Models;
using LINQtoCSV;

namespace EPWI.Web.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : LoggingController
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            TempData["version"] = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return View();
        }

        public ActionResult UserList()
        {
            return View();  
        }

        public ActionResult DownloadUserList()
        {
            var rep = new UserRepository();
            var users = rep.GetAll();
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            var cc = new CsvContext();
            CsvFileDescription outputFileDescription = new CsvFileDescription

            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                FileCultureName = "en-US"
            };

            var formattedUsers = from u in users
                select new
                {
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Company = u.CompanyName,
                    Email = u.EmailAddress,
                    UserID = u.CustomerID,
                    NS = u.CompanyCode,
                    CreateDate = u.CreateDate,
                    Active = u.ActiveAccount,
                    AccessAccountStatus = u.RoleMemberships.Any(rm => rm.Role.RoleKey == "ACCESS_ACCOUNT_STATUS"),
                    Customer = u.RoleMemberships.Any(rm => rm.Role.RoleKey == "CUSTOMER"),
                    Employee = u.RoleMemberships.Any(rm => rm.Role.RoleKey == "EMPLOYEE"),
                    Admin = u.RoleMemberships.Any(rm => rm.Role.RoleKey == "ADMIN"),
                    KitBuilder = u.RoleMemberships.Any(rm => rm.Role.RoleKey == "KIT_BUILDER"),
                    DataDownloads = u.RoleMemberships.Any(rm => rm.Role.RoleKey == "DATA_DOWNLOADS")
                };

            cc.Write(formattedUsers, sw, outputFileDescription);
            sw.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "text/csv", "EPWIUserList.csv");
        }

        public ActionResult GetUserList(int startIndex, int results, string sort, string dir, string searchCriteria, string roleFilter)
        {
            // persuade IE not to cache the user list to fix problem with deleted users still showing up
            Response.AddHeader("Cache-Control", "no-store, no-cache, must-revalidate");
            Response.AddHeader("Cache-Control", "post-check=0, pre-check=0");
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -6000;

            var rep = new UserRepository();
            var users = rep.GetAll();

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                users = from u in users
                    where u.UserName.Contains(searchCriteria) || u.FirstName.Contains(searchCriteria) || u.LastName.Contains(searchCriteria)
                          || u.CompanyName.Contains(searchCriteria) || u.EmailAddress.Contains(searchCriteria) || u.CustomerID.GetValueOrDefault(0).ToString().Equals(searchCriteria)
                    select u;
            }

            var searchTokens = searchCriteria.Split(' ');
            if (searchTokens.Length == 2)
            {
                // if two words, search first name and last name combinations
                var users2 = from u in rep.GetAll()
                    where ((u.FirstName == searchTokens[0] && u.LastName == searchTokens[1]) ||
                           (u.FirstName == searchTokens[1] && u.LastName == searchTokens[0]))
                    select u;
                users = users.Union(users2);
            }

            if (roleFilter != "ALL")
            {
                users = users.Where(u => u.RoleMemberships.Any(rm => rm.Role.RoleKey == roleFilter));
            }

            int recordCount = users.Count();

            // need to handle role columns as a special case for sorting. For now it's just customer
            if (sort != "Customer")
            {
                users = from u in users.OrderBy($"{sort} {dir}").Skip(startIndex).Take(results) select u;
            }
            else
            {
                users = from u in users.OrderBy(u => dir == "desc" && u.RoleMemberships.Any(rm => rm.Role.RoleKey == "CUSTOMER")).Skip(startIndex).Take(results) select u;
            }

            var userList = (from u in users
                select new
                {
                    UserID = u.UserID,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    EmailAddress = u.EmailAddress,
                    CompanyName = u.CompanyName,
                    CustomerID = u.CustomerID,
                    CompanyCode = u.CompanyCode,
                    Customer = u.RoleMemberships.Any(rm => rm.Role.RoleKey == "CUSTOMER"),
                    CreateDate = u.CreateDate
                }).ToList();

            //hack for now, convert date to proper format so client can understand it

            var userGrid = from u in userList
                select new
                {
                    u.UserID,
                    u.UserName,
                    u.FirstName,
                    u.LastName,
                    u.EmailAddress,
                    u.CompanyName,
                    u.CustomerID,
                    u.CompanyCode,
                    u.Customer,
                    CreateDate = u.CreateDate.ToString("ddd d MMM yyyy HH:mm:ss")
                };

            return Json(new {data = userGrid, recordCount = recordCount}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TableMaintenance()
        {
            var tables = from t in MetaModel.Default.VisibleTables where t.Attributes.OfType<ScaffoldTableAttribute>().FirstOrDefault() != null select t;
            return View(tables);
        }
    }
}