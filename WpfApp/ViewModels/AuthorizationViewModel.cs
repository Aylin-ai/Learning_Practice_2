using WpfApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Infrastructure.Commands.Base;
using MySql.Data.MySqlClient;
using WpfApp.Models;
using System.Windows;
using WpfApp.Infrastructure.Commands;
using System.Windows.Input;
using WpfApp.Views;

namespace WpfApp.ViewModels
{
    internal class AuthorizationViewModel : ViewModel
    {

        #region Actions

        public Action CloseAction { get; set; }

        #endregion

        #region Данные внешнего вида страницы

        public string IconSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-01.jpg";
        public string ImageSource { get; set; } = "D:\\Учеба\\Учебная практика 2\\WSR2017_NC_Skill09_RU\\Сессия 1\\Logo\\logo-02.jpg";

        #endregion

        #region Данные для авторизации

        private string _login = "";
        private string _password1 = "";
        private string _password2 = "";

        public string Login { get => _login; set => Set(ref _login, value); }
        public string Password1 { get => _password1; set => Set(ref _password1, value); }
        public string Password2 { get => _password2; set => Set(ref _password2, value); }

        #endregion

        #region Команды

        #region Команда для регистрации

        public ICommand RegistrationCommand { get; }

        private bool CanRegistrationCommandExecute(object parameter) => true;
        private async void OnRegistrationCommandExecuted(object parameter)
        {
            if (Login != "" && Password1 != "" && Password2 != "")
            {
                if (Password1 == Password2)
                {
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    try
                    {
                        string sqlLoginCheck = "SELECT * from userinformation WHERE UserInformation_Login = @Login";
                        MySqlCommand cmdChecker = new MySqlCommand();
                        cmdChecker.Connection = conn;
                        cmdChecker.CommandText = sqlLoginCheck;
                        cmdChecker.Parameters.AddWithValue("@Login", Login);
                        var reader = await cmdChecker.ExecuteReaderAsync();

                        if (reader.HasRows)
                        {
                            if (reader[0] != null)
                            {
                                MessageBox.Show("Пользователь с таким логином уже существует");
                                Login = "";
                            }
                            else
                            {
                                reader.Close();
                                string sqlAddCustomer = $"INSERT INTO userinformation (UserInformation_Login, " +
                                    $"UserInformation_Password, UserInformation_UserRole_Id)" +
                                    $"VALUES (@Login, @Password, @Role_Id)";

                                MySqlCommand cmd = new MySqlCommand();
                                cmd.Connection = conn;
                                cmd.CommandText = sqlAddCustomer;

                                cmd.Parameters.AddWithValue("@Login", Login);
                                cmd.Parameters.AddWithValue("@Password", Password1);
                                cmd.Parameters.AddWithValue("@Role_Id", 1);

                                await cmd.ExecuteNonQueryAsync();
                                MessageBox.Show($"Пользователь {Login} успешно добавлен");
                            }
                        }
                        else
                        {
                            reader.Close();
                            string sqlAddCustomer = $"INSERT INTO userinformation (UserInformation_Login, " +
                                    $"UserInformation_Password, UserInformation_UserRole_Id)" +
                                    $"VALUES (@Login, @Password, @Role_Id)";

                            MySqlCommand cmd = new MySqlCommand();
                            cmd.Connection = conn;
                            cmd.CommandText = sqlAddCustomer;

                            cmd.Parameters.AddWithValue("@Login", Login);
                            cmd.Parameters.AddWithValue("@Password", Password1);
                            cmd.Parameters.AddWithValue("@Role_Id", 1);

                            await cmd.ExecuteNonQueryAsync();
                            MessageBox.Show($"Пользователь {Login} успешно добавлен");
                        }                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
                else
                {
                    MessageBox.Show("Пароли не совпадают");
                }

            }
            else
                MessageBox.Show("Введены не все данные");
        }

        #endregion

        #region Команда для входа

        public ICommand AuthorizationCommand { get; }

        private bool CanAuthorizationCommandExecute(object parameter) => true;
        private async void OnAuthorizationCommandExecuted(object parameter)
        {
            if (Login != "" && Password1 != "")
            {
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                try
                {
                    string sqlLoginCheck = "SELECT * from userinformation WHERE UserInformation_Login = @Login";
                    MySqlCommand cmdChecker = new MySqlCommand();
                    cmdChecker.Connection = conn;
                    cmdChecker.CommandText = sqlLoginCheck;
                    cmdChecker.Parameters.AddWithValue("@Login", Login);
                    var reader = await cmdChecker.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader[0].ToString() == Login && reader[1].ToString() == Password1)
                            {
                                switch (reader[2].ToString())
                                {
                                    case "1":
                                        CustomerMainWindow customerMainWindow = new CustomerMainWindow(parameter as string);
                                        customerMainWindow.Show();
                                        CloseAction();
                                        break;
                                    case "2":
                                        ManagerMainWindow managerMainWindow = new ManagerMainWindow(parameter as string);
                                        managerMainWindow.Show();
                                        CloseAction();
                                        break;
                                    case "3":
                                        StorekeeperMainWindow storekeeperMainWindow = new StorekeeperMainWindow();
                                        storekeeperMainWindow.Show();
                                        CloseAction();
                                        break;
                                    case "4":
                                        DirectorateMainWindow directorateMainWindow = new DirectorateMainWindow();
                                        directorateMainWindow.Show();
                                        CloseAction();
                                        break;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль");
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Пользователь {Login} не существует");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            else
            {
                MessageBox.Show("Введены не все данные");
            }
        }

        #endregion

        #endregion

        public AuthorizationViewModel()
        {
            #region Команды

            RegistrationCommand = new LambdaCommand(OnRegistrationCommandExecuted, CanRegistrationCommandExecute);
            AuthorizationCommand = new LambdaCommand(OnAuthorizationCommandExecuted, CanAuthorizationCommandExecute);

            #endregion
        }

    }
}
