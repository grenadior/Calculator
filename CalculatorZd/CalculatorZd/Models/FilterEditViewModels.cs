using System.Collections.Generic;
using System.Web.Mvc;
using BO;
using Common.Api;
using Common.Api.Extensions;
using Localization.WebResources.Common;
using Newtonsoft.Json;
using Filter = BO.Filter;

namespace CalculatorZd.Models
{
    public class CoefficientItemViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }
    }

    public class SelectedColumnsViewModel
    {
        public int id;

        public string name;
    }

    public class FilterParamstItemViewModel
    {
        /// <summary>
        /// FilterName
        /// </summary>
        [JsonProperty("filterId")]
        public int filterId { get; set; }

        /// <summary>
        /// CountValues
        /// </summary>
        [JsonProperty("cv")]
        public int cv { get; set; }
        /// <summary>
        /// SelectedValues
        /// </summary>
        [JsonProperty("sv")]
        public List<Filter> sv { get; set; }
    }

    public class CalculatorFilterSettingsViewModel
    {
        [JsonProperty("filterId")]
        public int filterId { get; set; }
        
        [JsonProperty("filterName")]
        public string filterName { get; set; }
       
        [JsonProperty("periodTransportation")]
        public string periodTransportation { get; set; }

        [JsonProperty("transportationType")]
        public string transportationType { get; set; }

        [JsonProperty("wagonType")]
        public string wagonType { get; set; }
        
        [JsonProperty("volumeType")]
        public string volumeType { get; set; }
    }

  
    public class FilterCalculatorResult
    {
        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("calcCoeffDetailReport")]
        public string CalcCoeffsDetailReport { get; set; }
    }

    public class FilterTypeViewModel
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

    }

    public class FilterTypeExistsModel
    {
        [JsonProperty("isExists")]
        public bool isExists;
    }

    public class FilterEditViewModels
    {
        public List<FilterTypeCoeffValue> Coefficients;
        public int FilterTypeID;
        public SelectList FiltersTypes;
    }

    public class FilterWagonTypeViewModel
    {
        [JsonProperty("ID")]
        public int ID;

        [JsonProperty("WagonGroup")]
        public string WagonGroup;

        [JsonProperty("WagonName")]
        public string WagonName;
    }

    public class FilterTransportationTypeViewModel
    {
        [JsonProperty("ID")]
        public int ID;

        [JsonProperty("Name")]
        public string Name;
    }
}