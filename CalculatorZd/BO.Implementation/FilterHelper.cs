using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO.Implementation
{
    public class FilterHelper
    {
        public static StringBuilder GetWagonTypeFilterByValue(IEnumerable<Filter> filtesList)
        {
            var sb = new StringBuilder();
            List<WagonGroupType> wagonHierarhy = FilterManager.GetWagonTypesHierarhy();
           
            foreach (Filter filter in filtesList)
            {
                if (filter.name.IndexOf("группа: ", System.StringComparison.Ordinal) == 0)
                {
                    int startIndex = "группа: ".Length;
                    sb.Append(GetSubGroupStringByGroup(wagonHierarhy, filter.name.Substring(startIndex, filter.name.Length - startIndex)));
                    sb.Append(",");
                }
                else if(filter.name.IndexOf("подгруппа: ", System.StringComparison.Ordinal) == 0)
                {
                    int startIndex = "подгруппа: ".Length;
                    sb.Append(String.Format("'{0}',", filter.name.Substring(startIndex, filter.name.Length - startIndex)));
                }
            }
            return sb.Remove(sb.Length - 1, 1);
        }

        private static String GetSubGroupStringByGroup(IEnumerable<WagonGroupType> wagonHierarhy, string groupName)
        {
            StringBuilder sb = new StringBuilder();

            foreach (WagonGroupType wagonGroup in wagonHierarhy)
            {
                if (wagonGroup.WagonGroupName == groupName)
                {
                    if (wagonGroup.WagonTypes.Count == 0)
                    {
                        sb.Append(String.Format("'{0}'", wagonGroup.WagonGroupName.Trim()));
                        sb.Append(",");
                        break;
                    }
                    else
                    {
                        foreach (WagonTypes wagonTypes in wagonGroup.WagonTypes)
                        {
                            sb.Append(String.Format("'{0}'", wagonTypes.WagonTypeName.Trim()));
                            sb.Append(",");
                        }
                    }
                }
            }
            if (sb.Length > 0)
                return sb.Remove(sb.Length - 1, 1).ToString();
            return sb.ToString();
        }
    }
}