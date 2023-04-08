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
using WpfApp.Views;

namespace WpfApp.ViewModels
{
    internal class OrdersViewModel : ViewModel
    {

        #region Данные менеджера

        private string _managerLogin;
        public string ManagerLogin { get => _managerLogin; set => Set(ref _managerLogin, value); }

        #endregion

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Коллекции элементов

        private ObservableCollection<ProductInOrder> _productsInOrder = new ObservableCollection<ProductInOrder>();
        public ObservableCollection<ProductInOrder> ProductsInOrder { get => _productsInOrder; set => Set(ref _productsInOrder, value); }

        private ObservableCollection<Order> _orders = new ObservableCollection<Order>();
        public ObservableCollection<Order> Orders { get => _orders; set => Set(ref _orders, value); }

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
                    OrderPhaseHandler(_selectedOrder.OrderId);
                }

            }
        }


        #endregion

        #region Команды

        #region Команда подтверждения заказа

        public ICommand OrderConfirmCommand { get; }

        private bool CanOrderConfirmCommandExecute(object parameter) => true;
        private async void OnOrderConfirmCommandExecuted(object parameter)
        {
            if (parameter == null)
            {
                MessageBox.Show("Вы не выбрали заказ");
            }
            else
            {
                Order order = (Order)parameter;
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                try
                {
                    string sql = "update generalorder " +
                        "set GeneralOrder_Phase = 'К оплате' " +
                        "where GeneralOrder_Id = @orderId;";

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = sql;
                    cmd.Connection = conn;

                    cmd.Parameters.AddWithValue("@orderId", order.OrderId);

                    await cmd.ExecuteNonQueryAsync();
                    GetOrders();
                    ProductsInOrder.Clear();
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

        #region Команда отклонения заказа

        public ICommand OrderDenyCommand { get; }

        private bool CanOrderDenyCommandExecute(object parameter) => true;
        private async void OnOrderDenyCommandExecuted(object parameter)
        {
            if (parameter == null)
            {
                MessageBox.Show("Вы не выбрали заказ");
            }
            else
            {
                Order order = (Order)parameter;
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                try
                {
                    string sql = "update generalorder " +
                        "set GeneralOrder_Phase = 'Отклонен' " +
                        "where GeneralOrder_Id = @orderId;";

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = sql;
                    cmd.Connection = conn;

                    cmd.Parameters.AddWithValue("@orderId", order.OrderId);

                    await cmd.ExecuteNonQueryAsync();
                    GetOrders();
                    ProductsInOrder.Clear();
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

        #region Команда перехода на страницу раскроя

        public ICommand ProductCutWindowCommand { get; }

        private bool CanProductCutWindowCommandExecute(object parameter) => true;
        private void OnProductCutWindowCommandExecuted(object parameter)
        {
            ProductCut productCut = new ProductCut();
            productCut.Show();
        }

        #endregion

        #endregion

        public OrdersViewModel(string login)
        {
            ManagerLogin = login;
            GetOrders();

            #region Команды

            OrderConfirmCommand = new LambdaCommand(OnOrderConfirmCommandExecuted, CanOrderConfirmCommandExecute);
            OrderDenyCommand = new LambdaCommand(OnOrderDenyCommandExecuted, CanOrderDenyCommandExecute);
            ProductCutWindowCommand = new LambdaCommand(OnProductCutWindowCommandExecuted, CanProductCutWindowCommandExecute);

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
                    "where GeneralOrder_Phase != 'Отклонен' " +
                    "and (GeneralOrder_Manager_UserInformation_Login = 'TestManager' " +
                    "or GeneralOrder_Manager_UserInformation_Login = @manager);";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@manager", ManagerLogin);

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
                        ProductsInOrder.Add(new ProductInOrder()
                        {
                            OrderId = reader.GetInt32(0),
                            ProductArticul = reader.GetString(1),
                            ProductName = reader.GetString(2),
                            ProductImage = reader.GetString(3),
                            ProductQuantity = reader.GetInt32(4),
                            ProductPriceXQuantity = reader.GetFloat(5),
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

        private async void OrderPhaseHandler(int orderId)
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                string sql = "update generalorder set GeneralOrder_Phase = 'Обработка', " +
                    "GeneralOrder_Manager_UserInformation_Login = @login " +
                    "where GeneralOrder_Id = @orderId";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@orderId", orderId);
                cmd.Parameters.AddWithValue("@login", ManagerLogin);

                var reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductsInOrder.Add(new ProductInOrder()
                        {
                            OrderId = reader.GetInt32(0),
                            ProductArticul = reader.GetString(1),
                            ProductName = reader.GetString(2),
                            ProductImage = reader.GetString(3),
                            ProductQuantity = reader.GetInt32(4),
                            ProductPriceXQuantity = reader.GetFloat(5),
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
