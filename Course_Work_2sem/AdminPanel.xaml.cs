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
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        DB_Connection conn = new DB_Connection();
        public static bool isOpened = false;
        public AdminPanel()
        {
            InitializeComponent();
            this.MaxWidth = 800;
            this.MinHeight = 470;
        }


        private void AddNewSeries(object sender, RoutedEventArgs e)
        {
            try
            {
                string NameOfSeries = NameS.Text;
                string categor = Categor.Text;
                string CountryOfSer = Country.Text;
                string YearRelize = Year.Text;
                string imageSRC = "https://tsum.by/upload/no-photo.png";
                if (NameOfSeries.Length > 0 && categor.Length > 0 && CountryOfSer.Length > 0 && YearRelize.Length > 0 && imageSRC.Length > 0)
                {
                    if (ImageSer.Text.Length > 0)
                    {
                        imageSRC = ImageSer.Text;
                    }
                    //Передаём информацию в БД
                    string queryString = $"Insert into SERIES (NameOfSeries,Category,CountryOfSeries,yearValue,imageSeries) values('{NameOfSeries}','{categor}','{CountryOfSer}','{YearRelize}','{imageSRC}')";

                    SqlCommand command = new SqlCommand(queryString, conn.GetConnection());

                    conn.OpenConnection();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Регистрация прошла успешно");
                    }
                    else
                    {
                        MessageBox.Show("Проблемы с подключением...");
                    }
                    conn.CloseConnection();

                    MessageBox.Show("Успешно добавлено");
                }
                else
                {
                    MessageBox.Show("Вы ввели не все поля");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Проверьте правильномсть введённых данных");
            }
        }

        private void AddSeasonAndEpisode(object sender, RoutedEventArgs e)
        {
            try
            {

                int SeasonB = Convert.ToInt32(Season.Text);
                int NumEpisodes = Convert.ToInt32(NumSeries.Text);
                int IdSeri = Convert.ToInt32(IdSer.Text);

                if (SeasonB.ToString().Length > 0 && NumEpisodes.ToString().Length > 0 && IdSeri.ToString().Length > 0)
                {
                    //Передаём информацию в БД
                    string queryString = $"Insert into Episodes (IdSeries,NumOfSeason,NumOfEpisodes) values('{IdSeri}','{SeasonB}','{NumEpisodes}')";

                    SqlCommand command = new SqlCommand(queryString, conn.GetConnection());

                    conn.OpenConnection();
                    if (command.ExecuteNonQuery() == 1)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Проблемы с подключением...");
                    }

                    MessageBox.Show("Успешно добавлено");

                    conn.CloseConnection();
                }
                else
                {
                    MessageBox.Show("Вы заполнили не все поля");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Проверьте правильномсть введённых данных");
            }
        }

        private void AddPremier(object sender, RoutedEventArgs e)
        {
            try
            {

                //Передаём информацию в БД
                string queryString = $"Insert into Premier (NameOfPremierSeries,CategoruPremier,CountryPremier,dateOfPremier,NumOfSeries) values('{PremierName.Text}','{CategoryPremier.Text}','{CountryPremier.Text}','{DatePremier.Text}','{Convert.ToInt32(NumOfSeriesPremier.Text)}')";

                SqlCommand command = new SqlCommand(queryString, conn.GetConnection());

                conn.OpenConnection();
                if (command.ExecuteNonQuery() == 1)
                {

                }
                else
                {
                    MessageBox.Show("Проблемы с подключением...");
                }
                conn.CloseConnection();

                MessageBox.Show("Успешно добавлено");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Проверьте правильномсть введённых данных");
            }
        }

        private void ChangeItem(object sender, RoutedEventArgs e)
        {
            try
            {
                int IdSeries = Convert.ToInt32(getID.Text);
                string NewNameDB = NewName.Text;
                string NewCatDB = NewCat.Text;
                string NewYearDB = NewYear.Text;
                string NewCountryDB = NewCountry.Text;
                string Newimage = NewImage.Text;


                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlConnection.Open();

                string cmd = $" UPDATE SERIES SET NameOfSeries = '{NewNameDB}',Category = '{NewCatDB}',CountryOfSeries = '{NewCountryDB}',yearValue = '{NewYearDB}',imageSeries = '{Newimage}' where IDSeries like '{IdSeries}'";


                SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                createCommand.ExecuteNonQuery();

                MessageBox.Show("Успешно изменено");
                sqlConnection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Проверьте правильномсть введённых данных");
            }
        }

        private void getInfByID(object sender, RoutedEventArgs e)
        {
            try
            {
                int IdSeries = Convert.ToInt32(getID.Text);

                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlConnection.Open();

                string name = $"SELECT NameOfSeries FROM SERIES where IDSeries like '{IdSeries}'";
                SqlCommand createCommand = new SqlCommand(name, sqlConnection);
                string name2 = createCommand.ExecuteScalar().ToString();
                NewName.Text = name2;

                string cat = $"SELECT Category FROM SERIES where IDSeries like '{IdSeries}'";
                SqlCommand createCommand1 = new SqlCommand(cat, sqlConnection);
                string cat2 = createCommand1.ExecuteScalar().ToString();
                NewCat.Text = cat2;

                string country = $"SELECT CountryOfSeries FROM SERIES where IDSeries like '{IdSeries}'";
                SqlCommand createCommand2 = new SqlCommand(country, sqlConnection);
                string country2 = createCommand2.ExecuteScalar().ToString();
                NewCountry.Text = country2;

                string year = $"SELECT yearValue FROM SERIES where IDSeries like '{IdSeries}'";
                SqlCommand createCommand3 = new SqlCommand(year, sqlConnection);
                string year2 = createCommand3.ExecuteScalar().ToString();
                NewYear.Text = year2;

                string img = $"SELECT imageSeries FROM SERIES where IDSeries like '{IdSeries}'";
                SqlCommand createCommand6 = new SqlCommand(img, sqlConnection);
                string img2 = createCommand6.ExecuteScalar().ToString();
                NewImage.Text = img2;

                sqlConnection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Проверьте правильномсть введённых данных");
            }
        }

        private void ChangeEpisodes(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NewNumSeries.Text != "" && NewSeason.Text != "")
                {
                    int IdSeries = Convert.ToInt32(getID.Text);
                    int NewSeas = Convert.ToInt32(NewSeason.Text);
                    int NumEpis = Convert.ToInt32(NewNumSeries.Text);

                    SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    sqlConnection.Open();


                    string cmd = $" UPDATE Episodes SET NumOfSeason = {NewSeas},NumOfEpisodes = {NumEpis} where IdSeries like '{IdSeries}'";


                    SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                    createCommand.ExecuteNonQuery();
                    MessageBox.Show("Успешно изменено");

                    sqlConnection.Close();
                }
                else
                {
                    MessageBox.Show("Заполните поля");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Проверьте правильномсть введённых данных");
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isOpened = false;
        }
    }
}
