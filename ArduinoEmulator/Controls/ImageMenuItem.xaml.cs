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
using ArduinoEmulator.Converters;
using ArduinoEmulator.Core;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ArduinoEmulator.Controls
{
    /// <summary>
    /// Логика взаимодействия для ImageMenuItem.xaml
    /// </summary>
    public partial class ImageMenuItem : MenuItem
    {
        public ImageMenuItem()
        {
            InitializeComponent();
        }

        public string ImageResourceName { get { return GetValue(ImageResourceNameProperty)?.ToString(); } set { SetValue(ImageResourceNameProperty, value); } }
        public static readonly DependencyProperty ImageResourceNameProperty = DependencyProperty.Register(nameof(ImageResourceName), typeof(string), typeof(ImageMenuItem), new PropertyMetadata(null, (sender, args) => {
            
            if (args.NewValue == null)
                return;
            ImageMenuItem control = (ImageMenuItem)sender;
            control.ResourceImage = ResourceStringResolver.ResolveImageString(args.NewValue.ToString());
        }));

        internal byte[] ResourceImage { get { return (byte[])GetValue(ResourceImageProperty); } set { SetValue(ResourceImageProperty, value); } }
        internal static readonly DependencyProperty ResourceImageProperty = DependencyProperty.Register(nameof(ResourceImage), typeof(byte[]), typeof(ImageMenuItem));
    }
}
