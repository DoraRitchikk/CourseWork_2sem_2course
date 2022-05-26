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
using System.Data.SqlClient;
using System.Data;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Course_Work_2sem
{
    /// <summary>
    /// Логика взаимодействия для SeriesUser.xaml
    /// </summary>
    public partial class SeriesUser : Window
    {
        public static SeriesUser SeriesU;
        DB_Connection conn = new DB_Connection();
        int IDU = 1;
        List<string> AllNames = new List<string>();
        public SeriesUser(int IDU1)
        {
            InitializeComponent();
            IDU = IDU1;

            AllSeries();

            ListOfAllSeries.SelectedIndex = 0;

            DataContext = new ApplicationsViewModel();

            SeriesU = this;

            SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            sqlConnection.Open();

            string cmd = $"SELECT isAdmin from REGISTRATION where UserID like '{IDU}'";


            SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
            createCommand.ExecuteNonQuery();


            string z = createCommand.ExecuteScalar().ToString();

            sqlConnection.Close();

            if (Convert.ToInt32(z) == 1)
            {
                PanelAdmin.Visibility = Visibility.Visible;
            }
            foreach (var n in ListOfAllSeries.Items)
            {
                AllNames.Add((n as DataRowView)["NameOfSeries"].ToString());
            }


            this.MinHeight = 700;
            this.MinWidth = 900;
        }

        public SeriesUser()
        {
            InitializeComponent();

        }

        //Перейти к списку своих сериалов 
        public void MySeries()
        {
            RightPabelMySeries.Visibility = Visibility.Visible;
            rightPanelSeries.Visibility = Visibility.Hidden;


            try
            {
                ListOfPremier.Visibility = Visibility.Hidden;
                SearchPanel.Visibility = Visibility.Hidden;
                PanelOnUsersSeries.Visibility = Visibility.Visible;
                ListOfAllSeries.Visibility = Visibility.Hidden;
                ListOfUserSeries.Visibility = Visibility.Visible;
                ListOfPremierUset.Visibility = Visibility.Hidden;   
                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlConnection.Open();

                string cmd = $"SELECT NameOfSeries,Category,UserSeries.rating,imageSeries,NumOfTheLastSesaon,NumOfTheLastSer FROM SERIES Inner Join TheLast ON SERIES.IDSeries = TheLast.IDSer Inner Join UserSeries ON SERIES.IDSeries = UserSeries.IdSeries Inner Join Registration ON UserSeries.IdOfUser = REGISTRATION.UserId where REGISTRATION.UserID like '{IDU}' and UserSeries.isWatched like 0 and TheLast.IDUser like '{IDU}'";

                SqlCommand createCommand = new SqlCommand(cmd.ToString(), sqlConnection);
                createCommand.ExecuteNonQuery(); 
                
                SqlDataAdapter adapter = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("SERIES");
                adapter.Fill(dt);

                ListOfUserSeries.ItemsSource = dt.DefaultView;



                //Получение изображения
                string cmd2 = $"SELECT photoOfuser FROM REGISTRATION where REGISTRATION.UserID like '{IDU}'";
                SqlCommand createCommand2 = new SqlCommand(cmd2, sqlConnection);
                string PathToImage = createCommand2.ExecuteScalar().ToString();
                imgUser.Source = new BitmapImage(new Uri(PathToImage));
                //Полученеие имени юзера
                string cmd4 = $"SELECT UserName FROM REGISTRATION where REGISTRATION.UserID like '{IDU}'";
                SqlCommand createCommand4 = new SqlCommand(cmd4, sqlConnection);
                string NameOfUser = createCommand4.ExecuteScalar().ToString();
                NameuserBpx.Text = NameOfUser;

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Просмотр списка других сериалов
        public void AllSeries()
        {
            RightPabelMySeries.Visibility = Visibility.Hidden;
            rightPanelSeries.Visibility = Visibility.Visible;

            ListOfPremier.Visibility = Visibility.Hidden;
            PanelOnUsersSeries.Visibility = Visibility.Hidden;
            SearchPanel.Visibility = Visibility.Visible;
            ListOfUserSeries.Visibility = Visibility.Hidden;
            ListOfPremierUset.Visibility = Visibility.Hidden;
            ListOfAllSeries.Visibility = Visibility.Visible;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlConnection.Open();

                string cmd = "SELECT NameOfSeries,Category,rating,imageSeries FROM SERIES ";

                SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                createCommand.ExecuteNonQuery();

                SqlDataAdapter adapter = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("SERIES");
                adapter.Fill(dt);
                ListOfAllSeries.ItemsSource = dt.DefaultView;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Окно "Мой аккаунт"
        public void MyAccount()
        { 
            MyAccount myAcc = new MyAccount(IDU);
            myAcc.Show();
        }

        //Премьеры
        public void Premier()
        {
            ListOfPremier.Visibility = Visibility.Visible;
            ListOfAllSeries.Visibility = Visibility.Hidden;
            ListOfUserSeries.Visibility = Visibility.Hidden;
            ListOfPremierUset.Visibility = Visibility.Hidden;

            try
            {
                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlConnection.Open();

                string cmd = "SELECT NameOfPremierSeries,CategoruPremier,CountryPremier,dateOfPremier,NumOfSeries FROM Premier";

                SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                createCommand.ExecuteNonQuery();

                SqlDataAdapter adapter = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("Premier");
                adapter.Fill(dt);
                ListOfPremier.ItemsSource = dt.DefaultView;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //Выход из учётной записи
        public void ExitEnter()
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        //Панель админа
        public void ControlItems()
        {
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
        }

        //Подробнее обо всех сериалах
        private void MoreInfClick(object sender, RoutedEventArgs e)
        {
            if (ListOfAllSeries.SelectedItems.Count == 1)
            {
                var text = (ListOfAllSeries.SelectedItem as DataRowView)["NameOfSeries"].ToString();
                MoreInformation moreWin = new MoreInformation(text);
                moreWin.Show();
            }
            else
            {
                MessageBox.Show("Вы не выбрали ни один элемент");
            }
        }
        
        //Добавить новый сериал в "Мои сериалы"
        private void AddNewSeries(object sender, RoutedEventArgs e)
        {
            if (ListOfAllSeries.SelectedItems.Count > 0)
            {
                var text = (ListOfAllSeries.SelectedItem as DataRowView)["NameOfSeries"].ToString();
                try
                {
                    SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    sqlConnection.Open();


                    string getID = $"SELECT IDSeries FROM SERIES Where NameOfSeries like '{text}'";
                    SqlCommand createCommand2 = new SqlCommand(getID, sqlConnection);
                    string value = createCommand2.ExecuteScalar().ToString();
                    int idSer = Convert.ToInt32(value);


                    string isAdd = $"SELECT IsAdded FROM UserSeries Where IdSeries like '{idSer}' and IdOfUser like '{IDU}'";
                    SqlCommand createCommand3 = new SqlCommand(isAdd, sqlConnection);
                    int isAdde = 0;
                    int i = Convert.ToInt32(createCommand3.ExecuteScalar());

                    if (i != 0)
                    {
                        string isAdded = createCommand3.ExecuteScalar().ToString();
                        isAdde = Convert.ToInt32(isAdded);
                    }
                    if (isAdde == 1)
                    {
                        MessageBox.Show("Сериал уже добавлен");
                    }
                    else
                    {
                        string getUserId = $"SELECT UserID FROM REGISTRATION WHERE UserID like '{IDU}'";
                        SqlCommand createCommand4 = new SqlCommand(getUserId, sqlConnection);
                        string value2 = createCommand4.ExecuteScalar().ToString();
                        int IDUs = Convert.ToInt32(value2);
                        sqlConnection.Close();
                        sqlConnection.Open();

                        int IsAd = 1;

                        string cmd = $" Insert  into UserSeries(IdSeries,IdOfUser,IsAdded) values ('{idSer}','{IDUs}','{IsAd}')" +
                            $"          Insert into UserEpisodes(idSeries,idUser,NumOfSesaon)" +
                            $"Select UserSeries.IdSeries,UserSeries.IdOfUser,Episodes.NumOfSeason FROM UserSeries,Episodes where UserSeries.IdSeries like '{idSer}' and UserSeries.IdOfUser like '{IDUs}' and Episodes.IdSeries like '{idSer}' ";

                        SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                        createCommand.ExecuteNonQuery();



                        string cmd2 = $" Insert into TheLast(IDUser,IDSer) values('{IDUs}','{idSer}')";
                        SqlCommand createCommand6 = new SqlCommand(cmd2, sqlConnection);
                        createCommand6.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите элемент из списка");
            }
        }
        
        //По 4 категориям
        public void ForCategoryFour(string filter1, string filter2, string filter3,string filter4)
        {
            SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            sqlConnection.Open();

            string cmd = $"SELECT NameOfSeries,Category,rating,imageSeries FROM SERIES " + filter1 + filter2 + filter3 + filter4;

            SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
            createCommand.ExecuteNonQuery();

            SqlDataAdapter adapter = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable("SERIES");
            adapter.Fill(dt);
            ListOfAllSeries.ItemsSource = dt.DefaultView;
        }
        
        //По 3 категориям
        public void ForCategoryThree(string filter1,string filter2,string filter3)
        {
            SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            sqlConnection.Open();

            string cmd = $"SELECT NameOfSeries,Category,rating,imageSeries FROM SERIES " + filter1 + filter2 + filter3;

            SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
            createCommand.ExecuteNonQuery();

            SqlDataAdapter adapter = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable("SERIES");
            adapter.Fill(dt);
            ListOfAllSeries.ItemsSource = dt.DefaultView;
        }
        
        //По 2 категориям
        public void ForCategoryTwo(string filter1, string filter2)
        {
            SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            sqlConnection.Open();

            string cmd = $"SELECT NameOfSeries,Category,rating,imageSeries FROM SERIES  " + filter1 + filter2;

            SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
            createCommand.ExecuteNonQuery();

            SqlDataAdapter adapter = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable("SERIES");
            adapter.Fill(dt);
            ListOfAllSeries.ItemsSource = dt.DefaultView;
        }
        
        //По 1 категории
        public void ForCategoryOne(string filter1)
        {
            SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            sqlConnection.Open();

            string cmd = $"SELECT NameOfSeries,Category,rating,imageSeries FROM SERIES " + filter1;

            SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
            createCommand.ExecuteNonQuery();

            SqlDataAdapter adapter = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable("SERIES");
            adapter.Fill(dt);
            ListOfAllSeries.ItemsSource = dt.DefaultView;
        }
        
        //Поиск по категориям
        public void SearchForCategory()
        {
            try
            {
                string cmd1 = "";
                string cmd2 = "";
                string cmd3 = "";
                string cmd4 = "";
                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlConnection.Open();
                int z = 0;
                if (BoxWithCategoryFilter.SelectedIndex > -1)
                {
                    string category = BoxWithCategoryFilter.Text;
                    cmd1 = $"where Category like '{category}' ";
                    z++;
                }
                if (FilterForCountry.SelectedIndex > -1)
                {
                    string country = FilterForCountry.Text;
                    if (cmd1 == "")
                    {
                        cmd1 = $"CountryOfSeries like '{country}'";
                    }
                    else
                    {
                        cmd2 = $"and CountryOfSeries like '{country}'";
                    }
                    z++;
                }
                
                if (YearFilter.Text != "")
                {
                    int year = Convert.ToInt32(YearFilter.Text);
                    if (cmd1 == "")
                    {
                        cmd1 = $"where yearValue like '{year}'";
                    }
                    else if(cmd2 == "")
                    {
                        cmd2 = $"and yearValue like '{year}'";
                    }
                    else
                    {
                        cmd3 = $"and yearValue like '{year}'";
                    }
                    z++;
                }

                if(MaxMin.IsChecked == true)
                {
                    if (cmd1 == "")
                    {
                        cmd1 = $"Order by rating desc";
                    }
                    else if (cmd2 == "")
                    {
                        cmd2 = $"Order by rating desc";
                    }
                    else if(cmd3 == "")
                    {
                        cmd3 = $"Order by rating desc";
                    }
                    else
                    {
                        cmd4 = $"Order by rating desc";
                    }
                    z++;
                }
                if(MinMax.IsChecked == true)
                {
                    if (cmd1 == "")
                    {
                        cmd1 = $"Order by rating asc";
                    }
                    else if (cmd2 == "")
                    {
                        cmd2 = $"Order by rating asc";
                    }
                    else if (cmd3 == "")
                    {
                        cmd3 = $"Order by rating asc";
                    }
                    else
                    {
                        cmd4 = $"Order by rating asc";
                    }
                    z++;
                }
                if (BoxWithCategoryFilter.SelectedIndex == -1 && FilterForCountry.SelectedIndex == -1 && YearFilter.Text == "" && MinMax.IsChecked == false && MaxMin.IsChecked == false)
                {
                    MessageBox.Show("Вы не выбрали ни одну категорию");
                }

                if(z == 4)
                {
                    ForCategoryFour(cmd1,cmd2,cmd3,cmd4);
                }
                else if(z == 3)
                {
                    ForCategoryThree(cmd1,cmd2,cmd3);
                }
                else if(z == 2)
                {
                    ForCategoryTwo(cmd1, cmd2);
                }
                else if(z == 1)
                {
                    ForCategoryOne(cmd1);
                }

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        //Подробнее для Юзера
        private void MoreInfClickForUser(object sender, RoutedEventArgs e)
        {
            if (ListOfUserSeries.SelectedItems.Count == 1)
            {
                var text = (ListOfUserSeries.SelectedItem as DataRowView)["NameOfSeries"].ToString();
                //Получаем ID юзера и сериала
                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlConnection.Open();

                //Получаем ID сериала
                string cmd2 = $"SELECT IDSeries from SERIES where NameOfSeries like '{text}'";

                SqlCommand createCommand2 = new SqlCommand(cmd2, sqlConnection);
                createCommand2.ExecuteNonQuery();

                string idSer = createCommand2.ExecuteScalar().ToString();

                sqlConnection.Close();

                UsersMore moreU = new UsersMore(IDU.ToString(), idSer);
                moreU.Show();
            }
            else
            {
                MessageBox.Show("Вы не выбрали ни один элемент");
            }
        }

        //Добавить премьеру в список своих
        private void AddPremier(object sender, RoutedEventArgs e)
        {
            if (ListOfPremier.SelectedItems.Count != 0)
            {
                var text = (ListOfPremier.SelectedItem as DataRowView)["NameOfPremierSeries"].ToString();
                try
                {

                    SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    sqlConnection.Open();

                    string cmd2 = $"SELECT PremierId FROM UserPremier Inner Join Premier ON UserPremier.PremierId = Premier.id where Premier.NameOfPremierSeries like '{text}' and UserId like '{IDU}'";
                    SqlCommand createCommand2 = new SqlCommand(cmd2, sqlConnection);
                    int i = Convert.ToInt32(createCommand2.ExecuteScalar());
                    if (i != 0)
                    {
                        MessageBox.Show("Премьера уже добавлена");
                    }
                    else
                    {
                        string getPremierId = $"SELECT id FROM Premier WHERE NameOfPremierSeries like '{text}'";
                        SqlCommand createCommand3 = new SqlCommand(getPremierId, sqlConnection);
                        string premierID = createCommand3.ExecuteScalar().ToString();
                        int IdPremier = Convert.ToInt32(premierID);

                        string cmd = $" Insert into UserPremier(UserId,PremierID) values('{IDU}','{IdPremier}')";

                        SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                        createCommand.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите элемент из списка");
            }
        }

        //Просмотренные сериалы
        private void ViewsSeries(object sender, RoutedEventArgs e)
        {
            ListOfPremierUset.Visibility = Visibility.Hidden;
            ListOfUserSeries.Visibility = Visibility.Visible;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlConnection.Open();

                string cmd =$"SELECT NameOfSeries,Category,UserSeries.rating,imageSeries,NumOfTheLastSesaon,NumOfTheLastSer FROM SERIES Inner Join TheLast ON SERIES.IDSeries = TheLast.IDSer Inner Join UserSeries ON SERIES.IDSeries = UserSeries.IdSeries Inner Join Registration ON UserSeries.IdOfUser = REGISTRATION.UserId where REGISTRATION.UserID like '{IDU}' and UserSeries.isWatched like 1 and TheLast.IDUser like '{IDU}'";

                SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                createCommand.ExecuteNonQuery();

                SqlDataAdapter adapter = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("SERIES");
                adapter.Fill(dt);
                ListOfUserSeries.ItemsSource = dt.DefaultView;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MyPremier(object sender, RoutedEventArgs e)
        {
            ListOfPremier.Visibility = Visibility.Hidden;
            ListOfAllSeries.Visibility = Visibility.Hidden;
            ListOfUserSeries.Visibility = Visibility.Hidden;

            RightPabelMySeries.Visibility = Visibility.Visible;
            rightPanelSeries.Visibility = Visibility.Hidden;

            ListOfPremierUset.Visibility = Visibility.Visible;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlConnection.Open();


                    string cmd = $"SELECT NameOfPremierSeries,CategoruPremier,CountryPremier,dateOfPremier,NumOfSeries FROM Premier Inner Join UserPremier ON Premier.id = UserPremier.PremierId where UserId like '{IDU}'";

                    SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                    createCommand.ExecuteNonQuery();

                    SqlDataAdapter adapter = new SqlDataAdapter(createCommand);
                    DataTable dt = new DataTable("Premier");
                    adapter.Fill(dt);
                    ListOfPremierUset.ItemsSource = dt.DefaultView;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CleanFilter(object sender, RoutedEventArgs e)
        {
            MinMax.IsChecked = false;
            MaxMin.IsChecked = false;
            BoxWithCategoryFilter.SelectedIndex = -1;
            FilterForCountry.SelectedIndex = -1;
            YearFilter.Text = "";

            
        }

        private void ChangeMethod(object sender, TextChangedEventArgs e)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                sqlConnection.Open();

                Regex regex = new Regex(SearchBoxText.Text.ToLower());
                string AllName = $"SELECT count(NameOfSeries) FROM SERIES ";
                SqlCommand createCom = new SqlCommand(AllName, sqlConnection);
                var i = Convert.ToInt32(createCom.ExecuteScalar());


                List<string> ListOfMatched = new List<string>();
                foreach (var t in AllNames)
                {
                    if (regex.IsMatch(t.ToLower()))
                    {
                        ListOfMatched.Add(t);
                    }
                }
                if (ListOfMatched.Count > 0)
                {
                    for (int v = 0; v < ListOfMatched.Count; v++)
                    {

                        string cmd2 = $" '{ListOfMatched[0]} ' ";
                        if (ListOfMatched.Count == 1)
                        {
                            cmd2 = $" '{ListOfMatched[v]}' ";
                        }
                        else
                        {
                            cmd2 = cmd2 + $" ,'{ListOfMatched[v]}' ";
                        }
                        string cmd = $"SELECT NameOfSeries,Category,imageSeries,rating FROM SERIES where NameOfSeries IN (" + cmd2 + ")";

                        SqlCommand createCommand = new SqlCommand(cmd, sqlConnection);
                        SqlDataAdapter adapter = new SqlDataAdapter(createCommand);
                        DataTable dt = new DataTable("SERIES");
                        adapter.Fill(dt);
                        ListOfAllSeries.ItemsSource = dt.DefaultView;
                        sqlConnection.Close();
                    }
                }
                else 
                {
                    ListOfAllSeries.ItemsSource = null;
                }
            }
            catch(Exception exc)
            {
            }
        }
    }

}

