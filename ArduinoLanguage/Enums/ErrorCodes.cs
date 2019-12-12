using System;

namespace ArduinoLanguage.Enums
{
    public enum ErrorCodes : Int16
    {
        EmptyFile = 1,
        NoLoopFunc = 2,
        StringError = 3,
        StringEndError = 4,
        StringConstError = 5,
        StringNotClosed = 6,        
        CharConstEmpty = 7,
        CharConstNotClosed = 8 
    }
}
