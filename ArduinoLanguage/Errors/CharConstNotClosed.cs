using ArduinoLanguage.Enums;

namespace ArduinoLanguage.Errors
{
    public class CharConstNotClosed : Error
    {
        public CharConstNotClosed(int line) : base (line, (int)ErrorCodes.CharConstNotClosed)
        {
        }
    }
}
