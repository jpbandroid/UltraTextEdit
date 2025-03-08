using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using UltraTextEdit.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UltraTextEdit
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            PageFrame.Navigate(typeof(MainPage));
            this.SystemBackdrop = new MicaBackdrop();
        }
    }
}
