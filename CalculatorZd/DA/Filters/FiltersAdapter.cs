using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BO;
using Common.Api;
using Configuration;
using DA.Accessors;

namespace DA.Filters
{
    public class FiltersAdapter
    {
        public static List<WagonGroupType> GetWagonTypes()
        {
            List<WagonGroupType> wagonTypeses = null;
            DataSet ds;

            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);

                try
                {
                    ds = fa.GetWagonTypes();
                    if (ds.Tables.Count > 0)
                    {
                        wagonTypeses = new List<WagonGroupType>();
                        foreach (DataRow groupRow in ds.Tables[0].Rows)
                        { 
                            var gr = new WagonGroupType();
                            gr.WagonGroupID = groupRow.Field<int>("WagonGroupID");
                            gr.WagonGroupName = groupRow.Field<string>("WagonGroupName");

                            
                            //type.WagonTypeID = gr.WagonGroupID;
                            //type.WagonTypeName = String.Format("{0}",gr.WagonGroupName);
                            //gr.WagonTypes.Add(type);

                            foreach (DataRow wagonTypeRow in ds.Tables[1].Rows)
                            { 
                                var type = new WagonTypes();
                                if (wagonTypeRow.Field<int>("WagonGroupID") == groupRow.Field<int>("WagonGroupID"))
                                {
                                    type = new WagonTypes();
                                    type.WagonTypeID = wagonTypeRow.Field<int>("WagonTypeID");
                                    type.WagonTypeName = String.Format("{0}", wagonTypeRow.Field<string>("WagonTypeName")); 
                                    gr.WagonTypes.Add(type);
                                }
                              
                            }
                            wagonTypeses.Add(gr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return wagonTypeses;
        }


        public static List<FiltersTypes> GetFiltersTypes()
        {
            List<FiltersTypes> entities = null;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);

                try
                {
                    entities = fa.GetFiltersTypes();
                }
                catch (Exception ex)
                {
                   // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return entities;
        }
        public static List<FiltersTypes> GetFiltersTypesSettings()
        {
            List<FiltersTypes> entities = null;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);

                try
                {
                    entities = fa.GetFiltersTypesSettings();
                }
                catch (Exception ex)
                {
                   // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return entities;
        }
        public static List<FilterCoefficient> GetCoefficientsByType(int? filterTypeID)
        {
            List<FilterCoefficient> entities = null;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);

                try
                {
                    entities = fa.GetFilterCoefficients(filterTypeID);
                }
                catch (Exception ex)
                {
                   // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return entities;
        }

        public static List<FilterCoefficient> GetCoefficientsAll()
        {
            List<FilterCoefficient> entities = null;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);

                try
                {
                    entities = fa.GetFilterCoefficients(null);
                }
                catch (Exception ex)
                {
                   // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return entities;
        }

        public static bool InsertCoefficients(List<FilterCoefficient> list, int typeId)
        {
            bool success = false;
            using (var db = new DbManager())
            {
                try
                {
                    db.SetSpCommand("p_FilterCoefficients_Delete",
                        db.Parameter("FilterTypeID", typeId, DbType.Int32)).ExecuteNonQuery();


                    db.SetSpCommand("p_FilterCoefficients_Insert")
                                      .ExecuteForEach<FilterCoefficient>(list);
                    success = true;
                }
                catch (Exception ex)
                {
                   // throw Trace.Log<ExceptionHolder>(ex);
                }
            }
            return success;
        }
       

        public static bool InsertCalculatorFirmFilter(CalculatorFirmFilter f)
        {
            bool success = false;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);

                try
                {
                    int id = 0;
                    fa.InsertFirmFilter(f.FirmID, f.FilterName, f.PeriodTransportation, f.TransportationType, f.WagonType,
                                        f.VolumeType, f.CargoName, f.CargoGroup, f.CompanySending, f.CompanyRecipient,
                                        f.CountrySending, 
                                        f.CountryDelivering,
                                        f.StationSending,
                                        f.StationDelivering,f.WaySending,f.WayDelivering, f.SubjectDelivering,f.SubjectSending,
                                        f.OwnerWagon, f.PayerWagon, f.RenterWagon, f.Columns, f.EarlyTransportationCargo, f.VagonType, out id);
                    success = true;
                }
                catch (Exception ex)
                {
                    // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return success;
        }
        

        public static CalculatorFirmFilter GetCalculatorFirmFilter(Guid firmid, int filterid)
        {
            List<CalculatorFirmFilter> filter = null;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);

                try
                {
                     filter = fa.GetCalculatorFirmFilter(filterid, firmid);
                }
                catch (Exception ex)
                {
                    // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

          return filter.SingleOrDefault();
        }

        public static bool DeleteFilter(int filterid)
        {
            bool success = false;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);

                try
                {
                    fa.DeleteFilter(filterid);
                    success = true;
                }
                catch (Exception ex)
                {
                    
                    // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return success;
        }

        public static List<CalcFirmSettings> GetCalculatorFirmFilterList(Guid firmid)
        {
            List<CalcFirmSettings> filter = null;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);

                try
                {
                    filter = fa.CalcFirmSettingsList(firmid);
                }
                catch (Exception ex)
                {
                    // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return filter;
        }
    }
}
