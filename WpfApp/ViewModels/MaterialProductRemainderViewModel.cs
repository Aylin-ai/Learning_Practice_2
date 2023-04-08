using MySql.Data.MySqlClient;
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
    internal class MaterialProductRemainderViewModel : ViewModel
    {

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Коллекции элементов

        private ObservableCollection<ProductStore> _productsAtStore = new ObservableCollection<ProductStore>();
        public ObservableCollection<ProductStore> ProductsAtStore { get => _productsAtStore; set => Set(ref  _productsAtStore, value); }

        private ObservableCollection<ClothStore> _clothsAtStore = new ObservableCollection<ClothStore>();
        public ObservableCollection<ClothStore> ClothsAtStore { get => _clothsAtStore; set => Set(ref _clothsAtStore, value); }

        private ObservableCollection<FurnitureStore> _furnituresAtStore = new ObservableCollection<FurnitureStore>();
        public ObservableCollection<FurnitureStore> FurnituresAtStore { get => _furnituresAtStore; set => Set(ref _furnituresAtStore, value); }

        private ObservableCollection<string> _customers = new ObservableCollection<string>() { "" };
        public ObservableCollection<string> Customers { get => _customers; set => Set(ref _customers, value); }

        #endregion

        #region Данные о выборе пользователя

        private string _selectedUser = "";
        public string SelectedUser 
        {
            get => _selectedUser;
            set 
            { 
                Set(ref _selectedUser, value);
                GetProducts();
            } 
        }

        #endregion

        #region Команды



        #endregion

        public MaterialProductRemainderViewModel()
        {
            GetProducts();
            GetCloths();
            GetFurnitures();
            GetCustomers();

            #region Команды



            #endregion
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
                if (SelectedUser == "")
                {
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
                else
                {
                    string sql = "select product.Product_Image, generalorderproduct.GeneralOrderProduct_Product_Articul, " +
                        "product.Product_Name, product.`Product_Cost(rub)`, generalorderproduct.GeneralOrderProduct_Quantity " +
                        "from generalorderproduct inner join generalorder " +
                        "on generalorderproduct.GeneralOrderProduct_GeneralOrder_Id = generalorder.GeneralOrder_Id " +
                        "inner join product on product.Product_Articul = generalorderproduct.GeneralOrderProduct_Product_Articul " +
                        "where generalorder.GeneralOrder_Customer_UserInformation_Login = @user and " +
                        "generalorder.GeneralOrder_Phase != 'Отклонено';";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@user", SelectedUser);

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
                    "cloth.`Cloth_Width(cm)`, cloth.`Cloth_Length(cm)`, cloth.`Cloth_Cost(rub)`, clothstore.ClothStore_WidthOfRoll, clothstore.ClothStore_LengthOfRoll " +
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

        private void GetCustomers()
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                string sql = "select UserInformation_Login from userinformation " +
                    "where UserInformation_UserRole_Id = 1;";
                cmd.CommandText = sql;

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Customers.Add(reader.GetString(0));
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
