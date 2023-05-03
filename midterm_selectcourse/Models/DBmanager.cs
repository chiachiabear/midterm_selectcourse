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


        public List<Student> GetStudents(string s)
        {
            List<Student> students = new List<Student>();  // 創建一個 Card 對象列表。
            SqlConnection sqlConnection = new SqlConnection(ConnStr); // 創建一個 SqlConnection 對象，使用 ConnStr 字符串指定的連接字符串進行初始化。
            SqlCommand sqlCommand = new SqlCommand(@"SELECT * FROM student where student_ID=@s");// 創建一個 SqlCommand 對象，它表示要在數據庫中執行的命令，本例中是選擇所有 card 表中的數據
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@s", s));
            sqlConnection.Open();// 打開 SqlConnection 對象。

            SqlDataReader reader = sqlCommand.ExecuteReader(); // 創建一個 SqlDataReader 對象，它可以讀取從數據庫返回的行。
            if (reader.HasRows)
            {
                while (reader.Read())// 循環讀取 SqlDataReader 對象中的每一行，並為每個行創建一個 Card 對象。
                {
                    Student student = new Student
                    {
                        Student_id = reader.GetString(reader.GetOrdinal("student_ID")),
                        Department_name = reader.GetString(reader.GetOrdinal("department_name")),
                        Account_number = reader.GetString(reader.GetOrdinal("account_number")),
                        Grade = reader.GetInt32(reader.GetOrdinal("grade")),
                        Class_name = reader.GetString(reader.GetOrdinal("class_name")),
                        Student_name = reader.GetString(reader.GetOrdinal("student_name")),
                        Student_grade = reader.GetInt32(reader.GetOrdinal("student_grade")),
                        Total_credits = reader.GetInt32(reader.GetOrdinal("total_credits")),

                    };
                    students.Add(student);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！");
            }
            sqlConnection.Close();
            return students;
        }
        public int LoginVerify(string account_number, string secret_code)
        {
           
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT * FROM account WHERE account_number=@account_number AND secret_code=@secret_code");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@account_number", account_number));
            sqlCommand.Parameters.Add(new SqlParameter("@secret_code", secret_code));
            sqlConnection.Open();// 打開 SqlConnection 對象。


            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                System.Diagnostics.Debug.WriteLine("有找到帳號");
                return 1;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("沒找到");
                return 0;
            }
            sqlConnection.Close();
        }


    }
}