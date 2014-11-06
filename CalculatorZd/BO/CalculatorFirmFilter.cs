using System;

namespace BO
{
    public class CalculatorFirmFilter
    {
        public int ID { get; set; }
        public Guid FirmID { get; set; }
        public string FilterName { get; set; }
        public string PeriodTransportation { get; set; }
        public string WagonType { get; set; }
        public string TransportationType { get; set; }
        public string VolumeType { get; set; }
        public string CargoName { get; set; }
        public string CargoGroup { get; set; }
        public string CompanySending { get; set; }
        public string CompanyRecipient { get; set; }
        public string CountrySending { get; set; }
        public string CountryDelivering { get; set; }

        public string WaySending { get; set; }
        public string WayDelivering { get; set; }
        public string StationSending { get; set; }
        public string StationDelivering { get; set; }
        public string SubjectSending { get; set; }
        public string SubjectDelivering { get; set; }
        public string OwnerWagon { get; set; }
        public string RenterWagon { get; set; }
        public string PayerWagon { get; set; }
        public string Columns { get; set; }
        public string EarlyTransportationCargo { get; set; }
        public string VagonType { get; set; }
    }

    public class CalcFirmSettings 
    {
        public int ID { get; set; }

        public string FilterName { get; set; }
    }
}