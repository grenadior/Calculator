using System.Collections.Generic;
using BLToolkit.DataAccess;
using BO;

namespace DA.Accessors
{
    public abstract class ExceptionAccessor : DataAccessor<Entity>
    {
        [SprocName("p_Insert_Exception")]
        public abstract List<ServerProperty> InsertExceptionInfo(string @message);
    }
}