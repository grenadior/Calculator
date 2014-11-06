using System;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using Configuration;
using DA.Accessors;

namespace DA.ExceptionAdapter
{
     public class ExceptionAdapter
    {
         public  static void InsertException(string message)
         {
             using (var db = new DbManager(DB.LocalDb))
             {
                 var fa = DataAccessor.CreateInstance<ExceptionAccessor>(db);

                 try
                 {
                     fa.InsertExceptionInfo(message);
                 }
                 catch (Exception ex)
                 {
                     // throw Trace.Log<ExceptionHolder>(ex);
                 }
             }
         }
      
    }
}
