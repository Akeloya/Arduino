using ArduinoEmulator.Resources;
using Xceed.Wpf.AvalonDock.Layout;

namespace ArduinoEmulator.AnchorablePanes
{
    /// <summary>
    /// Логика взаимодействия для ErrorPane.xaml
    /// </summary>
    public partial class ErrorPane : LayoutAnchorable
    {
        public ErrorPane()
        {
            InitializeComponent();
            Title = Resource.ErrorPaneName;
        }
    }
}
