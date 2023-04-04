using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModels.Base;

namespace WpfApp.Models
{
    internal class ProductStore : ViewModel
    {

        private string _image;
        private string _articul;
        private string _name;
        private float _cost;
        private int _quantityAtStore;
        private float _costOfAllProducts;

        public string Image { get => _image; set => Set(ref  _image, value); }
        public string Articul { get => _articul; set => Set(ref _articul, value); }
        public string Name { get => _name; set => Set(ref _name, value); }
        public float Cost { get => _cost; set => Set(ref _cost, value); }
        public int QuantityAtStore { get => _quantityAtStore; set => Set(ref _quantityAtStore, value); }
        public float CostOfAllProducts { get => _costOfAllProducts; set => Set(ref _costOfAllProducts, value); }
    }
}
