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
    /// Логика взаимодействия для ClothList.xaml
    /// </summary>
    public partial class ClothList : Window
    {
        public ClothList()
        {
            var vm = new ClothListViewModel();
            DataContext = vm;
            InitializeComponent();
        }
    }
}
