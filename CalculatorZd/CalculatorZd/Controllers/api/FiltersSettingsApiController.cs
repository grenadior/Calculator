using System.Collections.Generic;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BO.Implementation;
using BO.Implementation.Calculator;
using BO;
using CalculatorZd.Models;
using System.Web.Caching;
namespace CalculatorZd
{
    public class FiltersSettingsApiController : ApiController
    {
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


        public IEnumerable<CoefficientItemViewModel> GetCoefficientsByType(int typeId)
        {
            List<FilterCoefficient> list = FilterManager.GetCoefficientsByType(typeId);
            var types = new List<CoefficientItemViewModel>();
            int i = 0;
            foreach (FilterCoefficient l in list)
            {
                i++;
                types.Add(new CoefficientItemViewModel
                {
                    Id = i,
                    Value = l.CoefficientValue
                });
            }

            return types;
        }

        public IEnumerable<FilterTypeViewModel> GetFilterTypes()
        {
            return GetTypes();
        }

        private IEnumerable<FilterTypeViewModel> GetTypes()
        {
            List<FiltersTypes> list = CalculatorManager.GetFiltersTypesSettings();
            var types = new List<FilterTypeViewModel>();
            foreach (FiltersTypes l in list)
            {
                types.Add(new FilterTypeViewModel
                {
                    id = l.FilterTypeID,
                    name = l.FilterTypeName
                });
            }

            return types;
        }
    }
}