using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.Calculator
{
    public class CalculatorHelper
    {
        public static string CombineColumns(string s1, string s2)
        {
            return String.Format("{0} + ' | '+ {1}", s1, s2);
        }
    }
}
