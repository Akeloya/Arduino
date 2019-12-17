using ArduinoLanguage;
using ArduinoLanguage.Errors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            var text = File.ReadAllText(@"C:\Users\Максим\Source\Repos\Arduino\ArduinoLanguageTest\TestCodeSample\Analog read serial.ino");
            LexemeAnalisis analisis = new LexemeAnalisis(text);
            IEnumerable<Error> errors = analisis.Analyse();
            rtbDisplay.Text = text + "\n\n-----------------------\n";
            foreach (Lexeme lexem in analisis.LexemeList)
            {
                rtbDisplay.Text += lexem.LexemValue + "\n";
            }
        }
    }
}
