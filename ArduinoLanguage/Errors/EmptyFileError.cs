using ArduinoLanguage.Enums;

namespace ArduinoLanguage.Errors
{
    public class EmptyFileError : Error
    {
        public EmptyFileError() : base(-1, (int)ErrorCodes.EmptyFile)
        {
        }
    }
}
