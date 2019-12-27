/*
 *  "Arduino emulator", the simple virtual emulator arduino circuit.
 *  Copyright (C) 2019 by Maxim V. Yugov.
 *
 *  This file is part of "Arduino emulator".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
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

        public static string ResolveStringValue(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentNullException(nameof(resourceName));
            PropertyInfo propertyInfo = typeof(Resource)
                .GetProperty(resourceName, BindingFlags.Public | BindingFlags.Static);

            return propertyInfo?.GetValue(null, null)?.ToString();
        }

        public static byte[] ResolveImageString(string imageName, params object?[] args)
        {
            if (string.IsNullOrEmpty(imageName))
                throw new ArgumentNullException(nameof(imageName));
            PropertyInfo propertyInfo = typeof(Resource)
                .GetProperty(imageName.Replace(" ", "_", StringComparison.InvariantCultureIgnoreCase), BindingFlags.Public | BindingFlags.Static);

            byte[] value = (byte[])propertyInfo?.GetValue(null, null);
            return value;
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
