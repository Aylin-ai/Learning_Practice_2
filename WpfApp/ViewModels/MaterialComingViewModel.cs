using MySql.Data.MySqlClient;
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
    internal class MaterialComingViewModel : ViewModel
    {

        #region Коллекции элементов тканей

        private ObservableCollection<string> _measurementUnits = new ObservableCollection<string>() { "см", "м", "дм", "мм" };
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

        #region Данные для добавления ткани

        private string _measurementUnit = "см";
        private string _selectedArticul;
        private string _userWidth = "";
        private string _userLength = "";
        private string _clothWidth;
        private string _clothLength;
        private string _clothComingCost;

        public string SelectedArticul
        {
            get => _selectedArticul;
            set
            {
                Set(ref _selectedArticul, value);
                float UserLength, UserWidth;
                foreach (var item in Cloths)
                {
                    if (item.Articul == _selectedArticul)
                    {
                        switch (MeasurementUnit)
                        {
                            case "см":
                                ClothLength = item.LengthOfClothAtStoreInSM.ToString();
                                ClothWidth = item.WidthOfClothAtStoreInSM.ToString();
                                if (float.TryParse(_userWidth, out UserWidth) && float.TryParse(_userLength, out UserLength))
                                {
                                    float first = UserLength * UserWidth;
                                    float second = item.WidthOfCloth * item.LengthOfCloth;
                                    float third = (first / second) * item.CostOfCloth;
                                    ClothComingCost = third.ToString();
                                }
                                break;
                            case "м":
                                ClothLength = (item.LengthOfClothAtStoreInSM / 100).ToString();
                                ClothWidth = (item.WidthOfClothAtStoreInSM / 100).ToString();
                                if (float.TryParse(_userWidth, out UserWidth) && float.TryParse(_userLength, out UserLength))
                                {
                                    float first = UserLength * UserWidth;
                                    float second = (item.WidthOfCloth / 100) * (item.LengthOfCloth / 100);
                                    float third = (first / second) * item.CostOfCloth;
                                    ClothComingCost = third.ToString();
                                }
                                break;
                            case "дм":
                                ClothLength = (item.LengthOfClothAtStoreInSM / 10).ToString();
                                ClothWidth = (item.WidthOfClothAtStoreInSM / 10).ToString();
                                if (float.TryParse(_userWidth, out UserWidth) && float.TryParse(_userLength, out UserLength))
                                {
                                    float first = UserLength * UserWidth;
                                    float second = (item.WidthOfCloth / 10) * (item.LengthOfCloth / 10);
                                    float third = (first / second) * item.CostOfCloth;
                                    ClothComingCost = third.ToString();
                                }
                                break;
                            case "мм":
                                ClothLength = (item.LengthOfClothAtStoreInSM * 10).ToString();
                                ClothWidth = (item.WidthOfClothAtStoreInSM * 10).ToString();
                                if (float.TryParse(_userWidth, out UserWidth) && float.TryParse(_userLength, out UserLength))
                                {
                                    float first = UserLength * UserWidth;
                                    float second = (item.WidthOfCloth * 10) * (item.LengthOfCloth * 10);
                                    float third = (first / second) * item.CostOfCloth;
                                    ClothComingCost = third.ToString();
                                }
                                break;
                        }
                    }
                }
            }
        }
        public string UserWidth
        {
            get => _userWidth;
            set
            {
                if (_userWidth != value)
                {
                    if (value.Any(x => !char.IsLetter(x)))
                    {
                        Set(ref _userWidth, value);
                    }
                    else
                        Set(ref _userWidth, "");
                    float UserLength, UserWidth;
                    foreach (var item in Cloths)
                    {
                        if (item.Articul == SelectedArticul)
                        {
                            switch (MeasurementUnit)
                            {
                                case "см":
                                    if (float.TryParse(_userWidth, out UserWidth) && float.TryParse(_userLength, out UserLength))
                                    {
                                        float first = UserLength * UserWidth;
                                        float second = item.WidthOfCloth * item.LengthOfCloth;
                                        float third = (first / second) * item.CostOfCloth;
                                        ClothComingCost = third.ToString();
                                    }
                                    break;
                                case "м":
                                    if (float.TryParse(_userWidth, out UserWidth) && float.TryParse(_userLength, out UserLength))
                                    {
                                        float first = UserLength * UserWidth;
                                        float second = (item.WidthOfCloth / 100) * (item.LengthOfCloth / 100);
                                        float third = (first / second) * item.CostOfCloth;
                                        ClothComingCost = third.ToString();
                                    }
                                    break;
                                case "дм":
                                    if (float.TryParse(_userWidth, out UserWidth) && float.TryParse(_userLength, out UserLength))
                                    {
                                        float first = UserLength * UserWidth;
                                        float second = (item.WidthOfCloth / 10) * (item.LengthOfCloth / 10);
                                        float third = (first / second) * item.CostOfCloth;
                                        ClothComingCost = third.ToString();
                                    }
                                    break;
                                case "мм":
                                    if (float.TryParse(_userWidth, out UserWidth) && float.TryParse(_userLength, out UserLength))
                                    {
                                        float first = UserLength * UserWidth;
                                        float second = (item.WidthOfCloth * 10) * (item.LengthOfCloth * 10);
                                        float third = (first / second) * item.CostOfCloth;
                                        ClothComingCost = third.ToString();
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public string UserLength
        {
            get => _userLength;
            set
            {
                if (_userLength != value)
                {
                    if (value.Any(x => !char.IsLetter(x)))
                    {
                        Set(ref _userLength, value);
                    }
                    else
                        Set(ref _userLength, "");
                }
                float UserLength, UserWidth;
                foreach (var item in Cloths)
                {
                    if (item.Articul == SelectedArticul)
                    {
                        switch (MeasurementUnit)
                        {
                            case "см":
                                if (float.TryParse(_userWidth, out UserWidth) && float.TryParse(_userLength, out UserLength))
                                {
                                    float first = UserLength * UserWidth;
                                    float second = item.WidthOfCloth * item.LengthOfCloth;
                                    float third = (first / second) * item.CostOfCloth;
                                    ClothComingCost = third.ToString();
                                }
                                break;
                            case "м":
                                if (float.TryParse(_userWidth, out UserWidth) && float.TryParse(_userLength, out UserLength))
                                {
                                    float first = UserLength * UserWidth;
                                    float second = (item.WidthOfCloth / 100) * (item.LengthOfCloth / 100);
                                    float third = (first / second) * item.CostOfCloth;
                                    ClothComingCost = third.ToString();
                                }
                                break;
                            case "дм":
                                if (float.TryParse(_userWidth, out UserWidth) && float.TryParse(_userLength, out UserLength))
                                {
                                    float first = UserLength * UserWidth;
                                    float second = (item.WidthOfCloth / 10) * (item.LengthOfCloth / 10);
                                    float third = (first / second) * item.CostOfCloth;
                                    ClothComingCost = third.ToString();
                                }
                                break;
                            case "мм":
                                if (float.TryParse(_userWidth, out UserWidth) && float.TryParse(_userLength, out UserLength))
                                {
                                    float first = UserLength * UserWidth;
                                    float second = (item.WidthOfCloth * 10) * (item.LengthOfCloth * 10);
                                    float third = (first / second) * item.CostOfCloth;
                                    ClothComingCost = third.ToString();
                                }
                                break;
                        }
                    }
                }
            }
        }
        public string ClothWidth { get => _clothWidth; set => Set(ref _clothWidth, value); }
        public string ClothLength { get => _clothLength; set => Set(ref _clothLength, value); }
        public string MeasurementUnit
        {
            get => _measurementUnit;
            set
            {
                Set(ref _measurementUnit, value);

                foreach (var item in Cloths)
                {
                    if (item.Articul == _selectedArticul)
                    {
                        switch (_measurementUnit)
                        {
                            case "см":
                                ClothLength = item.LengthOfClothAtStoreInSM.ToString();
                                ClothWidth = item.WidthOfClothAtStoreInSM.ToString();
                                break;
                            case "м":
                                ClothLength = (item.LengthOfClothAtStoreInSM / 100).ToString();
                                ClothWidth = (item.WidthOfClothAtStoreInSM / 100).ToString();
                                break;
                            case "дм":
                                ClothLength = (item.LengthOfClothAtStoreInSM / 10).ToString();
                                ClothWidth = (item.WidthOfClothAtStoreInSM / 10).ToString();
                                break;
                            case "мм":
                                ClothLength = (item.LengthOfClothAtStoreInSM * 10).ToString();
                                ClothWidth = (item.WidthOfClothAtStoreInSM * 10).ToString();
                                break;
                        }
                        UserLength = "";
                        UserWidth = "";
                    }
                }
            }
        }
        public string ClothComingCost { get => _clothComingCost; set => Set(ref _clothComingCost, value); }

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
            if (UserLength == "" || UserWidth == "" || UserLength.Any(x => char.IsLetter(x)) || UserWidth.Any(x => char.IsLetter(x)))
            {
                MessageBox.Show("Вы не ввели необходимые для добавления данные");
            }
            else
            {
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                try
                {
                    string sql = "update clothstore " +
                        "set ClothStore_Width = ClothStore_Width + @width, ClothStore_Length = ClothStore_Length + @length " +
                        "where ClothStore_Cloth_Articul = @articul;";

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = sql;
                    cmd.Connection = conn;

                    switch (MeasurementUnit)
                    {
                        case "см":
                            cmd.Parameters.AddWithValue("@width", float.Parse(UserWidth) / 100);
                            cmd.Parameters.AddWithValue("@length", float.Parse(UserLength) / 100);
                            break;
                        case "м":
                            cmd.Parameters.AddWithValue("@width", float.Parse(UserWidth));
                            cmd.Parameters.AddWithValue("@length", float.Parse(UserLength));
                            break;
                        case "мм":
                            cmd.Parameters.AddWithValue("@width", float.Parse(UserWidth) / 1000);
                            cmd.Parameters.AddWithValue("@length", float.Parse(UserLength) / 1000);
                            break;
                        case "дм":
                            cmd.Parameters.AddWithValue("@width", float.Parse(UserWidth) / 10);
                            cmd.Parameters.AddWithValue("@length", float.Parse(UserLength) / 10);
                            break;
                    }


                    cmd.Parameters.AddWithValue("@articul", SelectedArticul);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show($"На склад успешно добавлено {UserWidth} {MeasurementUnit} ширины и {UserLength} {MeasurementUnit} длины ткани {SelectedArticul}");
                    GetCloths();
                    foreach (var item in Cloths)
                    {
                        if (item.Articul == _selectedArticul)
                        {
                            ClothLength = item.LengthOfClothAtStoreInSM.ToString();
                            ClothWidth = item.WidthOfClothAtStoreInSM.ToString();
                            break;
                        }
                    }
                    UserLength = "";
                    UserWidth = "";
                    ClothComingCost = "";
                    MeasurementUnit = "см";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.ToString());
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        #endregion

        #region Команда добавления фурнитуры

        public ICommand FurnituresComingCommand { get; }

        private bool CanFurnituresComingCommandExecute(object parameter) => true;
        private void OnFurnituresComingCommandExecuted(object parameter)
        {
            if (UserQuantity == "" || UserQuantity.Any(x => char.IsLetter(x)))
            {
                MessageBox.Show("Не введены данные для добавления");
            }
            else
            {
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                try
                {
                    string sql = "update furniturestore " +
                        "set FurnitureStore_Quantity = FurnitureStore_Quantity + @quantity " +
                        "where FurnitureStore_Furniture_Articul = @articul;";

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = sql;
                    cmd.Connection = conn;

                    cmd.Parameters.AddWithValue("@quantity", int.Parse(UserQuantity));
                    cmd.Parameters.AddWithValue("@articul", SelectedArticuleOfFurniture);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show($"На склад успешно добавлено {UserQuantity} единиц фурнитуры {SelectedArticuleOfFurniture}");
                    GetFurnitures();
                    foreach (var item in Furnitures)
                    {
                        if (item.Articul == _selectedArticulOfFurniture)
                        {
                            FurnitureQuantity = item.Quantity.ToString();
                            break;
                        }
                    }
                    UserQuantity = "";
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
        }

        #endregion

        #endregion

        public MaterialComingViewModel()
        {
            GetFurnitures();
            GetCloths();
            GetClothsArticuls();
            GetFurnituresArticuls();
            SelectedArticul = ClothsArticuls[0];
            SelectedArticuleOfFurniture = FurnituresArticuls[0];

            #region Команды

            ClothComingCommand = new LambdaCommand(OnClothComingCommandExecuted, CanClothComingCommandExecute);
            FurnituresComingCommand = new LambdaCommand(OnFurnituresComingCommandExecuted, CanFurnituresComingCommandExecute);

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
                    "cs.ClothStore_Width, cs.ClothStore_Length " +
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
                            WidthOfClothAtStoreInSM = reader.GetFloat(6) * 100,
                            LengthOfClothAtStoreInSM = reader.GetFloat(7) * 100,
                            CostOfAllCloth = (((reader.GetFloat(6) * 100) * (reader.GetFloat(7) * 100)) / (reader.GetFloat(2) * reader.GetFloat(3))) * reader.GetFloat(4)
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
