using System;
using Microsoft.UI.Xaml;
using Windows.Storage.Pickers;
using Windows.Storage;
using FileFragment.Core;
using WinRT.Interop;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileFragment.GUI
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class FragmentPage : Window
	{
		public FragmentPage()
		{
			InitializeComponent();
			Content.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(LoseFocusEnterPressed), true);
		}

		private async void LoseFocusEnterPressed(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key != Windows.System.VirtualKey.Enter) return;
			await FocusManager.TryFocusAsync(fragmentButton, FocusState.Programmatic);
		}

		private async void Fragment_Click(object sender, RoutedEventArgs e)
		{
			nint hwnd = WindowNative.GetWindowHandle(this);

			FileOpenPicker inputPicker = new()
			{
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary
			};

			inputPicker.FileTypeFilter.Add("*");

			InitializeWithWindow.Initialize(inputPicker, hwnd);

			StorageFile inputFile = await inputPicker.PickSingleFileAsync();
			if (inputFile is null)
			{
				return;
			}

			FolderPicker outputPicker = new()
			{
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary
			};

			InitializeWithWindow.Initialize(outputPicker, hwnd);

			StorageFolder outputFolder = await outputPicker.PickSingleFolderAsync();
			if (outputFolder is null)
			{
				return;
			}

			DiskOp.Fragment(50, outputFolder.Path, inputFile.Path);
		}

		private async void Defragment_Click(object sender, RoutedEventArgs e)
		{
			nint hwnd = WindowNative.GetWindowHandle(this);

			FolderPicker inputPicker = new()
			{
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary
			};

			InitializeWithWindow.Initialize(inputPicker, hwnd);

			StorageFolder inputFolder = await inputPicker.PickSingleFolderAsync();
			if (inputFolder is null)
			{
				return;
			}

			FileSavePicker outputPicker = new()
			{
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary
			};

			outputPicker.FileTypeChoices.Add("All files", ["."]);

			InitializeWithWindow.Initialize(outputPicker, hwnd);

			StorageFile outputFile = await outputPicker.PickSaveFileAsync();
			if (outputFile is null)
			{
				return;
			}

			DiskOp.Defragment(inputFolder.Path, outputFile.Path);
		}

		private void PacketSizeInput_LostFocus(object sender, RoutedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			Config.PacketSize = Convert.ToUInt32(textBox.Text);
        }
    }
}
