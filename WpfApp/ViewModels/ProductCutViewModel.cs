using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WpfApp.Models;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels
{
    internal class ProductCutViewModel : ViewModel
    {

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Коллекции элементов

        private ObservableCollection<ClothRollToCut> _productsOnCloth = new ObservableCollection<ClothRollToCut>();
        public ObservableCollection<ClothRollToCut> ProductsOnCloth { get => _productsOnCloth; set => Set(ref _productsOnCloth, value); }

        private ObservableCollection<ProductToCut> _productsInRoll = new ObservableCollection<ProductToCut>();
        public ObservableCollection<ProductToCut> ProductsInRoll{ get => _productsInRoll; set => Set(ref _productsInRoll, value); }


        #endregion

        #region Выбор пользователя

        private float _width;
        public float Width { get => _width; set => Set(ref _width, value); }

        private float _length;
        public float Length { get => _length; set => Set(ref _length, value); }

        private ClothRollToCut _selectedRoll;
        public ClothRollToCut SelectedRoll
        {
            get => _selectedRoll;
            set
            {
                Set(ref _selectedRoll, value);
                Width = _selectedRoll.WidthOfRoll;
                Length = _selectedRoll.LengthOfRoll;
                ProductsInRoll = _selectedRoll.ProductsToCut;
            }
        }

        #endregion

        #region Команды



        #endregion

        public ProductCutViewModel()
        {
            GetProductsAtSelectedOrder();

            #region Команды



            #endregion
        }

        private void GetProductsAtSelectedOrder()
        {
            GetRollOfCloth();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                string sql = "select cp.ClothProduct_Cloth_Articul, gop.GeneralOrderProduct_Product_Articul, " +
                    "p.Product_Name, p.Product_Image, p.`Product_Width(cm)`, p.`Product_Length(cm)`, gop.GeneralOrderProduct_Quantity " +
                    "from generalorderproduct gop inner join product p on gop.GeneralOrderProduct_Product_Articul = p.Product_Articul " +
                    "inner join clothproduct cp on gop.GeneralOrderProduct_Product_Articul = cp.ClothProduct_Product_Articul " +
                    "where cp.ClothProduct_Cloth_Articul = @clothArticul " +
                    "order by (p.`Product_Width(cm)` * p.`Product_Length(cm)`) desc;";
                cmd.CommandText = sql;

                for (int i = 0; i < ProductsOnCloth.Count; i++)
                {
                    cmd.Parameters.AddWithValue("@clothArticul", ProductsOnCloth[i].ClothArticul);
                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            for (int j = 0; j < reader.GetInt32(6); j++)
                            {
                                ProductsOnCloth[i].ProductsToCut.Add(new ProductToCut()
                                {
                                    Articul = reader.GetString(1),
                                    Name = reader.GetString(2),
                                    Image = reader.GetString(3),
                                    Width = reader.GetFloat(4),
                                    Length = reader.GetFloat(5),
                                });
                            }
                        }
                    }
                    ProductsOnCloth[i].ProductQuantity = ProductsOnCloth[i].ProductsToCut.Count;
                    cmd.Parameters.Clear();
                    reader.Close();
                    foreach (var product in ProductsOnCloth[i].ProductsToCut.Where(x => x.Length <= ProductsOnCloth[i].WidthOfRoll))
                    {
                        float swapVar = product.Length;
                        product.Length = product.Width;
                        product.Width = swapVar;
                    }

                    for (int j = 0; j < ProductsOnCloth[i].ProductsToCut.Count; j++)
                    {
                        if (j == 0)
                        {
                            ProductsOnCloth[i].ProductsToCut[j].X = 0;
                            ProductsOnCloth[i].ProductsToCut[j].Y = 0;
                        }
                        else
                        {
                            if (ProductsOnCloth[i].ProductsToCut[j - 1].Y + ProductsOnCloth[i].ProductsToCut[j - 1].Width + ProductsOnCloth[i].ProductsToCut[j].Width >= ProductsOnCloth[i].WidthOfRoll)
                            {
                                ProductsOnCloth[i].ProductsToCut[j].X += ProductsOnCloth[i].ProductsToCut[j - 1].Length + ProductsOnCloth[i].ProductsToCut[j - 1].X;
                                //ProductsInOrder[i].Y += ProductsInOrder[i - 1].Width + ProductsInOrder[i - 1].Y;
                            }
                            else
                            {
                                ProductsOnCloth[i].ProductsToCut[j].X = ProductsOnCloth[i].ProductsToCut[j - 1].X;
                                ProductsOnCloth[i].ProductsToCut[j].Y = ProductsOnCloth[i].ProductsToCut[j - 1].Y + ProductsOnCloth[i].ProductsToCut[j - 1].Width;
                            }
                        }
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

        private void GetRollOfCloth()
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                string sql = "select c.Cloth_Image, cp.ClothProduct_Cloth_Articul, " +
                    "cs.ClothStore_WidthOfRoll, cs.ClothStore_LengthOfRoll, gop.GeneralOrderProduct_Quantity " +
                    "from generalorderproduct gop inner join clothproduct cp " +
                    "on gop.GeneralOrderProduct_Product_Articul = cp.ClothProduct_Product_Articul " +
                    "inner join clothstore cs on cp.ClothProduct_Cloth_Articul = cs.ClothStore_Cloth_Articul " +
                    "inner join cloth c on c.Cloth_Articul = cs.ClothStore_Cloth_Articul " +
                    "group by cs.ClothStore_Cloth_Articul;";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductsOnCloth.Add(new ClothRollToCut()
                        {
                            ClothImage = reader.GetString(0),
                            ClothArticul = reader.GetString(1),
                            WidthOfRoll = reader.GetFloat(2),
                            LengthOfRoll = reader.GetFloat(3),
                            ProductQuantity = reader.GetInt32(4),
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
    }
}
