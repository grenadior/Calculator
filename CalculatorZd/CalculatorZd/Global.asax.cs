using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BO;
using BO.Implementation;
using BO.Implementation.Caching;
using BO.Implementation.Caching.Provider;
using BO.Implementation.Calculator;
using Common.Api;
using Microsoft.AspNet.Identity;

namespace CalculatorZd
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/fwlink/?LinkId=301868
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            IdentityConfig.ConfigureIdentity();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ServerProperties.Instance.Init();
            var http = new HttpCache();
            http.CacheDuration = 10440;
            InitFilter(http, FilterNameHelper.CargoNameEarlyTransportationFilterName,
                ServerProperties.Instance.DBPorozhn);
            InitFilter(http, FilterNameHelper.CargoNameFilterName);
            InitFilter(http, FilterNameHelper.ContainerVolumeFilterName);
            InitFilter(http, FilterNameHelper.CargoGroupNamesFilterName);
            InitFilter(http, FilterNameHelper.TransportationTypeFilterName);
            InitFilter(http, FilterNameHelper.CompanyRecipientFilterName);
            InitFilter(http, FilterNameHelper.CompanySendingFilterName);
            InitFilter(http, FilterNameHelper.CountriesSendingFilterName);
            InitFilter(http, FilterNameHelper.CountriesDeliveringFilterName);
            InitFilter(http, FilterNameHelper.CountriesDeliveringFilterName);
            InitFilter(http, FilterNameHelper.WaySendingFilterName);
            InitFilter(http, FilterNameHelper.WayDeliveringFilterName);
            InitFilter(http, FilterNameHelper.SubjectsSendingFilterName);
            InitFilter(http, FilterNameHelper.SubjectsDeliveringFilterName);
            InitFilter(http, FilterNameHelper.PayerFilterName);
            InitFilter(http, FilterNameHelper.OwnerVagonEGRPOFilterName);
            InitFilter(http, FilterNameHelper.RenterVagonGVCFilterName);
            FilterManager.GetWagonTypesHierarhy();

            List<string> list = null;
            try
            {
                string[] columns = FilterNameHelper.StationsSendingFilterNameArr;
                list = CalculatorManager.GetEntities(columns[0], columns[1]);
                http.Set(FilterNameHelper.StationsSendingFilterKey, list);
            }
            catch (Exception)
            {
                //  throw;
            }

            try
            {
                string[] columns = FilterNameHelper.StationsDeliveringFilterNameArr;
                list = CalculatorManager.GetEntities(columns[0], columns[1]);
                http.Set(FilterNameHelper.StationsDeliveringFilterKey, list);
            }
            catch (Exception)
            {
                //  throw;
            }
        }

        private void Session_Start(object sender, EventArgs e)
        {
            HttpContext.Current.Session.Add("__calcAppSession", string.Empty);
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                Firm firm = FirmsManager.GetFirmByID(new Guid(userId));
                SessionManager.FirmInfo = firm;
            }
        }

        private void InitFilter(HttpCache http, string filterNameHelper, string orderBy = "", string dbname = null)
        {
            try
            {
                List<string> list = CalculatorManager.GetEntities(filterNameHelper, "", "", orderBy, dbname);
                if (list != null) http.Set(filterNameHelper, list);
            }
            catch (Exception)
            {
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            Route route = routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}/{id}",
                new {id = RouteParameter.Optional}
                );
            route.RouteHandler = new MyHttpControllerRouteHandler();
        }
    }
}