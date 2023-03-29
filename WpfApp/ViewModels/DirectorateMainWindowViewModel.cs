using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Infrastructure.Commands;
using WpfApp.ViewModels.Base;
using WpfApp.Views;

namespace WpfApp.ViewModels
{
    internal class DirectorateMainWindowViewModel : ViewModel
    {

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Команды

        #region Команда для перехода на страницу со списком изделий

        public ICommand ProductListWindowCommand { get; }

        private bool CanProductKistWindowCommandExecute(object parameter) => true;
        private void OnProductKistWindowCommandExecuted(object parameter)
        {
            ProductList productList = new ProductList();
            productList.Show();
        }

        #endregion

        #endregion

        public DirectorateMainWindowViewModel()
        {

            ProductListWindowCommand = new LambdaCommand(OnProductKistWindowCommandExecuted, CanProductKistWindowCommandExecute);
        }

    }
}
