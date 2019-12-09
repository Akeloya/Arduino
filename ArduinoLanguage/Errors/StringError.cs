using ArduinoLanguage.Enums;

namespace ArduinoLanguage.Errors
{
    public class StringError : Error
    {
        public StringError(int line) : base(line, (int)ErrorCodes.StringError)
        {
        }
    }
}
