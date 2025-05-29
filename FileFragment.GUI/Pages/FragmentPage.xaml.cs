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
    public sealed partial class FragmentPage : Page
    {
        private string selectedInputFile = "";
        private string selectedOutputDirectory = "";

        private nint? hwnd;

        public FragmentPage()
        {
            InitializeComponent();
        }

        private void SetStatusText(string text) => StatusTextBlock.Text = text;

        private void OnFragmentClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedInputFile)) { SetStatusText("Error: No input file selected"); return; }
            if (string.IsNullOrEmpty(selectedOutputDirectory)) { SetStatusText("Error: No output directory selected"); return; }

            try
            {
                _ = uint.TryParse(PacketSizeTextBox.Text, out uint packetSize);
                if (packetSize == 0) throw new Exception("Invalid packet size");

                DiskOp.Fragment(packetSize, selectedInputFile, selectedOutputDirectory);

                StatusTextBlock.Text = "Fragmentation complete!";
            }
            catch (Exception ex)
            {
                SetStatusText($"Error: {ex.Message}");
            }
        }

        private async void OnSelectInputFileClicked(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            picker.FileTypeFilter.Add("*");

            hwnd ??= WindowNative.GetWindowHandle(App.Window);
            InitializeWithWindow.Initialize(picker, hwnd!.Value);

            StorageFile file = await picker.PickSingleFileAsync();

            if (file is null) return;

            selectedInputFile = file.Path;
            SelectFileTextBlock.Text = $"Selected: {file.Path}";
        }

        private async void OnSelectOutputDirectoryClicked(object sender, RoutedEventArgs e)
        {
            FolderPicker picker = new()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            hwnd ??= WindowNative.GetWindowHandle(App.Window);
            InitializeWithWindow.Initialize(picker, hwnd!.Value);

            StorageFolder folder = await picker.PickSingleFolderAsync();

            if (folder is null) return;

            selectedOutputDirectory = folder.Path;
            SelectOutputDirectoryTextBlock.Text = $"Selected: {folder.Path}";
        }
    }
}