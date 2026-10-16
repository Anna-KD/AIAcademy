using System.Windows;
using AIAcademy.WPF.ViewModels;

namespace AIAcademy.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}