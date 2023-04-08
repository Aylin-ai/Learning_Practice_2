using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModels.Base;

namespace WpfApp.Models
{
    public class ClothRollToCut : ViewModel
    {

        private string _clothImage;
        private string _clothArticul;
        private float _widthOfRoll;
        private float _lengthOfRoll;
        private int _productQuantity;
        private ObservableCollection<ProductToCut> _productsToCut = new ObservableCollection<ProductToCut>();

        public string ClothImage { get => _clothImage; set => Set(ref _clothImage, value); }
        public string ClothArticul { get => _clothArticul; set => Set(ref _clothArticul, value); }
        public float WidthOfRoll { get => _widthOfRoll; set => Set(ref _widthOfRoll, value); }
        public float LengthOfRoll { get => _lengthOfRoll; set => Set(ref _lengthOfRoll, value); }
        public int ProductQuantity { get => _productQuantity; set => Set(ref _productQuantity, value); }
        public ObservableCollection<ProductToCut> ProductsToCut { get => _productsToCut; set => Set(ref _productsToCut, value); }

    }
}
