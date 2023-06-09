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
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;

namespace WpfApp.ViewModels
{
    internal class MaterialComingViewModel : ViewModel
    {

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Коллекции элементов

        private ObservableCollection<ComingMaterial> _comingMaterials = new ObservableCollection<ComingMaterial>();
        public ObservableCollection<ComingMaterial> ComingMaterials { get => _comingMaterials; set => Set(ref _comingMaterials, value); }



        private ObservableCollection<string> _measurementUnits = new ObservableCollection<string>() { "см", "м", "дм", "мм" };
        public ObservableCollection<string> MeasurementUnits { get => _measurementUnits; set => Set(ref _measurementUnits, value); }



        private ObservableCollection<string> _clothsArticuls = new ObservableCollection<string>();
        public ObservableCollection<string> ClothsArticuls { get => _clothsArticuls; set => Set(ref _clothsArticuls, value); }

        private ObservableCollection<ClothStore> _cloths = new ObservableCollection<ClothStore>();
        public ObservableCollection<ClothStore> Cloths { get => _cloths; set => Set(ref _cloths, value); }



        private ObservableCollection<FurnitureStore> _furnitures = new ObservableCollection<FurnitureStore>();
        public ObservableCollection<FurnitureStore> Furnitures { get => _furnitures; set => Set(ref _furnitures, value); }

        private ObservableCollection<string> _furnituresArticuls = new ObservableCollection<string>();
        public ObservableCollection<string> FurnituresArticuls { get => _furnituresArticuls; set => Set(ref _furnituresArticuls, value); }

        #endregion

        #region Булевые переменные для доступности элементов страницы

        private bool _isDockPanelEnabled = true;
        private bool _isTabControlEnabled = false;

        public bool IsDockPanelEnabled { get => _isDockPanelEnabled; set => Set(ref _isDockPanelEnabled, value); }
        public bool IsTabControlEnabled { get => _isTabControlEnabled; set => Set(ref _isTabControlEnabled, value); }

        #endregion

        #region Данные для добавления ткани

        private ComingMaterial _selectedMaterial;
        private string _measurementUnit = "см";
        private string _selectedArticulOfCloth;
        private float _userWidth;
        private float _userLength;
        private float _widthOfRoll;
        private float _lengthOfRoll;
        private float _clothComingCost;

        public ComingMaterial SelectedMaterial { get => _selectedMaterial; set => Set(ref _selectedMaterial, value); }
        public string SelectedArticulOfCloth
        {
            get => _selectedArticulOfCloth;
            set
            {
                Set(ref _selectedArticulOfCloth, value);
                foreach (var item in Cloths)
                {
                    if (item.Articul == _selectedArticulOfCloth)
                    {
                        switch (MeasurementUnit)
                        {
                            case "см":
                                WidthOfRoll = item.WidthOfClothAtStore;
                                LengthOfRoll = item.LengthOfClothAtStore;
                                break;
                            case "м":
                                WidthOfRoll = (item.WidthOfClothAtStore / 100);
                                LengthOfRoll = (item.LengthOfClothAtStore / 100);
                                break;
                            case "дм":
                                WidthOfRoll = (item.WidthOfClothAtStore / 10);
                                LengthOfRoll = (item.LengthOfClothAtStore / 10);
                                break;
                            case "мм":
                                WidthOfRoll = (item.WidthOfClothAtStore * 10);
                                LengthOfRoll = (item.LengthOfClothAtStore * 10);
                                break;
                        }
                        float first = (_userWidth * UserLength) * item.CostOfAllCloth;
                        float second = WidthOfRoll * LengthOfRoll;
                        float third = (first / second);
                        ClothComingCost = third;
                    }
                }
            }
        }

        public float UserWidth
        {
            get => _userWidth;
            set
            {
                Set(ref _userWidth, value);
                foreach (var item in Cloths)
                {
                    if (item.Articul == SelectedArticulOfCloth)
                    {
                        switch (MeasurementUnit)
                        {
                            case "см":
                                WidthOfRoll = item.WidthOfClothAtStore;
                                LengthOfRoll = item.LengthOfClothAtStore;
                                break;
                            case "м":
                                WidthOfRoll = (item.WidthOfClothAtStore / 100);
                                LengthOfRoll = (item.LengthOfClothAtStore / 100);
                                break;
                            case "дм":
                                WidthOfRoll = (item.WidthOfClothAtStore / 10);
                                LengthOfRoll = (item.LengthOfClothAtStore / 10);
                                break;
                            case "мм":
                                WidthOfRoll = (item.WidthOfClothAtStore * 10);
                                LengthOfRoll = (item.LengthOfClothAtStore * 10);
                                break;
                        }
                        float first = (_userWidth * UserLength) * item.CostOfAllCloth;
                        float second = WidthOfRoll * LengthOfRoll;
                        float third = (first / second);
                        ClothComingCost = third;
                    }
                }
            }
        }

        public float UserLength
        {
            get => _userLength;
            set
            {
                Set(ref _userLength, value);
                foreach (var item in Cloths)
                {
                    if (item.Articul == SelectedArticulOfCloth)
                    {
                        switch (MeasurementUnit)
                        {
                            case "см":
                                WidthOfRoll = item.WidthOfClothAtStore;
                                LengthOfRoll = item.LengthOfClothAtStore;
                                break;
                            case "м":
                                WidthOfRoll = (item.WidthOfClothAtStore / 100);
                                LengthOfRoll = (item.LengthOfClothAtStore / 100);
                                break;
                            case "дм":
                                WidthOfRoll = (item.WidthOfClothAtStore / 10);
                                LengthOfRoll = (item.LengthOfClothAtStore / 10);
                                break;
                            case "мм":
                                WidthOfRoll = (item.WidthOfClothAtStore * 10);
                                LengthOfRoll = (item.LengthOfClothAtStore * 10);
                                break;
                        }
                        float first = (_userWidth * UserLength) * item.CostOfAllCloth;
                        float second = WidthOfRoll * LengthOfRoll;
                        float third = (first / second);
                        ClothComingCost = third;
                    }
                }
            }
        }


        public string MeasurementUnit
        {
            get => _measurementUnit;
            set
            {
                Set(ref _measurementUnit, value);

                foreach (var item in Cloths)
                {
                    if (item.Articul == SelectedArticulOfCloth)
                    {
                        switch (_measurementUnit)
                        {
                            case "см":
                                WidthOfRoll = item.WidthOfClothAtStore;
                                LengthOfRoll = item.LengthOfClothAtStore;
                                break;
                            case "м":
                                WidthOfRoll = (item.WidthOfClothAtStore / 100);
                                LengthOfRoll = (item.LengthOfClothAtStore / 100);
                                break;
                            case "дм":
                                WidthOfRoll = (item.WidthOfClothAtStore / 10);
                                LengthOfRoll = (item.LengthOfClothAtStore / 10);
                                break;
                            case "мм":
                                WidthOfRoll = (item.WidthOfClothAtStore * 10);
                                LengthOfRoll = (item.LengthOfClothAtStore * 10);
                                break;
                        }
                        UserWidth = 0;
                        UserLength = 0;
                    }
                }
            }
        }
        public float WidthOfRoll { get => _widthOfRoll; set => Set(ref _widthOfRoll, value); }
        public float LengthOfRoll { get => _lengthOfRoll; set => Set(ref _lengthOfRoll, value); }
        public float ClothComingCost { get => _clothComingCost; set => Set(ref _clothComingCost, value); }

        #endregion

        #region Данные для добавления фурнитуры

        private string _selectedArticulOfFurniture;
        private string _userQuantity;
        private string _furnitureQuantity;
        private string _furnitureComingCost;

        public string SelectedArticuleOfFurniture
        {
            get => _selectedArticulOfFurniture;
            set
            {
                Set(ref _selectedArticulOfFurniture, value);

                float UserQuantity;
                foreach (var item in Furnitures)
                {
                    if (item.Articul == _selectedArticulOfFurniture)
                    {
                        if (float.TryParse(_userQuantity, out UserQuantity))
                        {
                            FurnitureComingCost = (item.Cost * UserQuantity).ToString();
                        }
                        FurnitureQuantity = item.Quantity.ToString();
                        break;
                    }
                }
            }
        }
        public string UserQuantity
        {
            get => _userQuantity;
            set
            {
                if (_userQuantity != value)
                {
                    if (value.Any(x => char.IsDigit(x)))
                        Set(ref _userQuantity, value);
                    else
                        Set(ref _userQuantity, "");
                }
                float UserQuantity;
                foreach (var item in Furnitures)
                {
                    if (item.Articul == SelectedArticuleOfFurniture)
                    {
                        if (float.TryParse(_userQuantity, out UserQuantity))
                        {
                            FurnitureComingCost = (item.Cost * UserQuantity).ToString();
                            break;
                        }
                    }
                }
            }
        }
        public string FurnitureQuantity { get => _furnitureQuantity; set => Set(ref _furnitureQuantity, value); }
        public string FurnitureComingCost { get => _furnitureComingCost; set => Set(ref _furnitureComingCost, value); }

        #endregion

        #region Команды

        #region Команда добавления ткани

        public ICommand ClothComingCommand { get; }

        private bool CanClothComingCommandExecute(object parameter) => true;
        private void OnClothComingCommandExecuted(object parameter)
        {
            if (_selectedArticulOfCloth == null)
            {
                MessageBox.Show("Вы не выбрали ткань");
            }
            else
            {
                if (UserWidth == 0 || UserLength == 0)
                {
                    MessageBox.Show("Вы не ввели необходимые для добавления данные");
                }
                else
                {
                    if (ComingMaterials.Where(x => x.TypeOfMaterial == "ткань").Count() != 0)
                    {
                        foreach (var material in ComingMaterials.Where(x => x.TypeOfMaterial == "ткань"))
                        {
                            if (SelectedArticulOfCloth == material.Articul)
                            {
                                switch (MeasurementUnit)
                                {
                                    case "см":
                                        material.ComingLengthOfRoll += UserLength;
                                        material.ComingWidthOfRoll += UserWidth;
                                        break;
                                    case "м":
                                        material.ComingLengthOfRoll += UserLength * 100;
                                        material.ComingWidthOfRoll += UserWidth * 100;
                                        break;
                                    case "дм":
                                        material.ComingLengthOfRoll += UserLength * 10;
                                        material.ComingWidthOfRoll += UserWidth * 10;
                                        break;
                                    case "мм":
                                        material.ComingLengthOfRoll += UserLength / 10;
                                        material.ComingWidthOfRoll += UserWidth / 10;
                                        break;
                                }
                                material.ComingCost += ClothComingCost;
                                Cleaner();
                                return;
                            }
                        }
                        switch (MeasurementUnit)
                        {
                            case "см":
                                ComingMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingWidthOfRoll = UserWidth,
                                    ComingLengthOfRoll = UserLength,
                                    ComingCost = ClothComingCost
                                });
                                break;
                            case "м":
                                ComingMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingWidthOfRoll = UserWidth * 100,
                                    ComingLengthOfRoll = UserLength * 100,
                                    ComingCost = ClothComingCost
                                });
                                break;
                            case "дм":
                                ComingMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingWidthOfRoll = UserWidth * 10,
                                    ComingLengthOfRoll = UserLength * 10,
                                    ComingCost = ClothComingCost
                                });
                                break;
                            case "мм":
                                ComingMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingWidthOfRoll = UserWidth / 10,
                                    ComingLengthOfRoll = UserLength / 10,
                                    ComingCost = ClothComingCost
                                });
                                break;
                        }
                        Cleaner();
                        return;
                    }
                    else
                    {
                        switch (MeasurementUnit)
                        {
                            case "см":
                                ComingMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingWidthOfRoll = UserWidth,
                                    ComingLengthOfRoll = UserLength,
                                    ComingCost = ClothComingCost
                                });
                                break;
                            case "м":
                                ComingMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingWidthOfRoll = UserWidth * 100,
                                    ComingLengthOfRoll = UserLength * 100,
                                    ComingCost = ClothComingCost
                                });
                                break;
                            case "дм":
                                ComingMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingWidthOfRoll = UserWidth * 10,
                                    ComingLengthOfRoll = UserLength * 10,
                                    ComingCost = ClothComingCost
                                });
                                break;
                            case "мм":
                                ComingMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingWidthOfRoll = UserWidth / 10,
                                    ComingLengthOfRoll = UserLength / 10,
                                    ComingCost = ClothComingCost
                                });
                                break;
                        }
                        Cleaner();
                    }
                }
            }
        }

        #endregion

        #region Команда добавления фурнитуры

        public ICommand FurnituresComingCommand { get; }

        private bool CanFurnituresComingCommandExecute(object parameter) => true;
        private void OnFurnituresComingCommandExecuted(object parameter)
        {
            if (SelectedArticuleOfFurniture == null)
            {
                MessageBox.Show("Вы не выбрали фурнитуру");
            }
            else
            {
                if (UserQuantity == "" || UserQuantity.Any(x => char.IsLetter(x)))
                {
                    MessageBox.Show("Вы не ввели необходимые для добавления данные");
                }
                else
                {
                    if (float.Parse(UserQuantity) > float.Parse(FurnitureQuantity))
                    {
                        MessageBox.Show("Вы не можете списать больше, чем есть на складе");
                        return;
                    }
                    if (ComingMaterials.Where(x => x.TypeOfMaterial == "фурнитура").Count() != 0)
                    {
                        foreach (var material in ComingMaterials.Where(x => x.TypeOfMaterial == "фурнитура"))
                        {
                            if (SelectedArticuleOfFurniture == material.Articul)
                            {
                                material.ComingQuantity += int.Parse(UserQuantity);
                                material.ComingCost += float.Parse(FurnitureComingCost);
                                Cleaner();
                                return;
                            }
                        }
                        ComingMaterials.Add(new ComingMaterial
                        {
                            Articul = SelectedArticuleOfFurniture,
                            TypeOfMaterial = "фурнитура",
                            ComingQuantity = float.Parse(UserQuantity),
                            ComingCost = float.Parse(FurnitureComingCost)
                        });
                        Cleaner();
                        return;
                    }
                    else
                    {
                        ComingMaterials.Add(new ComingMaterial
                        {
                            Articul = SelectedArticuleOfFurniture,
                            TypeOfMaterial = "фурнитура",
                            ComingQuantity = float.Parse(UserQuantity),
                            ComingCost = float.Parse(FurnitureComingCost)
                        });
                        Cleaner();
                    }
                }
            }
        }

        #endregion

        #region Команда перехода к добавлению материала 

        public ICommand ToAddMaterialCommand { get; }

        private bool CanToAddMaterialCommandExecute(object parameter) => true;
        private void OnToAddMaterialCommandExecuted(object parameter)
        {
            IsDockPanelEnabled = false;
            IsTabControlEnabled = true;

        }

        #endregion

        #region Команда отмены добавления материала

        public ICommand CancellAddMaterialCommand { get; }

        private bool CanCancellAddMaterialCommandExecute(object parameter) => true;
        private void OnCancellAddMaterialCommandExecuted(object parameter)
        {
            Cleaner();
        }

        #endregion

        #region Команда удаления материала из списка

        public ICommand RemoveMaterialFromCollectionCommand { get; }

        private bool CanRemoveMaterialFromCollectionCommandExecute(object parameter) => true;
        private void OnRemoveMaterialFromCollectionCommandExecuted(object parameter)
        {
            if (parameter == null)
            {
                MessageBox.Show("Вы не выбрали материал");
                return;
            }
            else
            {
                ComingMaterial comingMaterial = parameter as ComingMaterial;
                ComingMaterials.Remove(comingMaterial);
            }
        }

        #endregion

        #region Команда утверждения поступления материалов на склад

        public ICommand ConfirmAdditionOfMaterials { get; }

        private bool CanConfirmAdditionOfMaterialsExecute(object parameter) => true;
        private async void OnConfirmAdditionOfMaterialsEcecuted(object parameter)
        {
            if (ComingMaterials.Count == 0)
            {
                MessageBox.Show("Вы не выбрали материалы");
            }
            else
            {
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                try
                {
                    string sqlFurniture = "update furniturestore " +
                        "set FurnitureStore_Quantity = FurnitureStore_Quantity + @quantity " +
                        "where FurnitureStore_Furniture_Articul = @furnitureArticul;";
                    string sqlCloth = "update clothstore " +
                        "set ClothStore_WidthOfRoll = ClothStore_WidthOfRoll + @widthOfRoll, " +
                        "ClothStore_LengthOfRoll = ClothStore_LengthOfRoll + @lengthOfRoll " +
                        "where ClothStore_Cloth_Articul = @clothArticul";
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    foreach (var material in ComingMaterials)
                    {
                        switch (material.TypeOfMaterial)
                        {
                            case "ткань":
                                cmd.CommandText = sqlCloth;
                                cmd.Parameters.AddWithValue("@widthOfRoll", material.ComingWidthOfRoll);
                                cmd.Parameters.AddWithValue("@lengthOfRoll", material.ComingLengthOfRoll);
                                cmd.Parameters.AddWithValue("@clothArticul", material.Articul);
                                await cmd.ExecuteNonQueryAsync();
                                cmd.Parameters.Clear();
                                break;
                            case "фурнитура":
                                cmd.CommandText = sqlFurniture;
                                cmd.Parameters.AddWithValue("@quantity", material.ComingQuantity);
                                cmd.Parameters.AddWithValue("@furnitureArticul", material.Articul);
                                await cmd.ExecuteNonQueryAsync();
                                cmd.Parameters.Clear();
                                break;
                        }
                    }
                    SelectedMaterial = null;
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
                try
                {
                    var app = new Word.Application();
                    Word.Document document = app.Documents.Add();

                    Word.Paragraph titleParagraph = document.Paragraphs.Add();
                    Word.Range titleRange = titleParagraph.Range;
                    titleRange.Text = "Отчет о списании материалов";
                    titleParagraph.set_Style("Заголовок 1;Title1");
                    titleRange.InsertParagraphAfter();

                    foreach (var material in ComingMaterials)
                    {
                        switch (material.TypeOfMaterial)
                        {
                            case "фурнитура":
                                Word.Paragraph FurnitureParagraph = document.Paragraphs.Add();
                                Word.Range FurnitureRange = FurnitureParagraph.Range;
                                FurnitureRange.Text = $"На склад поступило {material.ComingQuantity} единиц фурнитуры {material.Articul} стоимостью {material.ComingCost} рублей";
                                FurnitureParagraph.set_Style("Обычный;MainStyle");
                                FurnitureRange.InsertParagraphAfter();
                                break;
                            case "ткань":
                                Word.Paragraph ClothParagraph = document.Paragraphs.Add();
                                Word.Range ClothRange = ClothParagraph.Range;
                                ClothRange.Text = $"На склад поступило {material.ComingWidthOfRoll} см ширины и {material.ComingLengthOfRoll} см длины" +
                                    $" ткани {material.Articul} стоимостью {material.ComingCost} рублей";
                                ClothParagraph.set_Style("Обычный;MainStyle");
                                ClothRange.InsertParagraphAfter();
                                break;
                        }
                    }
                    ComingMaterials.Clear();
                    app.Visible = true;

                    document.SaveAs2(@$"D:\Учеба\Учебная практика 2\Приложение\Отчеты_Word\{"Отчет о поступлении материалов за " + DateTime.Now.ToString("yyyy_MM_dd HH_mm") + ".docx"}");
                    document.SaveAs2(@$"D:\Учеба\Учебная практика 2\Приложение\Отчеты_Pdf\{"Отчет о поступлении материалов за " + DateTime.Now.ToString("yyyy_MM_dd HH_mm") + ".pdf"}",
                        WdExportFormat.wdExportFormatPDF);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        #endregion

        #endregion

        public MaterialComingViewModel()
        {
            GetFurnitures();
            GetCloths();
            GetClothsArticuls();
            GetFurnituresArticuls();

            #region Команды

            ClothComingCommand = new LambdaCommand(OnClothComingCommandExecuted, CanClothComingCommandExecute);
            FurnituresComingCommand = new LambdaCommand(OnFurnituresComingCommandExecuted, CanFurnituresComingCommandExecute);
            ToAddMaterialCommand = new LambdaCommand(OnToAddMaterialCommandExecuted, CanToAddMaterialCommandExecute);
            CancellAddMaterialCommand = new LambdaCommand(OnCancellAddMaterialCommandExecuted, CanCancellAddMaterialCommandExecute);
            RemoveMaterialFromCollectionCommand = new LambdaCommand(OnRemoveMaterialFromCollectionCommandExecuted, CanRemoveMaterialFromCollectionCommandExecute);
            ConfirmAdditionOfMaterials = new LambdaCommand(OnConfirmAdditionOfMaterialsEcecuted, CanConfirmAdditionOfMaterialsExecute);

            #endregion
        }

        private void GetCloths()
        {
            Cloths.Clear();
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
                        Cloths.Add(new ClothStore()
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
        }
        private void GetClothsArticuls()
        {
            ClothsArticuls.Clear();
            foreach (var item in Cloths)
            {
                ClothsArticuls.Add(item.Articul);
            }
        }

        private void GetFurnitures()
        {
            Furnitures.Clear();
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
                        Furnitures.Add(new FurnitureStore()
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
        private void GetFurnituresArticuls()
        {
            FurnituresArticuls.Clear();
            foreach (var item in Furnitures)
            {
                FurnituresArticuls.Add(item.Articul);
            }
        }

        private void Cleaner()
        {
            SelectedArticuleOfFurniture = null;
            FurnitureQuantity = "";
            FurnitureComingCost = "";
            UserQuantity = "";

            SelectedArticulOfCloth = null;
            WidthOfRoll = 0;
            LengthOfRoll = 0;
            UserLength = 0;
            UserWidth = 0;
            MeasurementUnit = "см";
            ClothComingCost = 0;

            IsDockPanelEnabled = true;
            IsTabControlEnabled = false;
        }

    }
}
