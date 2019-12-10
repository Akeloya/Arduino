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
                LexemeAnalisis analisis = new LexemeAnalisis();
                analisis.Analyse(code, out IEnumerable<Error> errors);
                Assert.Empty(errors);
            }
        }
    }
}
