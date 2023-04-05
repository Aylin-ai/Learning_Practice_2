using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModels.Base;

namespace WpfApp.Models
{
    internal class HistoryOfChangesAtStore : ViewModel
    {

        private string _type;
        private string _articul;
        private float _oldQuantity;
        private float _newQuantity;
        private MySqlDateTime _dateTimeOfChanges;

        public string Type { get => _type; set => Set(ref _type, value); }
        public string Articul { get => _articul; set =>  Set(ref _articul, value); }
        public float OldQuantity { get => _oldQuantity; set => Set(ref _oldQuantity, value); }
        public float NewQuantity { get => _newQuantity; set => Set(ref _newQuantity, value); }
        public MySqlDateTime DateTimeOfChanges { get => _dateTimeOfChanges; set => Set(ref _dateTimeOfChanges, value); }

    }
}
