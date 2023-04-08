using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModels.Base;

namespace WpfApp.Models
{
    internal class ComingMaterial : ViewModel
    {

        private string _image;
        private string _articul;
        private string _name;
        private string _typeOfMaterial;
        private float _comingQuantity;
        private float _comingWidthOfRoll;
        private float _comingLengthOfRoll;
        private float _comingCost;

        public string Image { get => _image; set => Set(ref _image, value); }
        public string Articul { get => _articul; set => Set(ref _articul, value); }
        public string Name { get => _name; set => Set(ref _name, value); }
        public string TypeOfMaterial { get => _typeOfMaterial; set => Set(ref _typeOfMaterial, value); }
        public float ComingQuantity { get => _comingQuantity; set => Set(ref _comingQuantity, value); }
        public float ComingWidthOfRoll { get => _comingWidthOfRoll; set => Set(ref _comingWidthOfRoll, value); }
        public float ComingLengthOfRoll { get => _comingLengthOfRoll; set => Set(ref _comingLengthOfRoll, value); }
        public float ComingCost { get => _comingCost; set => Set(ref _comingCost, value); }

    }
}
