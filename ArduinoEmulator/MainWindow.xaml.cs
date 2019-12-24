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
using ArduinoEmulator.MVVM;
using ArduinoLanguage;
using ArduinoLanguage.Errors;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock.Layout;

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
            /*var text = File.ReadAllText(@"C:\Users\Максим\Source\Repos\Arduino\ArduinoLanguageTest\TestCodeSample\Analog read serial.ino");
            rtbDisplay.Text = text + "\n\n-----------------------\n";
            LexemeAnalisis analisis = new LexemeAnalisis(text);
            IEnumerable<Error> errors = analisis.Analyse();
            foreach (Lexeme lexem in analisis.LexemeList)
            {
                rtbDisplay.Text += lexem.LexemValue + "\n";
            }*/
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

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LdXceedDocPanel.Children.Add(ContentControl.DocumentFactory<LayoutDocument>());
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                using (Stream fileStream = ofd.OpenFile())
                {
                    byte[] bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, (int)fileStream.Length);//TODO: fix type mismatch
                    fileStream.Close();
                    string text = Encoding.UTF8.GetString(bytes);
                    LdXceedDocPanel.Children.Add(ContentControl.DocumentFactory<LayoutDocument>(new object[] { ofd.SafeFileName, text }));
                }
            }
        }
    }
}
