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
 using System.Reflection;
using System.Text;
using System.Windows;

namespace ArduinoEmulator.Forms
{
    /// <summary>
    /// Логика взаимодействия для AboutBox.xaml
    /// </summary>
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            InitializeComponent();
            License = Encoding.UTF8.GetString(ArduinoEmulator.Resources.Resource.LICENSE);
            Assembly assembly = Assembly.GetExecutingAssembly();
            var assName = assembly.GetName();
            AppName = assName.Name;
            Version = assName.Version.ToString();
            Copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
            Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
            Configuration = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>().Configuration;
        }

        public string License { get { return GetValue(LicenseProperty)?.ToString(); } set { SetValue(LicenseProperty, value); } }
        public static readonly DependencyProperty LicenseProperty = DependencyProperty.Register(nameof(License), typeof(string), typeof(AboutBox));

        public string AppName { get { return GetValue(AppNameProperty)?.ToString(); } set { SetValue(AppNameProperty, value); } }
        public static readonly DependencyProperty AppNameProperty = DependencyProperty.Register(nameof(AppName), typeof(string), typeof(AboutBox));

        public string Version { get { return GetValue(VersionProperty)?.ToString(); } set { SetValue(VersionProperty, value); } }
        public static readonly DependencyProperty VersionProperty = DependencyProperty.Register(nameof(Version), typeof(string), typeof(AboutBox));

        public string Copyright { get { return GetValue(CopyrightProperty)?.ToString(); } set { SetValue(CopyrightProperty, value); } }
        public static readonly DependencyProperty CopyrightProperty = DependencyProperty.Register(nameof(Copyright), typeof(string), typeof(AboutBox));
        public string Configuration { get { return GetValue(ConfigurationProperty)?.ToString(); } set { SetValue(ConfigurationProperty, value); } }
        public static readonly DependencyProperty ConfigurationProperty = DependencyProperty.Register(nameof(Configuration), typeof(string), typeof(AboutBox));
        public string Description { get { return GetValue(DescriptionProperty)?.ToString(); } set { SetValue(DescriptionProperty, value); } }
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(string), typeof(AboutBox));
    }
}
