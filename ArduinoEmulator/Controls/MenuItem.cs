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
using ArduinoEmulator.Converters;
using ArduinoEmulator.Core;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ArduinoEmulator.Controls
{
    public class MenuItem : System.Windows.Controls.MenuItem
    {
        public string CommandName { get { return GetValue(CommandNameProperty)?.ToString(); }set { SetValue(CommandNameProperty, value); } }
        public static readonly DependencyProperty CommandNameProperty = DependencyProperty.Register(nameof(CommandName), typeof(string), typeof(MenuItem), new PropertyMetadata(null, (sender, args) => {

            MenuItem menuItem = (MenuItem)sender;
            if(args.NewValue == null)
            {
                menuItem.Command = null;
            }
            else
            {
                menuItem.Command = MenuCommands.CommandFromString(args.NewValue.ToString());
            }
        }));

        public string ResourceHeader { get { return GetValue(ResourceHeaderProperty)?.ToString(); } set { SetValue(ResourceHeaderProperty, value); } }
        public static readonly DependencyProperty ResourceHeaderProperty = DependencyProperty.Register(nameof(ResourceHeader), typeof(string), typeof(MenuItem), new PropertyMetadata(null, (sender, args)=> {
            MenuItem menuItem = (MenuItem)sender;
            if(args.NewValue == null)
            {
                menuItem.Header = null;
            }
            else
            {
                menuItem.Header = ResourceStringResolver.ResolveStringValue(args.NewValue.ToString());
            }
        }));

        public string ImageName { get { return GetValue(ImageResourceNameProperty)?.ToString(); } set { SetValue(ImageResourceNameProperty, value); } }
        public static readonly DependencyProperty ImageResourceNameProperty = DependencyProperty.Register(nameof(ImageName), typeof(string), typeof(MenuItem), new PropertyMetadata(null, (sender, args) => {

            if (args.NewValue == null)
                return;
            byte[] imageData = ResourceStringResolver.ResolveImageString(args.NewValue.ToString());
            MenuItem control = (MenuItem)sender;
            using MemoryStream stream = new MemoryStream(imageData);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            control.Icon = new Image() { Source = image };
        }));

        internal byte[] ResourceImage { get { return (byte[])GetValue(ResourceImageProperty); } set { SetValue(ResourceImageProperty, value); } }
        internal static readonly DependencyProperty ResourceImageProperty = DependencyProperty.Register(nameof(ResourceImage), typeof(byte[]), typeof(MenuItem));
    }
}
