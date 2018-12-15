using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Spinx.Core.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Does a list contain all values of another list?
        /// </summary>
        /// <remarks>Needs .NET 3.5 or greater.  Source:  https://stackoverflow.com/a/1520664/1037948 </remarks>
        /// <typeparam name="T">list value type</typeparam>
        /// <param name="containingList">the larger list we're checking in</param>
        /// <param name="lookupList">the list to look for in the containing list</param>
        /// <returns>true if it has everything</returns>
        public static bool ContainsAll<T>(this IEnumerable<T> containingList, IEnumerable<T> lookupList)
        {
            return !lookupList.Except(containingList).Any();
        }

        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));
            }
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return enumerationValue.ToString();
        }
    }
}