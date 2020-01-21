using System;
using System.ComponentModel;
using System.Reflection;

namespace DataHandler.Conversion
{
    public class Convertor
    {
        public static void ConvertValue(object target, PropertyInfo property, object value)
        {
            var propertyType = property.PropertyType;
            object convertedValue;

            if (propertyType.IsGenericType &&
                propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    property.SetValue(target, null);

                    return;
                }

                propertyType = new NullableConverter(propertyType).UnderlyingType;
            }

            convertedValue = property.PropertyType.IsEnum ?
                Enum.Parse(property.PropertyType, value.ToString()) :
                Convert.ChangeType(value, propertyType);

            property.SetValue(target, convertedValue);
        }
    }
}
