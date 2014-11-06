using System.Collections.Generic;
using System.Globalization;
using BO.Implementation.Caching;
using BO.Implementation.Caching.Provider;
using BO;
using DA.Filters;

namespace BO.Implementation
{
    public class FilterManager
    {
        public static List<FiltersTypes> GetFiltersTypes()
        {
            List<FiltersTypes> listEntities = FiltersAdapter.GetFiltersTypes();
            return listEntities;
        }
        public static List<WagonGroupType> GetWagonTypes()
        {
            List<WagonGroupType> listEntities = FiltersAdapter.GetWagonTypes();
            return listEntities;
        }
        public static List<FilterCoefficient> GetCoefficientsByType(int filterTypeID)
        {
            List<FilterCoefficient> list = FiltersAdapter.GetCoefficientsByType(filterTypeID);
            return list;
        }

        public static bool InsertCoefficients(List<FilterCoefficient> list, int typeId)
        {
            return FiltersAdapter.InsertCoefficients(list, typeId);
        }

        public static List<FilterCoefficient> GetCoefficientsAll()
        {
            List<FilterCoefficient> list = FiltersAdapter.GetCoefficientsByType(null);
            return list;
        }

        public static List<WagonGroupType> GetWagonTypesHierarhy()
        {
            var http = new HttpCache();
            string cacheKeyName = FilterNameHelper.WagonTypeHierarhyListFilterKey;

            List<WagonGroupType> objValue = null;
            http.Get(cacheKeyName, out objValue);
            if (objValue == null)
            {
                objValue = GetWagonTypes();
                http.Set(FilterNameHelper.WagonTypeHierarhyListFilterKey, objValue);
            }

            return objValue;
        }
    }
}