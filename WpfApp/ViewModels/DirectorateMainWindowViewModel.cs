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

        #region Actions
        
        public Action CloseAction { get; set; }
        
        #endregion

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

        #region Команда для перехода на страницу авторизации

        public ICommand AuthorizationWindowCommand { get; }

        private bool CanAuthorizationWindowCommandExecute(object parameter) => true;
        private void OnAuthorizationWindowCommandExecuted(object parameter)
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            CloseAction();
        }

        #endregion

        #region Команда перехода на страницу с остатками материалов/изделий

        public ICommand MaterialProductRemainderWindowCommand { get; }

        private bool CanMaterialProductRemainderWindowCommandExecute(object parameter) => true;
        private void OnMaterialProductRemainderWindowCommandExecuted(object paramaeter)
        {
            MaterialProductRemainder materialProductRemainder = new MaterialProductRemainder();
            materialProductRemainder.Show();
        }

        #endregion

        #region Команда перехода на страницу с движением материалов/изделий за период

        public ICommand MaterialProductChangingWindowCommand { get; }

        private bool CanMaterialProductChangingWindowCommandExecute(object paramaeter) => true;
        private void OnMaterialProductChangingWindowCommandExecuted(object parameter)
        {
            MaterialProductChanging window = new MaterialProductChanging();
            window.Show();
        }

        #endregion

        #endregion

        public DirectorateMainWindowViewModel()
        {

            #region Команды

            ProductListWindowCommand = new LambdaCommand(OnProductKistWindowCommandExecuted, CanProductKistWindowCommandExecute);
            MaterialProductRemainderWindowCommand = new LambdaCommand(OnMaterialProductRemainderWindowCommandExecuted, CanMaterialProductRemainderWindowCommandExecute);
            MaterialProductChangingWindowCommand = new LambdaCommand(OnMaterialProductChangingWindowCommandExecuted, CanMaterialProductChangingWindowCommandExecute);
            AuthorizationWindowCommand = new LambdaCommand(OnAuthorizationWindowCommandExecuted, CanAuthorizationWindowCommandExecute);

            #endregion

        }

    }
}
