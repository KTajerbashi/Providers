using System;
using System.Reflection;

namespace SoapParserWCF.Extensions
{
    public static class TypeExtensions
    {

        public static Type GetEnumeratedType(this Type type)
        {
            // provided by Array
            var elType = type.GetElementType();
            if (null != elType) return elType;

            // otherwise provided by collection
            var elTypes = type.GetGenericArguments();
            if (elTypes.Length > 0) return elTypes[0];

            // otherwise is not an 'enumerated' type
            return null;
        }

        public static TView MapToView<TView>(this object DataSource)
        {
            var dst = DataSource.GetType();
            object obj = Activator.CreateInstance<TView>();
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                var ds = dst.GetProperty(prop.Name);
                if (ds != null)
                {
                    var val = ds.GetValue(DataSource);
                    if (val != null) prop.SetValue(obj, CastTo(prop.PropertyType, val));
                }
            }
            return (TView)obj;
        }

        private static object CastTo(Type t, object value)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                return Convert.ChangeType(value, Nullable.GetUnderlyingType(t));
            if (t.IsEnum) return Convert.ToByte(Enum.ToObject(t, value));
            else return Convert.ChangeType(value, t);
        }

        public static T To<T>(this object Item)
        {
            if (Item == null) return default(T);
            Type t = typeof(T);
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                return (T)Convert.ChangeType(Item, Nullable.GetUnderlyingType(typeof(T)));
            else return (T)Convert.ChangeType(Item, t);
        }

        public static object ReflectValue(this object Item, string PropertyName)
        {
            return Item.GetType().GetProperty(PropertyName).GetValue(Item);
        }
        public static T ReflectValue<T>(this object Item, string PropertyName) where T : struct
        {
            return Item.GetType().GetProperty(PropertyName).GetValue(Item).To<T>();
        }

        public static bool ReflectValue(this object Item, string PropertyName, object PropertyValue)
        {
            PropertyInfo Property = Item.GetType().GetProperty(PropertyName);
            if (Property == null) return false;

            Property.SetValue(Item, PropertyValue);
            return true;
        }

    }
}
