using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModels.Base;

namespace WpfApp.Models
{
    internal class ProductInOrder : ViewModel
    {
        private int _orderId;
        private string _productArticul;
        private string _productName;
        private string _productImage;
        private int _productQiuantity;
        private float _productPriceXQuantity;

        public int OrderId { get => _orderId; set => Set(ref _orderId, value); }
        public string ProductArticul { get => _productArticul; set => Set(ref _productArticul, value); }
        public string ProductName { get => _productName; set => Set(ref _productName, value); }
        public string ProductImage { get => _productImage; set => Set(ref _productImage, value); }
        public int ProductQuantity { get => _productQiuantity; set => Set(ref _productQiuantity, value); }
        public float ProductPriceXQuantity { get => _productPriceXQuantity; set => Set(ref _productPriceXQuantity, value); }
    }
}
