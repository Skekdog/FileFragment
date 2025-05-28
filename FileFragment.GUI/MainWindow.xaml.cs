using FileFragment.GUI.Pages;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileFragment.GUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Navigate(typeof(FragmentPage));
        }

        private void OnFragmentTabClicked(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(FragmentPage));
        }

        private void OnDefragmentTabClicked(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(DefragmentPage));
        }
    }
}
