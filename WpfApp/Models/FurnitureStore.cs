using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModels.Base;

namespace WpfApp.Models
{
    class FurnitureStore : ViewModel
    {
        private string _articul;
        private string _name;
        private float _cost;
        private int _party;
        private int _quantity;
        private float _costOfAllFurniture;

        public string Articul { get => _articul; set => Set(ref _articul, value); }

        public string Name { get => _name; set => Set(ref _name, value); }
        public float Cost { get => _cost; set => Set(ref _cost, value); }
        public int Party { get => _party; set => Set(ref _party, value); }
        public int Quantity { get => _quantity; set => Set(ref _quantity, value); }
        public float CostOfAllFurniture { get => _costOfAllFurniture; set => Set(ref _costOfAllFurniture, value); }
    }
}
