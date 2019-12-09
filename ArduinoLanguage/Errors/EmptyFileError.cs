using ArduinoLanguage.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArduinoLanguage.Errors
{
    public class EmptyFileError : Error
    {
        public EmptyFileError() : base(-1, (int)ErrorCodes.EmptyFile)
        {

        }
    }
}
