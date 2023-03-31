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
    internal class MaterialWriteOffViewModel : ViewModel
    {

        #region Коллекции элементов

        private string _measurementUnit = "см";
        public string MeasurementUnit { get => _measurementUnit; set => Set(ref _measurementUnit, value); }

        private ObservableCollection<string> _measurementUnits = new ObservableCollection<string>() { "см", "м", "дм", "мм" };
        public ObservableCollection<string> MeasurementUnits { get => _measurementUnits; set => Set(ref _measurementUnits, value); }

        private ObservableCollection<string> _clothsArticuls = new ObservableCollection<string>();
        public ObservableCollection<string> ClothsArticuls { get => _clothsArticuls; set => Set(ref _clothsArticuls, value); }

        private ObservableCollection<ClothStore> _cloths = new ObservableCollection<ClothStore>();
        public ObservableCollection<ClothStore> Cloths { get => _cloths; set => Set(ref _cloths, value); }

        #endregion

        #region Данные для списания

        private string _selectedArticul;
        private string _userWidth = "";
        private string _userLength = "";
        private string _clothWidth;
        private string _clothLength;

        public string SelectedArticul 
        { 
            get => _selectedArticul;
            set 
            { 
                Set(ref _selectedArticul, value); 
                foreach (var item in Cloths)
                {
                    if (item.Articul == _selectedArticul)
                    {
                        ClothLength = item.LengthOfClothAtStoreInSM.ToString();
                        ClothWidth = item.WidthOfClothAtStoreInSM.ToString();
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
            }
        }
        public string ClothWidth { get => _clothWidth; set => Set(ref _clothWidth, value); }
        public string ClothLength { get => _clothLength; set => Set(ref _clothLength, value); } 

        #endregion

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Команды

        #region Команда списания ткани

        public ICommand ClothWriteOffCommand { get; }

        private bool CanClothWriteOffCommandExecute(object parameter) => true;
        private void OnClothWriteOffCommandExecuted(object parameter)
        {
            if (UserLength == "" || UserWidth == "" || UserLength.Any(x => char.IsLetter(x)) || UserWidth.Any(x => char.IsLetter(x)))
            {
                MessageBox.Show("Вы не ввели необходимые для списания данные");
            }
            else
            {
                if (float.Parse(UserLength) > float.Parse(ClothLength) ||
                    float.Parse(UserWidth) > float.Parse(ClothWidth))
                {
                    MessageBox.Show("На складе нет столько ткани для списания");
                }
                else
                {
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    try
                    {
                        string sql = "update clothstore " +
                            "set ClothStore_Width = ClothStore_Width - @width, ClothStore_Length = ClothStore_Length - @length " +
                            "where ClothStore_Cloth_Articul = @articul;";

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.CommandText = sql;
                        cmd.Connection = conn;

                        cmd.Parameters.AddWithValue("@width", float.Parse(UserWidth) / 100);
                        cmd.Parameters.AddWithValue("@length", float.Parse(UserLength) / 100);
                        cmd.Parameters.AddWithValue("@articul", SelectedArticul);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"Со склада успешно списано {UserWidth}{MeasurementUnit} ширины и {UserLength}{MeasurementUnit} длины ткани {SelectedArticul}");
                        GetCloths();
                        foreach (var item in Cloths)
                        {
                            if (item.Articul == _selectedArticul)
                            {
                                ClothLength = item.LengthOfClothAtStoreInSM.ToString();
                                ClothWidth = item.WidthOfClothAtStoreInSM.ToString();
                            }
                        }
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
        }

        #endregion

        #endregion

        public MaterialWriteOffViewModel()
        {
            GetCloths();
            GetArticuls();
            SelectedArticul = ClothsArticuls[0];

            #region Команды

            ClothWriteOffCommand = new LambdaCommand(OnClothWriteOffCommandExecuted, CanClothWriteOffCommandExecute);

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

        private void GetArticuls()
        {
            ClothsArticuls.Clear();
            foreach (var item in Cloths)
            {
                ClothsArticuls.Add(item.Articul);
            }
        }

    }
}
