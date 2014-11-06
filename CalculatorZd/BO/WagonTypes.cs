using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class WagonGroupType
    {
        public WagonGroupType ()
        {
            WagonTypes = new List<WagonTypes>();
        }
        public int WagonGroupID;
        public string WagonGroupName;
        public List<WagonTypes> WagonTypes;
    } 

    public class WagonTypes
    {
        public int WagonTypeID;
        public string WagonTypeName;
    }
}
