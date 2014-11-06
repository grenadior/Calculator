using System;
using System.Collections.Generic;
using BLToolkit.DataAccess;
using BO;

namespace DA.Accessors
{
    public abstract class ServerPropertiesAccessor : DataAccessor<Entity>
    {
        [SprocName("p_ServerProperties_Get")]
        public abstract List<ServerProperty> GetServerProperties();
    }
}