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
    internal class ProductListViewModel : ViewModel
    {

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Коллекции элементов

        private ObservableCollection<Product> _products = new ObservableCollection<Product>();
        public ObservableCollection<Product> Products { get => _products; set => Set(ref _products, value); }

        private ObservableCollection<ClothInProduct> _clothsInProduct = new ObservableCollection<ClothInProduct>();
        public ObservableCollection<ClothInProduct> ClothsInProduct { get => _clothsInProduct; set => Set(ref _clothsInProduct, value); }

        private ObservableCollection<FurnitureInProduct> _furnituresInProduct = new ObservableCollection<FurnitureInProduct>();
        public ObservableCollection<FurnitureInProduct> FurnituresInProduct { get => _furnituresInProduct; set => Set(ref _furnituresInProduct, value); }

        private ObservableCollection<ClothInProduct> _previousClothInProduct = new ObservableCollection<ClothInProduct>();
        public ObservableCollection<ClothInProduct> PreviousClothInProduct { get => _previousClothInProduct; set => Set(ref _previousClothInProduct, value); }

        private ObservableCollection<FurnitureInProduct> _previousFurnitureInProduct = new ObservableCollection<FurnitureInProduct>();
        public ObservableCollection<FurnitureInProduct> PreviousFurnitureInProduct { get => _previousFurnitureInProduct; set => Set(ref _previousFurnitureInProduct, value); }

        #endregion

        #region Данные изделий

        private string _myPreviousSelectedItem = "см";
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                Set(ref _selectedProduct, value);
                GetProductSpecification(_selectedProduct);
                GetPreviousProductSpecification(_selectedProduct);
            }
        }

        private float _selfCost = 0;
        public float SelfCost { get => _selfCost; set => Set(ref _selfCost, value); }

        private string _productImage;
        private string _productArticul;
        private string _productName;
        private float _productLength;
        private float _productWidth;
        private float _productCost;

        public string ProductImage { get => _productImage; set => Set(ref _productImage, value); }
        public string ProductArticul { get => _productArticul; set => Set(ref _productArticul, value); }
        public string ProductName { get => _productName; set => Set(ref _productName, value); }
        public float ProductLength { get => _productLength; set => Set(ref _productLength, value); }
        public float ProductWidth { get => _productWidth; set => Set(ref _productWidth, value); }
        public float ProductCost { get => _productCost; set => Set(ref _productCost, value); }

        #endregion

        #region Команды

        #region Команды для изменения единицы измерения DataGrid

        public ICommand ToMCommand { get; }
        public ICommand ToDMCommand { get; }
        public ICommand ToMMCommand { get; }
        public ICommand ToCMCommand { get; }

        private bool CanToMCommandExecute(object parameter) => true;
        private bool CanToDMCommandExecute(object parameter) => true;
        private bool CanToMMCommandExecute(object parameter) => true;
        private bool CanToCMCommandExecute(object parameter) => true;

        private void OnToMCommandExecuted(object parameter)
        {
            switch (_myPreviousSelectedItem)
            {
                case "см":
                    GetProducts("/100");
                    break;
                case "м":
                    break;
                case "мм":
                    GetProducts("/1000");
                    break;
                case "дм":
                    GetProducts("/10");
                    break;
            }
            _myPreviousSelectedItem = "м";
        }
        private void OnToDMCommandExecuted(object parameter)
        {
            switch (_myPreviousSelectedItem)
            {
                case "см":
                    GetProducts("/10");
                    break;
                case "м":
                    GetProducts("*10");
                    break;
                case "мм":
                    GetProducts("/100");
                    break;
                case "дм":
                    break;
            }
            _myPreviousSelectedItem = "дм";
        }
        private void OnToMMCommandExecuted(object parameter)
        {
            switch (_myPreviousSelectedItem)
            {
                case "см":
                    GetProducts("*10");
                    break;
                case "м":
                    GetProducts("*1000");
                    break;
                case "мм":
                    break;
                case "дм":
                    GetProducts("*100");
                    break;
            }
            _myPreviousSelectedItem = "мм";
        }
        private void OnToCMCommandExecuted(object parameter)
        {
            switch (_myPreviousSelectedItem)
            {
                case "см":
                    break;
                case "м":
                    GetProducts("*100");
                    break;
                case "мм":
                    GetProducts("/10");
                    break;
                case "дм":
                    GetProducts("*10");
                    break;
            }
            _myPreviousSelectedItem = "см";
        }

        #endregion

        #region Команда для перехода к окну производства изделия

        public ICommand ProductManufacturingWindowCommand { get; }

        private bool CanProductManufacturingWindowCommandExecute(object parameter) => true;
        private void OnProductManufacturingWindowCommandExecuted(object parameter)
        {
            if (parameter == null)
                MessageBox.Show("Вы не выбрали изделие");
            else
            {
                Product product = parameter as Product;
                ProductManufacturing productManufacturing = new ProductManufacturing(product);
                productManufacturing.Show();
            }
        }

        #endregion

        #endregion

        public ProductListViewModel()
        {
            GetProducts();

            #region Команды

            ToMCommand = new LambdaCommand(OnToMCommandExecuted, CanToMCommandExecute);
            ToCMCommand = new LambdaCommand(OnToCMCommandExecuted, CanToCMCommandExecute);
            ToDMCommand = new LambdaCommand(OnToDMCommandExecuted, CanToDMCommandExecute);
            ToMMCommand = new LambdaCommand(OnToMMCommandExecuted, CanToMMCommandExecute);
            ProductManufacturingWindowCommand = new LambdaCommand(OnProductManufacturingWindowCommandExecuted, CanProductManufacturingWindowCommandExecute);

            #endregion
        }


        private void GetProducts(string unit = "")
        {
            switch (unit)
            {
                case "*10":
                    for (int i = 0; i < Products.Count; i++)
                    {
                        Products[i].Length *= 10;
                        Products[i].Width *= 10;
                    }
                    break;
                case "*100":
                    for (int i = 0; i < Products.Count; i++)
                    {
                        Products[i].Length *= 100;
                        Products[i].Width *= 100;
                    }
                    break;
                case "/10":
                    for (int i = 0; i < Products.Count; i++)
                    {
                        Products[i].Length /= 10;
                        Products[i].Width /= 10;
                    }
                    break;
                case "/100":
                    for (int i = 0; i < Products.Count; i++)
                    {
                        Products[i].Length /= 100;
                        Products[i].Width /= 100;
                    }
                    break;
                case "*1000":
                    for (int i = 0; i < Products.Count; i++)
                    {
                        Products[i].Length *= 1000;
                        Products[i].Width *= 1000;
                    }
                    break;
                case "/1000":
                    for (int i = 0; i < Products.Count; i++)
                    {
                        Products[i].Length /= 1000;
                        Products[i].Width /= 1000;
                    }
                    break;
                default:
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
                    break;
            }

        }

        private void GetProductSpecification(Product product)
        {
            ProductImage = product.Image;
            ProductName = product.Name;
            ProductArticul = product.Articul;
            ProductLength = product.Length;
            ProductWidth = product.Width;
            ProductCost = product.Cost;


            SelfCost = 0;
            ClothsInProduct.Clear();
            FurnituresInProduct.Clear();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                string sql = "select cloth.Cloth_Image, cloth.Cloth_Articul, cloth.Cloth_Name, cloth.Cloth_Area, cloth.`Cloth_Cost(rub)`, " +
                    "clothproduct.ClothProduct_ClothWidth, clothproduct.ClothProduct_ClothLength from clothproduct inner join cloth " +
                    "on clothproduct.ClothProduct_Cloth_Articul = cloth.Cloth_Articul " +
                    "where clothproduct.ClothProduct_Product_Articul = @articul;";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@articul", product.Articul);

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ClothsInProduct.Add(new ClothInProduct()
                        {
                            Image = reader.GetString(0),
                            Articul = reader.GetString(1),
                            Name = reader.GetString(2),
                            ClothArea = reader.GetFloat(5) * reader.GetFloat(6),
                            Cost = ((reader.GetFloat(5) * reader.GetFloat(6)) / reader.GetFloat(3)) * reader.GetFloat(4),
                        });
                        SelfCost += ((reader.GetFloat(5) * reader.GetFloat(6)) / reader.GetFloat(3)) * reader.GetFloat(4);
                    }
                }
                reader.Close();

                string sqlFurnitures = "select furniture.Furniture_Image, furniture.Furniture_Articul, furniture.Furniture_Name, " +
                    "furnitureproduct.FurnitureProduct_Length, furnitureproduct.FurnitureProduct_Width, " +
                    "furnitureproduct.FurnitureProduct_Placing, furnitureproduct.FurnitureProduct_Rotation, " +
                    "furnitureproduct.FurnitureProduct_Quantity, furniture.Furniture_Cost " +
                    "from furnitureproduct inner join furniture " +
                    "on furnitureproduct.FurnitureProduct_Furniture_Articul = furniture.Furniture_Articul " +
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
                            Placing = readerOfFurnitures.GetString(5),
                            Rotation = readerOfFurnitures.GetString(6),
                            Quantity = readerOfFurnitures.GetInt32(7),
                            Cost = readerOfFurnitures.GetFloat(8) * readerOfFurnitures.GetInt32(7),
                        });
                        SelfCost += readerOfFurnitures.GetFloat(8) * readerOfFurnitures.GetInt32(7);
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

        private void GetPreviousProductSpecification(Product product)
        {
            PreviousClothInProduct.Clear();
            PreviousFurnitureInProduct.Clear();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                string sql = "select cloth.Cloth_Image, cloth.Cloth_Articul, cloth.Cloth_Name, cloth.Cloth_Area, cloth.`Cloth_Cost(rub)`, " +
                    "historyofclothproduct.HistoryOfClothProduct_ClothArea, historyofclothproduct.HistoryOfClothProduct_DateTimeOfWriteOffSepcification " +
                    "from historyofclothproduct inner join cloth " +
                    "on historyofclothproduct.HistoryOfClothProduct_Cloth_Articul = cloth.Cloth_Articul " +
                    "where historyofclothproduct.HistoryOfClothProduct_Product_Articul = @articul;";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@articul", product.Articul);

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PreviousClothInProduct.Add(new ClothInProduct()
                        {
                            Image = reader.GetString(0),
                            Articul = reader.GetString(1),
                            Name = reader.GetString(2),
                            ClothArea = reader.GetFloat(5),
                            Cost = reader.GetFloat(5) / reader.GetFloat(3) * reader.GetFloat(4),
                            DateTimeOfWriteOff = reader.GetMySqlDateTime(6),
                        });
                    }
                }
                reader.Close();

                string sqlFurnitures = "select furniture.Furniture_Image, furniture.Furniture_Articul, furniture.Furniture_Name, " +
                    "historyoffurnitureproduct.HistotyOfFurnitureProduct_Furniture_Length, historyoffurnitureproduct.HistotyOfFurnitureProduct_Furniture_Width, " +
                    "historyoffurnitureproduct.HistotyOfFurnitureProduct_Placing, historyoffurnitureproduct.HistotyOfFurnitureProduct_Rotation, " +
                    "historyoffurnitureproduct.HistotyOfFurnitureProduct_Quantity, furniture.Furniture_Cost,  " +
                    "historyoffurnitureproduct.HistotyOfFurnitureProduct_DateTimeOfWriteOffSepcification " +
                    "from historyoffurnitureproduct inner join furniture " +
                    "on historyoffurnitureproduct.HistotyOfFurnitureProduct_Furniture_Articul = furniture.Furniture_Articul " +
                    "where historyoffurnitureproduct.HistotyOfFurnitureProduct_Product_Articul = @articul;";
                cmd.CommandText = sqlFurnitures;

                var readerOfFurnitures = cmd.ExecuteReader();

                if (readerOfFurnitures.HasRows)
                {
                    while (readerOfFurnitures.Read())
                    {
                        PreviousFurnitureInProduct.Add(new FurnitureInProduct()
                        {
                            Image = readerOfFurnitures.GetString(0),
                            Articul = readerOfFurnitures.GetString(1),
                            Name = readerOfFurnitures.GetString(2),
                            Length = readerOfFurnitures.GetFloat(3),
                            Width = readerOfFurnitures.GetFloat(4),
                            Placing = readerOfFurnitures.GetString(5),
                            Rotation = readerOfFurnitures.GetString(6),
                            Quantity = readerOfFurnitures.GetInt32(7),
                            Cost = readerOfFurnitures.GetFloat(8) * readerOfFurnitures.GetInt32(7),
                            DateTimeOfWriteOff = readerOfFurnitures.GetMySqlDateTime(9),
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
