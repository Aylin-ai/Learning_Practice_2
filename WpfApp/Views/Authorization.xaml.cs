﻿using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp.Models;
using WpfApp.ViewModels;

namespace WpfApp.Views
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            AuthorizationViewModel viewModel = new AuthorizationViewModel();
            DataContext = viewModel;
            InitializeComponent();
            if (viewModel.CloseAction == null)
                viewModel.CloseAction = new Action(this.Close);

            //OpenFileDialog openFileDialog = new OpenFileDialog()
            //{
            //    DefaultExt = ".xls;*.xlsx",
            //    Title = "Выберите файл базы данных"
            //};
            //if (!(openFileDialog.ShowDialog() == true))
            //    return;

            //string[,] list;
            //Microsoft.Office.Interop.Excel.Application ObjWorkExcel = new Microsoft.Office.Interop.Excel.Application();
            //Microsoft.Office.Interop.Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(openFileDialog.FileName);
            //Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ObjWorkExcel.Sheets[11];
            //var lastCell = ObjWorkSheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell);
            //int _columns = (int)lastCell.Column;
            //int _rows = 285;
            //list = new string[_rows, _columns];


            //MySqlConnection conn = DBUtils.GetDBConnection();
            //conn.Open();
            //int index = 0;
            //try
            //{
            //    string sqlLoginCheck = "INSERT INTO clothcomposition" +
            //        " VALUES (@a, @b)";
            //    MySqlCommand cmdChecker = new MySqlCommand();
            //    cmdChecker.Connection = conn;
            //    cmdChecker.CommandText = sqlLoginCheck;
            //    for (index = 2; index <= _rows; index++)
            //    {
            //        cmdChecker.Parameters.AddWithValue("@a", ObjWorkSheet.Cells[index, 1].Text);
            //        cmdChecker.Parameters.AddWithValue("@b", ObjWorkSheet.Cells[index, 2].Text);

            //        var reader = cmdChecker.ExecuteReader();
            //        reader.Close();
            //        cmdChecker.Parameters.Clear();
            //    }
            //    MessageBox.Show("Успех");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    MessageBox.Show($"Articul - {ObjWorkSheet.Cells[index, 1].Text}");
            //}
            //finally
            //{
            //    conn.Close();
            //    conn.Dispose();
            //    ObjWorkBook.Close(false, Type.Missing, Type.Missing);
            //    ObjWorkExcel.Quit();
            //    GC.Collect();
            //}
        }
    }
}
