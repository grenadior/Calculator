using System.Collections.Generic;
using System.Data;
using System.Globalization;
using BO;
using Common.Api;
using Common.Api.Extensions;
using DA.Entities;
using DA.Filters;

namespace BL.Calculator
{
    public class CalculatorManager
    {
        public static List<string> GetEntities(string columnName, string columnName2 = "", string sWhere = "", string orderBy = "")
        {
            return EntityAdapter.GetEntitiesByID(columnName, columnName2, sWhere, orderBy);
        }
        
        public static List<FiltersTypes> GetFiltersTypesSettings()
        {
           return FiltersAdapter.GetFiltersTypesSettings();
        }

        public static DataTable GetCalculatorResultByFilter(string allQuery, string allQuerySummary, out int totalRowsCount)
        {
            return EntityAdapter.GetCalculatorResultByFilter(allQuery, allQuerySummary, out totalRowsCount);
        }
    }
}