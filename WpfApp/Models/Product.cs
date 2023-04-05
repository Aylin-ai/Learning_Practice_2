using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModels.Base;

namespace WpfApp.Models
{
    public class Product : ViewModel
    {

        private string _image;
        private string _articul;
        private string _name;
        private float _length;
        private float _width;
        private float _cost;

        public string Image { get => _image; set => Set(ref _image, value); }
        public string Articul { get => _articul; set => Set(ref  _articul, value); }
        public string Name { get => _name; set => Set(ref _name, value); } 
        public float Length { get => _length; set => Set(ref _length, value); }
        public float Width { get => _width; set => Set(ref _width, value); }
        public float Cost { get => _cost; set => Set(ref _cost, value); }

    }
}
