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
        private string _articul;
        private string? _name;
        private float? _lengthOfCloth;
        private float? _widthOfCloth;
        private float? _costOfCloth;
        private int? _rollAtStore;
        private float? _widthOfClothAtStoreInSM;
        private float? _lengthOfClothAtStoreInSM;
        private float? _costofAllCloth;

        public string Articul { get => _articul; set => Set(ref _articul, value); }
        public string Name { get => _name; set => Set(ref _name, value); }
        public float? LengthOfCloth { get => _lengthOfCloth; set => Set(ref _lengthOfCloth, value); }
        public float? WidthOfCloth { get => _widthOfCloth; set => Set(ref _widthOfCloth, value); }
        public float? CostOfCloth { get => _costOfCloth; set => Set(ref _costOfCloth, value); }
        public int? RollAtStore { get => _rollAtStore; set => Set(ref _rollAtStore, value); }
        public float? WidthOfClothAtStoreInSM { get => _widthOfClothAtStoreInSM; set => Set(ref _widthOfClothAtStoreInSM, value); }
        public float? LengthOfClothAtStoreInSM { get => _lengthOfClothAtStoreInSM; set => Set(ref _lengthOfClothAtStoreInSM, value); }
        public float? CostOfAllCloth { get => _costofAllCloth; set => Set(ref _costofAllCloth, value); }
    }
}
