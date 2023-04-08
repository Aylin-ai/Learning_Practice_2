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
    internal class FurnitureCostAssessmentViewModel : ViewModel
    {

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Коллекции элементов

        private ObservableCollection<ProductInOrder> _productsInOrder = new ObservableCollection<ProductInOrder>();
        public ObservableCollection<ProductInOrder> ProductsInOrder { get => _productsInOrder; set => Set(ref _productsInOrder, value); }

        private ObservableCollection<Order> _orders = new ObservableCollection<Order>();
        public ObservableCollection<Order> Orders { get => _orders; set => Set(ref _orders, value); }

        private ObservableCollection<FurnitureInProduct> _furnituresInProduct = new ObservableCollection<FurnitureInProduct>();
        public ObservableCollection<FurnitureInProduct> FurnituresInProduct { get => _furnituresInProduct; set => Set(ref _furnituresInProduct, value); }

        #endregion

        #region Данные о выборе пользователя

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                Set(ref _selectedOrder, value);
                if (_selectedOrder != null)
                {
                    GetProductsAtSelectedOrder(_selectedOrder.OrderId);
                }
            }
        }

        private ProductInOrder _selectedItem;
        public ProductInOrder SelectedItem
        {
            get => _selectedItem;
            set
            {
                Set(ref _selectedItem, value);
                if (_selectedItem != null)
                {
                    GetProductSpecification(_selectedItem);
                }
            }
        }

        #endregion

        #region Команды



        #endregion

        public FurnitureCostAssessmentViewModel()
        {
            GetOrders();

            #region Команды



            #endregion
        }

        private void GetOrders()
        {
            Orders.Clear();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                string sql = "SELECT * FROM generalorder " +
                    "where GeneralOrder_Phase != 'Отклонен';";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Orders.Add(new Order()
                        {
                            OrderId = reader.GetInt32(0),
                            OrderCreationDate = reader.GetMySqlDateTime(1),
                            OrderPhase = reader.GetString(2),
                            OrderCustomer = reader.GetString(3),
                            OrderManager = reader.GetString(4),
                            OrderCost = reader.GetFloat(5),
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

        private async void GetProductsAtSelectedOrder(int orderId)
        {
            ProductsInOrder.Clear();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                string sql = "select gop.GeneralOrderProduct_GeneralOrder_Id, gop.GeneralOrderProduct_Product_Articul, " +
                    "p.Product_Name, p.Product_Image, gop.GeneralOrderProduct_Quantity, gop.GeneralOrderProduct_CostOfAllProfucts " +
                    "from generalorderproduct gop inner join product p on gop.GeneralOrderProduct_Product_Articul = p.Product_Articul " +
                    "where gop.GeneralOrderProduct_GeneralOrder_Id = @orderId;";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@orderId", orderId);

                var reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.GetInt32(4); i++)
                        {
                            ProductsInOrder.Add(new ProductInOrder()
                            {
                                OrderId = reader.GetInt32(0),
                                ProductArticul = reader.GetString(1),
                                ProductName = reader.GetString(2),
                                ProductImage = reader.GetString(3),
                            });
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

        private void GetProductSpecification(ProductInOrder product)
        {
            FurnituresInProduct.Clear();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@articul", product.ProductArticul);

                string sqlFurnitures = "select furniture.Furniture_Image, furniture.Furniture_Articul, furniture.Furniture_Name, " +
                    "furnitureproduct.FurnitureProduct_Length, furnitureproduct.FurnitureProduct_Width, " +
                    "furnitureproduct.FurnitureProduct_Placing, furnitureproduct.FurnitureProduct_Rotation, " +
                    "furnitureproduct.FurnitureProduct_Quantity, furniture.Furniture_Cost, " +
                    "furniturestore.FurnitureStore_Quantity " +
                    "from furnitureproduct inner join furniture " +
                    "on furnitureproduct.FurnitureProduct_Furniture_Articul = furniture.Furniture_Articul " +
                    "inner join furniturestore on furniturestore.FurnitureStore_Furniture_Articul = furniture.Furniture_Articul " +
                    "where furnitureproduct.FurnitureProduct_Product_Articul = @articul;";
                cmd.CommandText = sqlFurnitures;

                var readerOfFurnitures = cmd.ExecuteReader();

                if (readerOfFurnitures.HasRows)
                {
                    while (readerOfFurnitures.Read())
                    {
                        FurnituresInProduct.Add(new FurnitureInProduct()
                        {
                            Image = readerOfFurnitures.GetString(0),
                            Articul = readerOfFurnitures.GetString(1),
                            Name = readerOfFurnitures.GetString(2),
                            Length = readerOfFurnitures.GetFloat(3),
                            Width = readerOfFurnitures.GetFloat(4),
                            Quantity = readerOfFurnitures.GetInt32(7),
                            Cost = readerOfFurnitures.GetFloat(8) * readerOfFurnitures.GetInt32(7),
                            QuantityAtStore = readerOfFurnitures.GetInt32(9),
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
