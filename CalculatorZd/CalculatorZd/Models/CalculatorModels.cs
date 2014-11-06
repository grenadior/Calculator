using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutoCompleteSample.Infrastructure;
using BO;

namespace CalculatorZd.Models
{
    public class CalculatorViewModel
    {
        public DateTime DateBegin;
        public DateTime DateEnd;

        [Display(Name = "Номер вагона")]
        public String NumberVagon { get; set; }

        public String TransportationTypeID { get; set; }
        [Display(Name = "Тип перевозки")]
        public SelectList TransportationType { get; set; }

        [Display(Name = "Наименование груза или код ЕТСНГ")]
        public string CargoName { get; set; }

        [Display(Name = "Ранее перевозимый груз")]
        public string CargoNameEarlyTransportation { get; set; }

        [Display(Name = "Наименование или ОКПО")]
        public string OKPO { get; set; }

        [Display(Name = "Наименование группы груза")]
        public string CargoGroupName { get; set; }

        [Display(Name = "Страна отправления")]
        public string CountrySending { get; set; }

        [Display(Name = "Страна назначения")]
        public string CountryDelivering { get; set; }
        
        [Display(Name = "Дорога отправления")]
        public string WaySending { get; set; }

        [Display(Name = "Дорога назначения")]
        public string WayDelivering { get; set; }


        [Display(Name = "Субъект отправления")]
        public string SubjectSending { get; set; }

        [Display(Name = "Субъект назначения")]
        public string SubjectDelivering { get; set; }

        [Display(Name = "Станция отправления РФ/СНГ (код станции)")]
        public string StationSendingCode { get; set; }

        [Display(Name = "Станция назначения РФ/СНГ (код станции)")]
        public string CodeNameStationDelivering { get; set; }


        [Display(Name = "Компания")]
        public string Company { get; set; }

        [Display(Name = "Тонажность контейнера")]
        public string ContainerVolume { get; set; }

        [Display(Name = "Подрод вагона")]
        public string VagonType { get; set; }

        [Display(Name = "Собственник вагона")]
        public string OwnerVagon { get; set; }

        [Display(Name = "Плательщик тарифа ")]
        public string Payer { get; set; }

        [Display(Name = "Арендатор вагона")]
        public string RenterVagon { get; set; }

        [Display(Name = "Объем перевозки тонн")]
        public bool IsIncludeVolumeTransportation_ViewFree { get; set; }

        [Display(Name = "Объем перевозки тонн")]
        public bool IsIncludeVolumeTransportation_ViewNakladnaya { get; set; }

        [Display(Name = "ПРОВОЗНАЯ ПЛАТА (БЕЗ НДС)")]
        public bool IsIncludeTransportationPay_ViewFree { get; set; }

        [Display(Name = "ПРОВОЗНАЯ ПЛАТА (БЕЗ НДС)")]
        public bool IsIncludeTransportationPay_ViewNakladnaya { get; set; }

        [Display(Name = "ТОННО-КИЛОМЕТРЫ")]
        public bool IsIncludeTonnKilometer_ViewFree { get; set; }

        [Display(Name = "РАССТОЯНИЕ ПЕРЕВОЗКИ")]
        public bool IsIncludeDistance_ViewNakladnaya { get; set; }
 
        public CalculatorFiltersSearchResult FilterList { get; set; }
    }
}