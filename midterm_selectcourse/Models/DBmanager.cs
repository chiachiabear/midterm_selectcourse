using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace midterm_selectcourse.Models
{
    public class DBmanager
    {
        private readonly string ConnStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=selectcourse;Integrated Security=True";

        public void LoginVerify(string account_number, string secret_code)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT * FROM card WHERE account_number=@account_number AND secret_code=@secret_code");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@account_number", account_number));
            sqlCommand.Parameters.Add(new SqlParameter("@secret_code", secret_code));
            sqlConnection.Open();// 打開 SqlConnection 對象。

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                System.Diagnostics.Debug.WriteLine("有找到帳號");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("沒找到");
            }
            sqlConnection.Close();
        }


    }
}