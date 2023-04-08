using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;
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
    internal class InventoryViewModel : ViewModel
    {

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Коллекции элементов

        private ObservableCollection<ClothStore> _clothsAtStoreNotInventory = new ObservableCollection<ClothStore>();
        public ObservableCollection<ClothStore> ClothsAtStoreNotInventory { get => _clothsAtStoreNotInventory; set => Set(ref _clothsAtStoreNotInventory, value); }

        private ObservableCollection<ClothStore> _clothsAtStoreInventory = new ObservableCollection<ClothStore>();
        public ObservableCollection<ClothStore> ClothsAtStoreInventory { get => _clothsAtStoreInventory; set => Set(ref _clothsAtStoreInventory, value); }

        private ObservableCollection<FurnitureStore> _furnituresAtStoreNotInventory = new ObservableCollection<FurnitureStore>();
        public ObservableCollection<FurnitureStore> FurnituresAtStoreNotInventory { get => _furnituresAtStoreNotInventory; set => Set(ref _furnituresAtStoreNotInventory, value); }

        private ObservableCollection<FurnitureStore> _furnituresAtStoreInventory = new ObservableCollection<FurnitureStore>();
        public ObservableCollection<FurnitureStore> FurnituresAtStoreInventory { get => _furnituresAtStoreInventory; set => Set(ref _furnituresAtStoreInventory, value); }

        private ObservableCollection<ProductStore> _productsAtStoreNotInventory = new ObservableCollection<ProductStore>();
        public ObservableCollection<ProductStore> ProductsAtStoreNotInventory { get => _productsAtStoreNotInventory; set => Set(ref _productsAtStoreNotInventory, value); }

        private ObservableCollection<ProductStore> _productsAtStoreInventory = new ObservableCollection<ProductStore>();
        public ObservableCollection<ProductStore> ProductsAtStoreInventory { get => _productsAtStoreInventory; set => Set(ref _productsAtStoreInventory, value); }

        private ObservableCollection<InventoryMaterial> _inventoryCloths = new ObservableCollection<InventoryMaterial>();
        public ObservableCollection<InventoryMaterial> InventoryCloths { get => _inventoryCloths; set => Set(ref _inventoryCloths, value); }

        private ObservableCollection<InventoryMaterial> _inventoryFurnitures = new ObservableCollection<InventoryMaterial>();
        public ObservableCollection<InventoryMaterial> InventoryFurnitures { get => _inventoryFurnitures; set => Set(ref _inventoryFurnitures, value); }

        private ObservableCollection<InventoryMaterial> _inventoryProducts = new ObservableCollection<InventoryMaterial>();
        public ObservableCollection<InventoryMaterial> InventoryProducts { get => _inventoryProducts; set => Set(ref _inventoryProducts, value); }

        #endregion

        #region Доп. данные к коллекциям

        private int _clothsAtStoreCount;
        private int _furnituresAtStoreCount;
        private int _productsAtStoreCount;

        #endregion

        #region Данные о выбранной ткани

        private string _clothArticul;
        public string ClothArticul { get => _clothArticul; set => Set(ref _clothArticul, value); }

        private string _clothName;
        public string ClothName { get => _clothName; set => Set(ref _clothName, value); }

        private float _widthOfRollAtStore;
        public float WidthOfRollAtStore { get => _widthOfRollAtStore; set => Set(ref _widthOfRollAtStore, value); }

        private float _lengthOfRollAtStore;
        public float LengthOfRollAtStore { get => _lengthOfRollAtStore; set => Set(ref _lengthOfRollAtStore, value); }

        private float _clothCostOfAllAtStore;
        public float ClothCostOfAllAtStore { get => _clothCostOfAllAtStore; set => Set(ref _clothCostOfAllAtStore, value); }

        private float _usersWidthOfRollAtStore;
        public float UsersWidthOfRollAtStore
        {
            get => _usersWidthOfRollAtStore;
            set
            {
                Set(ref _usersWidthOfRollAtStore, value);
                float AreaOfCloth = WidthOfRollAtStore * LengthOfRollAtStore;
                float UsersAreaOfCloth = _usersWidthOfRollAtStore * UsersLengthOfRollAtStore;
                if (SelectedCloth != null)
                {
                    if (UsersAreaOfCloth > AreaOfCloth)
                    {
                        ClothSurplusCost = (UsersAreaOfCloth - AreaOfCloth) * _selectedCloth.CostOfCloth;
                        ClothShortageCost = 0;
                    }
                    else if (UsersAreaOfCloth < AreaOfCloth)
                    {
                        ClothShortageCost = ClothCostOfAllAtStore * UsersAreaOfCloth / AreaOfCloth;
                        ClothSurplusCost = 0;
                    }
                }
                if ((AreaOfCloth - UsersAreaOfCloth) > (AreaOfCloth * 0.2) ||
                    (UsersAreaOfCloth - AreaOfCloth) > (AreaOfCloth * 0.2))
                {
                    ClothDiscrepancy = "Расхождения между реальными и учетными данными по закупочным суммам превышают 20%";
                }
                else
                {
                    ClothDiscrepancy = "";
                }
            }
        }

        private float _usersLengthOfRollAtStore;
        public float UsersLengthOfRollAtStore
        {
            get => _usersLengthOfRollAtStore;
            set
            {
                Set(ref _usersLengthOfRollAtStore, value);
                float AreaOfCloth = WidthOfRollAtStore * LengthOfRollAtStore;
                float UsersAreaOfCloth = UsersWidthOfRollAtStore * _usersLengthOfRollAtStore;
                if (SelectedCloth != null)
                {
                    if (UsersAreaOfCloth > AreaOfCloth)
                    {
                        ClothSurplusCost = (UsersAreaOfCloth - AreaOfCloth) * _selectedCloth.CostOfCloth;
                        ClothShortageCost = 0;
                    }
                    else if (UsersAreaOfCloth < AreaOfCloth)
                    {
                        ClothShortageCost = ClothCostOfAllAtStore * UsersAreaOfCloth / AreaOfCloth;
                        ClothSurplusCost = 0;
                    }
                }
                if ((AreaOfCloth - UsersAreaOfCloth) > (AreaOfCloth * 0.2) ||
                    (UsersAreaOfCloth - AreaOfCloth) > (AreaOfCloth * 0.2))
                {
                    ClothDiscrepancy = "Расхождения между реальными и учетными данными по закупочным суммам превышают 20%";
                }
                else
                {
                    ClothDiscrepancy = "";
                }
            }
        }

        private float _clothSurplusCost;
        public float ClothSurplusCost { get => _clothSurplusCost; set => Set(ref _clothSurplusCost, value); }

        private float _clothShortageCost;
        public float ClothShortageCost { get => _clothShortageCost; set => Set(ref _clothShortageCost, value); }

        private string _clothDiscrepancy;
        public string ClothDiscrepancy { get => _clothDiscrepancy; set => Set(ref _clothDiscrepancy, value); }

        #endregion

        #region Данные о выбранной фурнитуре

        private string _furnitureArticul;
        public string FurnitureArticul { get => _furnitureArticul; set => Set(ref _furnitureArticul, value); }

        private string _furnitureName;
        public string FurnitureName { get => _furnitureName; set => Set(ref _furnitureName, value); }

        private float _furnitureQuantityAtStore;
        public float FurnitureQuantityAtStore { get => _furnitureQuantityAtStore; set => Set(ref _furnitureQuantityAtStore, value); }

        private float _furnitureCostOfAllAtStore;
        public float FurnitureCostOfAllAtStore { get => _furnitureCostOfAllAtStore; set => Set(ref _furnitureCostOfAllAtStore, value); }

        private float _furnitureSurplusCost;
        public float FurnitureSurplusCost { get => _furnitureSurplusCost; set => Set(ref _furnitureSurplusCost, value); }

        private float _furnitureShortageCost;
        public float FurnitureShortageCost { get => _furnitureShortageCost; set => Set(ref _furnitureShortageCost, value); }

        private string _furnitureDiscrepancy;
        public string FurnitureDiscrepancy { get => _furnitureDiscrepancy; set => Set(ref _furnitureDiscrepancy, value); }

        private float _usersQuantityAtStore;
        public float UsersQuantityAtStore
        { 
            get => _usersQuantityAtStore;
            set 
            { 
                Set(ref _usersQuantityAtStore, value);
                if (SelectedFurniture != null)
                {
                    if (_usersQuantityAtStore > FurnitureQuantityAtStore)
                    {
                        FurnitureSurplusCost = (_usersQuantityAtStore - FurnitureQuantityAtStore) * _selectedFurniture.Cost;
                        FurnitureShortageCost = 0;
                    }
                    else if (_usersQuantityAtStore < FurnitureQuantityAtStore)
                    {
                        FurnitureShortageCost = FurnitureCostOfAllAtStore * _usersQuantityAtStore / FurnitureQuantityAtStore;
                        FurnitureSurplusCost = 0;
                    }
                }
                if ((FurnitureQuantityAtStore - _usersQuantityAtStore) > (FurnitureQuantityAtStore * 0.2) || 
                    (_usersQuantityAtStore - FurnitureQuantityAtStore) > (FurnitureQuantityAtStore * 0.2))
                {
                    FurnitureDiscrepancy = "Расхождения между реальными и учетными данными по закупочным суммам превышают 20%";
                }
                else
                {
                    FurnitureDiscrepancy = "";
                }
            } 
        }

        #endregion

        #region Данные о выбранном изделии

        private string _productArticul;
        public string ProductArticul { get => _productArticul; set => Set(ref _productArticul, value); }

        private string _productName;
        public string ProductName { get => _productName; set => Set(ref _productName, value); }

        private float _productQuantityAtStore;
        public float ProductQuantityAtStore { get => _productQuantityAtStore; set => Set(ref _productQuantityAtStore, value); }

        private float _productCostOfAllAtStore;
        public float ProductCostOfAllAtStore { get => _productCostOfAllAtStore; set => Set(ref _productCostOfAllAtStore, value); }

        private float _productSurplusCost;
        public float ProductSurplusCost { get => _productSurplusCost; set => Set(ref _productSurplusCost, value); }

        private float _productShortageCost;
        public float ProductShortageCost { get => _productShortageCost; set => Set(ref _productShortageCost, value); }

        private string _productDiscrepancy;
        public string ProductDiscrepancy { get => _productDiscrepancy; set => Set(ref _productDiscrepancy, value); }

        private float _usersProductQuantityAtStore;
        public float UsersProductQuantityAtStore
        {
            get => _usersProductQuantityAtStore;
            set
            {
                Set(ref _usersProductQuantityAtStore, value);
                if (SelectedProduct != null)
                {
                    if (_usersProductQuantityAtStore > ProductQuantityAtStore)
                    {
                        ProductSurplusCost = (_usersProductQuantityAtStore - ProductQuantityAtStore) * SelectedProduct.Cost;
                        ProductShortageCost = 0;
                    }
                    else if (_usersProductQuantityAtStore < ProductQuantityAtStore)
                    {
                        ProductShortageCost = ProductCostOfAllAtStore * _usersProductQuantityAtStore / ProductQuantityAtStore;
                        ProductSurplusCost = 0;
                    }
                }
                if ((ProductQuantityAtStore - _usersProductQuantityAtStore) > (ProductQuantityAtStore * 0.2) ||
                    (_usersProductQuantityAtStore - ProductQuantityAtStore) > (ProductQuantityAtStore * 0.2))
                {
                    ProductDiscrepancy = "Расхождения между реальными и учетными данными по закупочным суммам превышают 20%";
                }
                else
                {
                    ProductDiscrepancy = "";
                }
            }
        }

        #endregion

        #region Выбранные элементы

        private ClothStore _selectedCloth;
        public ClothStore SelectedCloth
        {
            get => _selectedCloth;
            set
            {
                Set(ref _selectedCloth, value);
                if (_selectedCloth != null)
                {
                    float AreaOfCloth = WidthOfRollAtStore * LengthOfRollAtStore;
                    float UsersAreaOfCloth = UsersWidthOfRollAtStore * UsersLengthOfRollAtStore;
                    ClothArticul = _selectedCloth.Articul;
                    ClothName = _selectedCloth.Name;
                    WidthOfRollAtStore = _selectedCloth.WidthOfClothAtStore;
                    LengthOfRollAtStore = _selectedCloth.LengthOfClothAtStore;
                    ClothCostOfAllAtStore = _selectedCloth.CostOfAllCloth;
                    if (UsersAreaOfCloth > AreaOfCloth)
                    {
                        ClothSurplusCost = (UsersAreaOfCloth - AreaOfCloth) * _selectedCloth.CostOfCloth;
                        ClothShortageCost = 0;
                    }
                    else if (UsersAreaOfCloth < AreaOfCloth)
                    {
                        ClothShortageCost = ClothCostOfAllAtStore * UsersAreaOfCloth / AreaOfCloth;
                        ClothSurplusCost = 0;
                    }
                    if ((AreaOfCloth - UsersAreaOfCloth) > (AreaOfCloth * 0.2) ||
                        (UsersAreaOfCloth - AreaOfCloth) > (AreaOfCloth * 0.2))
                    {
                        ClothDiscrepancy = "Расхождения между реальными и учетными данными по закупочным суммам превышают 20%";
                    }
                    else
                    {
                        ClothDiscrepancy = "";
                    }
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
                if (_selectedFurniture != null)
                {
                    FurnitureArticul = _selectedFurniture.Articul;
                    FurnitureName = _selectedFurniture.Name;
                    FurnitureQuantityAtStore = _selectedFurniture.Quantity;
                    FurnitureCostOfAllAtStore = _selectedFurniture.CostOfAllFurniture;
                    if (UsersQuantityAtStore > FurnitureQuantityAtStore)
                    {
                        FurnitureSurplusCost = (UsersQuantityAtStore - FurnitureQuantityAtStore) * _selectedFurniture.Cost;
                        FurnitureShortageCost = 0;
                    }
                    else if (UsersQuantityAtStore < FurnitureQuantityAtStore)
                    {
                        FurnitureShortageCost = FurnitureCostOfAllAtStore * UsersQuantityAtStore / FurnitureQuantityAtStore;
                        FurnitureSurplusCost = 0;
                    }
                    if ((ProductQuantityAtStore - _usersProductQuantityAtStore) > (ProductQuantityAtStore * 0.2) ||
                        (_usersProductQuantityAtStore - ProductQuantityAtStore) > (ProductQuantityAtStore * 0.2))
                    {
                        ProductDiscrepancy = "Расхождения между реальными и учетными данными по закупочным суммам превышают 20%";
                    }
                    else
                    {
                        ProductDiscrepancy = "";
                    }
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
                if (_selectedProduct != null)
                {
                    ProductArticul = _selectedProduct.Articul;
                    ProductName = _selectedProduct.Name;
                    ProductQuantityAtStore = _selectedProduct.QuantityAtStore;
                    ProductCostOfAllAtStore = _selectedProduct.CostOfAllProducts;
                    if (UsersProductQuantityAtStore > ProductQuantityAtStore)
                    {
                        ProductSurplusCost = (UsersProductQuantityAtStore - ProductQuantityAtStore) * SelectedProduct.Cost;
                        ProductShortageCost = 0;
                    }
                    else if (UsersProductQuantityAtStore < ProductQuantityAtStore)
                    {
                        ProductShortageCost = ProductCostOfAllAtStore * UsersProductQuantityAtStore / ProductQuantityAtStore;
                        ProductSurplusCost = 0;
                    }
                    if ((ProductQuantityAtStore - UsersProductQuantityAtStore) > (ProductQuantityAtStore * 0.2) ||
                        (UsersProductQuantityAtStore - ProductQuantityAtStore) > (ProductQuantityAtStore * 0.2))
                    {
                        FurnitureDiscrepancy = "Расхождения между реальными и учетными данными по закупочным суммам превышают 20%";
                    }
                    else
                    {
                        FurnitureDiscrepancy = "";
                    }
                }
            }
        }

        #endregion

        #region Команды

        #region Команда подтвержения инвентаризации к ткани

        public ICommand AddClothCommand { get; }

        private bool CanAddClothCommandExecute(object parameter) => true;
        private void OnAddClothCommandExecuted(object parameter)
        {
            if (SelectedCloth == null)
                MessageBox.Show("Вы не выбрали элемент");
            else
            {
                if (UsersLengthOfRollAtStore == 0 || UsersWidthOfRollAtStore == 0)
                    MessageBox.Show("Введите реальные данные об элементе");
                else
                {
                    foreach (var cloth in ClothsAtStoreNotInventory)
                    {
                        if (ClothArticul == cloth.Articul)
                        {
                            ClothsAtStoreInventory.Add(cloth);
                            ClothsAtStoreNotInventory.Remove(cloth);
                            InventoryCloths.Add(new InventoryMaterial()
                            {
                                Articul = cloth.Articul,
                                Type = "ткань",
                                SystemLengthOfCloth = cloth.LengthOfClothAtStore,
                                SystemWidthOfCloth = cloth.WidthOfClothAtStore,
                                RealWidthOfCloth = UsersWidthOfRollAtStore,
                                RealLengthOfCloth = UsersLengthOfRollAtStore,
                                CostOfAllMaterials = cloth.CostOfAllCloth,
                            });
                            break;
                        }
                    }
                    Cleaner();
                }

            }
        }

        #endregion

        #region Команда подтвержения инвентаризации к фурнитуре

        public ICommand AddFurnutureCommand { get; }

        private bool CanAddFurnutureCommandExecute(object parameter) => true;
        private void OnAddFurnutureCommandExecuted(object parameter)
        {
            if (SelectedFurniture == null)
                MessageBox.Show("Вы не выбрали элемент");
            else
            {
                foreach (var furniture in FurnituresAtStoreNotInventory)
                {
                    if (FurnitureArticul == furniture.Articul)
                    {
                        FurnituresAtStoreInventory.Add(furniture);
                        FurnituresAtStoreNotInventory.Remove(furniture);
                        InventoryFurnitures.Add(new InventoryMaterial()
                        {
                            Articul = furniture.Articul,
                            Type = "фурнитура",
                            SystemQuantity = furniture.Quantity,
                            RealQuantity = UsersQuantityAtStore,
                            CostOfAllMaterials = furniture.CostOfAllFurniture,
                        });
                        break;
                    }
                }
                Cleaner();
            }
        }

        #endregion

        #region Команда подтвержения инвентаризации к изделию

        public ICommand AddProductCommand { get; }

        private bool CanAddProductCommandExecute(object parameter) => true;
        private void OnAddProductCommandExecuted(object parameter)
        {
            if (SelectedProduct == null)
                MessageBox.Show("Вы не выбрали элемент");
            else
            {
                foreach (var product in ProductsAtStoreNotInventory)
                {
                    if (ProductArticul == product.Articul)
                    {
                        ProductsAtStoreInventory.Add(product);
                        ProductsAtStoreNotInventory.Remove(product);
                        InventoryProducts.Add(new InventoryMaterial()
                        {
                            Articul = product.Articul,
                            Type = "изделие",
                            SystemQuantity = product.QuantityAtStore,
                            RealQuantity = UsersProductQuantityAtStore,
                            CostOfAllMaterials = product.CostOfAllProducts,
                        });
                        break;
                    }
                }
                Cleaner();
            }
        }

        #endregion

        #region Команда утверждения всей инвентаризации

        public ICommand ConfirmInventoryCommand { get; }

        private bool CanConfirmInventoryCommandExecute(object parameter) => true;
        private void OnConfirmInventoryCommandExecuted(object parameter)
        {
            int inventory_id = 0;
            float clothSystemQuantity = 0;
            float clothRealQuantity = 0;
            float clothShortageCost = 0;
            float clothSurplusCost = 0;
            float clothCostOfAll = 0;

            float furnitureSystemQuantity = 0;
            float furnitureRealQuantity = 0;
            float furnitureShortageCost = 0;
            float furnitureSurplusCost = 0;
            float furnitureCostOfAll = 0;

            float productSystemQuantity = 0;
            float productRealQuantity = 0;
            float productShortageCost = 0;
            float productSurplusCost = 0;
            float productCostOfAll = 0;

            if (InventoryCloths.Count == 0 || InventoryFurnitures.Count == 0 || InventoryProducts.Count == 0)
                MessageBox.Show("Вы не провели инвентаризацию");
            else
            {
                if (FurnituresAtStoreInventory.Count != _furnituresAtStoreCount ||
                    ClothsAtStoreInventory.Count != _clothsAtStoreCount ||
                    ProductsAtStoreInventory.Count != _productsAtStoreCount)
                    MessageBox.Show("Вы не провели полную инвентаризацию");
                else
                {
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    try
                    {
                        string sql = "select count(*) from inventory";

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.CommandText = sql;
                        cmd.Connection = conn;

                        var reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                inventory_id = reader.GetInt32(0);
                            }
                        }
                        reader.Close();

                        sql = "insert into inventory values " +
                            "(@id, @datetime)";
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id", inventory_id);
                        cmd.Parameters.AddWithValue("@datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        foreach (var cloth in InventoryCloths)
                        {
                            clothSystemQuantity += (cloth.SystemLengthOfCloth * cloth.SystemWidthOfCloth);
                            clothRealQuantity += (cloth.RealLengthOfCloth * cloth.RealWidthOfCloth);
                            clothCostOfAll += cloth.CostOfAllMaterials;
                        }
                        if (clothSystemQuantity > clothRealQuantity)
                        {
                            clothShortageCost = clothCostOfAll * clothRealQuantity / clothSystemQuantity;
                            clothSurplusCost = 0;
                        }
                        else if (clothSystemQuantity < clothRealQuantity)
                        {
                            clothShortageCost = 0;
                            clothSurplusCost = (clothRealQuantity - clothSystemQuantity) * clothCostOfAll;
                        }

                        foreach (var furniture in InventoryFurnitures)
                        {
                            furnitureSystemQuantity += furniture.SystemQuantity;
                            furnitureRealQuantity += furniture.RealQuantity;
                            furnitureCostOfAll += furniture.CostOfAllMaterials;
                        }
                        if (furnitureSystemQuantity > furnitureRealQuantity)
                        {
                            furnitureShortageCost = furnitureCostOfAll * furnitureRealQuantity / furnitureSystemQuantity;
                            furnitureSurplusCost = 0;
                        }
                        else if (furnitureSystemQuantity < furnitureRealQuantity)
                        {
                            furnitureShortageCost = 0;
                            furnitureSurplusCost = (furnitureRealQuantity - furnitureSystemQuantity) * furnitureCostOfAll;
                        }

                        foreach (var product in InventoryProducts)
                        {
                            productSystemQuantity += product.SystemQuantity;
                            productRealQuantity += product.RealQuantity;
                            productCostOfAll += product.CostOfAllMaterials;
                        }
                        if (productSystemQuantity > productRealQuantity)
                        {
                            productShortageCost = productCostOfAll * productRealQuantity / productSystemQuantity;
                            productSurplusCost = 0;
                        }
                        else if (productSystemQuantity < productRealQuantity)
                        {
                            productShortageCost = 0;
                            productSurplusCost = (productRealQuantity - productSystemQuantity) * productCostOfAll;
                        }

                        sql = "insert into inventorymaterials values " +
                            "(@id, @type, @systemQuantity, @realQuantity, @shortage, @surplus)";
                        cmd.CommandText = sql;

                        cmd.Parameters.AddWithValue("@id", inventory_id);
                        cmd.Parameters.AddWithValue("@type", "ткань");
                        cmd.Parameters.AddWithValue("@systemQuantity", clothSystemQuantity);
                        cmd.Parameters.AddWithValue("@realQuantity", clothRealQuantity);
                        cmd.Parameters.AddWithValue("@shortage", clothShortageCost);
                        cmd.Parameters.AddWithValue("@surplus", clothSurplusCost);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("@id", inventory_id);
                        cmd.Parameters.AddWithValue("@type", "фурнитура");
                        cmd.Parameters.AddWithValue("@systemQuantity", furnitureSystemQuantity);
                        cmd.Parameters.AddWithValue("@realQuantity", furnitureRealQuantity);
                        cmd.Parameters.AddWithValue("@shortage", furnitureShortageCost);
                        cmd.Parameters.AddWithValue("@surplus", furnitureSurplusCost);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("@id", inventory_id);
                        cmd.Parameters.AddWithValue("@type", "изделие");
                        cmd.Parameters.AddWithValue("@systemQuantity", productSystemQuantity);
                        cmd.Parameters.AddWithValue("@realQuantity", productRealQuantity);
                        cmd.Parameters.AddWithValue("@shortage", productShortageCost);
                        cmd.Parameters.AddWithValue("@surplus", productSurplusCost);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
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

        #endregion

        #endregion

        public InventoryViewModel()
        {
            GetFurnituresAtStore();
            GetClothsAtStore();
            GetProductsAtStore();

            #region Команды

            AddClothCommand = new LambdaCommand(OnAddClothCommandExecuted, CanAddClothCommandExecute);
            AddFurnutureCommand = new LambdaCommand(OnAddFurnutureCommandExecuted, CanAddFurnutureCommandExecute);
            AddProductCommand = new LambdaCommand(OnAddProductCommandExecuted, CanAddProductCommandExecute);
            ConfirmInventoryCommand = new LambdaCommand(OnConfirmInventoryCommandExecuted, CanConfirmInventoryCommandExecute);

            #endregion
        }

        private void GetFurnituresAtStore()
        {
            FurnituresAtStoreNotInventory.Clear();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                string sql = "SELECT f.Furniture_Image, f.Furniture_Articul, f.Furniture_Name, " +
                    "f.Furniture_Cost, fs.FurnitureStore_Quantity " +
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
                        FurnituresAtStoreNotInventory.Add(new FurnitureStore()
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
                _furnituresAtStoreCount = FurnituresAtStoreNotInventory.Count;
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

        private void GetClothsAtStore()
        {
            ClothsAtStoreNotInventory.Clear();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                string sql = "select cloth.Cloth_Image, cloth.Cloth_Articul, cloth.Cloth_Name, " +
                    "cloth.`Cloth_Width(cm)`, cloth.`Cloth_Length(cm)`, cloth.`Cloth_Cost(rub)`, " +
                    "clothstore.ClothStore_WidthOfRoll, clothstore.ClothStore_LengthOfRoll from clothstore  " +
                    "inner join cloth " +
                    "on clothstore.ClothStore_Cloth_Articul = cloth.Cloth_Articul;";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ClothsAtStoreNotInventory.Add(new ClothStore()
                        {
                            Image = reader.GetString(0),
                            Articul = reader.GetString(1),
                            Name = reader.GetString(2),
                            WidthOfCloth = reader.GetFloat(3),
                            LengthOfCloth = reader.GetFloat(4),
                            CostOfCloth = reader.GetFloat(5),
                            WidthOfClothAtStore = reader.GetFloat(6),
                            LengthOfClothAtStore = reader.GetFloat(7),
                            CostOfAllCloth = (reader.GetFloat(6) * reader.GetFloat(6)) / (reader.GetFloat(3) * reader.GetFloat(4)) * reader.GetFloat(4),
                        });
                    }
                }
                _clothsAtStoreCount = ClothsAtStoreNotInventory.Count;
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

        private void GetProductsAtStore()
        {
            ProductsAtStoreNotInventory.Clear();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                string sql = "SELECT p.Product_Image, p.Product_Articul, p.Product_Name, " +
                    "p.`Product_Cost(rub)`, ps.ProductStore_Quantity " +
                    "from product p " +
                    "inner join productstore ps " +
                    "on p.Product_Articul = ps.ProductStore_Product_Articul;";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductsAtStoreNotInventory.Add(new ProductStore()
                        {
                            Image = reader.GetString(0),
                            Articul = reader.GetString(1),
                            Name = reader.GetString(2),
                            Cost = reader.GetFloat(3),
                            QuantityAtStore = reader.GetInt32(4),
                            CostOfAllProducts = reader.GetFloat(3) * reader.GetInt32(4)
                        });
                    }
                }
                _productsAtStoreCount = ProductsAtStoreNotInventory.Count;
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

        private void Cleaner()
        {
            ClothArticul = "";
            FurnitureArticul = "";

            ClothName = "";
            FurnitureName = "";

            WidthOfRollAtStore = 0;
            LengthOfRollAtStore = 0;
            FurnitureQuantityAtStore = 0;

            ClothCostOfAllAtStore = 0;
            FurnitureCostOfAllAtStore = 0;

            ClothShortageCost = 0;
            FurnitureShortageCost = 0;

            ClothSurplusCost = 0;
            FurnitureSurplusCost = 0;

            ClothDiscrepancy = "";
            FurnitureDiscrepancy = "";

            UsersLengthOfRollAtStore = 0;
            UsersWidthOfRollAtStore = 0;
            UsersQuantityAtStore = 0;
        }

    }
}
