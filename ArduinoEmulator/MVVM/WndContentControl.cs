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
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace ArduinoEmulator.MVVM
{
    public class WndContentControl
    {
        public string Name { get; set; }
        public static T DocumentFactory<T>(object[] data = null) where T : notnull
        {
            Type t = typeof(T);
            if(t == typeof(LayoutDocument))
            {
                string text = null;
                string fileName = Resources.Resource.NewSketchName;
                if (data != null && data.Length > 1)
                {
                    fileName = data[0]?.ToString();
                    text = data[1]?.ToString();
                }
                object document = new LayoutDocument()
                {
                    Title = fileName,
                    Content= new TextBox {Text = text }
                };
                return (T)document;
            }

            throw new NotImplementedException();
        }
    }
}
