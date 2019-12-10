using System.Collections.Generic;
using System.Linq;
using ArduinoLanguage.Enums;
using ArduinoLanguage.Errors;

namespace ArduinoLanguage
{
    public class LexemeAnalisis
    {
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
        /// <summary>
        /// Lexical code analysis, finde lexemes and error list building
        /// </summary>
        /// <param name="code">Parsed code</param>
        /// <param name="errors">List of warnings and errors derived from lexical analysis</param>
        /// <returns>Founden lexeme list</returns>
        public LinkedList<Lexeme> Analyse(string code, out IEnumerable<Error> errors)
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
            int codeLine = 1; //Code line counter
            string lexem = null; //Current pocess lexem
            bool stringState = false; //String build process state
            try
            {
                for (var i = 0; i < codeLen; i++)
                {
                    char c = code[i];
                    lexem += c;
                    if (c != '(' && c != ')' && c != ';' && c != '+' && c != '-' && c != '=' && c != '>' && c != '<' && c != '*' && c != '/' && c != ',' && c != '{' && c != '}' && c != '!' && c != '|' && c != '&' && c != ' ' && c != '\n' && c != '\t')
                    {
                        if (c == '"' && !stringState)
                            stringState = true;
                        else
                            if (stringState && c == '"')
                            stringState = false;
                        if (c == '\'')
                        {
                            if (lexem[0] != '\'')
                                throw new StringError(codeLine);

                            lexem += code[i++];
                            if (!(lexem[1] >= 'a' && lexem[0] <= 'z') && !(lexem[1] >= 'A' && lexem[0] <= 'Z') && !(lexem[1] >= '0' && lexem[1] <= '9'))
                            {
                                if (lexem[1] != '\'')
                                    throw new StringEndError(codeLine);
                                else
                                    throw new CharConstEmpty(codeLine);
                            }
                            else
                                lexem += code[i++];
                            if (lexem[2] != '\'')
                                throw new CharConstNotClosed(codeLine);
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
        private bool TryGetLexemeCharType(char c, out LexemeTypes type)
        {
            List<Error> errorList = new List<Error>();
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
