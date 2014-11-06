using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using BO;

namespace Common.Api
{
    namespace Extensions
    {
        public static class StringEnum
        {
            /// <summary>
            /// Will get the string value for a given enums value, this will
            /// only work if you assign the StringValue attribute to
            /// the items in your enum.
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static string GetStringValue(this Enum value)
            {
                // Get the type
                Type type = value.GetType();

                // Get fieldinfo for this type
                FieldInfo fieldInfo = type.GetField(value.ToString());

                // Get the stringvalue attributes
                StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                    typeof(StringValueAttribute), false) as StringValueAttribute[];

                // Return the first if there was a match.
                return attribs.Length > 0 ? attribs[0].StringValue : null;
            }
        }

        public static class TimeSpanExtensions
        {
            public static int GetYears(this TimeSpan timespan)
            {
                return (int)(timespan.Days / 365.2425);
            }

            public static int GetMonths(this TimeSpan timespan)
            {
                return (int)(timespan.Days / 30.436875);
            }
        }
    }
}