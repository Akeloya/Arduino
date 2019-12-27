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
using ArduinoEmulator.Resources;
using System.Collections.Generic;
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
            Assembly assembly = Assembly.GetExecutingAssembly();
            var assName = assembly.GetName();
            AppName = assName.Name;
            Version = assName.Version.ToString();
            Copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
            Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
            Configuration = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>().Configuration;
        }

        internal string License { get { return GetValue(LicenseProperty)?.ToString(); } set { SetValue(LicenseProperty, value); } }
        internal static readonly DependencyProperty LicenseProperty = DependencyProperty.Register(nameof(License), typeof(string), typeof(AboutBox));

        internal string AppName { get { return GetValue(AppNameProperty)?.ToString(); } set { SetValue(AppNameProperty, value); } }
        internal static readonly DependencyProperty AppNameProperty = DependencyProperty.Register(nameof(AppName), typeof(string), typeof(AboutBox));

        internal string Version { get { return GetValue(VersionProperty)?.ToString(); } set { SetValue(VersionProperty, value); } }
        internal static readonly DependencyProperty VersionProperty = DependencyProperty.Register(nameof(Version), typeof(string), typeof(AboutBox));

        internal string Copyright { get { return GetValue(CopyrightProperty)?.ToString(); } set { SetValue(CopyrightProperty, value); } }
        internal static readonly DependencyProperty CopyrightProperty = DependencyProperty.Register(nameof(Copyright), typeof(string), typeof(AboutBox));
        internal string Configuration { get { return GetValue(ConfigurationProperty)?.ToString(); } set { SetValue(ConfigurationProperty, value); } }
        internal static readonly DependencyProperty ConfigurationProperty = DependencyProperty.Register(nameof(Configuration), typeof(string), typeof(AboutBox));
        internal string Description { get { return GetValue(DescriptionProperty)?.ToString(); } set { SetValue(DescriptionProperty, value); } }
        internal static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(string), typeof(AboutBox));

        internal List<ComponentItem> ComponentLicenses { get { return (List<ComponentItem>)GetValue(ComponentLicensesProperty); } set { SetValue(ComponentLicensesProperty, value); } }
        internal static readonly DependencyProperty ComponentLicensesProperty = DependencyProperty.Register(nameof(ComponentLicenses), typeof(List<ComponentItem>), typeof(AboutBox), new PropertyMetadata(new List<ComponentItem> {
        new ComponentItem(Resource.MainWindowTitle,Encoding.UTF8.GetString(Resource.LICENSE)),
            new ComponentItem(Resource.XceedWpfToolkit,Resource.XCEEDLICENSE),
            new ComponentItem(Resource.AvalonEdit, Encoding.UTF8.GetString(Resource.AVALONEDITLICENSE))
        }));

        private void ComponentLicenses_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
                License = ((ComponentItem)e.AddedItems[0]).License;
        }
    }

    internal class ComponentItem
    {
        public string Component { get; }
        public string License { get; }
        public ComponentItem(string component, string license)
        {
            Component = component;
            License = license;
        }
    }
}
