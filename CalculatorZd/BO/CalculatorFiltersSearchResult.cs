using System.Collections.Generic;
using Newtonsoft.Json;

namespace BO
{
    public class CalculatorFiltersSearchResult
    {
        public CalculatorFiltersSearchResult()
        {
            FilterItems = new List<CalcFilterItem>();
        }

        [JsonProperty("items")]
        public List<CalcFilterItem> FilterItems { get; set; }
    }

    public class CalcFilterItem
    {
        [JsonProperty("filterName")]
        public string FilterName { get; set; }

        [JsonProperty("filterId")]
        public int FilterID { get; set; }
    }


    public class CalculatorFilterSettingsSearchResult
    {
        public CalculatorFilterSettingsSearchResult()
        {
            FilterItems = new CalcFilterSettingsItem();
        }

        [JsonProperty("items")]
        public CalcFilterSettingsItem FilterItems { get; set; }
    }

    public class CalcFilterSettingsItem
    {
        [JsonProperty("filterId")]
        public int FilterID { get; set; }

        [JsonProperty("periodTransportation")]
        public string periodTransportation { get; set; }

        [JsonProperty("transportationType")]
        public string TransportationType { get; set; }

        [JsonProperty("wagonType")]
        public string WagonType { get; set; }

        [JsonProperty("volumeType")]
        public string VolumeType { get; set; }

        [JsonProperty("cargoName")]
        public string CargoName { get; set; }

        [JsonProperty("cargoGroup")]
        public string CargoGroup { get; set; }

        [JsonProperty("companyRecipient")]
        public string  CompanyRecipient { get; set; }

        [JsonProperty("companySending")]
        public string CompanySending { get; set; }

        [JsonProperty("countrySending")]
        public string CountrySending { get; set; }

        [JsonProperty("countryDelivering")]
        public string CountryDelivering { get; set; }

        [JsonProperty("waySending")]
        public string WaySending { get; set; }

        [JsonProperty("wayDelivering")]
        public string WayDelivering { get; set; }

        [JsonProperty("stationSending")]
        public string StationSending { get; set; }

        [JsonProperty("stationDelivering")]
        public string StationDelivering { get; set; }

        [JsonProperty("subjectSending")]
        public string SubjectSending { get; set; }

        [JsonProperty("subjectDelivering")]
        public string SubjectDelivering { get; set; }

        [JsonProperty("ownerWagon")]
        public string OwnerWagon { get; set; }

        [JsonProperty("renterWagon")]
        public string RenterWagon { get; set; }

        [JsonProperty("payerWagon")]
        public string PayerWagon { get; set; }
        
        [JsonProperty("columns")]
        public string Columns { get; set; }

        [JsonProperty("earlyTransportationCargo")]
        public string EarlyTransportationCargo { get; set; }

        [JsonProperty("vagonType")]
        public string VagonType { get; set; }
    }

}
