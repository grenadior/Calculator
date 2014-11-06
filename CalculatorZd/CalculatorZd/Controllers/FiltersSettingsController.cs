using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BO.Implementation;
using BO.Implementation.Calculator;
using BO;
using CalculatorZd.Models;
using Common.Api.Users;

namespace CalculatorZd
{
    public class FiltersSettingsController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated && (User.IsInRole(UserRoles.ADMIN) || User.IsInRole(UserRoles.GOD)))
                return View();
            else
            {
                return RedirectToAction("Login","Account");
            }
        }
      
        public IEnumerable<CoefficientItemViewModel> Save(string items, int typeId)
        {
            var serializer = new JavaScriptSerializer();
            var list = serializer.Deserialize<List<CoefficientItemViewModel>>(items);
            var coeffs = new List<FilterCoefficient>();
            foreach (CoefficientItemViewModel l in list)
            {
                coeffs.Add(new FilterCoefficient
                {
                    FilterTypeID = typeId,
                    CoefficientValue = l.Value,
                    CountItems = l.Id
                });
            }

            FilterManager.InsertCoefficients(coeffs, typeId);
            return list;
        }
    }
}