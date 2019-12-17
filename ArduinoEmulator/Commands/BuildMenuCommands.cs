using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArduinoEmulator.Commands
{
    public static class BuildMenuCommands
    {
        public static RoutedCommand Build = new RoutedUICommand("Build", nameof(Build), typeof(UserControl));
    }
}
