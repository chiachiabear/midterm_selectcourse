using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using midterm_selectcourse.ViewModels;
using System.Web.Services.Description;
using System.Diagnostics;

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
                sqlConnection.Close();
                return 1;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("沒找到");
                sqlConnection.Close();
                return 0;
            }
            
        }
        public List<CurrentCurriculum> GetStudentsCurriculum(string student_id)
        {
            List<CurrentCurriculum> CCs = new List<CurrentCurriculum>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr); // 創建一個 SqlConnection 對象，使用 ConnStr 字符串指定的連接字符串進行初始化。
            SqlCommand sqlCommand = new SqlCommand(@"SELECT course.course_ID, course.course_name, course.sort, course.credits,
                                                            set_up.department_name, set_up.grade, set_up.class_name,
                                                            teacher.teacher_name,
                                                            classroom.building, classroom.room_id, classroom.capacity,
                                                            section.section_id, section.weekday, section.start_time, section.end_time 
                                                    FROM (
                                                            (
                                                                (
                                                                    (
                                                                        (
                                                                            (
                                                                                (student
                                                                                INNER JOIN learn
                                                                                ON student.student_ID = learn.student_ID)
                                                                            INNER JOIN course
                                                                            ON learn.course_ID = course.course_ID)
                                                                        INNER JOIN set_up
                                                                        ON course.course_ID = set_up.course_ID)
                                                                    INNER JOIN teaches
                                                                    ON course.course_ID = teaches.course_ID)
                                                                INNER JOIN teacher
                                                                ON teaches.teacher_ID = teacher.teacher_ID)
                                                            INNER JOIN occurred_in
                                                            ON course.course_ID = occurred_in.course_ID)
                                                        INNER JOIN classroom
                                                        ON occurred_in.building = classroom.building AND occurred_in.room_ID = classroom.room_ID)
                                                    INNER JOIN section
                                                    ON occurred_in.section_ID = section.section_ID
                                                    WHERE student.student_ID = @student_id
                                                 ");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@student_id", student_id));
            sqlConnection.Open();// 打開 SqlConnection 對象。

            SqlDataReader reader = sqlCommand.ExecuteReader(); // 創建一個 SqlDataReader 對象，它可以讀取從數據庫返回的行。
            if (reader.HasRows)
            {
                while (reader.Read())// 循環讀取 SqlDataReader 對象中的每一行，並為每個行創建一個 Card 對象。
                {
                    CurrentCurriculum CC = new CurrentCurriculum();
                    CC.Course.Course_id = reader.GetInt32(reader.GetOrdinal("course_ID"));
                    CC.Course.Course_name = reader.GetString(reader.GetOrdinal("course_name"));
                    CC.Course.Sort = reader.GetString(reader.GetOrdinal("sort"));
                    CC.Course.Credits = reader.GetInt32(reader.GetOrdinal("credits"));

                    CC.Set_up.Department_name = reader.GetString(reader.GetOrdinal("department_name"));
                    CC.Set_up.Grade = reader.GetInt32(reader.GetOrdinal("grade"));
                    CC.Set_up.Class_name = reader.GetString(reader.GetOrdinal("class_name"));

                    CC.Teacher_name = reader.GetString(reader.GetOrdinal("teacher_name"));

                    CC.Classroom.Building = reader.GetString(reader.GetOrdinal("building"));
                    CC.Classroom.Room_id = reader.GetString(reader.GetOrdinal("room_id"));
                    CC.Classroom.Capacity = reader.GetInt32(reader.GetOrdinal("capacity"));

                    CC.Section.Section_id = reader.GetInt32(reader.GetOrdinal("section_ID"));
                    CC.Section.Weekday = reader.GetString(reader.GetOrdinal("weekday"));
                    CC.Section.Start_time = reader.GetTimeSpan(reader.GetOrdinal("start_time"));
                    CC.Section.End_time = reader.GetTimeSpan(reader.GetOrdinal("end_time"));

                    //CC.Student_amount = reader.GetInt32(reader.GetOrdinal("student_amount"));
                    CCs.Add(CC);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！");
            }
            sqlConnection.Close();

            //新增一組Command跟Connection
            SqlConnection sqlConnection2 = new SqlConnection(ConnStr);
            SqlCommand sqlCommand2= new SqlCommand(@"SELECT COUNT(student.student_ID)　as count
                                                    FROM student
                                                    INNER JOIN learn
                                                    ON student.student_ID = learn.student_ID
                                                    WHERE student.student_ID = @student_id
                                                    GROUP BY learn.course_ID 
                                                  ");
            sqlCommand2.Connection = sqlConnection2;
            sqlCommand2.Parameters.Add(new SqlParameter("@student_id", student_id));
            sqlConnection2.Open();
            
            SqlDataReader reader2 = sqlCommand2.ExecuteReader();
            List<int> counts = new List<int>();
            if (reader2.HasRows)
            {
                while(reader2.Read())
                {
                    int temp = new int();
                    temp = reader2.GetInt32(reader2.GetOrdinal("count"));
                    
                    counts.Add(temp);
                }
            }
            sqlConnection2.Close();
            for (int i = 0; i < CCs.Count; i++)
            {
                CCs[i].Student_amount = counts[i];
            }

            return CCs;
        }


        public List<Learns> GetLearn_Nows(string s)
        {
            List<Learns> learns = new List<Learns>();  // 創建一個 Card 對象列表。
            SqlConnection sqlConnection = new SqlConnection(ConnStr); // 創建一個 SqlConnection 對象，使用 ConnStr 字符串指定的連接字符串進行初始化。
            SqlCommand sqlCommand = new SqlCommand(@"SELECT * FROM learn WHERE student_ID=@s");// 創建一個 SqlCommand 對象，它表示要在數據庫中執行的命令，本例中是選擇所有 card 表中的數據
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@s", s));
            sqlConnection.Open();// 打開 SqlConnection 對象。

            SqlDataReader reader = sqlCommand.ExecuteReader(); // 創建一個 SqlDataReader 對象，它可以讀取從數據庫返回的行。
            if (reader.HasRows)
            {
                while (reader.Read())// 循環讀取 SqlDataReader 對象中的每一行，並為每個行創建一個 Card 對象。
                {
                    Learns learn = new Learns
                    {
                        Student_id = reader.GetString(reader.GetOrdinal("student_ID")),
                        Course_id = reader.GetInt32(reader.GetOrdinal("course_ID")),
                        Semester = reader.GetInt32(reader.GetOrdinal("semester")),
                        Condition = reader.GetString(reader.GetOrdinal("condition"))
                    };
                    learns.Add(learn);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！");
            }
            sqlConnection.Close();
            return learns;
        }

        //public List<Course> GetCourse(List<Learns> learns)
        //{
        //    List<Learns> courses = new List<Course>();  // 創建一個 Card 對象列表。
        //    SqlConnection sqlConnection = new SqlConnection(ConnStr); // 創建一個 SqlConnection 對象，使用 ConnStr 字符串指定的連接字符串進行初始化。
        //    foreach (cID in learns)
        //        List<SqlCommand> sqlCommand = new List<SqlCommand>(@"SELECT * FROM course WHERE course_ID=@s");// 創建一個 SqlCommand 對象，它表示要在數據庫中執行的命令，本例中是選擇所有 card 表中的數據
        //    sqlCommand.Connection = sqlConnection;
        //    sqlCommand.Parameters.Add(new SqlParameter("@s", s));
        //    sqlConnection.Open();// 打開 SqlConnection 對象。

        //    SqlDataReader reader = sqlCommand.ExecuteReader(); // 創建一個 SqlDataReader 對象，它可以讀取從數據庫返回的行。
        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())// 循環讀取 SqlDataReader 對象中的每一行，並為每個行創建一個 Card 對象。
        //        {
        //            Learns learn = new Learns
        //            {
        //                Student_id = reader.GetString(reader.GetOrdinal("student_ID")),
        //                Course_id = reader.GetInt32(reader.GetOrdinal("course_ID")),
        //                Semester = reader.GetInt32(reader.GetOrdinal("semester")),
        //                Condition = reader.GetString(reader.GetOrdinal("condition"))
        //            };
        //            learns.Add(learn);
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("資料庫為空！");
        //    }
        //    sqlConnection.Close();
        //    return learns;
        //}

    }
}