using System;
using System.Collections.Generic;
using System.IO;
using ArduinoLanguage;
using ArduinoLanguage.Errors;
using Xunit;

namespace ArduinoLanguageTest
{
    public class LexemAnalisisTest
    {
        [Fact]
        public void Test_LexemeAnalisis_FromTestCodeSample()
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestCodeSample");
            foreach(string filePath in Directory.EnumerateFiles(directoryPath, "*.ino",SearchOption.TopDirectoryOnly))
            {
                string code = File.ReadAllText(filePath);
                LexemeAnalisis analisis = new LexemeAnalisis(code);
                IEnumerable<Error> errors = analisis.Analyse();
                Assert.Empty(errors);
                foreach(Lexeme lexem in analisis.LexemeList)
                {
                    Assert.NotNull(lexem.LexemValue);
                    Assert.NotEqual(ArduinoLanguage.Enums.LexemeTypes.Underfined, lexem.Type);
                    Assert.NotEqual(0, lexem.Line);
                }
            }
        }
    }
}
