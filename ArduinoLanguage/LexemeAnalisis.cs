﻿using System;
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
                    ProcessSymbol(c);
                }
            }
            catch (Error err)
            {
                _errors.Add(err);
            }
            return _errors;
        }
        private void ProcessSymbol(char c)
        {
            if (c == '/') //Комментарий или знак деления
                if (ProcesshDivisionSymbol())
                    return;

            if (c == '*')
            {
                if (ProcessMultiplyingSymbol())
                    return;
            }
            if (c == '\n' || c == '\r')
                if (ProcessNewLine(c))
                    return;
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                if (ProcessLetter())
                    return;
            if ((c >= '0' && c <= '9'))
                if (ProcessDigit())
                    return;
            ProcessChars(c);
        }

        private bool ProcesshDivisionSymbol()
        {
            LexemAnalisisState state = analysState.Peek();
            if (state == LexemAnalisisState.LexemAnalys)
            {
                analysState.Push(LexemAnalisisState.Division);
                return true;
            }
            //Строчный комментарий
            if (state == LexemAnalisisState.Division)
            {
                analysState.Pop();
                state = analysState.Peek();
                if (state == LexemAnalisisState.BlockComment || state == LexemAnalisisState.LineComment) //не интересуют символы косой черты в комментариях
                    return true;
                int endLineIndex = _code.IndexOf('\n', _position);
                if (endLineIndex == -1) //дошли до конца, а перевода строки нет
                    endLineIndex = _codeLen;
                _lexeme += _code.Substring(_position + 1, endLineIndex - _position - 1); //весь комментарий в лексему
                _position = endLineIndex;
                //_lexemeList.AddLast(new Lexeme(_lexeme, LexemeTypes.SingleLineComment, _codeLine));
                _lexeme = null;
                _codeLine++;
                return true;
            }
            //Если ранее было число - формируем 2 лексемы
            if (state == LexemAnalisisState.Digit || state == LexemAnalisisState.Decimal)
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
                _lexeme = null;
                return true;
            }
            if(state == LexemAnalisisState.BlockComment || state == LexemAnalisisState.LineComment)
            {
                return true;
            }
            return false;
        }
        private bool ProcessMultiplyingSymbol()
        {
            LexemAnalisisState state = analysState.Peek();
            bool processSecceed = false;
            switch(state)
            {
                //Если предыдущий символ был "/", то это блоковый комментарий
                case LexemAnalisisState.Division:
                    analysState.Pop();
                    //Ищем конец блокового комментария
                    int endLineIndex = _code.IndexOf("*/", _position);
                    if (endLineIndex == -1) //дошли до конца, а комментарий не закрыт!
                    {
                        endLineIndex = _codeLen;
                        throw new NotImplementedException("Не реализована ошибка незакрытого комментария");
                    }
                    _lexeme += _code.Substring(_position + 1, endLineIndex - _position - 1); //весь комментарий в лексему
                    _position = endLineIndex;
                    //Посчитаем переводы строк:
                    _codeLine += _lexeme.Where(x => x == '\n').Count();
                    //Затираем лексему, т.к. комментарий нас не интересует совсем
                    _lexeme = null;
                    processSecceed = true;
                    break;
                //Если мы формируем строку или строчный комментарий - то добавляем лишь очередной символ игнорируя значение
                case LexemAnalisisState.String:
                case LexemAnalisisState.LineComment:
                    processSecceed = true;
                    break;
                case LexemAnalisisState.BlockComment:
                    analysState.Push(LexemAnalisisState.Multiplying); //Возможно следующим символом будет закрыт блочный комментарий
                    processSecceed = true;
                    break;
            }
            return processSecceed;
        }

        private bool ProcessNewLine(char c)
        {
            LexemAnalisisState state = analysState.Peek();
            switch(state)
            {
                case LexemAnalisisState.BlockComment:
                    //Если формируется блоковый комментарий, просто увеличиваем счетчик новых строк
                    if (c == '\n')
                        _codeLine++;
                    return true;
                case LexemAnalisisState.LexemAnalys:
                    if (_lexeme.Length > 1)
                        _lexeme = _lexeme.Substring(0, _lexeme.Length - 1);
                    else
                        _lexeme = null;
                    _codeLine++;
                    return false;
                case LexemAnalisisState.String:
                    //Если формировали строковую константу и был перевод строки, формируем лексему, увеличиваем счетчик и завершаем обработку
                    //А так же добавляем ошибку
                    _errors.Add(new StringStartError(_codeLine));
                    _codeLine++;
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
                default:
                    if(c == ' ')
                    {
                        if (_lexeme.Length > 1)
                        {
                            _lexeme = _lexeme.Substring(0, _lexeme.Length - 1);
                            _lexemeList.AddLast(new Lexeme(_lexeme, LexemeTypes.String, _codeLine));
                            _lexeme = null;
                        }
                    }
                    break;
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
    }
}
