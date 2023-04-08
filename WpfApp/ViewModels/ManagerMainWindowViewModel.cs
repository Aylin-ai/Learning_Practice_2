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
    internal class ManagerMainWindowViewModel : ViewModel
    {

        #region Данные менеджера

        private string _managerLogin;
        public string ManagerLogin { get => _managerLogin; set => Set(ref _managerLogin, value); }

        #endregion

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

        #region Команда для перехода на страницу со списком заказов

        public ICommand OrdersWindowCommand { get; }

        private bool CanOrdersWindowCommandExecute(object parameter) => true;
        private void OnOrdersWindowCommandExecuted(object parameter)
        {
            Orders orders = new Orders(ManagerLogin);
            orders.Show();
        }

        #endregion

        #region Команда перехода на страницу раскроя

        public ICommand ProductCutWindowCommand { get; }

        private bool CanProductCutWindowCommandExecute(object parameter) => true;
        private void OnProductCutWindowCommandExecuted(object parameter)
        {
            ProductCut productCut = new ProductCut();
            productCut.Show();
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

        #endregion

        public ManagerMainWindowViewModel(string login)
        {
            ManagerLogin = login;

            #region Команды

            ProductListWindowCommand = new LambdaCommand(OnProductKistWindowCommandExecuted, CanProductKistWindowCommandExecute);
            OrdersWindowCommand = new LambdaCommand(OnOrdersWindowCommandExecuted, CanOrdersWindowCommandExecute);
            ProductCutWindowCommand = new LambdaCommand(OnProductCutWindowCommandExecuted, CanProductCutWindowCommandExecute);
            AuthorizationWindowCommand = new LambdaCommand(OnAuthorizationWindowCommandExecuted, CanAuthorizationWindowCommandExecute);

            #endregion

        }


    }
}
