using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Infrastructure.Commands;
using WpfApp.Models;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels
{
    internal class ProductManufacturingViewModel : ViewModel
    {

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Данные об изделии

        private Product _selectedProduct;
        public Product SelectedProduct { get => _selectedProduct; set => Set(ref _selectedProduct, value); }

        private string _productArticul;
        private string _productName;
        private float _systemProductSelfCost = 0;
        private float _realProductSelfCost = 0;

        public string ProductArticul { get => _productArticul; set => Set(ref _productArticul, value); }
        public string ProductName { get => _productName; set => Set(ref _productName, value); }
        public float SystemProductSelfCost { get => _systemProductSelfCost; set => Set(ref _systemProductSelfCost, value); }
        public float RealProductSelfCost { get => _realProductSelfCost; set => Set(ref _realProductSelfCost, value); }

        private string _warning;
        public string Warning { get => _warning; set => Set(ref _warning, value); }

        #endregion

        #region Коллекции элементов

        private ObservableCollection<ClothInProduct> _clothsInProduct = new ObservableCollection<ClothInProduct>();
        public ObservableCollection<ClothInProduct> ClothsInProduct { get => _clothsInProduct; set => Set(ref _clothsInProduct, value); }

        private ObservableCollection<FurnitureInProduct> _furnituresInProduct = new ObservableCollection<FurnitureInProduct>();
        public ObservableCollection<FurnitureInProduct> FurnituresInProduct { get => _furnituresInProduct; set => Set(ref _furnituresInProduct, value); }

        private ObservableCollection<(string, float)> _realQuantityOfMaterials = new ObservableCollection<(string, float)>();
        private ObservableCollection<(string, float, float)> _costOfMaterials = new ObservableCollection<(string, float, float)>();

        #endregion

        #region Данные материалов

        private string _materialArticul;
        public string MaterialArticul { get => _materialArticul; set => Set(ref _materialArticul, value); }

        private string _image;
        public string Image { get => _image; set => Set(ref _image, value); }

        private float _systemQuantity;
        public float SystemQuantity { get => _systemQuantity; set => Set(ref _systemQuantity, value); }

        private float _userRealQuantity;
        public float UserRealQuantity { get => _userRealQuantity; set => Set(ref _userRealQuantity, value); }

        private float _realCost;
        public float RealCost { get => _realCost; set => Set(ref _realCost, value); }

        #endregion

        #region Данные о выборе пользователя

        private ClothInProduct _selectedCloth;
        public ClothInProduct SelectedCloth
        {
            get => _selectedCloth;
            set
            {
                Set(ref _selectedCloth, value);
                MaterialArticul = _selectedCloth.Articul;
                Image = _selectedCloth.Image;
                SystemQuantity = _selectedCloth.ClothArea;
                Set(ref _selectedCloth, null);
            }
        }

        private FurnitureInProduct _selectedFurniture;
        public FurnitureInProduct SelectedFurniture
        {
            get => _selectedFurniture;
            set
            {
                Set(ref _selectedFurniture, value);
                MaterialArticul = _selectedFurniture.Articul;
                Image = _selectedFurniture.Image;
                SystemQuantity = _selectedFurniture.Quantity;
                Set(ref _selectedFurniture, null);
            }
        }

        #endregion

        #region Команды

        #region Команда добавления информации о фактическом кол-ве требуемого материала

        public ICommand AddRealQuantityOfMaterialCommand { get; }

        private bool CanAddRealQuantityOfMaterialCommandExecute(object parameter) => true;
        private void OnAddRealQuantityOfMaterialCommandExecuted(object parameter)
        {
            if (_realQuantityOfMaterials.Count > 0)
            {
                if (_realQuantityOfMaterials.Any(x => x.Item1 == MaterialArticul))
                {
                    MessageBox.Show("Вы уже указали данные этого материала");
                }
                else
                    _realQuantityOfMaterials.Add(item: (MaterialArticul, UserRealQuantity));

            }
            else
                _realQuantityOfMaterials.Add(item: (MaterialArticul, UserRealQuantity));
            UserRealQuantity = 0;
            if (_realQuantityOfMaterials.Count == (ClothsInProduct.Count + FurnituresInProduct.Count) &&
                _costOfMaterials.Count == (ClothsInProduct.Count + FurnituresInProduct.Count))
            {
                for (int i = 0; i  < _costOfMaterials.Count; i++)
                {
                    for (int j = 0;  j < _costOfMaterials.Count; j++)
                    {
                        if (_costOfMaterials[i].Item1 == _realQuantityOfMaterials[j].Item1)
                        {
                            if (ClothsInProduct.Any(x => x.Articul == _costOfMaterials[i].Item1))
                                RealProductSelfCost += (_realQuantityOfMaterials[j].Item2 / _costOfMaterials[i].Item2) * _costOfMaterials[i].Item3;
                            else if (FurnituresInProduct.Any(x => x.Articul == _costOfMaterials[i].Item1))
                                RealProductSelfCost += _realQuantityOfMaterials[j].Item2 * _costOfMaterials[i].Item3;
                        }
                    }
                }
                if (RealProductSelfCost > ((SystemProductSelfCost * 0.15) + SystemProductSelfCost))
                {
                    Warning = "Объем фактически использованных материалов превышает плановый более чем на 15%";
                }
            }
        }

        #endregion

        #endregion

        public ProductManufacturingViewModel(Product product)
        {
            SelectedProduct = product;
            ProductArticul = product.Articul;
            ProductName = product.Name;
            GetMaterials();

            #region Команды

            AddRealQuantityOfMaterialCommand = new LambdaCommand(OnAddRealQuantityOfMaterialCommandExecuted, CanAddRealQuantityOfMaterialCommandExecute);

            #endregion
        }

        private void GetMaterials()
        {
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
                cmd.Parameters.AddWithValue("@articul", ProductArticul);

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
                        _costOfMaterials.Add((reader.GetString(1), reader.GetFloat(3), reader.GetFloat(4)));
                        SystemProductSelfCost += ((reader.GetFloat(5) * reader.GetFloat(6)) / reader.GetFloat(3)) * reader.GetFloat(4);
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
                        float quantity = readerOfFurnitures.GetInt32(7);
                        _costOfMaterials.Add((readerOfFurnitures.GetString(1), quantity, readerOfFurnitures.GetFloat(8)));
                        SystemProductSelfCost += readerOfFurnitures.GetFloat(8) * readerOfFurnitures.GetInt32(7);
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
