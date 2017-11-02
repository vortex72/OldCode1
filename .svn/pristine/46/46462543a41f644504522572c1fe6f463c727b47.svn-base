using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPWI.Components.Models;
using EPWI.Components.Proxies;
using EPWI.Web.Models;

namespace EPWI.Web.Controllers
{
    [Authorize(Roles = "DATA_DOWNLOADS")]
    public class DataDownloadController : LoggingController
    {
        //
        // GET: /DataDownload/
        public ActionResult Index()
        {
            var rep = new LineRepository();

            return
                View(new LineDownloadViewModel
                {
                    Lines = rep.GetAllLines(false).Where(l => l.LDDEL.GetValueOrDefault() != 'D').OrderBy(l => l.LINED)
                });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(Dictionary<string, bool> line, int formatOption)
        {
            var selectedLines = (from k in line
                where k.Value
                select k.Key).ToArray();

            var proxy = DataDownloadProxy.Instance;

            if (proxy.SubmitRequest(CustomerData, selectedLines, formatOption))
            {
                TempData["message"] = "The requested lines will be emailed to you shortly.";
                return RedirectToAction("Index", "Home");
            }
            TempData["message"] = "Error submitting request. Please try again. Contact EPWI if the problem persists.";
            return RedirectToAction("Index");
        }
    }
}