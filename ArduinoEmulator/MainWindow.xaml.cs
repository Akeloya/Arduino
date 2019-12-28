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

using ArduinoEmulator.Commands;
using ArduinoEmulator.Controls;
using ArduinoEmulator.Forms;
using ArduinoEmulator.MVVM;
using ArduinoLanguage;
using ArduinoLanguage.Errors;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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

        internal bool IsFullScreen { get { return (bool)GetValue(IsFullScreenProperty); } set { SetValue(IsFullScreenProperty, value); } }
        internal static readonly DependencyProperty IsFullScreenProperty = DependencyProperty.Register(nameof(IsFullScreen), typeof(bool), typeof(MainWindow), new PropertyMetadata(false, (sender, args)=> {

            MainWindow window = (MainWindow)sender;
            bool newState = (bool)args.NewValue;

            if (newState)
            {
                window.WindowStyle = WindowStyle.None;
                window.ResizeMode = ResizeMode.NoResize;
                window.WindowState = WindowState.Normal;
                window.WindowState = WindowState.Maximized;
                window.MenuStrip.VerticalAlignment = VerticalAlignment.Bottom;
            }
            else
            {
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.ResizeMode = ResizeMode.CanResize;
                window.MenuStrip.VerticalAlignment = VerticalAlignment.Stretch;
            }
        }));

        private void Build_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /*var text = File.ReadAllText(@"C:\Users\Максим\Source\Repos\Arduino\ArduinoLanguageTest\TestCodeSample\Analog read serial.ino");
            rtbDisplay.Text = text + "\n\n-----------------------\n";
            LexemeAnalisis analisis = new LexemeAnalisis(text);
            IEnumerable<Error> errors = analisis.Analyse();
            rtbDisplay.Text = text + "\n\n-----------------------\n";
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
            LdXceedDocPanel.Children.Add(WndContentControl.DocumentFactory<LayoutDocument>());
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFile();
        }

        private void Recent_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string fileName = e.Parameter?.ToString();
            if (string.IsNullOrEmpty(fileName))
                return;
            OpenFile(fileName);
        }

        private void Recent_SubMenuOpened(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem item = (System.Windows.Controls.MenuItem)e.Source;
            item.Items.Clear();
#if DEBUG
            string[] testItems = new[] { "Bare minimum code.ino", "Analog read serial.ino", "Arrays.ino", "Blink.ino", "Graph.ino", "SwitchCase2.ino", "StringComparisonOperator.ino" };
            foreach (string testItem in testItems)
            {
                item.Items.Add(new System.Windows.Controls.MenuItem
                { Header = testItem,
                    CommandParameter = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, testItem),
                    Command = MenuCommands.Recent });
            }
#endif   
        }

        private void OpenFile(string fileName = null)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (string.IsNullOrEmpty(fileName))
            {
                if (ofd.ShowDialog() != true)
                {
                    return;
                }
            }
            else
            {
                ofd.FileName = fileName;
            }
            try
            {
                using (Stream fileStream = ofd.OpenFile())
                {
                    byte[] bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, (int)fileStream.Length);//TODO: fix type mismatch
                    fileStream.Close();
                    string text = Encoding.UTF8.GetString(bytes);
                    LdXceedDocPanel.Children.Add(WndContentControl.DocumentFactory<LayoutDocument>(new object[] { ofd.SafeFileName, text }));
                }
            }
            catch(FileNotFoundException e)
            {
                new MessageWnd(e, ofd.FileName).Show();
            }
        }

        private void FullScreen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IsFullScreen = !IsFullScreen;
        }

        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog
            {
                CurrentPageEnabled = true
            };
            if (printDlg.ShowDialog() == true)
            {
                
            }
        }

        private void CloseFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(LdXceedDocPanel.SelectedContent is LayoutDocument document)
            {
                document.Close();
            }
        }
    }
}
