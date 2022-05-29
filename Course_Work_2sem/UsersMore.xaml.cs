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
    /// Логика взаимодействия для UsersMore.xaml
    /// </summary>
    public partial class UsersMore : Window
    {
        int idSeries = 0;
        int idUser = 0;

        public UsersMore(string idUs,string idSer)
        {
            InitializeComponent();
            GetAll(idUs,idSer);
            idSeries = Convert.ToInt32(idSer);
            idUser = Convert.ToInt32(idUs);

            this.MaxWidth = 600;
            this.MaxHeight = 550;

            this.MinWidth = 600 ;
            this.MinHeight = 450 ;
        }
        public void GetAll(string idU,string ids)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                sqlConnection.Open();
                //Добавление данных Юзера
                string cmd = $"SELECT NumOfSesaon,NumOfViewEpisodes,NumOfEpisodes from UserEpisodes Inner Join Episodes ON UserEpisodes.idSeries = Episodes.IdSeries where UserEpisodes.idUser like '{idU}' and UserEpisodes.idSeries like '{ids}' and UserEpisodes.NumOfSesaon = Episodes.NumOfSeason order by NumOfSesaon";
                SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                createCommand.ExecuteNonQuery();

                SqlDataAdapter adapter = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("UserEpisodes");
                adapter.Fill(dt);
                ListOfAllSeries.ItemsSource = dt.DefaultView;

                string cmd2 = $"SELECT NameOfSeries,Category,CountryOfSeries,UserSeries.rating FROM SERIES Inner Join UserSeries ON SERIES.IDSeries = UserSeries.IdSeries where UserSeries.IdOfUser like '{idU}' and UserSeries.IdSeries like '{ids}' and SERIES.IDSeries like '{ids}'  ";

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
        //Добавляем один просмотренный эпизод
        private void AddEpisode(object sender, RoutedEventArgs e)
        {
            if (ListOfAllSeries.SelectedItems.Count == 1)
            {
                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlConnection.Open();

                var text = (ListOfAllSeries.SelectedItem as DataRowView)["NumOfViewEpisodes"].ToString();
                var text2 = (ListOfAllSeries.SelectedItem as DataRowView)["NumOfSesaon"].ToString();
                var text3 = (ListOfAllSeries.SelectedItem as DataRowView)["NumOfEpisodes"].ToString();
                int NewEpisodes = Convert.ToInt32(text);
                int maxEpisodes = Convert.ToInt32(text3);
                if (NewEpisodes < maxEpisodes)
                {
                    NewEpisodes = Convert.ToInt32(text) + 1;
                }
                else
                {
                    MessageBox.Show("Больше серий нет");
                }
                int NumOfSeason = Convert.ToInt32(text2);

                string cmd = $" UPDATE UserEpisodes SET NumOfViewEpisodes = {NewEpisodes} where UserEpisodes.idSeries like '{idSeries}' and UserEpisodes.idUser like '{idUser}' and UserEpisodes.NumOfSesaon like '{NumOfSeason}' ";

                SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                createCommand.ExecuteNonQuery();

                //Просмотренный ли полностью
                string avgSeries = $"SELECT sum(NumOfEpisodes) FROM Episodes where IdSeries like '{idSeries}'";
                SqlCommand createCommand1 = new SqlCommand(avgSeries, sqlConnection);
                string AvgSer = createCommand1.ExecuteScalar().ToString();

                string avgUsersSeries = $"SELECT sum(NumOfViewEpisodes) FROM UserEpisodes where idSeries like '{idSeries}' and idUser like '{idUser}'";
                SqlCommand createCommand2 = new SqlCommand(avgUsersSeries, sqlConnection);
                string AvgUserSer = createCommand2.ExecuteScalar().ToString();

                if (Convert.ToInt32(AvgSer) == Convert.ToInt32(AvgUserSer))
                {
                    string isW = $" UPDATE UserSeries set isWatched = 1 where IdSeries like '{idSeries}' and IdOfUser like '{idUser}' ";
                    SqlCommand createCommand3 = new SqlCommand(isW, sqlConnection);
                    createCommand3.ExecuteNonQuery();
                }


                string cmdd = $" UPDATE TheLast SET NumOftheLastSesaon = {NumOfSeason}, NumOfTheLastSer = {NewEpisodes} where IDUser like '{idUser}' and IDSer like '{idSeries}'";


                SqlCommand createCommandd = new SqlCommand(cmdd, sqlConnection);
                createCommandd.ExecuteNonQuery();

                sqlConnection.Close();

                GetAll(idUser.ToString(), idSeries.ToString());
            }
            else
            {
                MessageBox.Show("Вы не выбрали ни один элементы");
            }

        }

        //Отнимаем один просмотренный эпизод
        private void DeleteEpisodes(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListOfAllSeries.SelectedItems.Count == 1)
                {
                    try
                    {

                        SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                        SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        sqlConnection.Open();

                        var text = (ListOfAllSeries.SelectedItem as DataRowView)["NumOfViewEpisodes"].ToString();
                        var text2 = (ListOfAllSeries.SelectedItem as DataRowView)["NumOfSesaon"].ToString();
                        int NewEpisodes = Convert.ToInt32(text);
                        if (Convert.ToInt32(text) != 0)
                        {
                            NewEpisodes = Convert.ToInt32(text) - 1;
                        }
                        else
                        {
                            MessageBox.Show("Больше нечего удалять(");
                            NewEpisodes = Convert.ToInt32(text);
                        }
                        int NumOfSeason = Convert.ToInt32(text2);

                        string cmd = $" UPDATE UserEpisodes SET NumOfViewEpisodes = {NewEpisodes} where UserEpisodes.idSeries like '{idSeries}' and UserEpisodes.idUser like '{idUser}' and UserEpisodes.NumOfSesaon like '{NumOfSeason}' ";

                        SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                        createCommand.ExecuteNonQuery();


                        //Просмотренный ли полностью
                        string avgSeries = $"SELECT sum(NumOfEpisodes) FROM Episodes where IdSeries like '{idSeries}'";
                        SqlCommand createCommand1 = new SqlCommand(avgSeries, sqlConnection);
                        string AvgSer = createCommand1.ExecuteScalar().ToString();

                        string avgUsersSeries = $"SELECT sum(NumOfViewEpisodes) FROM UserEpisodes where idSeries like '{idSeries}' and idUser like '{idUser}'";
                        SqlCommand createCommand2 = new SqlCommand(avgSeries, sqlConnection);
                        string AvgUserSer = createCommand2.ExecuteScalar().ToString();

                        if (Convert.ToInt32(AvgSer) == Convert.ToInt32(AvgUserSer))
                        {
                            string isW = $" UPDATE UserSeries set isWatched = 0 where IdSeries like '{idSeries}' and IdOfUser like '{idUser}' ";
                            SqlCommand createCommand3 = new SqlCommand(isW, sqlConnection);
                            createCommand3.ExecuteNonQuery();
                        }

                        //Получаем предыдущий сезон
                        string cmdq = $" SELECT TOP(1) NumOfSesaon FROM UserEpisodes where idUser like '{idUser}' and idSeries like '{idSeries}' and NumOfViewEpisodes > 0 order by NumOfSesaon desc ";
                        SqlCommand createCommandq = new SqlCommand(cmdq, sqlConnection);
                        string NewSes = createCommandq.ExecuteScalar().ToString();

                        //Получаем предыдущую серию сезона
                        string cmdd = $" SELECT TOP(1) NumOfViewEpisodes FROM UserEpisodes where idUser like '{idUser}' and idSeries like '{idSeries}' and NumOfViewEpisodes > 0 order by NumOfSesaon desc ";
                        SqlCommand createCommandd = new SqlCommand(cmdd, sqlConnection);
                        string NewSer = createCommandd.ExecuteScalar().ToString();

                        //Обновляем последнее просмотренное
                        string UPD = $" UPDATE TheLast SET NumOftheLastSesaon = {Convert.ToInt32(NewSes)}, NumOfTheLastSer = {Convert.ToInt32(NewSer)} where IDUser like '{idUser}' and IDSer like '{idSeries}'";
                        SqlCommand createCommandu = new SqlCommand(UPD, sqlConnection);
                        createCommandu.ExecuteNonQuery();


                        sqlConnection.Close();

                        GetAll(idUser.ToString(), idSeries.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Вы не выбрали ни один элемент");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Вы не выбрали ни один элемент");
            }
        }
       

        private void AddRating(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlConnection.Open();

                int URating = Convert.ToInt32(UsersRating.Text);
                if (URating > 0 && URating < 11 && URating % 1 == 0)
                {
                    string cmd = $" UPDATE UserSeries SET rating = {URating} where UserSeries.IdOfUser = '{idUser}' and UserSeries.IdSeries like '{idSeries}'";
                    SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                    createCommand.ExecuteNonQuery();

                    string getID = $"SELECT round(avg(UserSeries.rating),0) as RatingAvg FROM UserSeries Where UserSeries.IdSeries like '{idSeries}' and UserSeries.rating > 0";
                    SqlCommand createCommand2 = new SqlCommand(getID, sqlConnection);
                    string value = createCommand2.ExecuteScalar().ToString();
                    int avgRating = Convert.ToInt32(value);

                    string cmd2 = $"UPDATE SERIES SET rating = '{avgRating}' where SERIES.IDSeries like '{idSeries}' ";
                    SqlCommand createCommand4 = new SqlCommand(cmd2, sqlConnection);
                    createCommand4.ExecuteNonQuery();

                    sqlConnection.Close();

                    GetAll(idUser.ToString(), idSeries.ToString());
                    MessageBox.Show("Рейтинг выставлен");
                }
                else
                {
                    MessageBox.Show("Введите целочисленное значение от 1 до 10");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ведите целочисленное значение от 1 до 10");
            }
        }
    }
}
