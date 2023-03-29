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
    internal class StorekeeperMainWindowViewModel : ViewModel
    {

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Команды

        #region Команда перехода на страницу со списоком тканей

        public ICommand ClothListWindowCommand { get; }

        private bool CanClothListWindowCommandExecute(object parameter) => true;
        private void OnClothListWindowCommandExecuted(object parameter)
        {
            ClothList clothList = new ClothList();
            clothList.Show();
        }

        #endregion

        #region Команда для перехода на страницу со списком изделий

        public ICommand ProductListWindowCommand { get; }

        private bool CanProductKistWindowCommandExecute(object parameter) => true;
        private void OnProductKistWindowCommandExecuted(object parameter)
        {
            ProductList productList = new ProductList();
            productList.Show();
        }

        #endregion

        #region Команда для перехода на страницу со списком фурнитуры

        public ICommand FurnitureListWindowCommand { get; }

        private bool CanFurnitureListWindowCommandExecute(object parameter) => true;
        private void OnFurnitureListWindowCommandExecuted(object parameter)
        {
            FurnitureList furnitureList = new FurnitureList();
            furnitureList.Show();
        }

        #endregion

        #endregion

        public StorekeeperMainWindowViewModel()
        {
            #region Команды

            ClothListWindowCommand = new LambdaCommand(OnClothListWindowCommandExecuted, CanClothListWindowCommandExecute);
            ProductListWindowCommand = new LambdaCommand(OnProductKistWindowCommandExecuted, CanProductKistWindowCommandExecute);
            FurnitureListWindowCommand = new LambdaCommand(OnFurnitureListWindowCommandExecuted, CanFurnitureListWindowCommandExecute);

            #endregion
        }

    }
}
