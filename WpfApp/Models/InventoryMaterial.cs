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
        private float _systemWidthOfCloth;
        private float _systemLengthOfCloth;
        private float _realWidthOfCloth;
        private float _realLengthOfCloth;
        private float _costOfAllMaterials;

        public string Articul { get => _articul; set => Set(ref _articul, value); }
        public string Type { get => _type; set => Set(ref _type, value); }
        public float SystemQuantity { get => _systemQuantity; set => Set(ref _systemQuantity, value); }
        public float RealQuantity { get => _realQuantity; set => Set(ref _realQuantity, value); }
        public float SystemWidthOfCloth { get => _systemWidthOfCloth; set => Set(ref _systemWidthOfCloth, value); }
        public float SystemLengthOfCloth { get => _systemLengthOfCloth; set => Set(ref _systemLengthOfCloth, value); }
        public float RealWidthOfCloth { get => _realWidthOfCloth; set => Set(ref _realWidthOfCloth, value); }
        public float RealLengthOfCloth { get => _realLengthOfCloth; set => Set(ref _realLengthOfCloth, value); }
        public float CostOfAllMaterials { get => _costOfAllMaterials; set => Set(ref _costOfAllMaterials, value); }

    }
}
