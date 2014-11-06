using System;
using System.Collections.Generic;
using System.Data;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BO;
using Common.Api;
using Configuration;
using DA.Accessors;

namespace DA.Entities
{
    public class EntityAdapter
    {
        public static List<String> GetEntitiesByID(string dbName, string columnName, string columnName2 = "", string sWhere = "", string orderBy = "")
        {
            List<String> entities = null;
            using (var db = new DbManager(DB.LocalDb))
            {
                db.Command.CommandTimeout = 600;
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);

                try
                {
                    entities = fa.GetEntitiesByID(columnName, columnName2, sWhere, orderBy, dbName);
                }
                catch (Exception ex)
                {
                   // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return entities;
        }

        public static DataTable GetCalculatorResultByFilter(string allQuery, string allQuerySummary, out int totalRowsCount)
        {
            var dtReportQuery = new DataTable();
            totalRowsCount = 0;
            using (var db = new DbManager(DB.LocalDb))
            {
                db.Command.CommandTimeout = 900;
                var fa = DataAccessor.CreateInstance<EntityAccessor>(db);
              
                try
                {
                    dtReportQuery = fa.GetCalculatorResultByFilter(allQuery, "");
                    DataTable dtRowsCountQuery = !String.IsNullOrEmpty(allQuerySummary) ? fa.GetCalculatorResultByFilter(allQuerySummary, "") : null;
                    if (dtRowsCountQuery != null && dtRowsCountQuery.Rows.Count > 0)
                        totalRowsCount = dtRowsCountQuery.Rows[0].Field<int>("TotalRowsCount");
                    else
                    {
                        totalRowsCount = int.Parse(dtReportQuery.Rows[0]["CountRows"].ToString());
                        dtReportQuery.Columns.Remove("CountRows");
                    }
                }
                catch (Exception ex)
                {
                   // throw Trace.Log<ExceptionHolder>(ex);
                }
            }

            return dtReportQuery;
        }
    }
}