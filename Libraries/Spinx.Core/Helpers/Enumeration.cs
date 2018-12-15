using System;
using System.Collections.Generic;
using System.Linq;

namespace Spinx.Core.Helpers
{
    public static class Enumeration
    {
        public static List<KeyValuePair<int, string>> GetAll<TEnum>() where TEnum: struct
        {
            var enumerationType = typeof (TEnum);

            if (!enumerationType.IsEnum)
                throw new ArgumentException("Enumeration type is expected.");

            var dictionary = new Dictionary<int, string>();

            foreach (int value in Enum.GetValues(enumerationType))
            {
                var name = Enum.GetName(enumerationType, value);
                dictionary.Add(value, name);
            }

            var myList = dictionary.ToList();
            
            myList.Sort(
                (pair1, pair2) => string.Compare(pair1.Value, pair2.Value, StringComparison.Ordinal)
            );

            return myList;
        }
    }
}