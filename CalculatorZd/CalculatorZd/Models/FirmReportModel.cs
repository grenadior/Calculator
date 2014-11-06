using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Api;

namespace CalculatorZd.Models
{
    public class FirmReportViewModel
    {
        public FirmReportViewModel()
        {
            FirmReports = new List<FirmReport>();
        }
        public List<FirmReport> FirmReports { get; set; }
    }

    public class FirmReport
    {
        public string FileName { get; set; }

        public int Number { get; set; }

        public DateTime AddDate { get; set; }

        public string Status { get; set; }

        public int StatusID { get; set; }

        public long? FileSize { get; set; }
    }
}