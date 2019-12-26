using System.Windows.Controls;
using System.Windows.Input;

namespace ArduinoEmulator.Commands
{
    public static class BuildMenuCommands
    {
        public static readonly RoutedCommand Build = new RoutedCommand(nameof(Build), typeof(Button));
    }
}
