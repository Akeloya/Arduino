using ArduinoEmulator.Resources;
using System;
using System.Reflection;

namespace ArduinoEmulator.Core
{
    public static class ResourceStringResolver
    {
        public static string ResolveExceptionString(Exception e, params object?[] args)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));
            PropertyInfo propertyInfo = typeof(Resource)
                .GetProperty(e.GetType().Name, BindingFlags.Public | BindingFlags.Static);

            string value = propertyInfo?.GetValue(null, null)?.ToString();
            StringProvider provider = new StringProvider();
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception(string.Format(provider, Resource.ExceptionNotRegistered, e.GetType().Name, args));
            }
            return string.Format(provider, value, args);
        }
    }

    public class StringProvider : IFormatProvider
    {
        public object GetFormat(Type formatType)
        {
            if (formatType is ICustomFormatter)
                return this;
            return null;
        }
    }
}
