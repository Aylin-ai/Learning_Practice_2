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

        private ObservableCollection<InventoryMaterial> _inventoryMaterials = new ObservableCollection<InventoryMaterial>();
        public ObservableCollection<InventoryMaterial> InventoryMaterials { get => _inventoryMaterials; set => Set(ref _inventoryMaterials, value); }

        #endregion

        #region Доп. данные к коллекциям

        private int _clothsAtStoreCount;
        private int _furnituresAtStoreCount;

        #endregion

        #region Данные о выбранном элементе

        private string _articul;
        private string _name;
        private float _atStore;
        private float _costOfAllAtStore;
        private float _surplusCost;
        private float _shortageCost;
        private string _discrepancy;
        private float _usersDataAtStore;

        public string Articul { get => _articul; set => Set(ref _articul, value); }
        public string Name { get => _name; set => Set(ref _name, value); }
        public float AtStore { get => _atStore; set => Set(ref _atStore, value); }
        public float CostOfAllAtStore { get => _costOfAllAtStore; set => Set(ref _costOfAllAtStore, value); }
        public float SurplusCost { get => _surplusCost; set => Set(ref _surplusCost, value); }
        public float ShortageCost { get => _shortageCost; set => Set(ref _shortageCost, value); }
        public string Discrepancy { get => _discrepancy; set => Set(ref _discrepancy, value); }
        public float UsersDataAtStore 
        { 
            get => _usersDataAtStore;
            set 
            { 
                Set(ref _usersDataAtStore, value);
                if (SelectedCloth != null)
                {
                    if (_usersDataAtStore > AtStore)
                    {
                        SurplusCost = (_usersDataAtStore - AtStore) * _selectedCloth.CostOfCloth;
                        ShortageCost = 0;
                    }
                    else if (_usersDataAtStore < AtStore)
                    {
                        ShortageCost = CostOfAllAtStore * _usersDataAtStore / AtStore;
                        SurplusCost = 0;
                    }
                }
                else if (SelectedFurniture != null)
                {
                    if (_usersDataAtStore > AtStore)
                    {
                        SurplusCost = (_usersDataAtStore - AtStore) * _selectedFurniture.Cost;
                        ShortageCost = 0;
                    }
                    else if (_usersDataAtStore < AtStore)
                    {
                        ShortageCost = CostOfAllAtStore * _usersDataAtStore / AtStore;
                        SurplusCost = 0;
                    }
                }
                if ((AtStore - _usersDataAtStore) > (AtStore * 0.2) || (_usersDataAtStore - AtStore) > (AtStore * 0.2))
                {
                    Discrepancy = "Расхождения между реальными и учетными данными по закупочным суммам превышают 20%";
                }
                else
                {
                    Discrepancy = "";
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
                    Articul = _selectedCloth.Articul;
                    Name = _selectedCloth.Name;
                    AtStore = _selectedCloth.AreaOfClothAtStoreIn;
                    CostOfAllAtStore = _selectedCloth.CostOfAllCloth;
                    if (UsersDataAtStore > AtStore)
                    {
                        SurplusCost = (UsersDataAtStore - AtStore) * _selectedCloth.CostOfCloth;
                        ShortageCost = 0;
                    }
                    else if (UsersDataAtStore < AtStore)
                    {
                        ShortageCost = CostOfAllAtStore * UsersDataAtStore / AtStore;
                        SurplusCost = 0;
                    }
                    if ((AtStore - _usersDataAtStore) > (AtStore * 0.2) || (_usersDataAtStore - AtStore) > (AtStore * 0.2))
                    {
                        Discrepancy = "Расхождения между реальными и учетными данными по закупочным суммам превышают 20%";
                    }
                    else
                    {
                        Discrepancy = "";
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
                    Articul = _selectedFurniture.Articul;
                    Name = _selectedFurniture.Name;
                    AtStore = _selectedFurniture.Quantity;
                    CostOfAllAtStore = _selectedFurniture.CostOfAllFurniture;
                    if (UsersDataAtStore > AtStore)
                    {
                        SurplusCost = (UsersDataAtStore - AtStore) * _selectedFurniture.Cost;
                        ShortageCost = 0;
                    }
                    else if (UsersDataAtStore < AtStore)
                    {
                        ShortageCost = CostOfAllAtStore * UsersDataAtStore / AtStore;
                        SurplusCost = 0;
                    }
                    if ((AtStore - _usersDataAtStore) > (AtStore * 0.2) || (_usersDataAtStore - AtStore) > (AtStore * 0.2))
                    {
                        Discrepancy = "Расхождения между реальными и учетными данными по закупочным суммам превышают 20%";
                    }
                    else
                    {
                        Discrepancy = "";
                    }
                }
            }
        }

        #endregion

        #region Команды

        #region Команда подтвержения инвентаризации к элементу

        public ICommand AddInventoryElement { get; }

        private bool CanAddInventoryElementExecute(object parameter) => true;
        private void OnAddInventoryElementExecuted(object parameter)
        {
            if (SelectedCloth == null && SelectedFurniture == null)
                MessageBox.Show("Вы не выбрали элемент");
            else
            {
                if (UsersDataAtStore == 0)
                    MessageBox.Show("Введите реальные данные об элементе");
                else
                {
                    if (ClothsAtStoreNotInventory.Any(x => x.Articul == Articul))
                    {
                        foreach (var cloth in ClothsAtStoreNotInventory)
                        {
                            if (Articul == cloth.Articul)
                            {
                                ClothsAtStoreInventory.Add(cloth);
                                ClothsAtStoreNotInventory.Remove(cloth);
                                InventoryMaterials.Add(new InventoryMaterial()
                                {
                                    Articul = cloth.Articul,
                                    Type = "ткань",
                                    SystemQuantity = cloth.AreaOfClothAtStoreIn,
                                    RealQuantity = UsersDataAtStore,
                                    CostOfAllMaterials = cloth.CostOfAllCloth,
                                });
                                break;
                            }
                        }
                        Articul = "";
                        Name = "";
                        AtStore = 0;
                        CostOfAllAtStore = 0;
                        ShortageCost = 0;
                        SurplusCost = 0;
                        Discrepancy = "";
                        UsersDataAtStore = 0;
                    }
                    else if (FurnituresAtStoreNotInventory.Any(x => x.Articul == Articul))
                    {
                        foreach (var furniture in FurnituresAtStoreNotInventory)
                        {
                            if (Articul == furniture.Articul)
                            {
                                FurnituresAtStoreInventory.Add(furniture);
                                FurnituresAtStoreNotInventory.Remove(furniture);
                                InventoryMaterials.Add(new InventoryMaterial()
                                {
                                    Articul = furniture.Articul,
                                    Type = "фурнитура",
                                    SystemQuantity = furniture.Quantity,
                                    RealQuantity = UsersDataAtStore,
                                    CostOfAllMaterials = furniture.CostOfAllFurniture,
                                });
                                break;
                            }
                        }
                        Articul = "";
                        Name = "";
                        AtStore = 0;
                        CostOfAllAtStore = 0;
                        ShortageCost = 0;
                        SurplusCost = 0;
                        Discrepancy = "";
                        UsersDataAtStore = 0;
                    }
                }

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

            if (InventoryMaterials.Count == 0)
                MessageBox.Show("Вы не провели инвентаризацию");
            else
            {
                if (FurnituresAtStoreInventory.Count != _furnituresAtStoreCount  || ClothsAtStoreInventory.Count != _clothsAtStoreCount)
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

                        foreach (var cloth in InventoryMaterials.Where(x => x.Type == "ткань"))
                        {
                            clothSystemQuantity += cloth.SystemQuantity;
                            clothRealQuantity += cloth.RealQuantity;
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

                        foreach (var furniture in InventoryMaterials.Where(x => x.Type == "фурнитура"))
                        {
                            furnitureSystemQuantity += furniture.SystemQuantity;
                            furnitureRealQuantity += furniture.RealQuantity;
                            furnitureCostOfAll += furniture.CostOfAllMaterials;
                        }
                        if (furnitureSystemQuantity > furnitureRealQuantity)
                        {
                            furnitureShortageCost = furnitureCostOfAll * furnitureRealQuantity / furnitureSystemQuantity;
                            clothSurplusCost = 0;
                        }
                        else if (furnitureSystemQuantity < furnitureRealQuantity)
                        {
                            furnitureShortageCost = 0;
                            furnitureSurplusCost = (furnitureRealQuantity - furnitureSystemQuantity) * furnitureCostOfAll;
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

            #region Команды

            AddInventoryElement = new LambdaCommand(OnAddInventoryElementExecuted, CanAddInventoryElementExecute);

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
                    "cloth.Cloth_Area, cloth.`Cloth_Cost(rub)`, " +
                    "clothstore.ClothStore_ClothArea from clothstore  " +
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
                            AreaOfCloth = reader.GetFloat(3) / 10000,
                            CostOfCloth = reader.GetFloat(4),
                            AreaOfClothAtStoreIn = reader.GetFloat(5),
                            CostOfAllCloth = reader.GetFloat(5) / (reader.GetFloat(3) / 10000) * reader.GetFloat(4),
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

    }
}
