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
 using ArduinoEmulator.Core;
using ArduinoEmulator.Resources;
using System;
using System.Windows;

namespace ArduinoEmulator.Forms
{
    /// <summary>
    /// Логика взаимодействия для MessageWnd.xaml
    /// </summary>
    public partial class MessageWnd : Window
    {
        public MessageWnd()
        {
            InitializeComponent();
        }

        public MessageWnd(Exception ex, params object?[] args)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));

            InitializeComponent();
            Title = Resource.ErrorWndMessage;

            TbMessate.Text = ResourceStringResolver.ResolveExceptionString(ex, args);
        }
    }
}
