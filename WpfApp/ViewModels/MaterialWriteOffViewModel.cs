using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
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
using Word = Microsoft.Office.Interop.Word;

namespace WpfApp.ViewModels
{
    internal class MaterialWriteOffViewModel : ViewModel
    {

        #region Коллекции элементов для добавления

        private ObservableCollection<ComingMaterial> _writeOffMaterials = new ObservableCollection<ComingMaterial>();
        public ObservableCollection<ComingMaterial> WriteOffMaterials { get => _writeOffMaterials; set => Set(ref _writeOffMaterials, value); }

        #endregion

        #region Коллекции элементов тканей

        private ObservableCollection<string> _measurementUnits = new ObservableCollection<string>() { "см2", "м2", "дм2", "мм2" };
        public ObservableCollection<string> MeasurementUnits { get => _measurementUnits; set => Set(ref _measurementUnits, value); }

        private ObservableCollection<string> _clothsArticuls = new ObservableCollection<string>();
        public ObservableCollection<string> ClothsArticuls { get => _clothsArticuls; set => Set(ref _clothsArticuls, value); }

        private ObservableCollection<ClothStore> _cloths = new ObservableCollection<ClothStore>();
        public ObservableCollection<ClothStore> Cloths { get => _cloths; set => Set(ref _cloths, value); }

        #endregion

        #region Коллекция элементов фурнитуры

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

        #region Данные для списания ткани

        private ComingMaterial _selectedMaterial;
        private string _measurementUnit = "см2";
        private string _selectedArticulOfCloth;
        private string _userArea = "";
        private string _clothArea;
        private string _clothWriteOffCost;

        public ComingMaterial SelectedMaterial { get => _selectedMaterial; set => Set(ref _selectedMaterial, value); }
        public string SelectedArticulOfCloth
        {
            get => _selectedArticulOfCloth;
            set
            {
                Set(ref _selectedArticulOfCloth, value);
                float UserArea;
                foreach (var item in Cloths)
                {
                    if (item.Articul == _selectedArticulOfCloth)
                    {
                        switch (MeasurementUnit)
                        {
                            case "см2":
                                ClothArea = item.AreaOfClothAtStoreInSM.ToString();
                                if (float.TryParse(_userArea, out UserArea))
                                {
                                    float first = UserArea * item.CostOfAllCloth;
                                    float second = float.Parse(ClothArea);
                                    float third = (first / second);
                                    ClothWriteOffCost = third.ToString();
                                }
                                break;
                            case "м2":
                                ClothArea = (item.AreaOfClothAtStoreInSM / 10000).ToString();
                                if (float.TryParse(_userArea, out UserArea))
                                {
                                    float first = UserArea * item.CostOfAllCloth;
                                    float second = float.Parse(ClothArea);
                                    float third = (first / second);
                                    ClothWriteOffCost = third.ToString();
                                }
                                break;
                            case "дм2":
                                ClothArea = (item.AreaOfClothAtStoreInSM / 100).ToString();
                                if (float.TryParse(_userArea, out UserArea))
                                {
                                    float first = UserArea * item.CostOfAllCloth;
                                    float second = float.Parse(ClothArea);
                                    float third = (first / second);
                                    ClothWriteOffCost = third.ToString();
                                }
                                break;
                            case "мм2":
                                ClothArea = (item.AreaOfClothAtStoreInSM * 100).ToString();
                                if (float.TryParse(_userArea, out UserArea))
                                {
                                    float first = UserArea * item.CostOfAllCloth;
                                    float second = float.Parse(ClothArea);
                                    float third = (first / second);
                                    ClothWriteOffCost = third.ToString();
                                }
                                break;
                        }
                    }
                }
            }
        }
        public string UserArea
        {
            get => _userArea;
            set
            {
                if (_userArea != value)
                {
                    if (value.Any(x => !char.IsLetter(x)))
                    {
                        Set(ref _userArea, value);
                    }
                    else
                        Set(ref _userArea, "");
                    float UserArea;
                    foreach (var item in Cloths)
                    {
                        if (item.Articul == SelectedArticulOfCloth)
                        {
                            switch (MeasurementUnit)
                            {
                                case "см2":
                                    ClothArea = item.AreaOfClothAtStoreInSM.ToString();
                                    if (float.TryParse(_userArea, out UserArea))
                                    {
                                        float first = UserArea * item.CostOfAllCloth;
                                        float second = float.Parse(ClothArea);
                                        float third = (first / second);
                                        ClothWriteOffCost = third.ToString();
                                    }
                                    break;
                                case "м2":
                                    ClothArea = (item.AreaOfClothAtStoreInSM / 10000).ToString();
                                    if (float.TryParse(_userArea, out UserArea))
                                    {
                                        float first = UserArea * item.CostOfAllCloth;
                                        float second = float.Parse(ClothArea);
                                        float third = (first / second);
                                        ClothWriteOffCost = third.ToString();
                                    }
                                    break;
                                case "дм2":
                                    ClothArea = (item.AreaOfClothAtStoreInSM / 100).ToString();
                                    if (float.TryParse(_userArea, out UserArea))
                                    {
                                        float first = UserArea * item.CostOfAllCloth;
                                        float second = float.Parse(ClothArea);
                                        float third = (first / second);
                                        ClothWriteOffCost = third.ToString();
                                    }
                                    break;
                                case "мм2":
                                    ClothArea = (item.AreaOfClothAtStoreInSM * 100).ToString();
                                    if (float.TryParse(_userArea, out UserArea))
                                    {
                                        float first = UserArea * item.CostOfAllCloth;
                                        float second = float.Parse(ClothArea);
                                        float third = (first / second);
                                        ClothWriteOffCost = third.ToString();
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public string ClothArea { get => _clothArea; set => Set(ref _clothArea, value); }
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
                            case "см2":
                                ClothArea = item.AreaOfClothAtStoreInSM.ToString();
                                break;
                            case "м2":
                                ClothArea = (item.AreaOfClothAtStoreInSM / 10000).ToString();
                                break;
                            case "дм2":
                                ClothArea = (item.AreaOfClothAtStoreInSM / 100).ToString();
                                break;
                            case "мм2":
                                ClothArea = (item.AreaOfClothAtStoreInSM * 100).ToString();
                                break;
                        }
                        UserArea = "";
                    }
                }
            }
        }
        public string ClothWriteOffCost { get => _clothWriteOffCost; set => Set(ref _clothWriteOffCost, value); }

        #endregion

        #region Данные для списания фурнитуры

        private string _selectedArticulOfFurniture;
        private string _userQuantity;
        private string _furnitureQuantity;
        private string _furnitureWriteOffCost;

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
                            FurnitureWriteOffCost = (item.Cost * UserQuantity).ToString();
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
                            FurnitureWriteOffCost = (item.Cost * UserQuantity).ToString();
                            break;
                        }
                    }
                }
            }
        }
        public string FurnitureQuantity { get => _furnitureQuantity; set => Set(ref _furnitureQuantity, value); }
        public string FurnitureWriteOffCost { get => _furnitureWriteOffCost; set => Set(ref _furnitureWriteOffCost, value); }

        #endregion

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

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
                if (UserArea == "" || UserArea.Any(x => char.IsLetter(x)))
                {
                    MessageBox.Show("Вы не ввели необходимые для добавления данные");
                }
                else
                {
                    if (float.Parse(UserArea) > float.Parse(ClothArea))
                    {
                        MessageBox.Show("Вы не можете списать больше, чем есть на складе");
                        return;
                    }
                    if (WriteOffMaterials.Where(x => x.TypeOfMaterial == "ткань").Count() != 0)
                    {
                        foreach (var material in WriteOffMaterials.Where(x => x.TypeOfMaterial == "ткань"))
                        {
                            if (SelectedArticulOfCloth == material.Articul)
                            {
                                switch (MeasurementUnit)
                                {
                                    case "см2":
                                        material.ComingQuantity += int.Parse(UserArea) / 10000;
                                        break;
                                    case "м2":
                                        material.ComingQuantity += int.Parse(UserArea);
                                        break;
                                    case "дм2":
                                        material.ComingQuantity += int.Parse(UserArea) / 100;
                                        break;
                                    case "мм2":
                                        material.ComingQuantity += int.Parse(UserArea) / 1000000;
                                        break;
                                }
                                material.ComingCost += float.Parse(ClothWriteOffCost);
                                SelectedArticuleOfFurniture = null;
                                FurnitureQuantity = "";
                                FurnitureWriteOffCost = "";
                                UserQuantity = "";
                                SelectedArticulOfCloth = null;
                                ClothArea = "";
                                MeasurementUnit = "см2";
                                ClothWriteOffCost = "";
                                UserArea = "";
                                IsDockPanelEnabled = true;
                                IsTabControlEnabled = false;
                                return;
                            }
                        }
                        switch (MeasurementUnit)
                        {
                            case "см2":
                                WriteOffMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingQuantity = float.Parse(UserArea) / 10000,
                                    ComingCost = float.Parse(ClothWriteOffCost)
                                });
                                break;
                            case "м2":
                                WriteOffMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingQuantity = float.Parse(UserArea),
                                    ComingCost = float.Parse(ClothWriteOffCost)
                                });
                                break;
                            case "дм2":
                                WriteOffMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingQuantity = float.Parse(UserArea) / 100,
                                    ComingCost = float.Parse(ClothWriteOffCost)
                                });
                                break;
                            case "мм2":
                                WriteOffMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingQuantity = float.Parse(UserArea) / 1000000,
                                    ComingCost = float.Parse(ClothWriteOffCost)
                                });
                                break;
                        }
                        SelectedArticuleOfFurniture = null;
                        FurnitureQuantity = "";
                        FurnitureWriteOffCost = "";
                        UserQuantity = "";
                        SelectedArticulOfCloth = null;
                        ClothArea = "";
                        MeasurementUnit = "см2";
                        ClothWriteOffCost = "";
                        UserArea = "";
                        IsDockPanelEnabled = true;
                        IsTabControlEnabled = false;
                        return;
                    }
                    else
                    {
                        switch (MeasurementUnit)
                        {
                            case "см2":
                                WriteOffMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingQuantity = float.Parse(UserArea) / 10000,
                                    ComingCost = float.Parse(ClothWriteOffCost)
                                });
                                break;
                            case "м2":
                                WriteOffMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingQuantity = float.Parse(UserArea),
                                    ComingCost = float.Parse(ClothWriteOffCost)
                                });
                                break;
                            case "дм2":
                                WriteOffMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingQuantity = float.Parse(UserArea) / 100,
                                    ComingCost = float.Parse(ClothWriteOffCost)
                                });
                                break;
                            case "мм2":
                                WriteOffMaterials.Add(new ComingMaterial
                                {
                                    Articul = SelectedArticulOfCloth,
                                    TypeOfMaterial = "ткань",
                                    ComingQuantity = float.Parse(UserArea) / 1000000,
                                    ComingCost = float.Parse(ClothWriteOffCost)
                                });
                                break;
                        }
                        SelectedArticuleOfFurniture = null;
                        FurnitureQuantity = "";
                        FurnitureWriteOffCost = "";
                        UserQuantity = "";
                        SelectedArticulOfCloth = null;
                        ClothArea = "";
                        MeasurementUnit = "см2";
                        ClothWriteOffCost = "";
                        UserArea = "";
                        IsDockPanelEnabled = true;
                        IsTabControlEnabled = false;
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
                    if (WriteOffMaterials.Where(x => x.TypeOfMaterial == "фурнитура").Count() != 0)
                    {
                        foreach (var material in WriteOffMaterials.Where(x => x.TypeOfMaterial == "фурнитура"))
                        {
                            if (SelectedArticuleOfFurniture == material.Articul)
                            {
                                material.ComingQuantity += int.Parse(UserQuantity);
                                material.ComingCost += float.Parse(FurnitureWriteOffCost);
                                SelectedArticuleOfFurniture = null;
                                FurnitureQuantity = "";
                                FurnitureWriteOffCost = "";
                                UserQuantity = "";
                                SelectedArticulOfCloth = null;
                                ClothArea = "";
                                MeasurementUnit = "см2";
                                ClothWriteOffCost = "";
                                UserArea = "";
                                IsDockPanelEnabled = true;
                                IsTabControlEnabled = false;
                                return;
                            }
                        }
                        WriteOffMaterials.Add(new ComingMaterial
                        {
                            Articul = SelectedArticuleOfFurniture,
                            TypeOfMaterial = "фурнитура",
                            ComingQuantity = float.Parse(UserQuantity),
                            ComingCost = float.Parse(FurnitureWriteOffCost)
                        });
                        SelectedArticuleOfFurniture = null;
                        FurnitureQuantity = "";
                        FurnitureWriteOffCost = "";
                        UserQuantity = "";
                        SelectedArticulOfCloth = null;
                        ClothArea = "";
                        MeasurementUnit = "см2";
                        ClothWriteOffCost = "";
                        UserArea = "";
                        IsDockPanelEnabled = true;
                        IsTabControlEnabled = false;
                        return;
                    }
                    else
                    {
                        WriteOffMaterials.Add(new ComingMaterial
                        {
                            Articul = SelectedArticuleOfFurniture,
                            TypeOfMaterial = "фурнитура",
                            ComingQuantity = float.Parse(UserQuantity),
                            ComingCost = float.Parse(FurnitureWriteOffCost)
                        });
                        SelectedArticuleOfFurniture = null;
                        FurnitureQuantity = "";
                        FurnitureWriteOffCost = "";
                        UserQuantity = "";
                        SelectedArticulOfCloth = null;
                        ClothArea = "";
                        MeasurementUnit = "см2";
                        ClothWriteOffCost = "";
                        UserArea = "";
                        IsDockPanelEnabled = true;
                        IsTabControlEnabled = false;
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
            IsDockPanelEnabled = true;
            IsTabControlEnabled = false;
            SelectedArticuleOfFurniture = null;
            FurnitureQuantity = "";
            FurnitureWriteOffCost = "";
            UserQuantity = "";
            SelectedArticulOfCloth = null;
            ClothArea = "";
            MeasurementUnit = "см2";
            ClothWriteOffCost = "";
            UserArea = "";

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
                WriteOffMaterials.Remove(comingMaterial);
            }
        }

        #endregion

        #region Команда утверждения списания материалов со склада

        public ICommand ConfirmWriteOffOfMaterials { get; }

        private bool CanConfirmWriteOffOfMaterialsExecute(object parameter) => true;
        private async void OnConfirmWriteOffOfMaterialsExecuted(object parameter)
        {
            if (WriteOffMaterials.Count == 0)
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
                        "set FurnitureStore_Quantity = FurnitureStore_Quantity - @quantity " +
                        "where FurnitureStore_Furniture_Articul = @furnitureArticul;";
                    string sqlCloth = "update clothstore " +
                        "set ClothStore_ClothArea = ClothStore_ClothArea - @area " +
                        "where ClothStore_Cloth_Articul = @clothArticul";
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    foreach (var material in WriteOffMaterials)
                    {
                        switch (material.TypeOfMaterial)
                        {
                            case "ткань":
                                cmd.CommandText = sqlCloth;
                                cmd.Parameters.AddWithValue("@area", material.ComingQuantity);
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

                    foreach (var material in WriteOffMaterials)
                    {
                        switch (material.TypeOfMaterial)
                        {
                            case "фурнитура":
                                Word.Paragraph FurnitureParagraph = document.Paragraphs.Add();
                                Word.Range FurnitureRange = FurnitureParagraph.Range;
                                FurnitureRange.Text = $"Со склада было списано {material.ComingQuantity} единиц фурнитуры {material.Articul} стоимостью {material.ComingCost} рублей";
                                FurnitureParagraph.set_Style("Обычный;MainStyle");
                                FurnitureRange.InsertParagraphAfter();
                                break;
                            case "ткань":
                                Word.Paragraph ClothParagraph = document.Paragraphs.Add();
                                Word.Range ClothRange = ClothParagraph.Range;
                                ClothRange.Text = $"Со склада было списано {material.ComingQuantity} м2 ткани {material.Articul} стоимостью {material.ComingCost} рублей";
                                ClothParagraph.set_Style("Обычный;MainStyle");
                                ClothRange.InsertParagraphAfter();
                                break;
                        }
                    }
                    WriteOffMaterials.Clear();
                    app.Visible = true;

                    document.SaveAs2(@$"D:\Учеба\Учебная практика 2\Приложение\Отчеты_Word\{"Отчет о списании материалов за " + DateTime.Now.ToString("yyyy_MM_dd HH_mm") + ".docx"}");
                    document.SaveAs2(@$"D:\Учеба\Учебная практика 2\Приложение\Отчеты_Pdf\{"Отчет о списании материалов за " + DateTime.Now.ToString("yyyy_MM_dd HH_mm") + ".pdf"}",
                        WdExportFormat.wdExportFormatPDF);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.ToString());
                }
            }
        }

        #endregion

        #endregion

        public MaterialWriteOffViewModel()
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
            ConfirmWriteOffOfMaterials = new LambdaCommand(OnConfirmWriteOffOfMaterialsExecuted, CanConfirmWriteOffOfMaterialsExecute);

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
                    "cs.ClothStore_ClothArea " +
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
                            AreaOfCloth = reader.GetFloat(2) * reader.GetFloat(3),
                            CostOfCloth = reader.GetFloat(4),
                            RollAtStore = reader.GetInt32(5),
                            AreaOfClothAtStoreInSM = reader.GetFloat(6) * 10000,
                            CostOfAllCloth = ((reader.GetFloat(6) * 10000) / (reader.GetFloat(2) * reader.GetFloat(3))) * reader.GetFloat(4)
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
    }
}
