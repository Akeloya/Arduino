using ArduinoEmulator.Resources;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;

namespace ArduinoEmulator.AnchorablePanes
{
    /// <summary>
    /// Логика взаимодействия для ViewLayoutPane.xaml
    /// </summary>
    public partial class ViewLayoutPane : LayoutAnchorable
    {
        public ViewLayoutPane()
        {
            InitializeComponent();
            Title = Resource.MenuItemViewDisplay;
        }

        public string LayoutText { get { return GetValue(LayoutTextProperty)?.ToString(); } set { SetValue(LayoutTextProperty, value); } }
        public static readonly DependencyProperty LayoutTextProperty = DependencyProperty.Register(nameof(LayoutText),
            typeof(string),
            typeof(LayoutAnchorable),
            new PropertyMetadata(null, (sender, args)=> {
                ViewLayoutPane pane = (ViewLayoutPane)sender;
                pane.TbDisplayData.Text = args.NewValue?.ToString();
            }));
    }
}
