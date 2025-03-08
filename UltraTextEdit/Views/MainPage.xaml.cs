using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using Microsoft.Graphics.Canvas.Text;
using System.Linq;

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

        public List<string> fonts
        {
            get
            {
                return CanvasTextFormat.GetSystemFontFamilies().OrderBy(f => f).ToList();
            }
        }

        public List<string> fontsizes { get; } = ["8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36", "48", "72"];

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
            ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Italic = FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

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
            //Configure underline
            var flyoutItem = (MenuFlyoutItem)sender;
            ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                UnderlineType characterFormat = selectedText.CharacterFormat.Underline;
                if (flyoutItem.Text == "None") characterFormat = UnderlineType.None;
                if (flyoutItem.Text == "Single") characterFormat = UnderlineType.Single;
                if (flyoutItem.Text == "Dash") characterFormat = UnderlineType.Dash;
                if (flyoutItem.Text == "Dotted") characterFormat = UnderlineType.Dotted;
                if (flyoutItem.Text == "Double") characterFormat = UnderlineType.Double;
                if (flyoutItem.Text == "Thick") characterFormat = UnderlineType.Thick;
                if (flyoutItem.Text == "Wave") characterFormat = UnderlineType.Wave;
                selectedText.CharacterFormat.Underline = characterFormat;
                editor.ContextFlyout.Hide();
            }
        }

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
                    if (selectedText != null)
                    {
                        UnderlineType characterFormat = selectedText.CharacterFormat.Underline;
                        characterFormat = UnderlineType.Single;
                        selectedText.CharacterFormat.Underline = characterFormat;
                    }
                    args.Handled = true;
                    break;
                case Windows.System.VirtualKey.S:
                    args.Handled = true;
                    break;
            }
        }

        private async void AboutApp(object sender, RoutedEventArgs e)
        {
            AboutUTE aboutUTE = new AboutUTE();
            ContentDialog aboutdialog = new ContentDialog();
            aboutdialog.DefaultButton = ContentDialogButton.Primary;
            aboutdialog.PrimaryButtonText = "OK";
            aboutdialog.Content = aboutUTE;
            aboutdialog.XamlRoot = this.XamlRoot;
            await aboutdialog.ShowAsync();
        }

        private void FontBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editor.Document.Selection != null)
            {
                editor.Document.Selection.CharacterFormat.Name = FontBox.SelectedValue.ToString();
            }
        }
        private void FontSizeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editor != null && editor.Document.Selection != null)
            {
                ITextSelection selectedText = editor.Document.Selection;
                selectedText.CharacterFormat.Size = float.Parse(FontSizeBox.SelectedValue.ToString());
            }
        }
    }
}