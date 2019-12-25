using System;
using System.Collections.Generic;
using System.Text;
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
