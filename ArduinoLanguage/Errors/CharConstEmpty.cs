using ArduinoLanguage.Enums;

namespace ArduinoLanguage.Errors
{
    public class CharConstEmpty : Error
    {
        public CharConstEmpty(int line) : base(line, (int)ErrorCodes.CharConstEmpty)
        {
        }
    }
}
