using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModels.Base;

namespace WpfApp.Models
{
    internal class InventoryMaterial : ViewModel
    {

        private string _articul;
        private string _type;
        private float _systemQuantity;
        private float _realQuantity;
        private float _costOfAllMaterials;

        public string Articul { get => _articul; set => Set(ref _articul, value); }
        public string Type { get => _type; set => Set(ref _type, value); }
        public float SystemQuantity { get => _systemQuantity; set => Set(ref _systemQuantity, value); }
        public float RealQuantity { get => _realQuantity; set => Set(ref _realQuantity, value); }
        public float CostOfAllMaterials { get => _costOfAllMaterials; set => Set(ref _costOfAllMaterials, value); }

    }
}
