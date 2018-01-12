using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HRTools.Crosscutting.Common.Helpers
{
    public static class EnumHelper
    {
		public static string GetEnumDescription(Enum value)
        {
            Guard.ArgumentIsNotNull(value, nameof(value));

            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Any() && attributes.Length > 0)
            {
                return attributes[0].Description;
            }

            return value.ToString();
        }
        
        public static T Parse<T>(string enumValue)
        {
            Guard.ArgumentIsNotNullOrWhiteSpace(enumValue, nameof(enumValue));

            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return (T)Enum.Parse(typeof(T), enumValue, true);
        }
        
        public static List<string> ParseDescriptions<T>(IEnumerable<T> enums)
        {
            Guard.ArgumentIsNotNull(enums, nameof(enums));

            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            List<string> result = new List<string>();

            foreach (T error in enums)
            {
                string errorDescription = GetEnumDescription(error as Enum);
                result.Add(errorDescription);
            }

            return result;
        }
    }
}
