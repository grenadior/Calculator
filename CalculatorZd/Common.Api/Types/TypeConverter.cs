using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Common.Api.Exceptions;
using Common.Api.Text;

namespace Common.Api.Types
{
    public static class TypeHelper
    {
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() ==
                   typeof(Nullable<>);
        }
    }
    /// <summary>
    /// Summary description for TypeConverter.
    /// </summary>
    public class TypeConverter
    {
        private const int BIT_COLLECTION_OF_INT_LENGTH = sizeof(int) * 8 - 1;
        private const int BIT_COLLECTION_OF_LONG_LENGHT = sizeof(long) * 8 - 1;
        private static readonly int[] _bitCollectionOfInt = new int[BIT_COLLECTION_OF_INT_LENGTH];
        private static readonly long[] _bitCollectionOfLong = new long[BIT_COLLECTION_OF_LONG_LENGHT];

        public static bool LogExceptions;

        static TypeConverter()
        {
            _bitCollectionOfInt[0] = 1;
            _bitCollectionOfLong[0] = 1;
            for (int i = 1; i < BIT_COLLECTION_OF_LONG_LENGHT; i++)
            {
                if (i < BIT_COLLECTION_OF_INT_LENGTH)
                {
                    _bitCollectionOfInt[i] = _bitCollectionOfInt[i - 1] << 1;
                }
                _bitCollectionOfLong[i] = _bitCollectionOfLong[i - 1] << 1;
            }
        }

        public static List<int> ToBitCollection(int bitSum)
        {
            List<int> bitCollection = new List<int>();
            for (int i = 0; i < _bitCollectionOfInt.Length; i++)
            {
                if ((bitSum | _bitCollectionOfInt[i]) == bitSum)
                {
                    bitCollection.Add(_bitCollectionOfInt[i]);
                }
            }
            return bitCollection;
        }

        public static List<long> ToBitCollection(long bitSum)
        {
            List<long> bitCollection = new List<long>();
            for (int i = 0; i < _bitCollectionOfLong.Length; i++)
            {
                if ((bitSum | _bitCollectionOfLong[i]) == bitSum)
                {
                    bitCollection.Add(_bitCollectionOfLong[i]);
                }
            }
            return bitCollection;
        }

        public static List<T> ToBitCollection<T>(T bitSum)
            where T : struct, IConvertible
        {
            if (typeof(T).Equals(typeof(int)))
            {
                return ToBitCollection(bitSum.ToInt32(CultureInfo.CurrentCulture))
                  .ConvertAll<T>(v => (T)(v as object));
            }
            return ToBitCollection(bitSum.ToInt64(CultureInfo.CurrentCulture))
              .ConvertAll<T>(v => (T)(v as object));
        }

        public static bool ToBool(object p_value)
        {
            return ToBool(p_value, false);
        }

        public static bool ToBool(object p_value, bool p_defaultValue)
        {
            bool bValue = p_defaultValue;
            if (!IsNull(p_value) && !IsNullOrEmpty(p_value))
            {
                try
                {
                    if (p_value is int)
                        return Int32ToBool((int)p_value);
                    if (p_value is string)
                        return StringToBool((string)p_value);
                    bValue = Convert.ToBoolean(p_value);
                }
                catch (Exception e)
                {
                    HandleException(e);
                }
            }
            return bValue;
        }

        private static bool Int32ToBool(int p_iValue)
        {
            return p_iValue > 0;
        }

        private static bool StringToBool(string p_sValue)
        {
            return !IsNullOrEmpty(p_sValue) && (p_sValue.ToUpper() == "ON" || p_sValue.ToUpper() == "TRUE" ||
                                                (StringUtils.IsDigitString(p_sValue) && ToInt32(p_sValue) > 0));
        }

        public static int ToInt32(object p_value)
        {
            return ToInt32(p_value, 0);
        }

        public static int ToInt32(object p_value, int p_iDefaultValue)
        {
            int iValue = p_iDefaultValue;
            if (!IsNull(p_value) && !IsNullOrEmpty(p_value))
            {
                try
                {
                    if (p_value is double ||
                        p_value is float)
                    {
                        double dValue = Convert.ToDouble(p_value);
                        iValue = Convert.ToInt32(dValue);
                        return iValue;
                    }

                    iValue = Convert.ToInt32(p_value);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
            return iValue;
        }

        public static long ToInt64(object p_value)
        {
            return ToInt64(p_value, 0);
        }

        public static long ToInt64(object p_value, long p_lDefaultValue)
        {
            long iValue = p_lDefaultValue;
            if (!IsNull(p_value) && !IsNullOrEmpty(p_value))
            {
                try
                {
                    if (p_value is double ||
                        p_value is float)
                    {
                        double dValue = Convert.ToDouble(p_value);
                        iValue = Convert.ToInt64(dValue);
                        return iValue;
                    }

                    iValue = Convert.ToInt64(p_value);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
            return iValue;
        }

        public static double ToDouble(object p_value)
        {
            return ToDouble(p_value, 0.0);
        }

        public static double ToDouble(object p_value, double p_dDefaultValue)
        {
            double dValue = p_dDefaultValue;
            if (!IsNull(p_value) && !IsNullOrEmpty(p_value))
            {
                try
                {
                    if (p_value.GetType() == typeof(string))
                    {
                        NumberFormatInfo provider = new NumberFormatInfo();

                        if (((string)p_value).Contains(","))
                        {
                            provider.NumberDecimalSeparator = ",";
                        }
                        else if (((string)p_value).Contains("."))
                        {
                            provider.NumberDecimalSeparator = ".";
                        }

                        dValue = Convert.ToDouble(p_value, provider);
                    }
                    else
                    {
                        dValue = Convert.ToDouble(p_value);
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
            return dValue;
        }

        public static decimal ToDecimal(object p_value)
        {
            return ToDecimal(p_value, 0);
        }

        public static decimal ToDecimal(object p_value, decimal p_dDefaultValue)
        {
            decimal dValue = p_dDefaultValue;
            if (!IsNull(p_value) && !IsNullOrEmpty(p_value))
            {
                try
                {
                    dValue = Convert.ToDecimal(p_value);
                }
                catch (FormatException fex)
                {
                    try
                    {
                        dValue = Convert.ToDecimal(p_value, NumberFormatInfo.InvariantInfo);
                    }
                    catch
                    {
                        HandleException(fex);
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
            return dValue;
        }

        public static string ToString(object p_value)
        {
            string sValue = string.Empty;
            if (!IsNull(p_value) && !IsNullOrEmpty(p_value))
            {
                try
                {
                    sValue = Convert.ToString(p_value);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
            return sValue;
        }

        public static DateTime? ToDateTime(TimeFrame timeFrameOffset, DateTime initialDateTime)
        {
            switch (timeFrameOffset)
            {
                case TimeFrame.Hour:
                    return initialDateTime.AddHours(-1);
                case TimeFrame.ThreeHours:
                    return initialDateTime.AddHours(-3);
                case TimeFrame.Today:
                    return initialDateTime.Date;
                case TimeFrame.ThreeDays:
                    return initialDateTime.Date.AddDays(-3);
                case TimeFrame.Week:
                    return initialDateTime.Date.AddDays(-7);
            }
            return null;
        }

        public static DateTime ToDateTime(object p_value)
        {
            return ToDateTime(p_value, DateTime.MinValue);
        }

        public static bool TryConvertToDateTime(string ticks, out DateTime result)
        {
            result = DateTime.MinValue;
            Int64 _ticks;
            if (Int64.TryParse(ticks, out _ticks))
            {
                result = new DateTime(_ticks);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DateTime ToDateTime(object p_value, DateTime p_dtDefault)
        {
            if (!IsNull(p_value) && !IsNullOrEmpty(p_value))
            {
                try
                {
                    return Convert.ToDateTime(p_value);
                }
                catch
                {
                }
            }
            return p_dtDefault;
        }

        public static DateTime ToDate()
        {
            return ToDate(DateTime.Now);
        }

        public static DateTime ToDate(DateTime p_dt)
        {
            return p_dt.AddTicks(-p_dt.TimeOfDay.Ticks);
        }

        public static DateTime GetDate(int p_iDay, int p_iMonth)
        {
            if (p_iMonth >= 1 && p_iMonth <= 12 &&
                p_iDay >= 1 && p_iDay <= 31)
            {
                int iYear = p_iMonth < DateTime.Now.Month ? DateTime.Now.Year + 1 : DateTime.Now.Year;
                if (DateTime.DaysInMonth(iYear, p_iMonth) < p_iDay ||
                    p_iDay < 1)
                    p_iDay = 1;
                return new DateTime(iYear, p_iMonth, p_iDay);
            }
            return DateTime.MinValue;
        }

        public static TimeSpan ToTimeSpan(object p_value)
        {
            DateTime dt = ToDateTime(p_value);
            return dt - ToDate(dt);
        }

        public static T ToEnumMember<T>(string p_sValue)
            where T : struct
        {
            return ToEnumMember<T>(p_sValue, default(T));
        }

        public static T ToEnumMember<T>(string p_sValue, T p_default)
            where T : struct
        {
            if (!string.IsNullOrEmpty(p_sValue))
                try
                {
                    return (T)Enum.Parse(typeof(T), p_sValue, true);
                }
                catch (Exception e)
                {
                    HandleException(e);
                }
            return p_default;
        }

        public static T ToEnumMember<T>(int p_iValue)
            where T : struct
        {
            try
            {
                return (T)Enum.Parse(typeof(T), Enum.GetName(typeof(T), p_iValue), true);
            }
            catch (Exception e)
            {
                HandleException(e);
                return default(T);
            }
        }

        public static Guid ToGuid(object p_value)
        {
            try
            {
                if (p_value is string)
                {
                    Guid result;
                    if (Guid.TryParse(p_value as string, out result))
                    {
                        return result;
                    }
                }
                else if (p_value is Guid)
                {
                    return (Guid)p_value;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return Guid.Empty;
        }
        /// <summary>
        /// Конвертирует Guid в фейковый Int
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GuidToFakeInt(Guid id)
        {
            byte[] seed = id.ToByteArray();
            for (int i = 0; i < 3; i++)
            {
                seed[i] ^= seed[i + 4];
                seed[i] ^= seed[i + 8];
                seed[i] ^= seed[i + 12];
            }
            return BitConverter.ToInt32(seed, 0);
        }

        /// <summary>
        /// Конвертирует Int в фейковый Guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Guid IntToFakeGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        /// <summary>
        /// Конвертирует Guid в html ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GuidToHtmlID(Guid id)
        {
            return id.ToString().Replace("-", "");
        }
        /// <summary>
        /// Конвертирует html ID в Guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Guid HtmlIDToGuid(string id)
        {
            if (id.Length != 32)
            {
                return Guid.Empty;
            }
            return ToGuid(id.Substring(0, 8) + "-" + id.Substring(8, 4) + "-" + id.Substring(12, 4) + "-" + id.Substring(16, 4) + "-" + id.Substring(20, 12)); 
        }

        public static int ByteArrayToInt32(byte[] p_bytes)
        {
            int i = 0;
            i |= p_bytes[0] & 0xFF;
            i <<= 8;
            i |= p_bytes[1] & 0xFF;
            i <<= 8;
            i |= p_bytes[2] & 0xFF;
            i <<= 8;
            i |= p_bytes[3] & 0xFF;
            return i;
        }

        public static string ByteArrayToString(byte[] p_bytes)
        {
            return ByteArrayToString(p_bytes, Encoding.ASCII);
        }

        public static string ByteArrayToString(byte[] p_bytes, Encoding p_encoding)
        {
            string sReturn = string.Empty;
            try
            {
                sReturn = p_encoding.GetString(p_bytes);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return sReturn;
        }

        public static bool IsNull(object p_oValue)
        {
            return (p_oValue == null || Convert.IsDBNull(p_oValue));
        }

        public static bool IsNullOrEmpty(object p_obj)
        {
            return p_obj == null || (p_obj is string && string.IsNullOrEmpty((string)p_obj));
        }

        public static bool IsNullOrEmpty(string p_sInput)
        {
            return string.IsNullOrEmpty(p_sInput);
        }

        public static NameValueCollection ToQueryStringParams(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) return null;
                string query = new Uri(url).Query;
                if (string.IsNullOrEmpty(query) == false)
                {
                    query = query.TrimStart('?');
                    return new HttpRequest("filename", url, query).QueryString;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return null;
        }

        public static object TryConvert(object val, Type p_targetType)
        {
            try
            {
                if (p_targetType.IsGenericType)
                {
                    Type[] genericTypes = p_targetType.GetGenericArguments();
                    if (genericTypes.Length == 1)
                    {
                        return TryConvert(val, genericTypes[0]);
                    }
                }
                else
                {
                    if (p_targetType.IsEnum)
                    {
                        return Convert.ChangeType(Enum.ToObject(p_targetType, ToInt32(val, 0)), p_targetType);
                    }
                    else if(p_targetType==typeof(Guid))
                    {
                        var guidValue = ToGuid(val);
                        return guidValue;
                    }
                    else
                    {
                        try
                        {
                            return Convert.ChangeType(val, p_targetType);
                        }
                        catch
                        {
                            return Convert.ChangeType(val, p_targetType, CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
            return null;
        }

        public static object TryConvert(ValueType p_val, Type p_targetType)
        {
            return TryConvert((object)p_val, p_targetType);
        }

        private static void HandleException(Exception p_ex)
        {
            if (LogExceptions)
                Trace.Log<ExceptionHolder>(p_ex);
        }

        public static List<int> StringToIntListConvert(string str, params string[] separator)
        {
            List<int> expandedCitiesList = new List<int>();
            try
            {
                string[] strElementMass = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                expandedCitiesList.AddRange(from oneElement in strElementMass
                                            where !string.IsNullOrEmpty(oneElement) && ToInt32(oneElement, 0) > 0
                                            select ToInt32(oneElement, 0));
            }
            catch (Exception e)
            {
                HandleException(e);
            }
            return expandedCitiesList;
        }

        public static string IntListToStringConvert(List<int> lst, string separator)
        {
            StringBuilder strRes = new StringBuilder();
            try
            {
                if (lst != null && lst.Count > 0)
                {
                    foreach (int id in lst)
                        strRes.AppendFormat("{0}{1}", id, separator);
                    if (strRes.Length > 0)
                        strRes.Remove(strRes.Length - 1, 1);
                }
            }
            catch (Exception e)
            {
                HandleException(e);
            }
            return strRes.ToString();
        }
    }
}