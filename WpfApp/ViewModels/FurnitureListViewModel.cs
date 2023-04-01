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
    internal class FurnitureListViewModel : ViewModel
    {
        private ObservableCollection<Furniture> _furnitures = new ObservableCollection<Furniture>();
        public ObservableCollection<Furniture> FrontFurnitures
        {
            get => _furnitures;
            set => Set(ref _furnitures, value);
        }

        #region Данные изделий

        private string _myPreviousSelectedItem = "см";
        private string _myPreviousSelectedWeight = "гр";

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
        
        public ICommand ToGRCommand { get; }
        public ICommand ToKGCommand { get; }
        public ICommand ToTCommand { get; }

        private bool CanToMCommandExecute(object parameter) => true;
        private bool CanToDMCommandExecute(object parameter) => true;
        private bool CanToMMCommandExecute(object parameter) => true;
        private bool CanToCMCommandExecute(object parameter) => true;

        private bool CanToGRCommandExecute(object parameter) => true;
        private bool CanToKGCommandExecute(object parameter) => true;
        private bool CanToTCommandExecute(object parameter) => true;


        private void OnToMCommandExecuted(object parameter)
        {
            switch (_myPreviousSelectedItem)
            {
                case "см":
                    GetFurniture("/100");
                    break;
                case "м":
                    break;
                case "мм":
                    GetFurniture("/1000");
                    break;
                case "дм":
                    GetFurniture("/10");
                    break;
            }
            _myPreviousSelectedItem = "м";
        }
        private void OnToDMCommandExecuted(object parameter)
        {
            switch (_myPreviousSelectedItem)
            {
                case "см":
                    GetFurniture("/10");
                    break;
                case "м":
                    GetFurniture("*10");
                    break;
                case "мм":
                    GetFurniture("/100");
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
                    GetFurniture("*10");
                    break;
                case "м":
                    GetFurniture("*1000");
                    break;
                case "мм":
                    break;
                case "дм":
                    GetFurniture("*100");
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
                    GetFurniture("*100");
                    break;
                case "мм":
                    GetFurniture("/10");
                    break;
                case "дм":
                    GetFurniture("*10");
                    break;
            }
            _myPreviousSelectedItem = "см";
        }

        private void OnToGRCommandExecuted(object parametr)
        {
            switch (_myPreviousSelectedWeight)
            {
                case "гр":
                    break;
                case "кг":
                    ChangeWeight("*1000");
                    break;
                case "т":
                    ChangeWeight("*1000000");
                    break;
            }
            _myPreviousSelectedWeight = "гр";
        }
        private void OnToKGCommandExecuted(object parametr)
        {
            switch (_myPreviousSelectedWeight)
            {
                case "гр":
                    ChangeWeight("/1000");
                    break;
                case "кг":
                    break;
                case "т":
                    ChangeWeight("*1000");
                    break;
            }
            _myPreviousSelectedWeight = "кг";
        }
        private void OnToTCommandExecuted(object parametr)
        {
            switch (_myPreviousSelectedWeight)
            {
                case "гр":
                    ChangeWeight("/1000000");
                    break;
                case "кг":
                    ChangeWeight("/1000");
                    break;
                case "т":
                    break;
            }
            _myPreviousSelectedWeight = "т";
        }

        #endregion

        #endregion

        public FurnitureListViewModel()
        {
            GetFurniture();

            #region Команды

            ToMCommand = new LambdaCommand(OnToMCommandExecuted, CanToMCommandExecute);
            ToCMCommand = new LambdaCommand(OnToCMCommandExecuted, CanToCMCommandExecute);
            ToDMCommand = new LambdaCommand(OnToDMCommandExecuted, CanToDMCommandExecute);
            ToMMCommand = new LambdaCommand(OnToMMCommandExecuted, CanToMMCommandExecute);

            ToGRCommand = new LambdaCommand(OnToGRCommandExecuted, CanToGRCommandExecute);
            ToKGCommand = new LambdaCommand(OnToKGCommandExecuted, CanToKGCommandExecute);
            ToTCommand = new LambdaCommand(OnToTCommandExecuted, CanToTCommandExecute);

            #endregion
        }


        private void GetFurniture(string unit = "")
        {
            switch (unit)
            {
                case "*10":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Length *= 10;
                        FrontFurnitures[i].Width *= 10;
                    }
                    break;
                case "*100":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Length *= 100;
                        FrontFurnitures[i].Width *= 100;
                    }
                    break;
                case "/10":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Length /= 10;
                        FrontFurnitures[i].Width /= 10;
                    }
                    break;
                case "/100":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Length /= 100;
                        FrontFurnitures[i].Width /= 100;
                    }
                    break;
                case "*1000":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Length *= 1000;
                        FrontFurnitures[i].Width *= 1000;
                    }
                    break;
                case "/1000":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Length /= 1000;
                        FrontFurnitures[i].Width /= 1000;
                    }
                    break;
                default:
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    try
                    {
                        string sql = "select f.Furniture_Articul, f.Furniture_Name, f.`Furniture_Width(mm)`, f.`Furniture_Length(mm)`, " +
                            "f.`Furniture_Weight(gr)`, f.Furniture_Image, f.Furniture_Cost, gt.GeneralType_Name " +
                            "from ( (furniture f inner join furnituretype ft on f.Furniture_Articul = ft.FurnitureType_Furniture_Articul) " +
                            "inner join generaltype gt on ft.FurnitureType_GeneralType_Id = gt.GeneralType_Id);";

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.CommandText = sql;
                        cmd.Connection = conn;

                        var reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                FrontFurnitures.Add(new Furniture()
                                {
                                    Articul = reader.GetString(0),
                                    Name = reader.GetString(1),
                                    Width = reader.GetFloat(2) / 10,
                                    Length = reader.GetFloat(3) / 10,
                                    Weight = reader.GetFloat(4),
                                    Cost = reader.GetFloat(6),
                                    Image = reader.GetString(5),
                                    Type = reader.GetString(7)
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

        private void ChangeWeight(string weight)
        {

            switch (weight)
            {
                case "*10":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Weight *= 10;
                    }
                    break;
                case "*100":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Weight *= 100;
                    }
                    break;
                case "/10":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Weight /= 10;
                    }
                    break;
                case "/100":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Weight /= 100;
                    }
                    break;
                case "*1000":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Weight *= 1000;
                    }
                    break;
                case "/1000":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Weight /= 1000;
                    }
                    break;
                case "*1000000":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Weight *= 1000000;
                    }
                    break;
                case "/1000000":
                    for (int i = 0; i < FrontFurnitures.Count; i++)
                    {
                        FrontFurnitures[i].Weight /= 1000000;
                    }
                    break;
            }

        }

    }
}
