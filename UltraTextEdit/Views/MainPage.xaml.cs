using Microsoft.UI.Input;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UltraTextEdit.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.KeyDown += OnKeyComboPressed;
            editor.KeyDown += OnKeyComboPressed;
        }

        private void OnKeyComboPressed(object sender, KeyRoutedEventArgs e)
        {
            var state = InputKeyboardSource.GetKeyStateForCurrentThread(Windows.System.VirtualKey.Control);
            var isCtrlPressed = state.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
            if (e.Key == VirtualKey.B && isCtrlPressed)
            {
                BoldButton.IsChecked = !BoldButton.IsChecked;
            }
        }

        #region Stub methods
        private void OpenEditTab(object sender, RoutedEventArgs e)
        {

        }

        private void OpenHelpTab(object sender, RoutedEventArgs e)
        {

        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Bold = FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}
