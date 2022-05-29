using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для MyAccount.xaml
    /// </summary>
    public partial class MyAccount : Window
    {
        public static bool isOpened = false;
        int IDU;
        public MyAccount(int IDU1)
        {
            InitializeComponent();
            IDU= IDU1;

            this.MinHeight = 470;
            this.MinWidth = 420 ;

            this.MaxHeight = 520;
            this.MaxWidth = 700;

            SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            sqlConnection.Open();

            string cmd = $"SELECT PhotoOfUser,UserName FROM REGISTRATION Where UserId like '{IDU}'";
            SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                string sour = createCommand.ExecuteScalar().ToString();
                imageBox.Source = new BitmapImage(new Uri(sour));
            string getPremierId = $"SELECT UserName FROM REGISTRATION WHERE UserID like '{IDU1}'";
            SqlCommand createCommand3 = new SqlCommand(getPremierId, sqlConnection);
            string NameU = createCommand3.ExecuteScalar().ToString();

            NewLogin.Text = NameU;
        }

        private void ChangeUser(object sender, RoutedEventArgs e)
        {
            try
            {
                string NewLog = NewLogin.Text;
                string cmd = "";

                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlConnection.Open();
                
                if (NewLogin.Text != "" && NewLogin.Text.Length < 15 && NewLogin.Text.Length > 6)
                {
                    cmd = $" UPDATE REGISTRATION SET UserName = '{NewLogin.Text}' where UserID like '{IDU}' ";
                    SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                    createCommand.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("Не валидное значение");
                }

                if (SRCForUserImage.Text.Length != 0)
                {
                    if (SRCForUserImage.Text.Length > 15)
                    {
                        string srcForPhoto = SRCForUserImage.Text;
                        string cmd5 = $" UPDATE REGISTRATION SET PhotoOfUser = '{srcForPhoto}' where UserID like '{IDU}' ";
                        imageBox.Source = new BitmapImage(new Uri(srcForPhoto));
                        SqlCommand createCommand5 = new SqlCommand(cmd5, sqlConnection);
                        createCommand5.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Некорректное значение ссылки");
                    }
                }


                string getPremierId = $"SELECT UserName,PhotoOfUser FROM REGISTRATION WHERE UserID like '{IDU}'";
                SqlCommand createCommand3 = new SqlCommand(getPremierId, sqlConnection);
                string NameU = createCommand3.ExecuteScalar().ToString();

                if (NewPassword.Text != "")
                {
                    string NewPasswordd = NewPassword.Text;
                    string NewPasswordRepeatt = NewPasswordRepeat.Text;
                    if (NewPasswordd == NewPasswordRepeatt)
                    {
                        string cmd2 = $" UPDATE REGISTRATION SET PasswordUser = '{NewPasswordd}' where UserID like '{IDU}' ";

                        SqlCommand createCommand2 = new SqlCommand(cmd2, sqlConnection);
                        createCommand2.ExecuteNonQuery();
                        MessageBox.Show("Изменено успешно");
                    }
                    else
                    {
                        MessageBox.Show("Пароли не совпадают");
                    }
                }

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Проверьте корректность введённых данных");
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isOpened = false;   
        }
    }
}
