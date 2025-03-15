using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using Microsoft.Graphics.Canvas.Text;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.Storage;
using System.IO;
using System.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UltraTextEdit.Views
{
    public sealed partial class MainPage : Page
    {
        private string originalDocText;

        public MainPage()
        {
            this.InitializeComponent();
            EditButton.IsChecked = true;
            HelpButton.IsChecked = false;
            Home.Visibility = Visibility.Visible;
            Help.Visibility = Visibility.Collapsed;
            FontSizeBox.SelectedItem = "11";
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

        private void NewDoc_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Activate();
        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);

            openPicker.FileTypeFilter.Add(".rtf");
            openPicker.FileTypeFilter.Add(".txt");

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                if (file.FileType == ".rtf")
                {
                    using (IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        IBuffer buffer = await FileIO.ReadBufferAsync(file);
                        var reader = DataReader.FromBuffer(buffer);
                        reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                        string text = reader.ReadString(buffer.Length);
                        // Load the file into the Document property of the RichEditBox.
                        editor.Document.LoadFromStream(TextSetOptions.FormatRtf, randAccStream);
                        editor.Document.GetText(TextGetOptions.UseObjectText, out originalDocText);
                    }
                }
                else if (file.FileType == ".txt")
                {
                    using (IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        using (Stream stream = randAccStream.AsStreamForRead())
                        {
                            // Use StreamReader with the appropriate encoding (e.g., UTF-8)
                            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                string text = await reader.ReadToEndAsync();

                                // Load the file into the Document property of the RichEditBox.
                                editor.Document.SetText(TextSetOptions.None, text);
                            }
                        }
                    }
                }
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });

            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we
                // finish making changes and call CompleteUpdatesAsync.
                //CachedFileManager.DeferUpdates(file);
                // write to file
                using (IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.ReadWrite))

                    if (file.Name.EndsWith(".txt"))
                    {
                        editor.Document.SaveToStream(Microsoft.UI.Text.TextGetOptions.None, randAccStream);
                    }
                    else
                    {
                        editor.Document.SaveToStream(Microsoft.UI.Text.TextGetOptions.FormatRtf, randAccStream);
                    }

                // Let Windows know that we're finished changing the file so the
                // other app can update the remote version of the file.
                //FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                //if (status != FileUpdateStatus.Complete)
                //{
                //    Windows.UI.Popups.MessageDialog errorBox = new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
                //    await errorBox.ShowAsync();
                //}
            }
        }
    }
}