using MySql.Data.MySqlClient;
using System;
using System.CodeDom;
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
    internal class MakeOrderViewModel : ViewModel
    {

        #region Данные заказчика

        private string _userLogin;
        public string UserLogin { get => _userLogin; set => Set(ref  _userLogin, value); }  

        #endregion

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Булевые данные для возможности добавлять товары

        private bool _isStackPanel1Enabled = true;
        private bool _isStackPanel2Enabled = false;
        private bool _isComboBoxOfProductsEnabled = true;
        private bool _isAddProductButtonEnabled = true;
        private bool _isUserQuantityTextBoxEnabled = true;

        public bool IsStackPanel1Enabled { get => _isStackPanel1Enabled; set => Set(ref  _isStackPanel1Enabled, value); }
        public bool IsStackPanel2Enabled { get => _isStackPanel2Enabled; set => Set(ref _isStackPanel2Enabled, value); }
        public bool IsComboBoxOfProductsEnabled { get => _isComboBoxOfProductsEnabled; set => Set(ref _isComboBoxOfProductsEnabled, value); }
        public bool IsAddProductButtonEnabled { get => _isAddProductButtonEnabled; set => Set(ref _isAddProductButtonEnabled, value); } 
        public bool IsUserQuantityTextBoxEnabled { get => _isUserQuantityTextBoxEnabled; set => Set(ref _isUserQuantityTextBoxEnabled, value); } 

        #endregion

        #region Данные товара

        private string _productImage = "";
        private string _productArticul = "";
        private string _productName = "";
        private string _productWidth = "";
        private string _productLength = "";
        private string _productCost = "";
        private string _userQuantity = "";

        public string ProductImage { get => _productImage; set => Set(ref _productImage, value); }
        public string ProductArticul { get => _productArticul; set => Set(ref _productArticul, value); }
        public string ProductName { get => _productName; set => Set(ref _productName, value); }
        public string ProductWidth { get => _productWidth; set => Set(ref _productWidth, value); }
        public string ProductLength { get => _productLength; set => Set(ref _productLength, value); }
        public string ProductCost { get => _productCost; set => Set(ref _productCost, value); }
        public string UserQuantity { get => _userQuantity; set => Set(ref _userQuantity, value); }

        #endregion

        #region Коллекции элементов

        private ObservableCollection<ProductInOrder> _productsInOrder = new ObservableCollection<ProductInOrder>();
        public ObservableCollection<ProductInOrder> ProductsInOrder { get => _productsInOrder; set => Set(ref _productsInOrder, value); }

        private ObservableCollection<Product> _products = new ObservableCollection<Product>();
        public ObservableCollection<Product> Products { get => _products; set => Set(ref _products, value); }

        private ObservableCollection<string> _productsNames = new ObservableCollection<string>();
        public ObservableCollection<string> ProductsNames { get => _productsNames; set => Set(ref _productsNames, value); }

        private ObservableCollection<string> _managers = new ObservableCollection<string>();
        public ObservableCollection<string> Managers { get => _managers; set => Set(ref _managers, value); }

        #endregion

        #region Данные о выборе пользователя

        private string _selectedProductName;
        public string SelectedProductName
        {
            get => _selectedProductName;
            set
            {
                Set(ref _selectedProductName, value);
                foreach (var product in Products)
                {
                    if ($"{product.Name}({product.Articul})" == _selectedProductName)
                    {
                        ProductImage = product.Image;
                        ProductName = product.Name;
                        ProductArticul = product.Articul;
                        ProductWidth = product.Width.ToString();
                        ProductLength = product.Length.ToString();
                        ProductCost = product.Cost.ToString();
                    }
                }
            }
        }

        private ProductInOrder _selectedProduct;
        public ProductInOrder SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                Set(ref _selectedProduct, value);
                if (_selectedProduct != null)
                {
                    foreach (var product in Products)
                    {
                        if (_selectedProduct.ProductArticul == product.Articul)
                        {
                            ProductImage = product.Image;
                            ProductName = product.Name;
                            ProductArticul = product.Articul;
                            ProductWidth = product.Width.ToString();
                            ProductLength = product.Length.ToString();
                            ProductCost = product.Cost.ToString();
                            SelectedProductName = $"{product.Name}({product.Articul})";
                            IsStackPanel2Enabled = true;
                            IsAddProductButtonEnabled = false;
                            IsComboBoxOfProductsEnabled = false;
                            IsUserQuantityTextBoxEnabled = false;
                        }
                    }
                }
            }
        }

        #endregion

        #region Команды

        #region Команда перехода к добавлению товара

        public ICommand ToAddProductCommand { get; }

        private bool CanToAddProductCommandExecute(object parameter) => true;
        private void OnToAddProductCommandExecuted(object parameter)
        {
            IsStackPanel1Enabled = false;
            IsStackPanel2Enabled = true;
            IsAddProductButtonEnabled = true;
            IsComboBoxOfProductsEnabled = true;
            IsUserQuantityTextBoxEnabled = true;
        }

        #endregion

        #region Команда добавления товара

        public ICommand AddProductCommand { get; }

        private bool CanAddProductCommandExecute(object parameter) => true;
        private void OnAddProductCommandExecuted(object parameter)
        {
            if (SelectedProductName == null)
            {
                MessageBox.Show("Вы не выбрали товар");
            }
            else
            {
                if (UserQuantity == "" || UserQuantity.Any(x => char.IsLetter(x)))
                {
                    MessageBox.Show("Данных о количестве товара нет либо они находятся в неправильном формате");
                }
                else
                {
                    if (ProductsInOrder.Count != 0)
                    {
                        foreach (var product in ProductsInOrder)
                        {
                            if (SelectedProductName == $"{product.ProductName}({ProductArticul})")
                            {
                                product.ProductQuantity += int.Parse(UserQuantity);
                                product.ProductPriceXQuantity += float.Parse(ProductCost) * int.Parse(UserQuantity);
                                Cleaner();
                                return;
                            }
                        }
                    }
                    ProductsInOrder.Add(new ProductInOrder
                    {
                        ProductName = ProductName,
                        ProductArticul = ProductArticul,
                        ProductImage = ProductImage,
                        ProductQuantity = int.Parse(UserQuantity),
                        ProductPriceXQuantity = float.Parse(ProductCost) * int.Parse(UserQuantity)
                    });
                    Cleaner();
                }
            }
        }

        #endregion

        #region Команда отмены добавления товара

        public ICommand CancelAddProductCommand { get; }

        private bool CanCancelAddProductCommandExecute(object parameter) => true;
        private void OnCancelAddProductCommandExecuted(object parameter)
        {
            Cleaner();
            SelectedProduct = null;
        }

        #endregion

        #region Команда удаления товара

        public ICommand RemoveProductCommand { get; }

        private bool CanRemoveProductCommandExecute(object parameter) => true;
        private void OnRemoveProductCommandExecuted(object parameter)
        {
            ProductInOrder? productInOrder = parameter as ProductInOrder;
            if (productInOrder != null)
            {
                ProductsInOrder.Remove(productInOrder);
                SelectedProductName = null;
                ProductArticul = "";
                ProductName = "";
                ProductImage = "";
                ProductCost = "";
                ProductLength = "";
                ProductWidth = "";
                UserQuantity = "";
                SelectedProduct = null;
            }
        }

        #endregion

        #region Команда добавления заказа

        public ICommand AddOrderCommand { get; }

        private bool CanAddOrderCommandExecute(object parameter) => true;
        private async void OnAddOrderCommandExecuted(object parameter)
        {
            if (ProductsInOrder.Count == 0)
            {
                MessageBox.Show("Вы не выбрали товары для заказа");
            }
            else
            {
                int OrderId = 0;
                float Cost = 0;
                foreach (var item in ProductsInOrder)
                {
                    Cost += item.ProductPriceXQuantity;
                }
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                try
                {
                    string sql = "SELECT count(*) FROM generalorder;";

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = sql;
                    cmd.Connection = conn;

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            OrderId = reader.GetInt32(0) + 1;
                        }
                    }
                    reader.Close();

                    sql = "insert into generalorder (GeneralOrder_Id, GeneralOrder_Date, GeneralOrder_Phase, " +
                        "GeneralOrder_Customer_UserInformation_Login, GeneralOrder_Cost) values " +
                        "(@Id, @Date, @Phase, @Customer, @Cost);";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@Id", OrderId);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Phase", "Ожидает");
                    cmd.Parameters.AddWithValue("@Customer", UserLogin);
                    cmd.Parameters.AddWithValue("@Cost", Cost);
                    int rowsaffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsaffected > 0)
                    {
                        sql = "insert into generalorderproduct values " +
                            "(@OrderId, @ProductArticul, @ProductQuantity, @CostOfAllProducts);";
                        cmd.CommandText = sql;
                        foreach (var item in ProductsInOrder)
                        {
                            cmd.Parameters.AddWithValue("@OrderId", OrderId);
                            cmd.Parameters.AddWithValue("@ProductArticul", item.ProductArticul);
                            cmd.Parameters.AddWithValue("@ProductQuantity", item.ProductQuantity);
                            cmd.Parameters.AddWithValue("@CostOfAllProducts", item.ProductPriceXQuantity);
                            await cmd.ExecuteNonQueryAsync();
                            cmd.Parameters.Clear();
                        }
                    }
                    MessageBox.Show("Заказ успешно создан");
                    ProductsInOrder.Clear();
                    Cleaner();
                    SelectedProduct = null;
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

        public MakeOrderViewModel(string userLogin)
        {
            UserLogin = userLogin;
            GetProducts();
            GetProductNames();

            #region Команды

            ToAddProductCommand = new LambdaCommand(OnToAddProductCommandExecuted, CanToAddProductCommandExecute);
            CancelAddProductCommand = new LambdaCommand(OnCancelAddProductCommandExecuted, CanCancelAddProductCommandExecute);
            AddProductCommand = new LambdaCommand(OnAddProductCommandExecuted, CanAddProductCommandExecute);
            RemoveProductCommand = new LambdaCommand(OnRemoveProductCommandExecuted, CanRemoveProductCommandExecute);
            AddOrderCommand = new LambdaCommand(OnAddOrderCommandExecuted, CanAddOrderCommandExecute);

            #endregion

        }

        private void GetProducts()
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                string sql = "SELECT * FROM product";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Products.Add(new Product()
                        {
                            Articul = reader.GetString(0),
                            Name = reader.GetString(1),
                            Length = reader.GetFloat(2),
                            Width = reader.GetFloat(3),
                            Cost = reader.GetFloat(4),
                            Image = reader.GetString(5),
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

        private void GetProductNames()
        {
            foreach (var product in Products)
            {
                ProductsNames.Add($"{product.Name}({product.Articul})");
            }
        }

        private void Cleaner()
        {
            IsStackPanel1Enabled = true;
            IsStackPanel2Enabled = false;
            SelectedProductName = null;
            ProductArticul = "";
            ProductName = "";
            ProductImage = "";
            ProductCost = "";
            ProductLength = "";
            ProductWidth = "";
            UserQuantity = "";
        }

    }
}
