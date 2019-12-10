using ArduinoLanguage.Enums;
using ArduinoLanguage.Errors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArduinoLanguage
{
    /// <summary>
    /// Definition code lexeme
    /// </summary>
    public class Lexeme
    {
        private dynamic _data;
        private readonly LexemeDataType _dataType;
        /// <summary>
        /// Lexeme type
        /// </summary>
        public LexemeTypes Type { get; }
        /// <summary>
        /// Code line number
        /// </summary>
        public int Line { get; }
        /// <summary>
        /// String representation of lexeme
        /// </summary>
        public string LexemValue { get; }
        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="lexeme">Lexeme value</param>
        /// <param name="type">Lexeme type</param>
        /// <param name="line">Code line number</param>
        public Lexeme(string lexeme, LexemeTypes type, int line)
        {
            Type = type;
            Line = line;
            LexemValue = lexeme;
            switch (type)
            {
                case LexemeTypes.Integer:
                    _dataType = LexemeDataType.Integer;
                    break;
                case LexemeTypes.Double:
                    _dataType = LexemeDataType.Double;
                    break;
                case LexemeTypes.Char:
                    _dataType = LexemeDataType.Char;
                    break;
                case LexemeTypes.Boolean:
                    _dataType = LexemeDataType.Boolean;
                    break;
            }
        }
        /// <summary>
        /// Getting the actual value of data from a string representation of a Lexeme
        /// </summary>
        internal void GetData()
        {
            switch (_dataType)
            {
                case LexemeDataType.Boolean:
                    _data = Boolean.Parse(LexemValue);
                    break;
                case LexemeDataType.Byte:
                    _data = Byte.Parse(LexemValue);
                    break;
                case LexemeDataType.Char:
                    _data = Char.Parse(LexemValue);
                    break;
                case LexemeDataType.Double:
                    _data = Double.Parse(LexemValue);
                    break;
                case LexemeDataType.Float:
                    _data = float.Parse(LexemValue);
                    break;
                case LexemeDataType.Integer:
                    _data = Int32.Parse(LexemValue);
                    break;
                case LexemeDataType.Long:
                    _data = Int64.Parse(LexemValue);
                    break;
                case LexemeDataType.Short:
                    _data = Int16.Parse(LexemValue);
                    break;
                case LexemeDataType.UnsignedChar:
                    _data = byte.Parse(LexemValue);
                    break;
                case LexemeDataType.UnsignedInt:
                    _data = UInt32.Parse(LexemValue);
                    break;
                case LexemeDataType.UnsignedLong:
                    _data = UInt64.Parse(LexemValue);
                    break;
                //TODO:Fill rest type conversion
            }
        }
        
    }    
}
