using System;
using BO.Implementation.Caching.Provider;
using BO.Implementation.Calculator;
using Common.Api.Extensions;

namespace BO.Implementation.Caching
{
    public class DictionaryHelper
    {
        public static T GetDictionaryByFilter<T>(string columnNameKey, string columnNameKey2 = "", string cacheKey = "") where T : class
        {
            var http = new HttpCache();
            string cacheKeyName;
            if (!string.IsNullOrEmpty(cacheKey))
            {
                cacheKeyName = cacheKey;
            }
            else
            {
               cacheKeyName = columnNameKey;
            }

            Object objValue = null;
            http.Get(cacheKeyName, out objValue);
            string dbname = columnNameKey == ColumnsMapping.CargoNameEarlyTransportation.GetStringValue()
                ? ServerProperties.Instance.DBPorozhn
                : ServerProperties.Instance.DBGruzhon;
            if (objValue == null)
            {
                objValue = CalculatorManager.GetEntities( cacheKeyName,"","","", dbname );
                http.Set(cacheKeyName, objValue);
            }

            return objValue as T;
        }
    }
}