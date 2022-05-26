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
    /// Логика взаимодействия для MoreInformation.xaml
    /// </summary>
    public partial class MoreInformation : Window
    {

        public MoreInformation(string text)
        {
            InitializeComponent();
            GetAll(text);
        }

        public void GetAll(string c)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                sqlConnection.Open();

                string cmd = $"SELECT NumOfSeason,NumOfEpisodes FROM Episodes Inner Join SERIES ON Episodes.IdSeries = SERIES.IDSeries WHERE SERIES.NameOfSeries like '{c}'";

                SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                createCommand.ExecuteNonQuery();

                SqlDataAdapter adapter = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("Episodes");
                adapter.Fill(dt);
                ListOfAllSeries.ItemsSource = dt.DefaultView;




                string cmd2 = $"SELECT NameOfSeries,Category,CountryOfSeries,rating ,yearValue FROM SERIES WHERE NameOfSeries like '{c}' ";

                SqlCommand createCommand2 = new SqlCommand(cmd2, sqlConnection);
                createCommand.ExecuteNonQuery();

                SqlDataAdapter adapter2 = new SqlDataAdapter(createCommand2);
                DataTable dt2 = new DataTable("SERIES");
                adapter2.Fill(dt2);
                ListOfAllInf.ItemsSource = dt2.DefaultView;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 

 /*       private void LoadMoreInfWin(object sender, RoutedEventArgs e)
        {

        }*/
    }
}
