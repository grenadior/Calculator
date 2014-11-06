using System.Collections.Generic;
using System.Globalization;
using BO;
using DA.Filters;

namespace BL.Calculator
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
        
    }
}