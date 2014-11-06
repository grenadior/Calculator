using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BO;
using Common.Api;
using Configuration;
using DA.Accessors;

namespace DA.Report
{
    public class ReportAdapter
    {
        public static Guid AddReportFileNameByFirm(Guid firmId, string fileName, int status)
        {
            Guid id = Guid.Empty;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);
                try
                {
                    fa.SaveReportFileNameByFirm(firmId, fileName, status, out id);
                 //   db.ExecuteScalar<Guid>(ScalarSourceType.OutputParameter, "outputString");
                }
                catch (Exception ex)
                {
                    // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return id;
        }
        public static bool UpdateReportStatusById(Guid id, int status, long fileSize)
        {
            bool success = false;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);
                try
                {
                    success = fa.UpdateReportStatusById(id, status, fileSize);
                }
                catch (Exception ex)
                {
                    // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return success;
        }
        public static DataTable GetReportsByFirm(Guid firmId)
        {
            DataTable dt = new DataTable(); ;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);
                try
                {
                    dt = fa.GetReportsByFirm(firmId);
                }
                catch (Exception ex)
                {
                    // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return dt;
        }
    }
}
