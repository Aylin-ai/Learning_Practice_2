using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModels.Base;

namespace WpfApp.Models
{
    internal class ClothInProduct : ViewModel
    {

        private string _image;
        private string _articul;
        private string _name;
        private float _clothArea;
        private float _cost;
        private MySqlDateTime _dateTimeOfWriteOff;

        public string Image { get => _image; set => Set(ref _image, value); }
        public string Articul { get => _articul; set => Set(ref _articul, value); }
        public string Name { get => _name; set => Set(ref _name, value); }
        public float ClothArea { get => _clothArea; set => Set(ref _clothArea, value); }
        public float Cost { get => _cost; set => Set(ref _cost, value); }
        public MySqlDateTime DateTimeOfWriteOff { get => _dateTimeOfWriteOff; set => Set(ref _dateTimeOfWriteOff, value); }

    }
}
