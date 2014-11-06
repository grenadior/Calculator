using System;
using BL.Caching.Provider;
using BL.Calculator;

namespace BL.Caching
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
            if (objValue == null)
            {
                objValue = CalculatorManager.GetEntities(cacheKeyName);
                http.Set(cacheKeyName, objValue);
            }

            return objValue as T;
        }
    }
}