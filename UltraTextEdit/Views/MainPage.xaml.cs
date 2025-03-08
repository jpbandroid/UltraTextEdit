using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UltraTextEdit.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            EditButton.IsChecked = true;
            HelpButton.IsChecked = false;
            Home.Visibility = Visibility.Visible;
            Help.Visibility = Visibility.Collapsed;
        }

        #region Stub methods
        private void OpenEditTab(object sender, RoutedEventArgs e)
        {
            EditButton.IsChecked = true;
            HelpButton.IsChecked = false;
            Home.Visibility = Visibility.Visible;
            Help.Visibility = Visibility.Collapsed;
        }

        private void OpenHelpTab(object sender, RoutedEventArgs e)
        {
            EditButton.IsChecked = false;
            HelpButton.IsChecked = true;
            Home.Visibility = Visibility.Collapsed;
            Help.Visibility = Visibility.Visible;
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void OnKeyboardAcceleratorInvoked(Microsoft.UI.Xaml.Input.KeyboardAccelerator sender, Microsoft.UI.Xaml.Input.KeyboardAcceleratorInvokedEventArgs args)
        {
            ITextSelection selectedText = editor.Document.Selection;
            switch (sender.Key)
            {
                case Windows.System.VirtualKey.B:
                    if (selectedText != null)
                    {
                        ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                        charFormatting.Bold = FormatEffect.Toggle;
                        selectedText.CharacterFormat = charFormatting;
                    }
                    BoldButton.IsChecked = editor.Document.Selection.CharacterFormat.Bold == FormatEffect.On;
                    args.Handled = true;
                    break;
                case Windows.System.VirtualKey.I:
                    if (selectedText != null)
                    {
                        ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                        charFormatting.Italic = FormatEffect.Toggle;
                        selectedText.CharacterFormat = charFormatting;
                    }
                    ItalicButton.IsChecked = editor.Document.Selection.CharacterFormat.Italic == FormatEffect.On;
                    args.Handled = true;
                    break;
                case Windows.System.VirtualKey.U:
                    args.Handled = true;
                    break;
                case Windows.System.VirtualKey.S:
                    args.Handled = true;
                    break;
            }
        }
    }
}