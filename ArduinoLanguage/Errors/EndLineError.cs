using ArduinoLanguage.Enums;

namespace ArduinoLanguage.Errors
{
    public class StringEndError : Error
    {
        public StringEndError(int line) : base(line, (int)ErrorCodes.StringEndError)
        {
        }
    }
}
