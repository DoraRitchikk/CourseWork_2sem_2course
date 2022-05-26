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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Course_Work_2sem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DB_Connection conn = new DB_Connection();
        public MainWindow()
        {
            InitializeComponent();

            this.MaxHeight = 470;
            this.MaxWidth = 400;
            this.MinHeight = 470;
            this.MinWidth = 400;

            loginBox.MaxLength = 15;
            passwordBox.MaxLength = 15;

            
        }
        //Окно, открывающееся после входа в систему
        private void Enter(object sender, RoutedEventArgs e)
        { 
            try
            {
                string[] NotValidItems = new string[7] { " \' ", " \" ", "@", "#", "%", "*", "^" };
                var loginUser = loginBox.Text;
                var passwordUser = passwordBox.Text;
                for (int i = 0; i < 7; i++)
                {
                    if (loginUser.Contains(NotValidItems[i]) || passwordUser.Contains(NotValidItems[i]))
                    {
                        MessageBox.Show("Строка не должна содержать запрещённые символы");
                    }
                }
                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                sqlConnection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();

                string IDU1 = "";
                string getUserId = $"SELECT UserID FROM REGISTRATION WHERE UserName like '{loginUser}'";
                SqlCommand createCommand3 = new SqlCommand(getUserId, sqlConnection);
                if(createCommand3.ExecuteScalar() != null) 
                {
                    IDU1 = createCommand3.ExecuteScalar().ToString();

                    int IDU = Convert.ToInt32(IDU1);

                    string queryString = $"select UserID,UserName,PasswordUser from REGISTRATION where UserName = '{loginUser}' and PasswordUser = '{passwordUser}'";

                    SqlCommand command = new SqlCommand(queryString, conn.GetConnection());

                    adapter.SelectCommand = command;
                    adapter.Fill(table);

                    if (table.Rows.Count == 1)
                    {
                        SeriesUser WinSer = new SeriesUser(IDU);
                        WinSer.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Такого юзера не существует");
                    }
                }
                else
                {
                    MessageBox.Show("Такого юзера не существует");
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        //Регистрация (Новое окно)
        private void Registration(object sender, RoutedEventArgs e)
        {
            RegistrationWin Register = new RegistrationWin();
            Register.Show();
            //Закрываем текущее окно
            this.Close();
        }
    }
}
