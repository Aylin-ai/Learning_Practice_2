using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModels.Base;

namespace WpfApp.Models
{
    internal class FurnitureInProduct : ViewModel
    {

        private string _image;
        private string _articul;
        private string _name;
        private float _length;
        private float _width;
        private string _placing;
        private string _rotation;
        private int _quantity;
        private float _cost;
        private int _quantityAtStore;
        private MySqlDateTime _dateTimeOfWriteOff;

        public string Image { get => _image; set => Set(ref _image, value); }
        public string Articul { get => _articul; set => Set(ref  _articul, value); }
        public string Name { get => _name; set => Set(ref _name, value); }
        public float Length { get => _length; set => Set(ref _length, value); }
        public float Width { get => _width; set => Set(ref _width, value); }
        public string Placing { get => _placing; set => Set(ref _placing, value); }
        public string Rotation { get => _rotation; set => Set(ref _rotation, value); }
        public int Quantity { get => _quantity; set => Set(ref _quantity, value); }
        public float Cost { get => _cost; set => Set(ref _cost, value); }
        public int QuantityAtStore { get => _quantityAtStore; set => Set(ref _quantityAtStore, value); }
        public MySqlDateTime DateTimeOfWriteOff { get => _dateTimeOfWriteOff; set => Set(ref _dateTimeOfWriteOff, value); }

    }
}
