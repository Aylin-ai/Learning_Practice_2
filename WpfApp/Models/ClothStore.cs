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
        private float _areaOfCloth;
        private float _costOfCloth;
        private int? _rollAtStore;
        private float _areaOfClothAtStoreInSM;
        private float _costofAllCloth;

        public string Articul { get => _articul; set => Set(ref _articul, value); }
        public string Name { get => _name; set => Set(ref _name, value); }
        public float AreaOfCloth { get => _areaOfCloth; set => Set(ref _areaOfCloth, value); }
        public float CostOfCloth { get => _costOfCloth; set => Set(ref _costOfCloth, value); }
        public int? RollAtStore { get => _rollAtStore; set => Set(ref _rollAtStore, value); }
        public float AreaOfClothAtStoreInSM { get => _areaOfClothAtStoreInSM; set => Set(ref _areaOfClothAtStoreInSM, value); }
        public float CostOfAllCloth { get => _costofAllCloth; set => Set(ref _costofAllCloth, value); }
    }
}
