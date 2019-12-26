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
