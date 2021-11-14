using Blotter.ViewModels;
using PriceSupplier;
using System.Windows;

namespace Blotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindowViewModel MainWindowViewModel { get; set; }

        public MainWindow()
        {
            ConsoleWrapper.ShowConsoleWindow();
            InitializeComponent();

            this.MainWindowViewModel = new MainWindowViewModel();
            this.DataContext = this.MainWindowViewModel;            
        }
    }
}
