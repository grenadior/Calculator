using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using Antlr.Runtime.Tree;
using BO;
using BO.Implementation;
using BO.Implementation.Caching;
using BO.Implementation.Caching.Provider;
using CalculatorZd.Models;
using Common.Api;
using Common.Api.Extensions;
using Utils;

namespace CalculatorZd.Services
{
    public class CalculatorService
    {
        public enum SelectType
        {
            Inner,
            Outer
        }

        public static String GetQueryReport(List<SelectedColumnsViewModel> selectedColumns,
            List<FilterParamstItemViewModel> filterParamstItems, int pageIdFrom, int pageIdTo,
            out string allQuerySummary, out ViewTypeReport viewTypeReport, out string queryForExelReport, VagonSourceTypeParamEnum dbType)
        {
            viewTypeReport = GetTypeReport(selectedColumns);
            string dbName = dbType == VagonSourceTypeParamEnum.Gruzhon ? ServerProperties.Instance.DBGruzhon : ServerProperties.Instance.DBPorozhn;
            //Where
            var limitRowsWhereString = new StringBuilder();
            limitRowsWhereString.Append(String.Format(" ID BETWEEN {0} AND {1}", pageIdFrom, pageIdTo));
                // (pageId - 1) * pageSize, (pageId * 10) * pageSize);
            string additionalWhere = "";

            string whereQuery = " WHERE " + GetWhereQuery(filterParamstItems, additionalWhere, viewTypeReport == ViewTypeReport.Svodnaya && dbType == VagonSourceTypeParamEnum.Gruzhon);
            string whereWithLimit = whereQuery + " AND " + limitRowsWhereString;
            string allSelect;
          //  const string columnIdQuery = "row_number() over(ORDER BY [Номер вагона]) ID";
            var groupBy = new StringBuilder();
            var groupingData = new StringBuilder();
            string quantityVagon = "";
            string quantityContainer = "";
            string monthTransportation = "";
             

            if (selectedColumns.Any(column => column.id == (int) ColumnsMapping.MonthTransportation))
            {
                monthTransportation = String.Format(" CAST ( MONTH( {0} ) as nvarchar(20)) + '.' + CAST ( YEAR( {0} ) as nvarchar(20)) as [{1}]",
                    ColumnsMapping.DateSending.GetStringValue(),
                    ColumnsMapping.MonthTransportation.GetStringValue());
            }

            if (viewTypeReport == ViewTypeReport.Svodnaya)
            {
                if (selectedColumns.Count == 2 &&
                    selectedColumns.Any(column => column.id == (int) ColumnsMapping.VolumeTransportation) &&
                    selectedColumns.Any(column => column.id == (int) ColumnsMapping.DestinationTariff)
                    ||
                    (selectedColumns.Count == 1 &&
                     (selectedColumns.Any(column => column.id == (int) ColumnsMapping.VolumeTransportation) ||
                      selectedColumns.Any(column => column.id == (int) ColumnsMapping.DestinationTariff) ||
                      selectedColumns.Any(column => column.id == (int) ColumnsMapping.TonnKm)))
                    || (selectedColumns.Count == 2 &&
                        (selectedColumns.Any(column => column.id == (int) ColumnsMapping.TonnKm) &&
                         selectedColumns.Any(column => column.id == (int) ColumnsMapping.DestinationTariff) ||
                         (selectedColumns.Any(column => column.id == (int) ColumnsMapping.TonnKm) &&
                          selectedColumns.Any(column => column.id == (int) ColumnsMapping.VolumeTransportation))))
                    || (selectedColumns.Count == 3 &&
                        (selectedColumns.Any(column => column.id == (int) ColumnsMapping.VolumeTransportation) &&
                         selectedColumns.Any(column => column.id == (int) ColumnsMapping.DestinationTariff) &&
                         selectedColumns.Any(column => column.id == (int) ColumnsMapping.TonnKm))))
                {
                    allSelect = String.Format("SELECT {0}  FROM {1} ", GetSelectedColumns(selectedColumns),
                        dbName);
                    queryForExelReport = allSelect;
                    allSelect += whereWithLimit;
                    allQuerySummary = String.Format("SELECT Count(*) as TotalRowsCount,  {0}  FROM {1} ",
                        GetSelectedColumns(selectedColumns),
                        dbName);
                    allQuerySummary = allQuerySummary + whereWithLimit;
                    return allSelect;
                }

                if (dbType == VagonSourceTypeParamEnum.Gruzhon)
                {
                    string whereQueryTopQuery = " WHERE " + GetWhereQuery(filterParamstItems, additionalWhere, false);
                   
                    string topQuery = String.Format(QueryUtils.GetGroupedFabricatedCargoQuery(), dbName, whereQueryTopQuery);
                    StringBuilder selectedColumnsList = GetSelectedColumns(selectedColumns, false, QueryUtils.PrefixGr, true);

                    // Order By
                    string orderByColumns = null;
                    StringBuilder orderBy = GetOrderColumns(selectedColumns, Utils.QueryUtils.Prefix);
                    if (orderBy.Length > 0)
                        orderByColumns = String.Format(" Order By {0}", orderBy);

                    ////Group by columns
                    groupingData = GetGroupByString(selectedColumns, viewTypeReport,Utils.QueryUtils.PrefixGr);
                    string mainQuery = String.Format(" {0} {1}", topQuery, String.Format(QueryUtils.GetMainGroupedQuery(), selectedColumnsList, dbName, whereQuery, groupingData));
                    string queryForExel = mainQuery;

                    var reportBuilder = new StringBuilder();
                    reportBuilder.Append(mainQuery + String.Format(" WHERE t.ID between {0} and {1} {2}", pageIdFrom, pageIdTo, orderByColumns));
                  
                    reportBuilder.Append(QueryUtils.DropTempTableQuery);
                    allSelect = reportBuilder.ToString();

                    queryForExelReport = queryForExel;

                    allQuerySummary = "";// " SELECT " + GetSelectedColumns(selectedColumns, false, true) + String.Format("FROM {0}", dbName) + whereQuery + groupBy;
                    
                }
                else
                {
                    const string rowNumberSelect = " Row_number() OVER (ORDER BY (SELECT 1)) AS  ID,";
                    StringBuilder groupedColumn = GetSelectedColumns(selectedColumns);
                    string commaQV = groupedColumn.Length > 0 && quantityVagon.Length > 0 ? "," : "";
                    string commaQC = groupedColumn.Length > 0 && quantityContainer.Length > 0 ? "," : "";
                    string select = String.Format("{0} {1} {2}  FROM {3} ", groupedColumn,
                        commaQV + quantityVagon, commaQC + quantityContainer, dbName);
                    const string selectFormat = "SELECT {0} {1}";
                    string innerSelect = String.Format(selectFormat, rowNumberSelect, @select);

                    allSelect = innerSelect + whereQuery;
                    ////Group by columns
                    groupingData = GetGroupByString(selectedColumns, viewTypeReport);
                    if (groupingData.Length > 0)
                        groupBy.Append(String.Format(" Group By  {0}", groupingData));

                    allSelect += groupBy;

                    var reportBuilder = new StringBuilder();
                    reportBuilder.Append("WITH OrderedReport AS");
                    reportBuilder.Append("(");
                    reportBuilder.Append(allSelect);
                    reportBuilder.Append(")");

                    string outerSelect = String.Format("SELECT {0} FROM OrderedReport",
                        GetSelectedColumns(selectedColumns, true));

                    // Order By
                    string orderByColumns = null;
                    StringBuilder orderBy = GetOrderColumns(selectedColumns);
                    if (groupingData.Length > 0 && orderBy.Length > 0)
                        orderByColumns = String.Format(" Order By {0}", orderBy);

                    outerSelect += "{0}";
                    outerSelect += orderByColumns;
                    reportBuilder.Append(outerSelect);
                    allSelect = String.Format(reportBuilder.ToString(), " WHERE " + limitRowsWhereString);

                    queryForExelReport = reportBuilder.ToString().Replace("{0}", "");

                    allQuerySummary = String.Format(selectFormat, "", @select) + whereQuery + groupBy;

                    //summary query
                    var totalReportBuilder = new StringBuilder();
                    totalReportBuilder.Append("WITH totalQuery AS");
                    totalReportBuilder.Append("(");
                    totalReportBuilder.Append(allQuerySummary);
                    totalReportBuilder.Append(")");
                    totalReportBuilder.Append("SELECT  Count(*) as TotalRowsCount from totalQuery");
                    allQuerySummary = totalReportBuilder.ToString();
                }
            }
            else
            {
                string selectedListColumns = GetListColumns(selectedColumns, viewTypeReport).ToString();
               
                string innerSelect = String.Format("SELECT Row_number() OVER (ORDER BY (SELECT 1)) AS  ID, {0} {1} FROM {2} ", selectedListColumns, 
                                                                                                       monthTransportation.Length > 0 ? "," + monthTransportation : monthTransportation,
                                                                                                       dbName);

                allSelect = innerSelect + whereQuery;
              
                string outerSelect = String.Format("SELECT {0}, {1}  FROM OrderedReport ", "ID",
                    GetListColumns(selectedColumns, viewTypeReport, SelectType.Outer));
                
                var reportBuilder = new StringBuilder();
                reportBuilder.Append("WITH OrderedReport AS");
                reportBuilder.Append("(");
                reportBuilder.Append(allSelect);
                reportBuilder.Append(")");

                outerSelect += "{0}";

                allQuerySummary = reportBuilder.ToString().Replace("{0}", "") +
                                  String.Format("SELECT Count(*) as TotalRowsCount FROM {0} ", "OrderedReport ");
                reportBuilder.Append(outerSelect);
                queryForExelReport = reportBuilder.ToString().Replace("{0}", "");

                allSelect = String.Format(reportBuilder.ToString(), " WHERE " + limitRowsWhereString);
            }

            return allSelect;
        }


        private static ViewTypeReport GetTypeReport(IEnumerable<SelectedColumnsViewModel> selectedColumns)
        {
            var typeNakl = ViewTypeReport.Svodnaya;
            foreach (SelectedColumnsViewModel column in selectedColumns)
            {
                string columnName = String.Format("[{0}]", column.name);
                if (columnName == ColumnsMapping.NumberVagon.GetStringValue() || columnName == ColumnsMapping.NumberContainer.GetStringValue() ||
                       columnName == ColumnsMapping.NumberDoc.GetStringValue() || columnName == ColumnsMapping.DateSending.GetStringValue())
                    {
                        typeNakl = ViewTypeReport.Nakladnaya;
                        break;
                    }
            }
            return typeNakl;
        }

        private static StringBuilder GetListColumns(IEnumerable<SelectedColumnsViewModel> selectedColumns,
            ViewTypeReport viewTypeNakl = ViewTypeReport.None, SelectType selectType = SelectType.Inner)
        {
            bool existTonnKm = false;
            var columnsQuery = new StringBuilder();

            if (selectedColumns.Any(column => column.id == (int) ColumnsMapping.TonnKm))
            {
                existTonnKm = true;
            }

            SelectedColumnsViewModel[] selectedColumnsViewModels = selectedColumns as SelectedColumnsViewModel[] ??
                                                                   selectedColumns.ToArray();

            foreach (SelectedColumnsViewModel column in selectedColumnsViewModels)
            {
                ColumnsMapping columnName;
                if (Enum.TryParse(column.id.ToString(CultureInfo.InvariantCulture), true, out columnName))
                {
                    if (selectType == SelectType.Inner && columnName == ColumnsMapping.QuantityVagon )
                    {
                        bool columnExistsNumberWagonInSelect = false;
                        foreach (SelectedColumnsViewModel selected in selectedColumns)
                        {
                            ColumnsMapping columnCheck;
                            if (Enum.TryParse(selected.id.ToString(CultureInfo.InvariantCulture), true, out columnCheck))
                            {
                                if (columnCheck == ColumnsMapping.NumberVagon)
                                {
                                    columnExistsNumberWagonInSelect = true;
                                    break;                                    
                                }
                            }
                        }

                        if (!columnExistsNumberWagonInSelect)
                        {
                            columnsQuery.Append(ColumnsMapping.NumberVagon.GetStringValue());
                            columnsQuery.Append(",");
                        }
                    }
                    else if (selectType == SelectType.Inner && columnName == ColumnsMapping.QuantityContainer )
                    {
                        bool columnExistsNumberContainerInSelect = false;
                        foreach (SelectedColumnsViewModel selected in selectedColumns)
                        {
                            ColumnsMapping columnCheck;
                            if (Enum.TryParse(selected.id.ToString(CultureInfo.InvariantCulture), true, out columnCheck))
                            {
                                if (columnCheck == ColumnsMapping.NumberContainer)
                                {
                                      columnExistsNumberContainerInSelect = true;
                                    break;
                                }
                            }
                        }
                    
                        if (!columnExistsNumberContainerInSelect)
                        {
                            columnsQuery.Append(ColumnsMapping.NumberContainer.GetStringValue());
                            columnsQuery.Append(",");
                        }
                    }
                    else if (selectType == SelectType.Outer && columnName == ColumnsMapping.QuantityVagon || columnName == ColumnsMapping.QuantityContainer)
                    {
                        if ( columnName == ColumnsMapping.QuantityVagon )
                        {
                            columnsQuery.Append(" case when [Номер вагона] <> '00000000000' then 1 else 0 end as [Количество вагонов]");
                            columnsQuery.Append(",");
                        }

                        if ( columnName == ColumnsMapping.QuantityContainer )
                        {
                            columnsQuery.Append(" case when [Номер контейнера] <> '00000000000' then 1 else 0 end as [Количество контейнеров]");
                            columnsQuery.Append(",");
                        }
                    }
                    else if (selectType == SelectType.Inner && existTonnKm &&
                        (columnName == ColumnsMapping.VolumeTransportation ||
                         columnName == ColumnsMapping.DestinationTariff))
                        continue;
                    else if (selectType == SelectType.Outer && columnName == ColumnsMapping.TonnKm)
                    {
                        if (viewTypeNakl == ViewTypeReport.Nakladnaya)
                        {
                            columnsQuery.Append(String.Format("CONVERT(bigint,{0}) * CONVERT(bigint,{1}) as {2}",
                                ColumnsMapping.VolumeTransportation.GetStringValue(),
                                ColumnsMapping.DestinationTariff.GetStringValue(),
                                ColumnsMapping.TonnKm.GetStringValue()));
                        }
                    }
                    else if (columnName == ColumnsMapping.DateSending)
                    {
                        columnsQuery.Append(ColumnsMapping.DateSending.GetStringValue());
                        columnsQuery.Append(",");
                    }
                    else if (columnName == ColumnsMapping.MonthTransportation)
                    {
                        if (selectType == SelectType.Outer)
                            columnsQuery.Append(String.Format("[{0}]", ColumnsMapping.MonthTransportation.GetStringValue()));
                        else
                        {
                            continue;
                        }
                    }
                    else if (columnName == ColumnsMapping.CODE_NAME_STATION_SENDING_RUS_SNG)
                    {
                        columnsQuery.Append(ColumnsMapping.StationSendingCodeRUS.GetStringValue());
                        columnsQuery.Append(",");
                        columnsQuery.Append(ColumnsMapping.StationSendingSNG.GetStringValue());
                        columnsQuery.Append(",");
                    }
                    else if (columnName == ColumnsMapping.CODE_NAME_STATION_DELIVERING_RUS_SNG)
                    {
                        columnsQuery.Append(ColumnsMapping.StationDeliveringCodeRUS.GetStringValue());
                        columnsQuery.Append(",");
                        columnsQuery.Append(ColumnsMapping.StationDeliveringCodeSNG.GetStringValue());
                        columnsQuery.Append(",");
                    }
                    else if (selectType == SelectType.Inner && columnName == ColumnsMapping.TonnKm)
                    {
                        if (existTonnKm)
                        {
                            columnsQuery.Append(ColumnsMapping.VolumeTransportation.GetStringValue());
                            columnsQuery.Append(",");
                            columnsQuery.Append(ColumnsMapping.DestinationTariff.GetStringValue());
                            columnsQuery.Append(",");
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        columnsQuery.Append(columnName.GetStringValue());
                        columnsQuery.Append(",");
                    }
                }
            }

            return columnsQuery.Remove(columnsQuery.Length - 1, 1);
        }

        private static bool ExistsColumn(IEnumerable<SelectedColumnsViewModel> selectedColumns,
            ColumnsMapping columnsMapping)
        {
            bool existVolumeTransportation = false;
            foreach (SelectedColumnsViewModel column in selectedColumns)
            {
                ColumnsMapping columnName;

                if (Enum.TryParse(column.id.ToString(CultureInfo.InvariantCulture), true, out columnName))
                {
                    if (columnName == columnsMapping)
                    {
                        existVolumeTransportation = true;
                        break;
                    }
                }
            }
            return existVolumeTransportation;
        }

        private static StringBuilder GetSelectedColumns(IEnumerable<SelectedColumnsViewModel> selectedColumns,
             bool isOuterSelect = false, string prefix = "", bool groupingQuery = false)
        {
            var resultSelectedcolumns = new StringBuilder();
           
            foreach (SelectedColumnsViewModel column in selectedColumns)
            {
                ColumnsMapping columnName;

                if (Enum.TryParse(column.id.ToString(CultureInfo.InvariantCulture), true, out columnName))
                {
                    if (groupingQuery && columnName == ColumnsMapping.CargoName)
                    {
                        resultSelectedcolumns.Append(
                            String.Format("Case When fb.Count > 1 Then 'Сборный груз' Else gr.{0} end {0}",
                                ColumnsMapping.CargoName.GetStringValue()));
                    }
                    else if (groupingQuery && columnName == ColumnsMapping.VolumeTransportation)
                    {
                        resultSelectedcolumns.Append(String.Format(" SUM ( Cast(gr.{0} as bigint)) as {0}", ColumnsMapping.VolumeTransportation.GetStringValue()));
                    }
                    else if (groupingQuery && columnName == ColumnsMapping.PaymentSum)
                    {
                        resultSelectedcolumns.Append(String.Format(" SUM ( Cast(gr.{0} as bigint)) as {0}", ColumnsMapping.PaymentSum.GetStringValue()));
                    }
                    else if (groupingQuery && columnName == ColumnsMapping.QuantityContainer)//columnName == ColumnsMapping.QuantityVagon || 
                    {
                        resultSelectedcolumns.Append(string.Format(" Count( distinct case when gr.{0} <> '00000000000' then gr.{0} end) as {1}",ColumnsMapping.NumberContainer.GetStringValue(),ColumnsMapping.QuantityContainer.GetStringValue()));

                    }
                    else if (groupingQuery && columnName == ColumnsMapping.QuantityVagon) 
                    {
                        resultSelectedcolumns.Append(string.Format(" Count( distinct case when gr.{0} <> '00000000000' then gr.{0} end) as {1}", ColumnsMapping.NumberVagon.GetStringValue(), ColumnsMapping.QuantityVagon.GetStringValue()));
                    }
                    else if (columnName == ColumnsMapping.MonthTransportation)
                    {
                        if (isOuterSelect)
                            resultSelectedcolumns.Append(String.Format("[{0}]",
                                ColumnsMapping.MonthTransportation.GetStringValue()));
                        else
                            resultSelectedcolumns.Append(String.Format("CAST ( MONTH( {0} ) as nvarchar(20)) + '.' + CAST ( YEAR( {0} ) as nvarchar(20)) as [{1}]",
                               prefix + ColumnsMapping.DateSending.GetStringValue(),
                               ColumnsMapping.MonthTransportation.GetStringValue()));
                    }
                    else if (columnName == ColumnsMapping.TonnKm)
                    {
                        if (isOuterSelect)
                            resultSelectedcolumns.Append(prefix + String.Format("{0}", ColumnsMapping.TonnKm.GetStringValue()));
                        else
                            resultSelectedcolumns.Append(
                                String.Format("SUM(CONVERT(bigint,{0})) * SUM(CONVERT(bigint,{1})) as {2}",
                                   prefix + ColumnsMapping.VolumeTransportation.GetStringValue(),
                                   prefix + ColumnsMapping.DestinationTariff.GetStringValue(),
                                   ColumnsMapping.TonnKm.GetStringValue()));
                    }
                    else
                    {
                        resultSelectedcolumns.Append(prefix + columnName.GetStringValue());
                    }
                    resultSelectedcolumns.Append(",");
                }
            }

            return resultSelectedcolumns.Length > 0 ? resultSelectedcolumns.Remove(resultSelectedcolumns.Length - 1, 1) : resultSelectedcolumns;
        }

        private static StringBuilder GetOrderColumns(IEnumerable<SelectedColumnsViewModel> selectedColumns, string prefix = "")
        {
            var orderColumns = new StringBuilder();
            foreach (SelectedColumnsViewModel column in selectedColumns)
            {
                ColumnsMapping columnName;
                if (Enum.TryParse(column.id.ToString(CultureInfo.InvariantCulture), true, out columnName))
                {
                    if(columnName == ColumnsMapping.QuantityContainer || columnName == ColumnsMapping.QuantityVagon)
                        continue;

                    if (columnName == ColumnsMapping.MonthTransportation)
                    {
                        orderColumns.Append(prefix + String.Format("[{0}]", ColumnsMapping.MonthTransportation.GetStringValue()));
                    }
                    else
                    {
                        orderColumns.Append(prefix + columnName.GetStringValue());
                    }
                    orderColumns.Append(",");
                }
            }
            return orderColumns.Length > 0 ? orderColumns.Remove(orderColumns.Length - 1, 1) : new StringBuilder();
        }

        private static StringBuilder GetWhereQuery(IEnumerable<FilterParamstItemViewModel> filterParamstItem, string additionalWhere, bool needPrefix)
        {
            var sbWhere = new StringBuilder();
            bool periodAdded = false;

            foreach (FilterParamstItemViewModel paramstItem in filterParamstItem)
            {
                bool allSelected = paramstItem.cv == -1; //"ВСЕ";

                if (allSelected || paramstItem.filterId == 45) //45 это радио баттон станции отправления - выключатель
                    continue;

                ColumnsMapping filterName;

                if (Enum.TryParse(paramstItem.filterId.ToString(CultureInfo.InvariantCulture), true, out filterName))
                {
                    string expressionWhere = null;
                    string prefix = needPrefix ? QueryUtils.PrefixGr : "";
                    if (filterName == ColumnsMapping.DateSending && !periodAdded)
                    {
                        DateTime dateBegin;
                        DateTime dateEnd;
                        var enUS = new CultureInfo("en-US");
                        DateTime.TryParseExact(paramstItem.sv[0].name, "dd.MM.yyyy", enUS, DateTimeStyles.None,
                            out dateBegin);

                        DateTime.TryParseExact(paramstItem.sv[1].name, "dd.MM.yyyy", enUS, DateTimeStyles.None,
                            out dateEnd);

                        TimeSpan dateDifferent = dateEnd - dateBegin;
                        int months = dateDifferent.GetMonths();
                        if (dateBegin != DateTime.MinValue || dateEnd != DateTime.MinValue && months >= 0)
                        {
                            string dateBeginQuery = "", dateEndQuery = "";
                            string dateBeginFormated = String.Format("{0:dd.MM.yyyy}", dateBegin);
                            string dateEndFormated = String.Format("{0:dd.MM.yyyy}", dateEnd);

                            if (dateBegin != DateTime.MinValue)
                            {
                               dateBeginQuery = "{0} >= convert(date,'{1}', 104 )";
                            }
                            if (dateEnd != DateTime.MinValue)
                            {
                                dateEndQuery = "{0} <= convert(date,'{1}', 104 )";
                            }

                            if (!String.IsNullOrEmpty(dateBeginQuery) && !String.IsNullOrEmpty(dateEndQuery))
                            {
                                expressionWhere = String.Format(dateBeginQuery, prefix + ColumnsMapping.DateSending.GetStringValue(), dateBeginFormated) + " AND "
                                                                                        + prefix+String.Format(dateEndQuery, ColumnsMapping.DateSending.GetStringValue(), dateEndFormated);
                            }
                            else if(!String.IsNullOrEmpty(dateBeginQuery))
                            {
                                expressionWhere = String.Format(dateBeginQuery, prefix + ColumnsMapping.DateSending.GetStringValue(), dateBeginFormated); 
                            }
                            else
                            {
                                expressionWhere = String.Format(dateEndQuery,
                                     prefix + ColumnsMapping.DateSending.GetStringValue(), dateEndFormated);
                            }
                        }
                        else
                            continue;

                        periodAdded = true;
                    }
                    else if (filterName == ColumnsMapping.CODE_NAME_STATION_SENDING_RUS_SNG)
                    {
                          expressionWhere = String.Format("({0} {1} OR {2} {3})",
                           ColumnsMapping.StationSendingCodeRUS.GetStringValue(),
                         " IN (" + GetColumnFilterByValue(paramstItem.sv, FilterNameHelper.StationsSendingFilterKey) + ")",
                           ColumnsMapping.StationSendingCodeSNG.GetStringValue(),
                         " IN (" + GetColumnFilterByValue(paramstItem.sv, FilterNameHelper.StationsSendingFilterKey) + ")");
                     
                    }
                    else if (filterName == ColumnsMapping.CODE_NAME_STATION_DELIVERING_RUS_SNG)
                    {
                        foreach (FilterParamstItemViewModel paramsFilter in filterParamstItem)
                        {
                            if (paramsFilter.filterId == 45)
                            {
                               
                                if (paramsFilter.sv[0].name != SearchStationListVariantsEnum.None.GetStringValue())
                                {
                                    int lengthDigitalCodeSearch = paramsFilter.sv[0].name == SearchStationListVariantsEnum.TwoDigitalCodeStation.GetStringValue() ? 2 : 3;
                                   
                                    expressionWhere += String.Format("({0} {1} OR {2} {3})",
                                   ColumnsMapping.StationDeliveringCodeRUS.GetStringValue(),
                                   " like (" + GetColumnFilterByValue(paramstItem.sv, true, lengthDigitalCodeSearch) + ")",
                                   ColumnsMapping.StationDeliveringCodeSNG.GetStringValue(),
                                   " like (" + GetColumnFilterByValue(paramstItem.sv, true, lengthDigitalCodeSearch) + ")");
                                }
                                else
                                {
                                    expressionWhere = String.Format("({0} {1} OR {2} {3})",
                                     ColumnsMapping.StationDeliveringCodeRUS.GetStringValue(),
                                     " IN (" + GetColumnFilterByValue(paramstItem.sv, true) + ")",
                                     ColumnsMapping.StationDeliveringCodeSNG.GetStringValue(),
                                     " IN (" + GetColumnFilterByValue(paramstItem.sv, true) + ")");
                                }

                                break;
                            }
                        }
                    }
                    else if (filterName == ColumnsMapping.WagonType)
                    {
                       expressionWhere = GetWagonTypesWhereQuery(paramstItem);
                    }
                    else if (filterName == ColumnsMapping.CompanySendingEGRPO)
                    {
                        expressionWhere = ColumnsMapping.CompanySendingCode.GetStringValue() + " IN (" + GetColumnFilterByValue(paramstItem.sv, FilterNameHelper.CompanySendingFilterName,true) + ")";
                    }
                    else if (filterName == ColumnsMapping.CompanyRecipientERPGO)
                    {
                        expressionWhere = ColumnsMapping.CompanyRecipientCode.GetStringValue() + " IN (" + GetColumnFilterByValue(paramstItem.sv, FilterNameHelper.CompanyRecipientFilterName,true) + ")";
                    }
                    else
                    {
                        expressionWhere = filterName.GetStringValue() + " IN (" +
                                        GetColumnFilterByValue(paramstItem.sv) +
                                        ")";
                    }


                    sbWhere.Append(expressionWhere);
                    sbWhere.Append(Constants.AND);
                }
            }

            if (additionalWhere.Length > 0)
            {
                sbWhere.Append(additionalWhere);
                sbWhere.Append(Constants.AND);
            }

            if (sbWhere.Length > 0)
                sbWhere.Remove(sbWhere.Length - Constants.AND.Length, Constants.AND.Length);
            return sbWhere;
        }

        private static string GetWagonTypesWhereQuery(FilterParamstItemViewModel paramstItem)
        {
            string expressionWhere = "";
            string wagonTypeExpWhere = null;
            string containerExpWhere = null;
            string delimetersValues = FilterHelper.GetWagonTypeFilterByValue(paramstItem.sv).ToString();
            string[] selectedValues = delimetersValues.Split(',');

            List<string> containerList;
            Http.Get(FilterNameHelper.ContainerVolumeFilterName, out containerList);


            if (containerList == null)
            {
                FilterService.GetFilterListAll(FilterNameHelper.ContainerVolumeFilterName, "", "");
                Http.Get(FilterNameHelper.ContainerVolumeFilterName, out containerList);
                if (containerList == null)
                {
                    return "";
                }
            }
            //выбираем тоннажность
            List<WagonGroupType> listWagonTypes;
            Http.Get(FilterNameHelper.WagonTypeHierarhyListFilterKey, out listWagonTypes);

            if (listWagonTypes == null)
            {
                FilterService.GetFilterListAll(FilterNameHelper.WagonTypeHierarhyListFilterKey, "", "");
                Http.Get(FilterNameHelper.WagonTypeHierarhyListFilterKey, out listWagonTypes);
                if (listWagonTypes == null)
                {
                    return "";
                }
            }

            foreach (var value in selectedValues)
            {
                bool endSearch = false;
                foreach (var containerTypeItem in containerList)
                {
                    if (containerTypeItem.Trim() == value.Trim().Replace("'", ""))
                    {
                        containerExpWhere += value + ",";
                        endSearch = true;
                        break;
                    }
                }
                if (endSearch)
                    continue;

              //  foreach (var wagonTypeItem in listWagonTypes)
              //  {
                  //  if (wagonTypeItem.WagonTypes.ToString() == value.Trim().Replace("'", ""))
                 //   {
                        wagonTypeExpWhere += value + ",";
                  //  }
               // }
            }

            if (!string.IsNullOrEmpty(containerExpWhere))
            {
                expressionWhere = ColumnsMapping.ContainerVolume.GetStringValue() + " IN (" + containerExpWhere.Remove(containerExpWhere.Length-1,1) + ")";
            }
            
            if (!string.IsNullOrEmpty(wagonTypeExpWhere))
            {
                if (!String.IsNullOrEmpty(expressionWhere))
                {
                    expressionWhere += " " + Constants.AND;
                }

                expressionWhere += " " + ColumnsMapping.WagonType.GetStringValue() + " IN (" +
                    wagonTypeExpWhere.Remove(wagonTypeExpWhere.Length-1,1) +
                    ")";
            }
       
            return expressionWhere;
        }

        static readonly HttpCache Http = new HttpCache();
        private static string FindCalculatorFieldCode(string filterName, string selectedValue)
        {
            string foundStringValue = null;
              List<string> list;
             
            Http.Get(filterName, out list);
            if (list == null)
                return "";

            string[] selectedValueArr = selectedValue.Split('|');
            string selectedCode = selectedValueArr[0];

            foreach (var item in list)
            {
                string[] listArr = item.Split('|');
                string listName = listArr[0];

                if (listName.Trim() == selectedCode.Trim())
                {
                    foundStringValue = listArr[0].Trim();
                     break;
                }
            }
            return foundStringValue;
        }

        private enum SearchStationListVariantsEnum
        {
            [StringValue("выключено")] 
            None = 0,
            [StringValue("поиск по 2-м цифрам")]
            TwoDigitalCodeStation = 1,
            [StringValue("поиск по 3-м цифрам")]
            ThreeDigitalCodeStation = 2
        }

        private static StringBuilder GetGroupByString(IEnumerable<SelectedColumnsViewModel> selectedColumns,
            ViewTypeReport viewTypeReport, string prefix ="")
        {
            var groupingColumns = new StringBuilder();

            IList<SelectedColumnsViewModel> selectedColumnsViewModels =
                selectedColumns as IList<SelectedColumnsViewModel> ?? selectedColumns.ToList();

            bool existTonnKm = selectedColumns.Any(column => column.id == (int) ColumnsMapping.TonnKm);

            foreach (SelectedColumnsViewModel column in selectedColumnsViewModels)
            {
                ColumnsMapping columnName;
                if (Enum.TryParse(column.id.ToString(CultureInfo.InvariantCulture), true, out columnName))
                {
                    if (columnName == ColumnsMapping.QuantityVagon)
                        continue;
                    if (columnName == ColumnsMapping.QuantityContainer)
                        continue;
                    if (columnName == ColumnsMapping.VolumeTransportation)
                        continue;
                    if (columnName == ColumnsMapping.PaymentSum)
                        continue;
                    if (columnName == ColumnsMapping.CargoName)
                    {
                        if (viewTypeReport == ViewTypeReport.Svodnaya)
                        {
                            groupingColumns.Append(String.Format("Case When fb.Count > 1 Then 'Сборный груз' Else {0} end", prefix + ColumnsMapping.CargoName.GetStringValue()));
                        }
                    }
                    else if (columnName == ColumnsMapping.TonnKm && viewTypeReport == ViewTypeReport.Svodnaya)
                    {
                        groupingColumns.Append(ColumnsMapping.VolumeTransportation.GetStringValue());
                        groupingColumns.Append(",");
                        groupingColumns.Append(ColumnsMapping.DestinationTariff.GetStringValue());
                        
                        //if (viewTypeReport == ViewTypeReport.Svodnaya && existTonnKm &&
                        //    (columnName == ColumnsMapping.VolumeTransportation ||
                        //     columnName == ColumnsMapping.DestinationTariff))
                        //{
                        //    continue;
                        //}
                    } 
                    else
                    if (columnName == ColumnsMapping.QuantityContainer)
                    {
                        if (!ExistsColumn(selectedColumnsViewModels, ColumnsMapping.NumberContainer))
                            groupingColumns.Append(ColumnsMapping.NumberContainer.GetStringValue());
                        else
                        {
                            continue;
                        }
                    }
                    else if (columnName == ColumnsMapping.MonthTransportation)
                    {
                        groupingColumns.Append(String.Format("CAST ( MONTH( {0} ) as nvarchar(20)) + '.' + CAST ( YEAR( {0} ) as nvarchar(20)) ",
                          prefix + ColumnsMapping.DateSending.GetStringValue()));
                    }
                    else
                    {
                        groupingColumns.Append(prefix + columnName.GetStringValue());
                    }
                    groupingColumns.Append(",");
                }
            }
            return groupingColumns.Length > 0
                ? groupingColumns.Remove(groupingColumns.Length - 1, 1)
                : new StringBuilder();
        }

        private static StringBuilder GetColumnFilterByValue(IEnumerable<Filter> filtesList, bool neededCode = false, int startLengthCode = 0)
        {
            var sb = new StringBuilder();

            foreach (Filter filter in filtesList)
            {
                string[] values = filter.name.Split('|');
                if (neededCode == false)
                {
                     sb.Append(String.Format("'{0}',",
                     values.Length > 1 ? values[1].Trim() : filter.name));
                }
                else
                {
                    if (startLengthCode > 0)
                    {
                        sb.Append(String.Format("'{0}%',", values.Length > 0 ? values[0].Trim().Substring(0, startLengthCode) : filter.name));
                    }
                    else
                    {
                        sb.Append(String.Format("'{0}',", values[0].Trim())); 
                    }
                }
            }
            return sb.Remove(sb.Length - 1, 1);
        }
        private static StringBuilder GetColumnFilterByValue(IEnumerable<Filter> filtesList, string filterName, bool isSelectedGoGp = false)
        {
            var sb = new StringBuilder();
            if (isSelectedGoGp)
           {
               foreach (Filter filter in filtesList)
               {
                   sb.Append(GetCodes(filterName, filter.name));
               }
           }
           else
           {
               foreach (Filter filter in filtesList)
               {
                   sb.Append(String.Format("'{0}'", FindCalculatorFieldCode(filterName, filter.name)));
               }
           }
            return sb;
        }

        private static string GetCodes(string filterName, string selectedValue)
        {
            string foundStringValue = "";
            List<string> list;

            Http.Get(filterName, out list);
            if (list == null)
                return "";

            string[] selectedValueArr = selectedValue.Split('|');
            string selectedCode = selectedValueArr[0];

            foreach (var item in list)
            {
                string[] listArr = item.Split('|');
                string listName = listArr[0];

                if (listName.Trim().Substring(listName.Length - 9, 8) == selectedCode.Trim())
                {
                    foundStringValue += String.Format("'{0}',", listArr[0].Trim());
                    //  break;
                }
            }
            return foundStringValue.Length > 0 ? foundStringValue.Remove(foundStringValue.Length - 1, 1) : "";
        }
    }
}