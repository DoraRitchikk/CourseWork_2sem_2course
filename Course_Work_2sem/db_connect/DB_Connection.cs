using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Course_Work_2sem
{

    class DB_Connection
    {
        //Подключение к БД и Таблице
        SqlConnection sqlConnection = new SqlConnection(@" Data Source = DESKTOP-H0E8CDQ\MSSQLSERVER01; Initial Catalog = YourSeries; Integrated Security = True");

        //Открытие связи с БД
        public void OpenConnection()
        {
            if(sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        //Закрытие связи с БД
        public void CloseConnection()
        {
            if(sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        //Получение состояния коннекта
        public SqlConnection GetConnection()
        {
            return sqlConnection;
        }
    }

}
