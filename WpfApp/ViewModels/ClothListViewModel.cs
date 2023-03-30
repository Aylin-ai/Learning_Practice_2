using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Infrastructure.Commands;
using WpfApp.Models;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels
{
    internal class ClothListViewModel : ViewModel
    {

        private ObservableCollection<Product> _cloths = new ObservableCollection<Product>();
        public ObservableCollection<Product> FrontCloths
        {
            get => _cloths;
            set => Set(ref _cloths, value);
        }

        #region Данные изделий

        private string _myPreviousSelectedItem = "см";

        #endregion

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Команды

        #region Команды для изменения единицы измерения DataGrid

        public ICommand ToMCommand { get; }
        public ICommand ToDMCommand { get; }
        public ICommand ToMMCommand { get; }
        public ICommand ToCMCommand { get; }

        private bool CanToMCommandExecute(object parameter) => true;
        private bool CanToDMCommandExecute(object parameter) => true;
        private bool CanToMMCommandExecute(object parameter) => true;
        private bool CanToCMCommandExecute(object parameter) => true;

        private void OnToMCommandExecuted(object parameter)
        {
            switch (_myPreviousSelectedItem)
            {
                case "см":
                    GetProducts("/100");
                    break;
                case "м":
                    break;
                case "мм":
                    GetProducts("/1000");
                    break;
                case "дм":
                    GetProducts("/10");
                    break;
            }
            _myPreviousSelectedItem = "м";
        }
        private void OnToDMCommandExecuted(object parameter)
        {
            switch (_myPreviousSelectedItem)
            {
                case "см":
                    GetProducts("/10");
                    break;
                case "м":
                    GetProducts("*10");
                    break;
                case "мм":
                    GetProducts("/100");
                    break;
                case "дм":
                    break;
            }
            _myPreviousSelectedItem = "дм";
        }
        private void OnToMMCommandExecuted(object parameter)
        {
            switch (_myPreviousSelectedItem)
            {
                case "см":
                    GetProducts("*10");
                    break;
                case "м":
                    GetProducts("*1000");
                    break;
                case "мм":
                    break;
                case "дм":
                    GetProducts("*100");
                    break;
            }
            _myPreviousSelectedItem = "мм";
        }
        private void OnToCMCommandExecuted(object parameter)
        {
            switch (_myPreviousSelectedItem)
            {
                case "см":
                    break;
                case "м":
                    GetProducts("*100");
                    break;
                case "мм":
                    GetProducts("/10");
                    break;
                case "дм":
                    GetProducts("*10");
                    break;
            }
            _myPreviousSelectedItem = "см";
        }

        #endregion

        #endregion

        public ClothListViewModel()
        {
            GetProducts();

            #region Команды

            ToMCommand = new LambdaCommand(OnToMCommandExecuted, CanToMCommandExecute);
            ToCMCommand = new LambdaCommand(OnToCMCommandExecuted, CanToCMCommandExecute);
            ToDMCommand = new LambdaCommand(OnToDMCommandExecuted, CanToDMCommandExecute);
            ToMMCommand = new LambdaCommand(OnToMMCommandExecuted, CanToMMCommandExecute);

            #endregion
        }


        private void GetProducts(string unit = "")
        {
            switch (unit)
            {
                case "*10":
                    for (int i = 0; i < FrontCloths.Count; i++)
                    {
                        FrontCloths[i].Length *= 10;
                        FrontCloths[i].Width *= 10;
                    }
                    break;
                case "*100":
                    for (int i = 0; i < FrontCloths.Count; i++)
                    {
                        FrontCloths[i].Length *= 100;
                        FrontCloths[i].Width *= 100;
                    }
                    break;
                case "/10":
                    for (int i = 0; i < FrontCloths.Count; i++)
                    {
                        FrontCloths[i].Length /= 10;
                        FrontCloths[i].Width /= 10;
                    }
                    break;
                case "/100":
                    for (int i = 0; i < FrontCloths.Count; i++)
                    {
                        FrontCloths[i].Length /= 100;
                        FrontCloths[i].Width /= 100;
                    }
                    break;
                case "*1000":
                    for (int i = 0; i < FrontCloths.Count; i++)
                    {
                        FrontCloths[i].Length *= 1000;
                        FrontCloths[i].Width *= 1000;
                    }
                    break;
                case "/1000":
                    for (int i = 0; i < FrontCloths.Count; i++)
                    {
                        FrontCloths[i].Length /= 1000;
                        FrontCloths[i].Width /= 1000;
                    }
                    break;
                default:
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    try
                    {
                        string sql = "SELECT * FROM product";

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.CommandText = sql;
                        cmd.Connection = conn;

                        var reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                FrontCloths.Add(new Product()
                                {
                                    Articul = reader.GetString(0),
                                    Name = reader.GetString(1),
                                    Length = reader.GetFloat(2),
                                    Width = reader.GetFloat(3),
                                    Cost = reader.GetFloat(4),
                                    Image = reader.GetString(5),
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                    break;
            }

        }

    }
}
