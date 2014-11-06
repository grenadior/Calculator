using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CalculatorZd.Models;
using CalculatorZd.Services;
using Common.Api;

namespace CalculatorZd.Controllers
{
    public class FirmReportController : Controller
    {
        //
        // GET: /FirmReport/

         ReportService _service = new ReportService();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = new FirmReportViewModel {FirmReports = _service.GetReportByFirm(SessionManager.FirmInfo.ID)};
                return View("Index", model);
            }
            else
            {
              return RedirectToAction("Login", "Account");
            }
        }
	}
}