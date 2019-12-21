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
 
 using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArduinoEmulator.Controls
{
    /// <summary>
    /// Логика взаимодействия для CloseButton.xaml
    /// </summary>
    public partial class CloseButton : UserControl
    {
        public CloseButton()
        {
            InitializeComponent();
        }

        private void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            CloseCanExecuteEventArgs canExec = new CloseCanExecuteEventArgs();
            CanClose?.Invoke(this, canExec);
            e.CanExecute = canExec.CanExecute;
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Closed?.Invoke(this, EventArgs.Empty);
            Window.GetWindow(this)?.Close();
        }

        public event EventHandler<CloseCanExecuteEventArgs> CanClose;
        public event EventHandler<EventArgs> Closed;
    }

    public class CloseCanExecuteEventArgs : EventArgs
    {
        public bool CanExecute { get; set; } = true;
    }
}
