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
        /// <summary>
        /// Lexical code analysis, finde lexemes and error list building
        /// </summary>
        /// <param name="code">Parsed code</param>
        /// <param name="errors">List of warnings and errors derived from lexical analysis</param>
        /// <returns>Founden lexeme list</returns>
        public static LinkedList<Lexeme> Analyse(string code, out IEnumerable<Error> errors)
        {
            List<Error> errorList = new List<Error>();
            LinkedList<Lexeme> lexemList = new LinkedList<Lexeme>();

            if (string.IsNullOrEmpty(code))
            {
                errorList.Add(new EmptyFileError());
                errors = errorList;
                return lexemList;
            }
            int codeLen = code.Length;
            int codeLine = 1;//Code line counter
            string lexem = null;//Current pocess lexem
            bool stringState = false;//String process state
            try
            {
                for (var i = 0; i < codeLen; i++)
                {
                    char c = code[i];
                    if (c != '(' && c != ')' && c != ';' && c != '+' && c != '-' && c != '=' && c != '>' && c != '<' && c != '*' && c != '/' && c != ',' && c != '{' && c != '}' && c != '!' && c != '|' && c != '&' && c != ' ' && c != '\n' && c != '\t')
                    {
                        lexem += c;
                        if (c == '"' && !stringState)
                            stringState = true;
                        else
                            if (stringState && c == '"')
                            stringState = false;
                        if (c == '\'')
                        {
                            if (lexem[0] != '\'')
                            {
                                throw new StringError(codeLine);
                            }
                            lexem += code[i++];
                            if (!(lexem[1] >= 'a' && lexem[0] <= 'z') && !(lexem[1] >= 'A' && lexem[0] <= 'Z') && !(lexem[1] >= '0' && lexem[1] <= '9'))
                            {
                                if (lexem[1] != '\'')
                                {
                                    throw new StringEndError(codeLine);
                                }
                                else
                                {
                                    throw new CharConstEmpty(codeLine);
                                }
                            }
                            else
                            {
                                lexem += code[i++];
                            }
                            if (lexem[2] != '\'')
                            {
                                throw new CharConstNotClosed(codeLine);
                            }
                        }
                    }
                    else
                    {
                        if (!stringState)
                        {
                            if (lexem[0] != 0)
                            {
                                LexemeTypes type = GetLexemType(lexem, codeLen, out IEnumerable<Error> subErrors);
                                Lexeme newLexem = new Lexeme(lexem, type, codeLine);
                                lexemList.AddLast(newLexem);
                                lexem = null;
                            }
                            if (c == '\n')
                                codeLine++;
                            lexem = null;
                            i = 0;
                            if (c != ' ' && c != '\n' && c != '\t')
                            {
                                string oneCharLexem = "" + c;
                                LexemeTypes type = GetLexemType(oneCharLexem, codeLine, out IEnumerable<Error> subErrors);
                                errorList.AddRange(subErrors);
                                Lexeme newlexem = new Lexeme(oneCharLexem, type, codeLine);
                                newlexem.GetData();
                                lexemList.AddLast(newlexem);
                                lexem = string.Empty;
                            }
                        }
                        else
                        {
                            lexem += c;
                        }
                    }
                }
            }
            catch (Error err)
            {
                errorList.Add(err);
            }
            errors = errorList;
            return lexemList;
        }
        /// <summary>
        /// Lexem type definition
        /// </summary>
        /// <param name="lexem">String lexem</param>
        /// <param name="codeLine">Code line position</param>
        /// <param name="errors">Out error list of lexem type definition</param>
        /// <returns>LexemTypes value</returns>
        internal static LexemeTypes GetLexemType(string lexem, int codeLine, out IEnumerable<Error> errors)
        {
            LexemeTypes type = LexemeTypes.Underfined;
            List<Error> errorList = new List<Error>();
            if (lexem.Length == 1)
            {

                switch (lexem[0])
                {
                    case ('('):
                        type = LexemeTypes.BracketOpen;
                        break;
                    case (')'):
                        type = LexemeTypes.BracketClose;
                        break;
                    case ('{'):
                        type = LexemeTypes.BlockOpen;
                        break;
                    case ('}'):
                        type = LexemeTypes.BlockClose;
                        break;
                    case ('-'):
                        type = LexemeTypes.Minus;
                        break;
                    case ('+'):
                        type = LexemeTypes.Plus;
                        break;
                    case ('='):
                        type = LexemeTypes.Assignment;
                        break;
                    case ('%'):
                        type = LexemeTypes.Percent;
                        break;
                    case ('*'):
                        type = LexemeTypes.Multiplying;
                        break;
                    case ('/'):
                        type = LexemeTypes.Division;
                        break;
                    case (';'):
                        type = LexemeTypes.Semicolon;
                        break;
                    case (','):
                        type = LexemeTypes.Comma;
                        break;
                    case ('!'):
                        type = LexemeTypes.Not;
                        break;
                    case ('>'):
                        type = LexemeTypes.Bigger;
                        break;
                    case ('<'):
                        type = LexemeTypes.Less;
                        break;
                    case ('|'):
                        type = LexemeTypes.BinaryOr;
                        break;
                    case ('&'):
                        type = LexemeTypes.BinaryAnd;
                        break;
                    default:
                        {
                            if (lexem[0] >= '0' && lexem[0] <= '9')
                                type = LexemeTypes.Integer;
                            else
                                if ((lexem[0] >= 'a' && lexem[0] <= 'z') || (lexem[0] >= 'A' && lexem[0] <= 'Z'))
                                type = LexemeTypes.Identificator;
                            else
                            {
                                errors = new List<Error> { new StringError(codeLine) };
                                return LexemeTypes.Identificator;
                            }
                        }
                        break;
                }
            }
            else
            {
                if ((lexem[0] >= 'a' && lexem[0] <= 'z') || (lexem[0] >= 'A' && lexem[0] <= 'Z'))
                {
                    int k = 0;
                    bool l = false;
                    while (k < lexem.Length)
                    {
                        if ((lexem[k] >= 'a' && lexem[k] <= 'z') || (lexem[k] >= 'A' && lexem[k] <= 'Z'))
                            k++;
                        else
                        {
                            k++;
                            if (!l)
                                l = true;
                            else
                            {
                                errorList.Add(new StringError(codeLine));
                                break;
                            }
                        }
                    }
                    //TODO: fill rest reserved words
                    string[] reservedWords = { "int", "double", "void", "char", "return", "if", "while", "else", "do", "switch", "case", "continue", "break" };
                    if (reservedWords.Contains(lexem))
                        type = LexemeTypes.ReservedWord;
                    else
                        type = LexemeTypes.Identificator;
                }
                if (lexem[0] == '"')
                {
                    if (lexem[lexem.Length - 1] != '"')
                    {
                        errorList.Add(new StringEndError(codeLine));
                    }
                    else
                        type = LexemeTypes.String;
                }
                if (lexem[0] >= '0' && lexem[0] <= '9')
                {
                    int k = 0;
                    int d = 0;
                    while (k < lexem.Length)
                    {
                        if (lexem[k] >= '0' && lexem[k] <= '9')
                            k++;
                        else
                            if (lexem[k] == '.' && k < lexem.Length - 1)
                        {
                            k++;
                            d = 1;
                        }
                        else
                        {
                            errorList.Add(new StringError(codeLine));                             
                            break;
                        }
                    }
                    if (d == 1)
                        type = LexemeTypes.Double;
                    else
                        type = LexemeTypes.Integer;
                }

                if (lexem[0] == '\'')
                    if (lexem.Length != 3 || lexem[2] != '\'')
                    {
                        errorList.Add(new StringError(codeLine));
                        type = LexemeTypes.Char;
                    }
                    else
                        type = LexemeTypes.Char;
            }
            errors = errorList;
            return type;
        }
    }
}
