using Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO.Implementation
{
    public class FirmHelper
    {
        public static DateTime GetPasswordExpirationDate()
        {
            var passLifeTime = Constants.PassLifeTimeDays;
            var passwordExpDate = DateTime.Now.AddDays(passLifeTime != 0 ? passLifeTime : 30);
            return passwordExpDate;
        }
    }
}
