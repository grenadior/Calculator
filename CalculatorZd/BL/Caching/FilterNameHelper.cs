using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BL.Calculator;
using BO;
using Common.Api.Extensions;

namespace BL.Caching
{
    public class FilterNameHelper
    {
        public static string CargoNameFilterName
        {
            get
            {
                return CalculatorHelper.CombineColumns(ColumnsMapping.CargoCode.GetStringValue(),
                    ColumnsMapping.CargoName.GetStringValue());
            }
        }
        
        const string FormatField = "SUBSTRING({0}, LEN({0}) - 7, 8)";
        public static string CompanySendingFilterName
        {
            get
            {
              return  CalculatorHelper.CombineColumns(
                    String.Format(FormatField, ColumnsMapping.CompanySendingCode.GetStringValue()),
                    ColumnsMapping.CompanySendingEGRPO.GetStringValue());
            }
        }

        public static string CompanyRecipientFilterName
        {
            get
            {
              return  CalculatorHelper.CombineColumns(
                    String.Format(FormatField, ColumnsMapping.CompanyRecipientCode.GetStringValue()),
                    ColumnsMapping.CompanyRecipientERPGO.GetStringValue());
            }
        }
        public static string ContainerVolumeFilterName
        {
            get
            {
                return ColumnsMapping.ContainerVolume.GetStringValue();
            }
        }

        public static string SubjectsSendingFilterName
        {
            get { return ColumnsMapping.SubjectSendingRUS.GetStringValue(); }
        }
        public static string WaySendingFilterName
        {
            get { return ColumnsMapping.WaySending.GetStringValue(); }
        }
        public static string CargoGroupNamesFilterName
        {
            get { return ColumnsMapping.CargoGroupName.GetStringValue(); }
        }
        
        public static string StationsSendingFilterKey
        {
            get
            {
                return "StationsSendingFilterNameCacheKey";
            }
        }

        public static string WagonTypeFilterKey
        {
            get
            {
                return "WagonTypesCacheKey";
            }
        }

        public static string WagonTypeHierarhyListFilterKey
        {
            get
            {
                return "WagonTypeHierarhyListFilterCacheKey";
            }
        }


        public static string CountriesDeliveringFilterName
        {
            get { return ColumnsMapping.CountryDelivering.GetStringValue(); }
        }

        public static string CountriesSendingFilterName
        {
            get { return ColumnsMapping.CountrySending.GetStringValue(); }
        }

        public static string[] StationsSendingFilterNameArr
        {
            get
            {
                string[] columns =
                {
                    CalculatorHelper.CombineColumns(ColumnsMapping.StationSendingCodeRUS.GetStringValue(),
                        ColumnsMapping.StationSendingRUS.GetStringValue()),
                    CalculatorHelper.CombineColumns(ColumnsMapping.StationSendingCodeSNG.GetStringValue(),
                        ColumnsMapping.StationSendingSNG.GetStringValue())

                };
                return columns;
            }
        }

        public static string[] StationsDeliveringFilterNameArr
        {
            get
            {
                string[] columns =
                {
                    CalculatorHelper.CombineColumns(ColumnsMapping.StationDeliveringCodeRUS.GetStringValue(),
                        ColumnsMapping.StationDeliveringRUS.GetStringValue()),
                    CalculatorHelper.CombineColumns(ColumnsMapping.StationDeliveringCodeSNG.GetStringValue(),
                        ColumnsMapping.StationDeliveringSNG.GetStringValue())
                };
                return columns;
            }
        }
        public static string StationsDeliveringFilterKey
        {
            get
            {
                return "StationsDeliveringFilterNameCacheKey";
            }
        }

        public static string WayDeliveringFilterName
        {
            get { return ColumnsMapping.WayDelivering.GetStringValue(); }
        }

        public static string SubjectsDeliveringFilterName
        {
            get { return ColumnsMapping.SubjectDeliveringRUS.GetStringValue(); }
        }

        public static string PayerFilterName
        {
            get { return ColumnsMapping.PayerTariff.GetStringValue(); }
        }
        public static string RenterVagonGVCFilterName
        {
            get { return ColumnsMapping.RenterVagonGVC.GetStringValue(); }
        }
        public static string OwnerVagonEGRPOFilterName
        {
            get { return ColumnsMapping.OwnerVagonEGRPO.GetStringValue(); }
        }

        public static string TransportationTypeFilterName
        {
            get { return ColumnsMapping.TransportationType.GetStringValue(); }
        } 
        public static string CompanySendingEGRPOFilterName
        {
            get { return ColumnsMapping.CompanySendingEGRPO.GetStringValue(); }
        }
    }
}

