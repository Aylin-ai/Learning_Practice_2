using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModels.Base;

namespace WpfApp.Models
{
    internal class ClothStore : ViewModel
    {
        private string _image;
        private string _articul;
        private string? _name;
        private float _widthOfCloth;
        private float _lengthOfCloth;
        private float _costOfCloth;
        private int? _rollAtStore;
        private float _widthOfRollAtStore;
        private float _lengthOfRollAtStore;
        private float _costofAllCloth;

        public string Image { get => _image; set => Set(ref _image, value); }
        public string Articul { get => _articul; set => Set(ref _articul, value); }
        public string Name { get => _name; set => Set(ref _name, value); }
        public float WidthOfCloth { get => _widthOfCloth; set => Set(ref _widthOfCloth, value); }
        public float LengthOfCloth { get => _lengthOfCloth; set => Set(ref _lengthOfCloth, value); }
        public float CostOfCloth { get => _costOfCloth; set => Set(ref _costOfCloth, value); }
        public int? RollAtStore { get => _rollAtStore; set => Set(ref _rollAtStore, value); }
        public float WidthOfClothAtStore { get => _widthOfRollAtStore; set => Set(ref _widthOfRollAtStore, value); }
        public float LengthOfClothAtStore { get => _lengthOfRollAtStore; set => Set(ref _lengthOfRollAtStore, value); }
        public float CostOfAllCloth { get => _costofAllCloth; set => Set(ref _costofAllCloth, value); }
    }
}
