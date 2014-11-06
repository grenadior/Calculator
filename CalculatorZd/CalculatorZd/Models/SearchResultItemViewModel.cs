using System;
using System.Collections.Generic;
using Common.Api;
using Newtonsoft.Json;

namespace CalculatorZd.Models
{
    public class SearchItemsResultViewModel
    {
        public SearchItemsResultViewModel()
        {
            SearchItems = new List<ItemsViewModel>();
        }

        [JsonProperty("sessionId")]
        public string SessionId;

        [JsonProperty("CurrentPageId")]
        public int CurrentPageId { get; set; }

        public string Query { get; set; }

        [JsonProperty("TimeProgress")]
        public string TimeProgress;

        [JsonProperty("totalPages")]
        public decimal TotalPages { get; set; }

        [JsonProperty("totalRecords")]
        public decimal TotalRecords { get; set; }

        [JsonProperty("items")]
        public List<ItemsViewModel> SearchItems { get; set; }
        [JsonProperty("headers")]
        public List<HeaderItemViewModel> Headers { get; set; }

        public ViewTypeReport ReportType;
    }

    public class ValueItemViewModel
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class DownloadReportModel
    {
        [JsonProperty("status")]
        public OperationStatus Status { get; set; }
    }


    public class ItemsViewModel
    {
        public ItemsViewModel()
        {
            ValuesItemViewModel = new List<ValueItemViewModel>();
        }

        [JsonProperty("id")]
        public Int64 Id { get; set; }
    
        [JsonProperty("values")]
        public List<ValueItemViewModel> ValuesItemViewModel { get; set; }
     
    }

    public class HeaderItemViewModel
    {
        [JsonProperty("header")]
        public string Name { get; set; }
    }

    public class ColumnsSearchResultViewModel
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class SessionViewModel
    {
         [JsonProperty("sessionId")]
        public string sessionId { get; set; }
        
    }
}
