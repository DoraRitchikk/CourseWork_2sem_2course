using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Course_Work_2sem
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWin.xaml
    /// </summary>
    public partial class RegistrationWin : Window
    {

        int z = 0;
        DB_Connection DataBase = new DB_Connection();
        public RegistrationWin()
        {
            InitializeComponent();

            this.MaxHeight = 500;
            this.MaxWidth = 400;

            this.MinHeight = 500;
            this.MinWidth = 400;
        }

        //Зарегистрироваться
        private void Registr(object sender, RoutedEventArgs e)
        {
            try
            {

                string NameOfUser = Login.Text;
                string UserPassword = Password.Text;
                string PasswordRe = PasswordN.Text;

                string[] NotValidItems = new string[7] { " \'\' ", " \"\" ", "@", "#", "%", "*", "^" };

                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                sqlConnection.Open();


                string TrueOrFalseUser = $"Select UserName from REGISTRATION where EXISTS(Select UserName from REGISTRATION where UserName like '{NameOfUser}')";
                SqlCommand createCommand3 = new SqlCommand(TrueOrFalseUser, sqlConnection);
                object trueOrFalse = createCommand3.ExecuteScalar();
                string srcUserDefault = "https://tsum.by/upload/no-photo.png";
                if (UserPassword.Length < 6)
                {
                    ErrorPassword.Content = "Минимальная длина - 6 символов";
                }
                else
                {
                    ErrorPassword.Content = "";
                }
                if (NameOfUser.Length < 6)
                {
                    ErrorLogin.Content = "Минимальная длина - 6 символов";
                }
                else
                {
                    ErrorLogin.Content = "";
                }

                for (int i = 0; i < 7; i++)
                {
                    if (NameOfUser.Contains(NotValidItems[i]))
                    {
                        z++;
                    }
                    if (UserPassword.Contains(NotValidItems[i]))
                    {
                        z++;
                    }
                    if (PasswordRe.Contains(NotValidItems[i]))
                    {
                        z++;
                    }
                }

                if (z == 0)
                {
                    if (trueOrFalse == null)
                    {
                        if (UserPassword == PasswordRe)
                        {
                            if (UserPassword.Length > 6 && NameOfUser.Length > 6)
                            {
                                //Передаём информацию в БД
                                string queryString = $"Insert into REGISTRATION(UserName,PasswordUser,PhotoOfUser) values('{NameOfUser}','{UserPassword}','{srcUserDefault}')";

                                SqlCommand command = new SqlCommand(queryString, DataBase.GetConnection());

                                DataBase.OpenConnection();
                                if (command.ExecuteNonQuery() == 1)
                                {
                                    MessageBox.Show("Регистрация прошла успешно");
                                }
                                else
                                {
                                    MessageBox.Show("Проблемы с подключением...");
                                }
                                DataBase.CloseConnection();

                                MainWindow MainW = new MainWindow();
                                MainW.Show();
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Пароли не совпадают");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Такой юзер уже есть");
                    }
                }
                else
                {
                    MessageBox.Show("Строка содержит запретные символы (@,#,$,%,^,\",\')");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Данные введены некорректно");
            }
        }
        private void ReturnBack(object sender, RoutedEventArgs e)
        {
            MainWindow MainW = new MainWindow();
            MainW.Show();
            this.Close();
        }
    }
}
