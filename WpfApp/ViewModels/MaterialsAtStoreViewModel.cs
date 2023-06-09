﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using WpfApp.Infrastructure.Commands;
using WpfApp.Models;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels
{
    internal class MaterialsAtStoreViewModel : ViewModel
    {

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Коллекции элементов

        private ObservableCollection<FurnitureStore> _furnitures = new ObservableCollection<FurnitureStore>();
        public ObservableCollection<FurnitureStore> FurnituresAtStore { get => _furnitures; set => Set(ref _furnitures, value); }

        private ObservableCollection<ClothStore> _cloths = new ObservableCollection<ClothStore>();
        public ObservableCollection<ClothStore> ClothsAtStore { get => _cloths; set => Set(ref _cloths, value); }

        #endregion

        #region Единица измерения размеров ткани

        private string _myPreviousSelectedItem = "см";

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
                    GetInformationAboutClothAtStore("/100");
                    break;
                case "м":
                    break;
                case "мм":
                    GetInformationAboutClothAtStore("/1000");
                    break;
                case "дм":
                    GetInformationAboutClothAtStore("/10");
                    break;
            }
            _myPreviousSelectedItem = "м2";
        }
        private void OnToDMCommandExecuted(object parameter)
        {
            switch (_myPreviousSelectedItem)
            {
                case "см":
                    GetInformationAboutClothAtStore("/10");
                    break;
                case "м":
                    GetInformationAboutClothAtStore("*10");
                    break;
                case "мм":
                    GetInformationAboutClothAtStore("/100");
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
                    GetInformationAboutClothAtStore("*10");
                    break;
                case "м":
                    GetInformationAboutClothAtStore("*1000");
                    break;
                case "мм":
                    break;
                case "дм":
                    GetInformationAboutClothAtStore("*100");
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
                    GetInformationAboutClothAtStore("*100");
                    break;
                case "мм":
                    GetInformationAboutClothAtStore("/10");
                    break;
                case "дм":
                    GetInformationAboutClothAtStore("*10");
                    break;
            }
            _myPreviousSelectedItem = "см";
        }

        #endregion
        

        #endregion

        public MaterialsAtStoreViewModel()
        {
            GetInformationAboutFurnitureAtStore();
            GetInformationAboutClothAtStore();

            #region Команды

            ToMCommand = new LambdaCommand(OnToMCommandExecuted, CanToMCommandExecute);
            ToCMCommand = new LambdaCommand(OnToCMCommandExecuted, CanToCMCommandExecute);
            ToDMCommand = new LambdaCommand(OnToDMCommandExecuted, CanToDMCommandExecute);
            ToMMCommand = new LambdaCommand(OnToMMCommandExecuted, CanToMMCommandExecute);

            #endregion
        }

        private void GetInformationAboutFurnitureAtStore()
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                string sql = "SELECT f.Furniture_Articul, f.Furniture_Name, f.Furniture_Cost, fs.FurnitureStore_Party, fs.FurnitureStore_Quantity " +
                    "from furniture f " +
                    "inner join furniturestore fs " +
                    "on f.Furniture_Articul = fs.FurnitureStore_Furniture_Articul;";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FurnituresAtStore.Add(new FurnitureStore()
                        {
                            Articul = reader.GetString(0),
                            Name = reader.GetString(1),
                            Cost = reader.GetFloat(2),
                            Party = reader.GetInt32(3),
                            Quantity = reader.GetInt32(4),
                            CostOfAllFurniture = reader.GetFloat(2) * reader.GetInt32(4)
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
        }

        private void GetInformationAboutClothAtStore(string unit = "")
        {
            switch (unit)
            {
                case "*10":
                    for (int i = 0; i < ClothsAtStore.Count; i++)
                    {
                        ClothsAtStore[i].LengthOfCloth *= 10;
                        ClothsAtStore[i].WidthOfCloth *= 10;
                        ClothsAtStore[i].WidthOfClothAtStore *= 10;
                        ClothsAtStore[i].LengthOfClothAtStore *= 10;
                    }
                    break;
                case "*100":
                    for (int i = 0; i < ClothsAtStore.Count; i++)
                    {
                        ClothsAtStore[i].LengthOfCloth *= 100;
                        ClothsAtStore[i].WidthOfCloth *= 100;
                        ClothsAtStore[i].WidthOfClothAtStore *= 100;
                        ClothsAtStore[i].LengthOfClothAtStore *= 100;
                    }
                    break;
                case "/10":
                    for (int i = 0; i < ClothsAtStore.Count; i++)
                    {
                        ClothsAtStore[i].LengthOfCloth /= 10;
                        ClothsAtStore[i].WidthOfCloth /= 10;
                        ClothsAtStore[i].WidthOfClothAtStore /= 10;
                        ClothsAtStore[i].LengthOfClothAtStore /= 10;
                    }
                    break;
                case "/100":
                    for (int i = 0; i < ClothsAtStore.Count; i++)
                    {
                        ClothsAtStore[i].LengthOfCloth /= 100;
                        ClothsAtStore[i].WidthOfCloth /= 100;
                        ClothsAtStore[i].WidthOfClothAtStore /= 100;
                        ClothsAtStore[i].LengthOfClothAtStore /= 100;
                    }
                    break;
                case "*1000":
                    for (int i = 0; i < ClothsAtStore.Count; i++)
                    {
                        ClothsAtStore[i].LengthOfCloth *= 1000;
                        ClothsAtStore[i].WidthOfCloth *= 1000;
                        ClothsAtStore[i].WidthOfClothAtStore *= 1000;
                        ClothsAtStore[i].LengthOfClothAtStore *= 1000;
                    }
                    break;
                case "/1000":
                    for (int i = 0; i < ClothsAtStore.Count; i++)
                    {
                        ClothsAtStore[i].LengthOfCloth /= 1000;
                        ClothsAtStore[i].WidthOfCloth /= 1000;
                        ClothsAtStore[i].WidthOfClothAtStore /= 1000;
                        ClothsAtStore[i].LengthOfClothAtStore /= 1000;
                    }
                    break;
                default:
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    try
                    {
                        string sql = "SELECT c.Cloth_Articul, c.Cloth_Name, c.`Cloth_Length(cm)`, c.`Cloth_Width(cm)`, c.`Cloth_Cost(rub)`, cs.ClothStore_Roll, " +
                            "cs.ClothStore_WidthOfRoll, cs.ClothStore_LengthOfRoll " +
                            "from cloth c " +
                            "inner join clothstore cs " +
                            "on c.Cloth_Articul = cs.ClothStore_Cloth_Articul;";

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.CommandText = sql;
                        cmd.Connection = conn;

                        var reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ClothsAtStore.Add(new ClothStore()
                                {
                                    Articul = reader.GetString(0),
                                    Name = reader.GetString(1),
                                    LengthOfCloth = reader.GetFloat(2),
                                    WidthOfCloth = reader.GetFloat(3),
                                    CostOfCloth = reader.GetFloat(4),
                                    RollAtStore = reader.GetInt32(5),
                                    WidthOfClothAtStore = reader.GetFloat(6),
                                    LengthOfClothAtStore = reader.GetFloat(7),
                                    CostOfAllCloth = ((reader.GetFloat(6) * reader.GetFloat(7)) / (reader.GetFloat(2) * reader.GetFloat(3))) * reader.GetFloat(4)
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
