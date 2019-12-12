using System.Collections.Generic;
using System.Linq;
using ArduinoLanguage.Enums;
using ArduinoLanguage.Errors;

namespace ArduinoLanguage
{
    /// <summary>
    /// Анализ кода и формирование списка лексем для синтаксического анализа
    /// </summary>
    public class LexemeAnalisis
    {
        private readonly Stack<LexemAnalisisState> analysState = new Stack<LexemAnalisisState>();
        private readonly LinkedList<Lexeme> _lexemeList = new LinkedList<Lexeme>();
        private readonly List<Error> _errors = new List<Error>();
        private readonly string _code;
        private readonly int _codeLen;
        private int _position;
        private int _codeLine;
        private string _lexeme;

        public LinkedList<Lexeme> LexemeList => _lexemeList;

        public static readonly string[] ReservedWords = { "int", "double", "void", "char", "return", "if", "while", "else", "do", "switch", "case", "continue", "break" };
        private static Dictionary<char, LexemeTypes> CharLexemTypes = new Dictionary<char, LexemeTypes>()
        {
            { '(', LexemeTypes.BracketOpen },
            { ')', LexemeTypes.BracketClose },
            { '{', LexemeTypes.BlockOpen },
            { '}', LexemeTypes.BlockClose },
            { '-', LexemeTypes.Minus },
            { '+', LexemeTypes.Plus },
            { '=', LexemeTypes.Assignment },
            { '%', LexemeTypes.Percent },
            { '*', LexemeTypes.Multiplying },
            { '/', LexemeTypes.Division },
            { ';', LexemeTypes.Semicolon },
            { ',', LexemeTypes.Comma },
            { '!', LexemeTypes.Not },
            { '>', LexemeTypes.Bigger },
            { '<', LexemeTypes.Less },
            { '|', LexemeTypes.BinaryOr },
            { '&', LexemeTypes.BinaryAnd }
        };

        public LexemeAnalisis(string code)
        {
            _code = code;
            _codeLen = _code.Length;
        }
        /// <summary>
        /// Lexical code analysis, finde lexemes and error list building
        /// </summary>
        /// <returns>Collection or errors, warnings in code</returns>
        public IEnumerable<Error> Analyse()
        {
            _errors.Clear();
            _lexemeList.Clear();

            if (string.IsNullOrEmpty(_code))
            {
                _errors.Add(new EmptyFileError());
                return _errors;
            }

            _lexeme = null;
            _codeLine = 1;
            _lexemeList.Clear();
            bool stringState = false;
            analysState.Push(LexemAnalisisState.LexemAnalys);
            try
            {
                for (_position = 0; _position < _codeLen; _position++)
                {
                    char c = _code[_position];
                    _lexeme += c;
                    if(c == '/') //Комментарий или знак деления
                    {
                        if (ProcesshDivisionSymbol())
                            continue;
                    }

                    if(c == '*')
                    {
                        if (ProcessMultiplyingSymbol())
                            continue;
                    }
                    if (c == '\n' || c == '\r')
                        if (ProcessNewLine(c))
                            continue;
                    if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                        if (ProcessLetter())
                            continue;
                    if ((c >= '0' && c <= '9'))
                        if (ProcessDigit())
                            continue;
                    ProcessChars(c);
                    /*
                    if (c != '(' && c != ')' && c != ';' && c != '+' && c != '-' && c != '=' && c != '>' && c != '<' && c != '*' && c != '/' && c != ',' && c != '{' && c != '}' && c != '!' && c != '|' && c != '&' && c != ' ' && c != '\n' && c != '\t')
                    {
                        if (c == '"' && !stringState)
                            stringState = true;
                        else
                            if (stringState && c == '"')
                            stringState = false;
                        if (c == '\'')
                        {
                            if (_lexeme[0] != '\'')
                                throw new StringError(_codeLine);

                            _lexeme += _code[_position++];
                            if (!(_lexeme[1] >= 'a' && _lexeme[0] <= 'z') && !(_lexeme[1] >= 'A' && _lexeme[0] <= 'Z') && !(_lexeme[1] >= '0' && _lexeme[1] <= '9'))
                            {
                                if (_lexeme[1] != '\'')
                                    throw new StringEndError(_codeLine);
                                else
                                    throw new CharConstEmpty(_codeLine);
                            }
                            else
                                _lexeme += _code[_position++];
                            if (_lexeme[2] != '\'')
                                throw new CharConstNotClosed(_codeLine);
                        }
                    }
                    else
                    {
                        if (!stringState)
                        {
                            if (_lexeme[0] != 0)
                            {
                                LexemeTypes type = GetLexemType(_lexeme, _codeLine, out IEnumerable<Error> subErrors);
                                Lexeme newLexem = new Lexeme(_lexeme, type, _codeLine);
                                _lexemeList.AddLast(newLexem);
                                _lexeme = null;
                            }
                            if (c == '\n')
                                _codeLine++;
                            _lexeme = null;
                            _position = 0;
                            if (c != ' ' && c != '\n' && c != '\t')
                            {
                                string oneCharLexem = "" + c;
                                LexemeTypes type = GetLexemType(oneCharLexem, _codeLine, out IEnumerable<Error> subErrors);
                                _errors.AddRange(subErrors);
                                Lexeme newlexem = new Lexeme(oneCharLexem, type, _codeLine);
                                newlexem.GetData();
                                _lexemeList.AddLast(newlexem);
                                _lexeme = string.Empty;
                            }
                        }
                        else
                        {
                            _lexeme += c;
                        }
                    }*/
                }
            }
            catch (Error err)
            {
                _errors.Add(err);
            }
            return _errors;
        }

        private bool ProcesshDivisionSymbol()
        {
            if (analysState.Peek() == LexemAnalisisState.LexemAnalys)
            {
                analysState.Push(LexemAnalisisState.Division);
                return true;
            }
            //Строчный комментарий
            if (analysState.Peek() == LexemAnalisisState.Division)
            {
                analysState.Pop();
                int endLineIndex = _code.IndexOf('\n', _position);
                if (endLineIndex == -1) //дошли до конца, а перевода строки нет
                    endLineIndex = _codeLen;
                _lexeme += _code.Substring(_position + 1, _codeLen - _position); //весь комментарий в лексему
                _position = endLineIndex;
                _lexemeList.AddLast(new Lexeme(_lexeme, LexemeTypes.SingleLineComment, _codeLine));
                _codeLine++;
                return true;
            }
            //Если ранее было число - формируем 2 лексемы
            if (analysState.Peek() == LexemAnalisisState.Digit || analysState.Peek() == LexemAnalisisState.Decimal)
            {
                LexemeTypes type = LexemeTypes.Underfined;
                if (analysState.Peek() == LexemAnalisisState.Decimal)
                {
                    analysState.Pop();
                    type = LexemeTypes.Double;
                }
                if (analysState.Peek() == LexemAnalisisState.Digit)
                {
                    analysState.Pop();
                    if(type != LexemeTypes.Double)
                        type = LexemeTypes.Integer;
                    
                }
                _lexemeList.AddLast(new Lexeme(_lexeme, type, _codeLine));
                _lexemeList.AddLast(new Lexeme("/", LexemeTypes.Division, _codeLine));
                return true;
            }
            return false;
        }
        private bool ProcessMultiplyingSymbol()
        {
            LexemAnalisisState state = analysState.Pop();
            bool processSecceed = false;
            switch(state)
            {
                //Если предыдущий символ был "/", то это блоковый комментарий
                case LexemAnalisisState.Division:
                    analysState.Pop();
                    analysState.Push(LexemAnalisisState.BlockComment);
                    processSecceed = true;
                    break;
                //Если мы формируем строку или строчный комментарий - то добавляем лишь очередной символ игнорируя значение
                case LexemAnalisisState.String:
                case LexemAnalisisState.LineComment:
                    processSecceed = true;
                    break;
                case LexemAnalisisState.BlockComment:
                    analysState.Push(LexemAnalisisState.Multiplying);
                    processSecceed = true;
                    break;
            }
            if (processSecceed)
                analysState.Push(state);
            return processSecceed;
        }

        private bool ProcessNewLine(char c)
        {
            LexemAnalisisState state = analysState.Peek();
            //Если формируется блоковый комментарий, просто увеличиваем счетчик новых строк
            if (state == LexemAnalisisState.BlockComment)
            {
                if(c == '\n')
                    _codeLine++;
                return true;
            }
            //Если формировали строковую константу и был перевод строки, формируем лексему, увеличиваем счетчик и завершаем обработку
            //А так же добавляем ошибку
            if(state == LexemAnalisisState.String)
            {
                if (c == '\n')
                {
                    _errors.Add(new StringStartError(_codeLine));
                    _codeLine++;
                }
                return true;
            }
            return false;
        }
        private bool ProcessLetter()
        {
            LexemAnalisisState state = analysState.Peek();
            switch(state)
            {
                //Если комментарий или строка, добавляем символ к лексеме без дальнейшей обработки
                case LexemAnalisisState.BlockComment:
                case LexemAnalisisState.String:
                case LexemAnalisisState.LineComment:
                    return true;
            }
            return false;
        }
        private bool ProcessDigit()
        {
            LexemAnalisisState state = analysState.Peek();
            switch(state)
            {
                //Если комментарий, или число добавляем число к лексеме и обрабатываем дальше
                case LexemAnalisisState.BlockComment:
                case LexemAnalisisState.Digit:
                case LexemAnalisisState.LineComment:
                case LexemAnalisisState.Decimal:
                    return true;
            }
            return false;
        }
        private void ProcessChars(char c)
        {
            LexemAnalisisState state = analysState.Peek();
            switch(state)
            {
                case LexemAnalisisState.BlockComment:
                case LexemAnalisisState.LineComment:
                    return;
            }
        }
        private bool TryGetLexemeCharType(char c, out LexemeTypes type)
        {
            bool result = true; //we can get one place with error, there will be false
            if (CharLexemTypes.ContainsKey(c))
                type = CharLexemTypes[c];
            else
            {
                if (c >= '0' && c <= '9')
                    type = LexemeTypes.Integer;
                else
                    if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                    type = LexemeTypes.Identificator;
                else
                {
                    result = false;
                    type = LexemeTypes.Identificator;
                }
            }
            return result;
        }

        /// <summary>
        /// Lexem type definition
        /// </summary>
        /// <param name="lexem">String lexem</param>
        /// <param name="codeLine">Code line position</param>
        /// <param name="errors">Out error list of lexem type definition</param>
        /// <returns>LexemTypes value</returns>
        private LexemeTypes GetLexemType(string lexem, int codeLine, out IEnumerable<Error> errors)
        {
            LexemeTypes type = LexemeTypes.Underfined;
            List<Error> errorList = new List<Error>();
            if (lexem.Length == 1)
            {
                if (!TryGetLexemeCharType(lexem[0], out type))
                    errorList.Add(new StringError(codeLine));
            }
            else
            {
                //checks for char const
                if (lexem[0] == '\'')
                {
                    if (lexem.Length != 3 || lexem[2] != '\'')
                    {
                        errorList.Add(new StringError(codeLine));
                        type = LexemeTypes.Char;
                    }
                    else
                        type = LexemeTypes.Char;
                }
                else
                {
                    //checks for string const line "some text"
                    if (lexem[0] == '"')
                    {
                        if (lexem[lexem.Length - 1] != '"')
                            errorList.Add(new StringEndError(codeLine));
                        else
                            type = LexemeTypes.String;
                    }
                    else
                    {
                        //checks for reserved word
                        if (ReservedWords.Contains(lexem))
                            type = LexemeTypes.ReservedWord;
                        else
                        {
                            type = LexemeTypes.Identificator;
                            //checks for correct identificator value
                            if ((lexem[0] >= 'a' && lexem[0] <= 'z') || (lexem[0] >= 'A' && lexem[0] <= 'Z'))
                            {
                                int k = 1;
                                bool l = false;
                                while (k < lexem.Length)
                                {
                                    if ((lexem[k] >= 'a' && lexem[k] <= 'z') || (lexem[k] >= 'A' && lexem[k] <= 'Z') || (lexem[0] >= '0' && lexem[0] <= '9'))
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

                            }
                            else
                            {
                                //checks for digit value
                                if (lexem[0] >= '0' && lexem[0] <= '9')
                                {
                                    int k = 0;
                                    int decimalPointCounts = 0;
                                    while (k < lexem.Length)
                                    {
                                        if (lexem[k] >= '0' && lexem[k] <= '9')
                                            k++;
                                        else
                                            if (lexem[k] == '.' && k < lexem.Length - 1)
                                        {
                                            k++;
                                            decimalPointCounts = 1;
                                        }
                                        else
                                        {
                                            errorList.Add(new StringError(codeLine));
                                            break;
                                        }
                                    }
                                    switch (decimalPointCounts)
                                    {
                                        case 0:
                                            type = LexemeTypes.Integer;
                                            break;
                                        case 1:
                                            type = LexemeTypes.Integer;
                                            break;
                                        default:
                                            errorList.Add(new StringError(codeLine));
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            errors = errorList;
            return type;
        }
    }
}
