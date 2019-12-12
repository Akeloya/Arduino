using ArduinoLanguage.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArduinoLanguage.Errors
{
    public class StringStartError : Error
    {
        public StringStartError(int codeLine) : base(codeLine, (int)ErrorCodes.StringConstError)
        {
        }
    }
}
