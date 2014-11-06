using System;
using System.Collections.Generic;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BO;
using Common.Api;
using Configuration;
using DA.Accessors;

namespace DA.ServerProperties
{
    public class ServerPropertiesAdapter
    {
        public static List<ServerProperty> GetServerProperties()
        {
            List<ServerProperty> entities = null;
            using (var db = new DbManager(DB.LocalDb))
            {
                var fa = DataAccessor.CreateInstance<ServerPropertiesAccessor> (db);

                try
                {
                    entities = fa.GetServerProperties();
                }
                catch (Exception ex)
                {
                  
                } // throw Trace.Log<ExceptionHolder>(new Exception());
            }

            return entities;
        }
    }
}