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
    internal class CustomerMainWindowViewModel : ViewModel
    {

        #region Данные пользователя
        
        private string _userLogin;
        public string UserLogin { get => _userLogin; set => Set(ref _userLogin, value); }
        
        #endregion

        #region Actions

        public Action CloseAction { get; set; }
        
        #endregion

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Команды

        #region Команда для перехода на страницу создания заказа

        public ICommand MakeOrderWindowCommand { get; }

        private bool CanMakeOrderWindowCommandExecute(object parameter) => true;
        private void OnMakeOrderWindowCommandExecuted(object parameter)
        {
            MakeOrder makeOrder = new MakeOrder(parameter as string);
            makeOrder.Show();
        }

        #endregion

        #region Команда перехода на страницу списка заказов

        public ICommand CustomerOrdersWindowCommand { get; }

        private bool CanCustomerOrdersWindowCommandExecute(object parameter) => true;
        private void OnCustomerOrdersWindowCommandExecuted(object parameter)
        {
            CustomerOrders customerOrders = new CustomerOrders(parameter as string);
            customerOrders.Show();
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

        public CustomerMainWindowViewModel(string userLogin)
        {
            UserLogin = userLogin;

            #region Команды

            MakeOrderWindowCommand = new LambdaCommand(OnMakeOrderWindowCommandExecuted, CanMakeOrderWindowCommandExecute);
            CustomerOrdersWindowCommand = new LambdaCommand(OnCustomerOrdersWindowCommandExecuted, CanCustomerOrdersWindowCommandExecute);
            AuthorizationWindowCommand = new LambdaCommand(OnAuthorizationWindowCommandExecuted, CanAuthorizationWindowCommandExecute);

            #endregion
        }

    }
}
