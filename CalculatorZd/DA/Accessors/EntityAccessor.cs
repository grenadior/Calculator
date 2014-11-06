using System.Data;
using BLToolkit.DataAccess;
using BO;
using System;
using System.Collections.Generic;
using Common.Api;


namespace DA.Accessors
{
    public abstract class EntityAccessor : DataAccessor<Entity>
    {
        [SprocName("p_Gr_GetByID")]
        public abstract List<String> GetEntitiesByID(string @columnName, string @columnName2, string @whereExp, string @orderByExp, string @dbname);

        [SprocName("p_FiltersTypes_Get")]
        public abstract List<FiltersTypes> GetFiltersTypes();

        [SprocName("p_FiltersTypesSetting_Get")]
        public abstract List<FiltersTypes> GetFiltersTypesSettings();

        [SprocName("p_FilterCoefficients_Get")]
        public abstract List<FilterCoefficient> GetFilterCoefficients(int? @filterTypeID);

        [SprocName("p_FirmFilters_Get")]
        public abstract List<CalculatorFirmFilter> GetCalculatorFirmFilter(int @filterID, Guid @firmID);

        [SprocName("p_FirmFilters_Delete")]
        public abstract void DeleteFilter(int @filterID);

        [SprocName("p_FirmFilters_GetAll")]
        public abstract List<CalcFirmSettings> CalcFirmSettingsList(Guid @firmID);

        [SprocName("p_FirmFilters_Insert")]
        public abstract Guid InsertFirmFilter(Guid @firmId, string @filterName, string @periodTransportation, string @transportationType, string @wagonType,
            string @volumeType, string @cargoName, string @cargoGroup, string @companySending, string @companyRecipient,
             string @countrySending,
             string @countryDelivering,
             string @stationSending,
             string @stationDelivering,
             string @waySending,
             string @wayDelivering,
             string @subjectDelivering,
             string @subjectSending,
             string @ownerWagon,
             string @payerWagon,
             string @renterWagon,
             string @columns,
             string @earlyTransportationCargo, string @vagonType,
            [ParamName("id"), Direction.Output] out int id);
        

        [SprocName("p_CalculatorResult_Get")]
        public abstract DataTable GetCalculatorResultByFilter(string @allQuery, string @allQuerySummary);
        
        [SprocName("p_WagonTypes_Get")]
        public abstract DataSet GetWagonTypes();

        [SprocName("p_Report_InsertByFirm")]
        public abstract Guid SaveReportFileNameByFirm(Guid @firmId, string @fileName, int @status, [ParamName("id"), Direction.Output] out Guid id);

        [SprocName("p_Report_UpdateStatus")]
        public abstract bool UpdateReportStatusById(Guid @Id, int @status, long @fileSize);
        
        [SprocName("p_Report_GetByFirm")]
        public abstract DataTable GetReportsByFirm(Guid @firmId);
        
    }
}
