using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO.Implementation.Caching;
using BO.Implementation.Caching.Provider;
using BO.Implementation.Calculator;
using BO;
using BO.Implementation;
using CalculatorZd.Models;
using CalculatorZd.Services;
using Common.Api;
using Common.Api.Extensions;
using Common.Api.Types;
using DA.Filters;
using DA.Report;
using Utils;

namespace CalculatorZd.Controllers.api
{
    public class EntitiesController : ApiController
    {
        public IEnumerable<CoefficientItemViewModel> Get()
        {
            var list = new List<CoefficientItemViewModel>();
            for (int i = 1; i < 11; i++)
            {
                list.Add(new CoefficientItemViewModel
                {
                    Id = i,
                    Value = i
                });
            }
            return list;
        }


        [HttpGet]
        public FilterTypeExistsModel CheckExistsFilterItem(int filterTypeId, string value)
        {
            ColumnsMapping columnName;
            IEnumerable<FilterTypeViewModel> filterList = null;
            if (Enum.TryParse(filterTypeId.ToString(CultureInfo.InvariantCulture), true, out columnName))
            {
                string key = String.Format(CacheKeys.FILTER_TYPE_FORMAT_KEY, columnName);

                if (HttpRuntime.Cache[key] == null)
                {
                    if (columnName == ColumnsMapping.CODE_NAME_STATION_SENDING_RUS_SNG)
                    {
                        string[] columns =
                        {
                            CombineColumns(ColumnsMapping.StationSendingCodeRUS.GetStringValue(),
                                ColumnsMapping.StationSendingRUS.GetStringValue()),
                            CombineColumns(ColumnsMapping.StationSendingCodeSNG.GetStringValue(),
                                ColumnsMapping.StationSendingSNG.GetStringValue())
                        };

                        filterList = GetFilterListByFilter(columns, "", "");
                        AddToCache(key, filterList);
                    }
                    else if (columnName == ColumnsMapping.CODE_NAME_STATION_DELIVERING_RUS_SNG)
                    {
                        var columns = new[]
                        {
                            CombineColumns(ColumnsMapping.StationDeliveringCodeRUS.GetStringValue(),
                                ColumnsMapping.StationDeliveringRUS.GetStringValue()),
                            CombineColumns(ColumnsMapping.StationDeliveringCodeSNG.GetStringValue(),
                                ColumnsMapping.StationDeliveringSNG.GetStringValue())
                        };

                        filterList = GetFilterListByFilter(columns, "", "");
                        AddToCache(key, filterList);
                    }
                    else
                    {
                        filterList = FilterService.GetFilterListAll(columnName.GetStringValue());
                        AddToCache(key, filterList);
                    }
                }
                else
                {
                    filterList = (IEnumerable<FilterTypeViewModel>) HttpRuntime.Cache[key];
                }
            }

            bool isFound = false;
            if (filterList != null)
                foreach (var item in filterList)
                {
                    if (String.Equals(item.name.Trim(), value.Trim(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        isFound = true;
                        break;
                    }
                }

            return new FilterTypeExistsModel() { isExists = isFound };
        }

        private void AddToCache(string key, IEnumerable<FilterTypeViewModel> filterList)
        {
             HttpRuntime.Cache.Add(key, filterList , null, DateTime.Now.AddDays(1), TimeSpan.Zero,
                        CacheItemPriority.Normal, null);
        }
        private string CombineColumns(string s1, string s2)
        {
            return String.Format("{0} + ' | '+ {1}", s1, s2);
        }
        
        private IEnumerable<FilterTypeViewModel> GetFilterListByFilter(string[] filterName, string sWhere, string orderBy)
        {
            var items = CalculatorManager.GetEntities(filterName[0], filterName.Length > 1 ? filterName[1] : "", sWhere, orderBy);
            var types = new List<FilterTypeViewModel>();

            if (items == null)
                return null;
            
            foreach (var item in items)
            {
                if (item != null && !String.IsNullOrEmpty(item.Trim()))
                {
                    types.Add(new FilterTypeViewModel
                    {
                        name = item.Split('|')[1].Trim()
                    });
                }
            }

            return types;
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
     
        public FilterCalculatorResult GetCalculatorResult(string filters)
        {
            var serializer = new JavaScriptSerializer();

            var filtersItems = serializer.Deserialize<List<FilterParamstItemViewModel>>(filters);
            List<FilterCoefficient> listCoeffs = FilterManager.GetCoefficientsAll();
            var calcCoeffsDetailReport = new StringBuilder();
            decimal sumCoeffs = 0;
            bool periodAdded = false;
            for (int i = 0; i < filtersItems.Count; i++)
            {
                int countItemsInFilter = filtersItems[i].cv;
                if (countItemsInFilter == 0)
                    continue;

                int filterId = TypeConverter.ToInt32(filtersItems[i].filterId);
                if (filterId == (int) ColumnsMapping.CargoCode)
                {
                    filterId = (int) ColumnsMapping.CargoName;
                }

                ColumnsMapping columnName;
                if (Enum.TryParse(filterId.ToString(CultureInfo.InvariantCulture), true, out columnName))
                {
                    foreach (FilterCoefficient coeff in listCoeffs)
                    {
                        if (filterId == (int) ColumnsMapping.DateSending)
                        {
                            if (periodAdded)
                                continue;

                            DateTime dateBegin;
                            DateTime dateEnd;
                            DateTime.TryParse(filtersItems[i].sv[0].name, out dateBegin);
                            DateTime.TryParse(filtersItems[i].sv[1].name, out dateEnd);
                            TimeSpan dateDifferent = dateEnd - dateBegin;
                            int months = dateDifferent.GetMonths();
                            if (months > 0)
                            {
                                decimal coeffValue = GetCoeffBySelectedCount(months, listCoeffs, filterId);
                                sumCoeffs += coeffValue;
                                calcCoeffsDetailReport.Append(String.Format("{0}:{1} = {2}, ",
                                    columnName.GetStringValue(), months, coeffValue));
                                periodAdded = true;
                            }
                        }
                        else if (coeff.FilterTypeID == filterId)
                        {
                            decimal coeffValue = GetCoeffBySelectedCount(countItemsInFilter, listCoeffs, filterId);
                            sumCoeffs += coeffValue;
                            calcCoeffsDetailReport.Append(String.Format("{0}:{1} = {2}, ", columnName.GetStringValue(),
                                countItemsInFilter, coeffValue));
                            break;
                        }
                    }
                }
            }

            calcCoeffsDetailReport.Append(String.Format(" Итого:{0}", sumCoeffs));
            return new FilterCalculatorResult
            {
                Result =
                    String.Format("{0:f2}",
                        ServerProperties.Instance.SearchResultMainCoeff * (double) sumCoeffs),
                CalcCoeffsDetailReport = calcCoeffsDetailReport.ToString()
            };
        }

        private decimal GetCoeffBySelectedCount(int selectedItemsCount, List<FilterCoefficient> listCoeffs, int typeId)
        {
            decimal coeff = 0;
            FilterCoefficient highest = listCoeffs.Where(rs => rs.FilterTypeID == typeId)
                .OrderByDescending(rs => rs.CoefficientValue)
                .FirstOrDefault();

            if (highest != null && (selectedItemsCount == -1 || highest.CountItems < selectedItemsCount))
            {
                coeff = highest.CoefficientValue;
            }
            else
            {
                FilterCoefficient filterCoefficient = listCoeffs.FirstOrDefault(rs => rs.FilterTypeID == typeId && rs.CountItems == selectedItemsCount);

                if (filterCoefficient != null) coeff = filterCoefficient.CoefficientValue;
            }
            return coeff;
        }

        private int pageSize = 30;
        public SearchItemsResultViewModel GetSearchResultBySessionId(string sessionId)
        {
            SearchItemsResultViewModel searchItemsResultViewModel = null;
            int totalRowsCount = 0;
            GetSearchResult(CacheKeys.CACHE_CALC_REPORT_MODEL_KEY, sessionId, out searchItemsResultViewModel);
            GetSearchResult(CacheKeys.CACHE_CALC_REPORT_TOTAL_ROWS_COUNT_KEY, sessionId, out totalRowsCount);

            if (searchItemsResultViewModel != null)
            {
                searchItemsResultViewModel.TotalRecords = totalRowsCount;
                searchItemsResultViewModel.TotalPages = totalRowsCount / pageSize == 0 ? 1 : (totalRowsCount / pageSize) +
                                                   (totalRowsCount % pageSize != totalRowsCount
                                                       ? 1
                                                       : 0);
            }
            
            return searchItemsResultViewModel;
        }

        public SessionViewModel GetSessionId()
        {
            return new SessionViewModel() {sessionId = SessionId};
        }

        private void PutSearchResult(string key, string sessionId, object value)
        {
            var http = new HttpCache();
            http.Set(CachingHelper.GetReportCachKey(key, sessionId), value);
        }

        private void Clear(string key, string sessionId)
        {
            var http = new HttpCache();
            http.Clear(CachingHelper.GetReportCachKey(key, sessionId));
        }

        public void GetSearchResult<T>(string key, string sessionId, out T value)
        {
            var http = new HttpCache();
            http.Get(CachingHelper.GetReportCachKey(key, sessionId), out value);
        }

        public async Task<DownloadReportModel> GetDownloadReport(string sessionId)
        {
           
            Guid firmId = SessionManager.FirmInfo.ID;
            string path =
                System.Web.HttpContext.Current.Server.MapPath(String.Format("{0}", ServerProperties.Instance.ReportStoragePath));
         
            var taskResult = Task<DownloadReportResult>.Factory.StartNew(() =>
            {
                var status = DownloadReportProcess(path, sessionId, firmId);
                var downloadResult = new DownloadReportResult(sessionId, firmId, path, OperationStatus.Failure);

                downloadResult._StatusReport = status;
                return null;
            });
            var model = new DownloadReportModel { Status =  OperationStatus.Success };
            return model;
        }

        public bool GetSaveFilter(string filterName, string period, string transportationType, string wagonType,
                                  string volumeType, string cargoName, string cargoGroup, string companySending, string companyRecipient,
                                  string countrySending,string countryDelivering,string waySending,string wayDelivering,string stationSending,string stationDelivering,
                                  string subjectSending, string subjectDelivering, string ownerWagon, string payerWagon, string renter, string columns, string earlyTransportationCargo, string vagonType)
        {
            if (SessionManager.FirmInfo == null)
                return false;

            Guid firmid = SessionManager.FirmInfo.ID;
            var f = new CalculatorFirmFilter
            {
                FirmID = firmid,
                FilterName = filterName,
                PeriodTransportation = period,
                TransportationType = transportationType,
                WagonType = wagonType,
                VolumeType = volumeType,
                CargoName = cargoName,
                CargoGroup = cargoGroup,
                CompanySending = companySending,
                CompanyRecipient = companyRecipient,
                CountrySending = countrySending,
                CountryDelivering = countryDelivering,
                WaySending = waySending,
                WayDelivering = wayDelivering,
                StationSending = stationSending,
                StationDelivering = stationDelivering,
                SubjectSending = subjectSending,
                SubjectDelivering = subjectDelivering,
                OwnerWagon = ownerWagon,
                RenterWagon = renter,
                PayerWagon = payerWagon,
                Columns = columns,
                EarlyTransportationCargo = earlyTransportationCargo,
                VagonType = vagonType
            };
            bool success = FiltersAdapter.InsertCalculatorFirmFilter(f);
 
            return success;
        }

        public CalculatorFilterSettingsSearchResult GetCalcFilterSettings(int filterId)
        {
            if (SessionManager.FirmInfo == null)
                return null;

            var model = new CalculatorFilterSettingsSearchResult();
            
            Guid firmid = SessionManager.FirmInfo.ID;
            var f = FiltersAdapter.GetCalculatorFirmFilter(firmid, filterId);

            var filterItems = new CalcFilterSettingsItem()
            {
                FilterID = f.ID,
                periodTransportation = f.PeriodTransportation,
                TransportationType = f.TransportationType,
                VolumeType = f.VolumeType,
                WagonType = f.WagonType,
                CargoName = f.CargoName,
                CargoGroup = f.CargoGroup,
                CompanyRecipient = f.CompanyRecipient,
                CompanySending = f.CompanySending,
                CountryDelivering = f.CountryDelivering,
                CountrySending = f.CountrySending,
                WaySending = f.WaySending,
                WayDelivering = f.WayDelivering,
                SubjectSending = f.SubjectSending,
                SubjectDelivering = f.CountryDelivering,
                StationSending = f.StationSending,
                StationDelivering = f.StationDelivering,
                OwnerWagon = f.OwnerWagon,
                PayerWagon = f.PayerWagon,
                RenterWagon = f.RenterWagon,
                Columns = f.Columns,
                EarlyTransportationCargo = f.EarlyTransportationCargo,
                VagonType = String.IsNullOrEmpty(f.VagonType) ? "0": f.VagonType
            };
               
            model.FilterItems = filterItems;
            return model;
        }

        public bool GetDeleteFilter(int filterId)
        {
             return FiltersAdapter.DeleteFilter(filterId);
        }
        public CalculatorFiltersSearchResult GetCalcFilterList()
        {
            if (SessionManager.FirmInfo == null)
                return null;

            var model = new CalculatorFiltersSearchResult();
            var filterItems = new List<CalcFilterItem>();
            Guid firmid = SessionManager.FirmInfo.ID;
            var dbfl = FiltersAdapter.GetCalculatorFirmFilterList(firmid);
            foreach (var filterItem in dbfl)
            {
                filterItems.Add(new CalcFilterItem() { FilterID = filterItem.ID, FilterName = filterItem.FilterName });
            }
            model.FilterItems = filterItems;
            return model;
        }
        
        public SearchItemsResultViewModel GetCalculatorSearchResult(int pageId, string filters, string selectedColumnsFilter, string sessionId, string vagonSourceTypeParam)
        {
            var serializer = new JavaScriptSerializer();
            var selectedColumns = serializer.Deserialize<List<SelectedColumnsViewModel>>(selectedColumnsFilter);
            if (selectedColumns.Count == 0)
                return null;

            VagonSourceTypeParamEnum dbType = VagonSourceTypeParamEnum.Gruzhon;
            if (vagonSourceTypeParam == "порожний рейс")
            {
                dbType = VagonSourceTypeParamEnum.Porozhn;
            }

            int totalRowsCount = 0;

            if (pageId > 1)
            {
                GetSearchResult(CacheKeys.CACHE_CALC_REPORT_TOTAL_ROWS_COUNT_KEY, SessionId, out totalRowsCount);
            }
            else
            {
                Clear(CacheKeys.CACHE_CALC_REPORT_QUERY_FULL_KEY, SessionId);
                Clear(CacheKeys.CACHE_CALC_REPORT_DATATABLE_KEY, SessionId);
                Clear(CacheKeys.CACHE_CALC_REPORT_MODEL_KEY, SessionId);   
            }
            

            Task<SearchResult> taskSearch = Task<SearchResult>.Factory.StartNew(() =>
            {
                const int pageSize = 30;
                var filterParamstItems = serializer.Deserialize<List<FilterParamstItemViewModel>>(filters);

                if (filterParamstItems.Count == 0)
                    return null;
                string allQuerySummary = "";
                //select 
                int pageIdFrom = (pageId - 1) * pageSize;
                int pageIdTo = pageId  * pageSize;

                ViewTypeReport viewTypeReport;
                string queryForExelReport;
                string allSelect = CalculatorService.GetQueryReport(selectedColumns, filterParamstItems, pageIdFrom, pageIdTo, out allQuerySummary, out viewTypeReport, out queryForExelReport, dbType);
                
                DataTable dt = CalculatorManager.GetCalculatorResultByFilter(allSelect, pageId == 1 ? allQuerySummary : "", out totalRowsCount);
            
                var searchResult = new SearchResult(dt, allSelect, queryForExelReport,  totalRowsCount);
                searchResult.SearchItemsModel = GetSearchItemsViewModel(searchResult.SearchResultDataTable, pageSize, pageId, totalRowsCount,  viewTypeReport);

               return searchResult;
            });
            taskSearch.Result.SearchItemsModel.Query = taskSearch.Result.Query;
           
            if (taskSearch.Result.SearchResultDataTable != null)
            {
                //full select
                PutSearchResult(CacheKeys.CACHE_CALC_REPORT_QUERY_FULL_KEY, SessionId,
                    taskSearch.Result.QueryForExelReport);
                //filtered datatable
                PutSearchResult(CacheKeys.CACHE_CALC_REPORT_DATATABLE_KEY, SessionId,
                    taskSearch.Result.SearchResultDataTable);
                //filtered model
                PutSearchResult(CacheKeys.CACHE_CALC_REPORT_MODEL_KEY, SessionId, taskSearch.Result.SearchItemsModel);
                
                if(pageId == 1)
                    PutSearchResult(CacheKeys.CACHE_CALC_REPORT_TOTAL_ROWS_COUNT_KEY, SessionId, taskSearch.Result.TotalRowsCount);
            }
           
            return null;
        }

        private string SessionId
        {
            get { return HttpContext.Current.Session.SessionID; }
        }

        class SearchResult
        {
            public readonly DataTable SearchResultDataTable;
            public SearchItemsResultViewModel SearchItemsModel;
            public string Query;
            public string QueryForExelReport;
            public int TotalRowsCount;
           
            public SearchResult(DataTable searchResultDataTable, string query, string queryForExelReport, int totalRowsCount)
            {
                SearchResultDataTable = searchResultDataTable;
                Query = query;
                QueryForExelReport = queryForExelReport;
                TotalRowsCount = totalRowsCount;
            }
        }

        class DownloadReportResult
        {
            public string _SessionId;
            public Guid _FirmId;
            public string _PathStorageReport;
            public OperationStatus _StatusReport ;
            public DownloadReportResult(string sessionId, Guid firmId, string path, OperationStatus statusReport)
            {
                _SessionId = sessionId;
                _FirmId = firmId;
                _PathStorageReport = path;
                _StatusReport = statusReport;
            }
        }

        public OperationStatus DownloadReportProcess(string pathStorageReport, string sessionId, Guid firmId)
        {
            string allSelect = "";
            DataTable dt;
            SearchItemsResultViewModel model;
            GetSearchResult(CacheKeys.CACHE_CALC_REPORT_QUERY_FULL_KEY, sessionId, out allSelect);
            GetSearchResult(CacheKeys.CACHE_CALC_REPORT_DATATABLE_KEY, sessionId, out dt);
              GetSearchResult(CacheKeys.CACHE_CALC_REPORT_MODEL_KEY, sessionId, out model);
            string fileName = String.Format("{0}.xls", Guid.NewGuid());

            Guid reportId;
            if (dt != null && dt.Rows.Count > 0)
            {
                reportId = ReportAdapter.AddReportFileNameByFirm(firmId, fileName, (int)StatusProcess.Process);
            }
            else
            {
                return OperationStatus.Failure;
            }
            if (String.IsNullOrEmpty(allSelect))
                return OperationStatus.Failure;

            const int iterationRowCount = 50000;
            bool next = true;
            const string whereTemplate = "WHERE  {2}ID BETWEEN {0} AND {1}";
            int from = 1;
            int to = iterationRowCount;
            string path = pathStorageReport;
            long fileLength = 0;
            try
            {
                FileStream file = System.IO.File.Create(path + "/" + fileName);
                int indexOrderBy = allSelect.IndexOf("Order By", System.StringComparison.Ordinal);
                bool existOrderBy = indexOrderBy > 0;
                string query = "";
                string orderBy = "";
                if (existOrderBy)
                {
                    query = allSelect.Substring(0, indexOrderBy);
                    orderBy = allSelect.Substring(indexOrderBy, allSelect.Length - indexOrderBy);
                }
                else
                {
                    query = allSelect;
                }

                while (next)
                {
                    string where = string.Format(whereTemplate, from, to, model.ReportType == ViewTypeReport.Svodnaya ? QueryUtils.Prefix:"");
                    int totalRowsCount;
                    string allQuery = String.Format("{0} {1} {2}", query, where, orderBy);

                    if (model.ReportType == ViewTypeReport.Svodnaya)
                        allQuery += QueryUtils.DropTempTableQuery;

                    DataTable dtTable = CalculatorManager.GetCalculatorResultByFilter(allQuery, "", out totalRowsCount);

                    if (dtTable == null || dtTable.Rows.Count == 0 || dtTable.Rows.Count < to)
                        next = false;
                    else
                    {
                        from = from + iterationRowCount;
                        to = to + iterationRowCount;
                    }

                    GridView dgGrid = new GridView();
                    dgGrid.DataSource = dtTable;
                    dgGrid.DataBind();

                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    var dnl = new DownloadFileActionResult(dgGrid, "Report.xls");

                    dnl.ExcelGridView.RenderControl(htw);

                    //Open a memory stream that you can use to write back to the response
                    byte[] byteArray = Encoding.UTF8.GetBytes(sw.ToString());

                    file.Write(byteArray, 0, byteArray.Length);
                    
                    if (!next)
                    {
                        fileLength = file.Length;
                    }
                }

                file.Close();
                ReportAdapter.UpdateReportStatusById(reportId, (int)StatusProcess.Ready, fileLength);
               return OperationStatus.Success;
            }
            catch (Exception ex)
            {
                ReportAdapter.UpdateReportStatusById(reportId, (int)StatusProcess.Error, 0);
                return OperationStatus.Failure;
            }
        }

        private SearchItemsResultViewModel GetSearchItemsViewModel(DataTable dt, int pageSize, int pageId, int totalRowsCount, ViewTypeReport viewTypeReport)
        {
            var searchItemsResultViewModel = new SearchItemsResultViewModel();
            foreach (DataRow dr in dt.Rows)
            {
                var itemsViewModel = new ItemsViewModel();
                
                foreach (DataColumn column in dt.Columns)
                {
                    itemsViewModel.ValuesItemViewModel.Add(new ValueItemViewModel
                    {
                        Value = dr[column.Caption].ToString()
                    });
                }
              
                searchItemsResultViewModel.SearchItems.Add(itemsViewModel);
            }

            IEnumerable<HeaderItemViewModel> columns = from DataColumn c in dt.Columns
                                                       select new HeaderItemViewModel { Name = c.ColumnName };
            searchItemsResultViewModel.Headers = columns.ToList();

            searchItemsResultViewModel.TotalPages = totalRowsCount / pageSize == 0 ? 1 : (totalRowsCount / pageSize) +
                                                    (totalRowsCount % pageSize != totalRowsCount
                                                        ? 1
                                                        : 0);

            var items = new SearchItemsResultViewModel();
            //IEnumerable<ItemsViewModel> query =
            //    searchItemsResultViewModel.SearchItems.Skip((pageId - 1) * pageSize).Take(pageSize);
            IEnumerable<ItemsViewModel> query = searchItemsResultViewModel.SearchItems.AsQueryable();
        

            items.SearchItems = query.ToList();
            items.TotalPages = searchItemsResultViewModel.TotalPages;
            items.TotalRecords = totalRowsCount;
            items.Headers = searchItemsResultViewModel.Headers;
            items.CurrentPageId = pageId;
            items.ReportType = viewTypeReport;
          
            return items;
        }
        
        public IEnumerable<ColumnsSearchResultViewModel> GetColumnsSearchResultUrl()
        {
            var items = new List<ColumnsSearchResultViewModel>();

            List<FiltersTypes> list = FilterManager.GetFiltersTypes();

            foreach (FiltersTypes l in list)
            {
                items.Add(new ColumnsSearchResultViewModel
                {
                    id = l.FilterTypeID,
                    Name = l.FilterTypeName
                });
            }

            return items;
        }

        public static IEnumerable<bool> GetColumnNameById(int id)
        {
            return
                Enum.GetNames(typeof (ColumnsMapping))
                    .Select(r => r == ColumnsMapping.TransportationType.GetStringValue());
        }


    }
}