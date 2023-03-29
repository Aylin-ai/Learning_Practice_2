using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Models
{
    internal class Product
    {
        public string Articul { get; set; }
        public string Name { get; set; }
        public float? Length { get; set; }
        public float? Width { get; set; }
        public float? Cost { get; set; }
    }
}
