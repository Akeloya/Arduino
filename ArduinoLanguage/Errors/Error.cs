using ArduinoLanguage.Resources;
using System;
using System.Globalization;

namespace ArduinoLanguage.Errors
{
    /// <summary>
    /// Definition Error
    /// </summary>
    public abstract class Error : Exception
    {
        /// <summary>
        /// Line that contains error
        /// </summary>
        public int Line { get; }
        /// <summary>
        /// Error type
        /// </summary>
        public int Type { get; }
        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="line">Error line</param>
        /// <param name="type">Error type</param>
        public Error(int line, int type)
        {
            Line = line;
            Type = type;
        }
        /// <summary>
        /// Override of ToString method - get localization error string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Resource.ResourceManager.GetString(Type.ToString(), CultureInfo.CurrentCulture);
        }        
    }
}
