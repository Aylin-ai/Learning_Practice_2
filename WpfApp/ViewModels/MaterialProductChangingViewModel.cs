using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.Models;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels
{
    internal class MaterialProductChangingViewModel : ViewModel
    {

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Коллекции элементов

        private ObservableCollection<ClothStore> _clothsAtStore = new ObservableCollection<ClothStore>();
        public ObservableCollection<ClothStore> ClothsAtStore { get => _clothsAtStore; set => Set(ref _clothsAtStore, value); }

        private ObservableCollection<FurnitureStore> _furnituresAtStore = new ObservableCollection<FurnitureStore>();
        public ObservableCollection<FurnitureStore> FurnituresAtStore { get => _furnituresAtStore; set => Set(ref _furnituresAtStore, value); }

        private ObservableCollection<ProductStore> _productsAtStore = new ObservableCollection<ProductStore>();
        public ObservableCollection<ProductStore> ProductsAtStore { get => _productsAtStore; set => Set(ref _productsAtStore, value); }

        private ObservableCollection<HistoryOfChangesAtStore> _historyOfChangesAtStore = new ObservableCollection<HistoryOfChangesAtStore>();
        public ObservableCollection<HistoryOfChangesAtStore> HistoryOfChangesAtStore { get => _historyOfChangesAtStore; set => Set(ref _historyOfChangesAtStore, value); }

        #endregion

        #region Промежуток времени

        private DateTime _selectedFirstDate = DateTime.Now;
        public DateTime SelectedFirstDate
        {
            get => _selectedFirstDate;
            set
            {
                Set(ref _selectedFirstDate, value);
            }
        }

        private DateTime _selectedSecondDate = DateTime.Now;
        public DateTime SelectedSecondDate
        {
            get => _selectedSecondDate;
            set
            {
                Set(ref _selectedSecondDate, value);
            }
        }

        #endregion

        #region Данные об изменениях

        private float _oldQuantity;
        public float OldQuantity { get => _oldQuantity; set => Set(ref _oldQuantity, value); }

        private float _newQuantity;
        public float NewQuantity { get => _newQuantity; set => Set(ref _newQuantity, value); }

        #endregion

        #region Выбор пользователя

        private ClothStore _selectedCloth;
        public ClothStore SelectedCloth
        {
            get => _selectedCloth;
            set
            {
                Set(ref _selectedCloth, value);
                OldQuantity = 0;
                NewQuantity = 0;
                GetChangesAtStore("ткань", _selectedCloth.Articul, SelectedFirstDate.ToString("yyyy-MM-dd HH:mm:ss"), SelectedSecondDate.ToString("yyyy-MM-dd HH:mm:ss"));
                if (HistoryOfChangesAtStore.Count > 0)
                {
                    OldQuantity = HistoryOfChangesAtStore[0].OldQuantity;
                    NewQuantity = HistoryOfChangesAtStore[^1].NewQuantity;
                }
            }
        }

        private FurnitureStore _selectedFurniture;
        public FurnitureStore SelectedFurniture
        {
            get => _selectedFurniture;
            set
            {
                Set(ref _selectedFurniture, value);
                OldQuantity = 0;
                NewQuantity = 0;
                GetChangesAtStore("фурнитура", _selectedFurniture.Articul, SelectedFirstDate.ToString("yyyy-MM-dd HH:mm:ss"), SelectedSecondDate.ToString("yyyy-MM-dd HH:mm:ss"));
                if (HistoryOfChangesAtStore.Count > 0)
                {
                    OldQuantity = HistoryOfChangesAtStore[0].OldQuantity;
                    NewQuantity = HistoryOfChangesAtStore[^1].NewQuantity;
                }
            }
        }

        private ProductStore _selectedProduct;
        public ProductStore SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                Set(ref _selectedProduct, value);
                OldQuantity = 0;
                NewQuantity = 0;
                GetChangesAtStore("изделие", _selectedProduct.Articul, SelectedFirstDate.ToString("yyyy-MM-dd HH:mm:ss"), SelectedSecondDate.ToString("yyyy-MM-dd HH:mm:ss"));
                if (HistoryOfChangesAtStore.Count > 0)
                {
                    OldQuantity = HistoryOfChangesAtStore[0].OldQuantity;
                    NewQuantity = HistoryOfChangesAtStore[^1].NewQuantity;
                }
            }
        }

        #endregion

        #region Команды



        #endregion

        public MaterialProductChangingViewModel()
        {
            GetCloths();
            GetFurnitures();
            GetProducts();

            #region Команды



            #endregion
        }

        private void GetCloths()
        {
            ClothsAtStore.Clear();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                string sql = "select cloth.Cloth_Image, clothstore.ClothStore_Cloth_Articul, cloth.Cloth_Name, " +
                    "cloth.`Cloth_Width(cm)`, cloth.`Cloth_Length(cm)`, cloth.`Cloth_Cost(rub)`, " +
                    "clothstore.ClothStore_WidthOfRoll, clothstore.ClothStore_LengthOfRoll " +
                    "from clothstore inner join cloth on " +
                    "clothstore.ClothStore_Cloth_Articul = cloth.Cloth_Articul;";
                cmd.CommandText = sql;

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ClothsAtStore.Add(new ClothStore()
                        {
                            Image = reader.GetString(0),
                            Articul = reader.GetString(1),
                            Name = reader.GetString(2),
                            WidthOfCloth = reader.GetFloat(3),
                            LengthOfCloth = reader.GetFloat(4),
                            CostOfCloth = reader.GetFloat(5),
                            WidthOfClothAtStore = reader.GetFloat(6),
                            LengthOfClothAtStore = reader.GetFloat(7),
                            CostOfAllCloth = (reader.GetFloat(6) * reader.GetFloat(7)) / (reader.GetFloat(3) * reader.GetFloat(4)) * reader.GetFloat(5),
                        });
                    }
                }
                reader.Close();
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

        private void GetFurnitures()
        {
            FurnituresAtStore.Clear();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                string sql = "SELECT f.Furniture_Image, f.Furniture_Articul, f.Furniture_Name, " +
                    "f.Furniture_Cost, fs.FurnitureStore_Quantity " +
                    "from furniture f " +
                    "inner join furniturestore fs " +
                    "on f.Furniture_Articul = fs.FurnitureStore_Furniture_Articul;";
                cmd.CommandText = sql;

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FurnituresAtStore.Add(new FurnitureStore()
                        {
                            Image = reader.GetString(0),
                            Articul = reader.GetString(1),
                            Name = reader.GetString(2),
                            Cost = reader.GetFloat(3),
                            Quantity = reader.GetInt32(4),
                            CostOfAllFurniture = reader.GetFloat(3) * reader.GetInt32(4)
                        });
                    }
                }
                reader.Close();
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

        private void GetProducts()
        {
            ProductsAtStore.Clear();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                    string sql = "select product.Product_Image, productstore.ProductStore_Product_Articul, product.Product_Name, " +
                    "product.`Product_Cost(rub)`, productstore.ProductStore_Quantity " +
                    "from product inner join productstore " +
                    "on product.Product_Articul = productstore.ProductStore_Product_Articul;";
                    cmd.CommandText = sql;

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ProductsAtStore.Add(new ProductStore()
                            {
                                Image = reader.GetString(0),
                                Articul = reader.GetString(1),
                                Name = reader.GetString(2),
                                Cost = reader.GetFloat(3),
                                QuantityAtStore = reader.GetInt32(4),
                                CostOfAllProducts = reader.GetInt32(4) * reader.GetFloat(3)
                            });
                        }
                    }
                    reader.Close();     
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

        private void GetChangesAtStore(string type, string articul, string date1, string date2)
        {
            HistoryOfChangesAtStore.Clear();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                string sql = "select * from historyofchangingmaterialquantity " +
                    "where HistoryOfChangingMaterialQuantity_Type = @type and " +
                    "HistoryOfChangingMaterialQuantity_Articul = @articul and " +
                    "HistoryOfChangingMaterialQuantity_DateTimeOfChanging > @date1 and " +
                    "HistoryOfChangingMaterialQuantity_DateTimeOfChanging < @date2 " +
                    "order by HistoryOfChangingMaterialQuantity_DateTimeOfChanging;";
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@articul", articul);
                cmd.Parameters.AddWithValue("@date1", date1);
                cmd.Parameters.AddWithValue("@date2", date2);

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        HistoryOfChangesAtStore.Add(new HistoryOfChangesAtStore()
                        {
                            Type = reader.GetString(1),
                            Articul = reader.GetString(2),
                            OldQuantity = reader.GetFloat(3),
                            NewQuantity = reader.GetFloat(3) + reader.GetFloat(4),
                            DateTimeOfChanges = reader.GetMySqlDateTime(5),
                        });
                    }
                }
                reader.Close();
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
}
