using System;
using System.Collections.Generic;
using System.Data;
using CalculatorZd.Models;
using DA.Report;

namespace CalculatorZd.Services
{
    public class ReportService
    {
        public List<FirmReport> GetReportByFirm(Guid firmId)
        {
            DataTable dt = ReportAdapter.GetReportsByFirm(firmId);
            var listReport = new List<FirmReport>();
            foreach (DataRow dataRow in dt.Rows)
            {
                listReport.Add(new FirmReport
                {
                    FileName = dataRow.Field<string>("FileName"),
                    AddDate = dataRow.Field<DateTime>("AddDate"),
                    Number = dataRow.Field<int>("Number"),
                    Status = dataRow.Field<string>("StatusName"),
                    StatusID = dataRow.Field<int>("StatusID"),
                    FileSize = dataRow.Field<long?>("FileSize"),
                });
            }
            return listReport;
        }
    }
}