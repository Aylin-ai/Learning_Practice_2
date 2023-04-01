using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModels.Base;

namespace WpfApp.Models
{
    class ClothInnerInformation : ViewModel
    {

        private string _articul;
        private string _color;
        private string _pattern;
        private string _composition;

        public string Articul { get => _articul; set => Set(ref _articul, value); }
        public string Color { get => _color; set => Set(ref _color, value); }
        public string Pattern { get => _pattern; set => Set(ref _pattern, value); }
        public string Composition { get => _composition; set => Set(ref _composition, value); }
    }
}
