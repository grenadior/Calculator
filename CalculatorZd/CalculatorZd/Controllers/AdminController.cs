using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BO.Implementation.Caching;
using BO.Implementation.Caching.Provider;
using BO.Implementation.Calculator;
using BO.Implementation;

namespace CalculatorZd.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var http = new HttpCache();
            http.CacheDuration = 10440;
            List<string> obj;
            StringBuilder sb = new StringBuilder();
            string format = "{0}, {1}  ";
            http.Get(FilterNameHelper.CargoNameFilterName, out obj);
            sb.Append(string.Format(format, FilterNameHelper.CargoNameFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            http.Get(FilterNameHelper.StationsSendingFilterKey, out obj);
            sb.Append(string.Format(format, FilterNameHelper.StationsSendingFilterKey, obj != null ? obj.Count : 0));
            http.Get(FilterNameHelper.ContainerVolumeFilterName, out obj);
            sb.AppendLine("<br>");
            sb.Append(string.Format(format, FilterNameHelper.ContainerVolumeFilterName, obj != null ? obj.Count : 0));
            http.Get(FilterNameHelper.CargoGroupNamesFilterName, out obj);
            sb.AppendLine("<br>");
            sb.Append(string.Format(format, FilterNameHelper.CargoGroupNamesFilterName, obj != null ? obj.Count : 0));
            http.Get(FilterNameHelper.CargoGroupNamesFilterName, out obj);

            sb.Append(string.Format(format, FilterNameHelper.CargoGroupNamesFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            http.Get(FilterNameHelper.TransportationTypeFilterName, out obj);
            sb.Append(string.Format(format, FilterNameHelper.TransportationTypeFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            http.Get(FilterNameHelper.CompanySendingFilterName, out obj);
            sb.Append(string.Format(format, FilterNameHelper.CompanySendingFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            http.Get(FilterNameHelper.CompanyRecipientFilterName, out obj);
            sb.Append(string.Format(format, FilterNameHelper.CompanyRecipientFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            http.Get(FilterNameHelper.CompanySendingFilterName, out obj);
            sb.Append(string.Format(format, FilterNameHelper.CompanySendingFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            http.Get(FilterNameHelper.CountriesDeliveringFilterName, out obj);
            sb.Append(string.Format(format, FilterNameHelper.CountriesDeliveringFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            http.Get(FilterNameHelper.WaySendingFilterName, out obj);
            sb.Append(string.Format(format, FilterNameHelper.WaySendingFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            http.Get(FilterNameHelper.WayDeliveringFilterName, out obj);
            sb.Append(string.Format(format, FilterNameHelper.WayDeliveringFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            http.Get(FilterNameHelper.SubjectsSendingFilterName, out obj);
            sb.Append(string.Format(format, FilterNameHelper.SubjectsSendingFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            http.Get(FilterNameHelper.SubjectsDeliveringFilterName, out obj);
            sb.Append(string.Format(format, FilterNameHelper.SubjectsDeliveringFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            http.Get(FilterNameHelper.PayerFilterName, out obj);
            sb.Append(string.Format(format, FilterNameHelper.PayerFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            http.Get(FilterNameHelper.OwnerVagonEGRPOFilterName, out obj);
            sb.Append(string.Format(format, FilterNameHelper.PayerFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            http.Get(FilterNameHelper.RenterVagonGVCFilterName, out obj);
            sb.Append(string.Format(format, FilterNameHelper.RenterVagonGVCFilterName, obj != null ? obj.Count : 0));
            sb.AppendLine("<br>");
            List<string> list = null;
            try
            {
                http.Get(FilterNameHelper.StationsSendingFilterKey, out obj);
                sb.Append(string.Format(format, FilterNameHelper.StationsSendingFilterKey, obj != null ? obj.Count : 0));
            }
            catch (Exception)
            {
                //  throw;
            }
            sb.AppendLine("<br>");
            try
            {
                http.Get(FilterNameHelper.StationsDeliveringFilterKey, out obj);
                sb.Append(string.Format(format, FilterNameHelper.StationsDeliveringFilterKey, obj != null ? obj.Count : 0));
            }
            catch (Exception)
            {
                //  throw;
            }
            ViewBag.CacheReport = sb;
            return View();
        }

        [HttpPost]
        public ActionResult ClearAllCache()
        {
            HttpCache cache = new HttpCache();
            var keys = cache.GetAll();
            foreach (var key in keys)
            {
                cache.Clear(key.Key);
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult ReloadServerProps()
        {
           ServerProperties.Instance.Init();

            return View("Index");
        }
    }
}