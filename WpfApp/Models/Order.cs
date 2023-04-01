using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModels.Base;

namespace WpfApp.Models
{
    internal class Order : ViewModel
    {

        private int _orderId;
        private MySqlDateTime _orderCreationDate;
        private string _orderPhase;
        private string _orderCustomer;
        private string _orderManager;
        private float _orderCost;

        public int OrderId { get => _orderId; set => Set(ref  _orderId, value); }  
        public MySqlDateTime OrderCreationDate { get => _orderCreationDate; set =>  Set(ref _orderCreationDate, value); }
        public string OrderPhase { get => _orderPhase; set => Set(ref _orderPhase, value); }
        public string OrderCustomer { get => _orderCustomer; set => Set(ref _orderCustomer, value); }
        public string OrderManager { get => _orderManager; set => Set(ref _orderManager, value); }
        public float OrderCost { get => _orderCost; set => Set(ref _orderCost, value); }

    }
}
