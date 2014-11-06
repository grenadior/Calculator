using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.Caching
{
    public class CachingHelper
    {
        public static string GetReportCachKey(string key, string sessionId)
        {
            return String.Format("{0}_{1}", key, sessionId);
        }
    }
}
