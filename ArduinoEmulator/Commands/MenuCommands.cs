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
using System.Windows.Controls;
using System.Windows.Input;

namespace ArduinoEmulator.Commands
{
    public static class MenuCommands
    {
        public static readonly RoutedCommand New = new RoutedUICommand(nameof(New), nameof(New), typeof(MenuItem));
        public static readonly RoutedCommand Open = new RoutedUICommand(nameof(Open), nameof(Open), typeof(MenuItem));
        public static readonly RoutedCommand Close = new RoutedUICommand(nameof(Close), nameof(Close), typeof(MenuItem));
        public static readonly RoutedCommand Print = new RoutedUICommand(nameof(Print), nameof(Print), typeof(MenuItem));
        public static readonly RoutedCommand PageSettings = new RoutedUICommand(nameof(PageSettings), nameof(PageSettings), typeof(MenuItem));
        public static readonly RoutedCommand Recent = new RoutedUICommand(nameof(Recent), nameof(Recent), typeof(MenuItem));
        public static readonly RoutedCommand Save = new RoutedUICommand(nameof(Save), nameof(Save), typeof(MenuItem));
        public static readonly RoutedCommand SaveAs = new RoutedUICommand(nameof(SaveAs), nameof(SaveAs), typeof(MenuItem));
        public static readonly RoutedCommand Build = new RoutedUICommand(nameof(Build), nameof(Build), typeof(MenuItem));
        public static readonly RoutedCommand AboutBox = new RoutedUICommand(nameof(AboutBox), nameof(Build), typeof(MenuItem));
    }
}
