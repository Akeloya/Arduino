/*
 *  "Arduino emulator", the simple virtual emulator arduino circuit.
 *  Copyright (C) 2019 by Maxim V. Yugov.
 *
 *  This file is part of "Arduino emulator".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using ArduinoEmulator.Forms;
using ArduinoLanguage;
using ArduinoLanguage.Errors;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ArduinoEmulator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Build_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var text = File.ReadAllText(@"..\..\..\..\ArduinoLanguageTest\TestCodeSample\Analog read serial.ino");
            LexemeAnalisis analisis = new LexemeAnalisis(text);
            IEnumerable<Error> errors = analisis.Analyse();
            rtbDisplay.Text = text + "\n\n-----------------------\n";
            foreach (Lexeme lexem in analisis.LexemeList)
            {
                rtbDisplay.Text += lexem.LexemValue + "\n";
            }
        }

        private void AboutBox_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AboutBox box = new AboutBox();
            box.Show();
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
