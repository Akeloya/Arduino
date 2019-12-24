using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ArduinoEmulator.MVVM
{
    public class CodeContent
    {
        public string Name { get; set; }
        public string FilePath { get; }
        public Stack<string> Undo { get; } = new Stack<string>();
        public Stack<string> Redo { get; } = new Stack<string>();
        public string Text { get; set; }

        public CodeContent()
        {

        }
        public CodeContent(string fileName)
        {
            Text = File.ReadAllText(fileName);
        }
    }
}
