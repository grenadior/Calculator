using System;
using System.ComponentModel;

namespace Common.Api
{
    public static class Constants
    {
        public const string SELECT_VALUES = "Выберите значение";
        public const string ENTER_VALUES = "Введите значение";
        public const string DefaultCountryName = "РОССИЯ";
        public static int PassLifeTimeDays = 365;
        public static int ExpiredDateActivationEmail = 30;
        public static string SendEmailActivationCodeAction = "SendEmailActivationCode";
        public const string AND = " AND ";
        public const string OR = " OR ";
        public const string Top150 = "TOP 150";

    }

    public enum VagonSourceTypeParamEnum
    {
        Gruzhon,
        Porozhn
    }

    public enum ViewTypeReport
    {
        None,
        Nakladnaya,
        Svodnaya
    }

    public enum StatusProcess
    {
        None = 0,
        Process = 1,
        Ready = 2,
        Error = 3
    }
    public class CacheKeys
    {
        public const string CACHE_CALC_REPORT_QUERY_FULL_KEY = "CalculatorReportQueryFull";
        public const string CACHE_CALC_REPORT_DATATABLE_KEY = "CalculatorReportDataTable";
        public const string CACHE_CALC_REPORT_MODEL_KEY = "CalculatorReportModel";
        public const string CACHEN_CALC_REPORT_KEY = "CalculatorReport";
        public const string CACHE_CALC_REPORT_TOTAL_ROWS_COUNT_KEY = "CalculatorReport_totalRowsCount";

        public const string FILTER_TYPES_CACHE_KEY = "FilterTypes_Cache_Key";
        public const string FILTER_TYPE_FORMAT_KEY = "FilterType_{0}_Key";
    }

    [Flags]
    public enum BracketType
    {
        Round = 1,
        Square = 2,
        Brace = 4,
        Angle = 8
    }

    public enum TimeFrame
    {
        None = 0,
        Hour,
        ThreeHours,
        Today,
        ThreeDays,
        Week
    }

    public class EmailConstants
    {
        //public static string EmailSMTPClient = "smtp.gmail.com";
        //public static string formMail = "bpro80@gmail.com";
        public static string fromMail_Password = "Nikolay123";
    }

    public class BaseSum
    {
        public static decimal SUMM = 1;
    }
    
    public enum EmailActivationStatus
    {
        Invalid,
        Success
    }

    public enum OperationStatus
    {
        Failure,
        Success
    }

    public enum ActivationType
    {
        None = 0,
        ActivationEmail = 1,
        ActivationChangePassword = 2,
        ChangeEmail = 3
    }
}