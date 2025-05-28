using System;
using FileFragment.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileFragment.GUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DefragmentPage : Page
    {
        private string selectedOutputFile = "";
        private string selectedInputFolder = "";

        private nint? hwnd;

        public DefragmentPage()
        {
            InitializeComponent();
        }

        private void SetStatusText(string text) => StatusTextBlock.Text = text;

        private void OnDefragmentClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedInputFolder)) { SetStatusText("Error: No input directory selected"); return; }
            if (string.IsNullOrEmpty(selectedOutputFile)) { SetStatusText("Error: No output file selected"); return; }

            try
            {
                DiskOp.Defragment(selectedInputFolder, selectedOutputFile);

                StatusTextBlock.Text = "Defragmentation complete!";
            }
            catch (Exception ex)
            {
                SetStatusText($"Error: {ex.Message}");
            }
        }

        private async void OnSelectInputFolderClicked(object sender, RoutedEventArgs e)
        {
            FolderPicker picker = new()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            hwnd ??= WindowNative.GetWindowHandle(App.Window);
            InitializeWithWindow.Initialize(picker, hwnd!.Value);

            StorageFolder folder = await picker.PickSingleFolderAsync();

            if (folder is null) return;

            selectedInputFolder = folder.Path;
            SelectInputFolderTextBlock.Text = $"Selected: {folder.Path}";
        }

        private async void OnSelectOutputFileClicked(object sender, RoutedEventArgs e)
        {
            FileSavePicker picker = new()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            picker.FileTypeChoices.Add("All files", ["."]);

            hwnd ??= WindowNative.GetWindowHandle(App.Window);
            InitializeWithWindow.Initialize(picker, hwnd!.Value);

            StorageFile file = await picker.PickSaveFileAsync();

            if (file is null) return;

            selectedOutputFile = file.Path;
            SelectOutputFileTextBlock.Text = $"Selected: {file.Path}";
        }
    }
}
