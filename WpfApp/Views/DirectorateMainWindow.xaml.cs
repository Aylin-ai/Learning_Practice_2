using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp.ViewModels;

namespace WpfApp.Views
{
    /// <summary>
    /// Логика взаимодействия для DirectorateMainWindow.xaml
    /// </summary>
    public partial class DirectorateMainWindow : Window
    {
        public DirectorateMainWindow()
        {
            DirectorateMainWindowViewModel viewModel = new DirectorateMainWindowViewModel();
            DataContext = viewModel;
            InitializeComponent();
            if (viewModel.CloseAction == null)
                viewModel.CloseAction = new Action(this.Close);
        }
    }
}
