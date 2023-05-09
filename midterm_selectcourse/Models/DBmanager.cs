using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using midterm_selectcourse.ViewModels;
using System.Web.Services.Description;
using System.Diagnostics;
using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;
using System.Drawing;

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
           
            SqlConnection sqlConnection = new SqlConnection(ConnStr); //
            SqlCommand sqlCommand = new SqlCommand(@"SELECT * FROM account WHERE account_number=@account_number AND secret_code=@secret_code");
            sqlCommand.Connection = sqlConnection;  //
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
                                                                                ON student.student_ID = learn.student_ID AND learn.condition = @condition)
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
            sqlCommand.Parameters.Add(new SqlParameter("@condition", "正在修"));
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
            SqlCommand sqlCommand2= new SqlCommand(@"SELECT COUNT(student.student_ID)　as count　,L2.course_ID
                                                    FROM 
														(student
														INNER JOIN learn L1
														ON student.student_ID = L1.student_ID AND L1.condition = @condition)
													RIGHT JOIN learn L2
													ON L1.course_ID = L2.course_ID
                                                    WHERE student.student_ID = @student_id AND L2.condition = @condition
                                                    GROUP BY L2.course_ID");
            sqlCommand2.Connection = sqlConnection2;
            sqlCommand2.Parameters.Add(new SqlParameter("@condition", "正在修"));
            sqlCommand2.Parameters.Add(new SqlParameter("@student_id", student_id));
            sqlConnection2.Open();
            
            SqlDataReader reader2 = sqlCommand2.ExecuteReader();
            List<int> counts = new List<int>();
            List<int> course_IDs = new List<int>();
            if (reader2.HasRows)
            {
                while(reader2.Read())
                {
                    int count = new int();
                    int cID = new int();
                    count = reader2.GetInt32(reader2.GetOrdinal("count"));
                    cID = reader2.GetInt32(reader2.GetOrdinal("course_ID"));
              
                    counts.Add(count);
                    course_IDs.Add(cID);
                }
            }
            sqlConnection2.Close();
            for (int i = 0; i < CCs.Count; i++)
            {
                for(int j=0; j<course_IDs.Count; j++)
                {
                    if(CCs[i].Course.Course_id == course_IDs[j])
                    {
                        CCs[i].Student_amount = counts[j];
                    }
                }
            }

            return CCs;
        }



        public List<CurrentCurriculum> GetCCsBYCourseID(string course_code_value)
        {
            List<CurrentCurriculum> CCs = new List<CurrentCurriculum>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr); // 創建一個 SqlConnection 對象，使用 ConnStr 字符串指定的連接字符串進行初始化。
            SqlCommand sqlCommand = new SqlCommand(@"select  course.course_ID, course.course_name, course.sort, course.credits,
                                                            set_up.department_name, set_up.grade, set_up.class_name,
                                                            teacher.teacher_name,
                                                            classroom.building, classroom.room_id, classroom.capacity,
                                                            section.section_id, section.weekday, section.start_time, section.end_time 
                                                    from course left outer join occurred_in on course.course_id =occurred_in.course_id 
                                                                left outer join set_up on set_up.course_id=course.course_id
                                                                left outer join teaches on course.course_id=teaches.course_ID 
                                                                left outer join teacher on teaches.teacher_ID=teacher.teacher_id 
                                                                left outer join classroom on occurred_in.building=classroom.building and occurred_in.room_ID=classroom.room_id
                                                                left outer join section on occurred_in.section_ID=section.section_ID
                                                                where course.course_ID=@course_code_value");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@course_code_value", course_code_value));
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
            SqlCommand sqlCommand2 = new SqlCommand(@"select  count(learn.student_id)as count,course.course_ID 
                                                        from learn  left outer join course on learn.course_id=course.course_id  
                                                         where learn.condition like '正在修%' and course.course_ID=@course_code_value group by course.course_id
                                                  ");
            sqlCommand2.Connection = sqlConnection2;
            sqlCommand2.Parameters.Add(new SqlParameter("@course_code_value", course_code_value));
            sqlConnection2.Open();

            SqlDataReader reader2 = sqlCommand2.ExecuteReader();
            List<int> counts = new List<int>();
            List<int> course_IDs = new List<int>();
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    int count = new int();
                    int cID = new int();
                    count = reader2.GetInt32(reader2.GetOrdinal("count"));
                    cID = reader2.GetInt32(reader2.GetOrdinal("course_ID"));

                    counts.Add(count);
                    course_IDs.Add(cID);
                }
            }
            sqlConnection2.Close();
            for (int i = 0; i < CCs.Count; i++)
            {
                for (int j = 0; j < course_IDs.Count; j++)
                {
                    if (CCs[i].Course.Course_id == course_IDs[j])
                    {
                        CCs[i].Student_amount = counts[j];
                    }
                }
            }

            return CCs;
        }


        public List<CurrentCurriculum> GetCCsBYdepartmentAndGradestring(string department, string grade)
        {

            List<CurrentCurriculum> CCs = new List<CurrentCurriculum>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr); // 創建一個 SqlConnection 對象，使用 ConnStr 字符串指定的連接字符串進行初始化。
            SqlCommand sqlCommand = new SqlCommand(@"select  course.course_ID, course.course_name, course.sort, course.credits,
                                                            set_up.department_name, set_up.grade, set_up.class_name,
                                                            teacher.teacher_name,
                                                            classroom.building, classroom.room_id, classroom.capacity,
                                                            section.section_id, section.weekday, section.start_time, section.end_time 
                                                    from course left outer join occurred_in on course.course_id =occurred_in.course_id 
                                                                left outer join set_up on set_up.course_id=course.course_id
                                                                left outer join teaches on course.course_id=teaches.course_ID 
                                                                left outer join teacher on teaches.teacher_ID=teacher.teacher_id 
                                                                left outer join classroom on occurred_in.building=classroom.building and occurred_in.room_ID=classroom.room_id
                                                                left outer join section on occurred_in.section_ID=section.section_ID
                                                                where grade=@grade and department_name=@department");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@grade", grade));
            sqlCommand.Parameters.Add(new SqlParameter("@department", department));

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
            SqlCommand sqlCommand2 = new SqlCommand(@"select  count(learn.student_ID) as count,course.course_ID
                                                        from learn  left outer join course on learn.course_id=course.course_id  
														inner  join (select distinct course_id ,department_name,grade from set_up)as s1 on  course.course_id=s1.course_id  
                                                         where learn.condition like '正在修%' and s1.department_name=@department_name and s1.grade=@grade group by course.course_ID,course.course_name
                                                  ");
            sqlCommand2.Connection = sqlConnection2;
            sqlCommand2.Parameters.Add(new SqlParameter("@department_name", department));
            sqlCommand2.Parameters.Add(new SqlParameter("@grade", grade));
            sqlConnection2.Open();

            SqlDataReader reader2 = sqlCommand2.ExecuteReader();
            List<int> counts = new List<int>();
            List<int> course_IDs = new List<int>();
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    int count = new int();
                    int cID = new int();
                    count = reader2.GetInt32(reader2.GetOrdinal("count"));
                    cID = reader2.GetInt32(reader2.GetOrdinal("course_ID"));

                    counts.Add(count);
                    course_IDs.Add(cID);
                }
            }
            sqlConnection2.Close();
            for (int i = 0; i < CCs.Count; i++)
            {
                for (int j = 0; j < course_IDs.Count; j++)
                {
                    if (CCs[i].Course.Course_id == course_IDs[j])
                    {
                        CCs[i].Student_amount = counts[j];
                    }
                }
            }

            return CCs;
        }

        public List<CurrentCurriculum> GetCCsBYweekdayallwalls(string week_value, string section_value)
        {
            List<CurrentCurriculum> CCs = new List<CurrentCurriculum>();
            return CCs;
        }
        public List<CurrentCurriculum> GetCCsBYweekdayallw(string week_value, string section_value)
        {
            List<CurrentCurriculum> CCs = new List<CurrentCurriculum>();
            return CCs;
        }
        public List<CurrentCurriculum> GetCCsBYweekdayalls(string week_value, string section_value)
        {
            List<CurrentCurriculum> CCs = new List<CurrentCurriculum>();
            return CCs;
        }
        public List<CurrentCurriculum> GetCCsBYweekday( string section_value)
        {

            List<CurrentCurriculum> CCs = new List<CurrentCurriculum>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr); // 創建一個 SqlConnection 對象，使用 ConnStr 字符串指定的連接字符串進行初始化。
            SqlCommand sqlCommand = new SqlCommand(@"select  course.course_ID, course.course_name, course.sort, course.credits,
                                                            set_up.department_name, set_up.grade, set_up.class_name,
                                                            teacher.teacher_name,
                                                            classroom.building, classroom.room_id, classroom.capacity,
                                                            section.section_id, section.weekday, section.start_time, section.end_time 
                                                    from course left outer join occurred_in on course.course_id =occurred_in.course_id 
                                                                left outer join set_up on set_up.course_id=course.course_id
                                                                left outer join teaches on course.course_id=teaches.course_ID 
                                                                left outer join teacher on teaches.teacher_ID=teacher.teacher_id 
                                                                left outer join classroom on occurred_in.building=classroom.building and occurred_in.room_ID=classroom.room_id
                                                                left outer join section on occurred_in.section_ID=section.section_ID
                                                                where section.section_ID=@section_value;");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@section_value", section_value));

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
            SqlCommand sqlCommand2 = new SqlCommand(@"select  count(learn.student_ID) as count,course.course_ID
                                                        from learn  left outer join course on learn.course_id=course.course_id  
														left outer  join  occurred_in on  course.course_ID= occurred_in.course_id
														left outer join section on occurred_in.section_id=section.section_ID
                                                         where learn.condition like '正在修%' and section.section_id=203 group by course.course_ID;
                                                  ");
            sqlCommand2.Connection = sqlConnection2;
            sqlCommand2.Parameters.Add(new SqlParameter("@section_value", section_value));
            sqlConnection2.Open();

            SqlDataReader reader2 = sqlCommand2.ExecuteReader();
            List<int> counts = new List<int>();
            List<int> course_IDs = new List<int>();
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    int count = new int();
                    int cID = new int();
                    count = reader2.GetInt32(reader2.GetOrdinal("count"));
                    cID = reader2.GetInt32(reader2.GetOrdinal("course_ID"));

                    counts.Add(count);
                    course_IDs.Add(cID);
                }
            }
            sqlConnection2.Close();
            for (int i = 0; i < CCs.Count; i++)
            {
                for (int j = 0; j < course_IDs.Count; j++)
                {
                    if (CCs[i].Course.Course_id == course_IDs[j])
                    {
                        CCs[i].Student_amount = counts[j];
                    }
                }
            }

            return CCs;
        }

        //回傳課程已選人數
        public int GetCoursePeople(int course_ID)
        {
            int reInt = -1;
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT COUNT(student_ID) AS count
                                                    FROM (course 
	                                                      INNER JOIN learn 
	                                                      ON course.course_ID = learn.course_ID AND learn.condition=@condition)
                                                    WHERE course.course_ID = @course_id");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@condition", "正在修"));
            sqlCommand.Parameters.Add(new SqlParameter("@course_id", course_ID));
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    reInt = reader.GetInt32(reader.GetOrdinal("count"));
                }
            }
            sqlConnection.Close();
            return reInt;
        }

        //回傳課程上限人數
        public int GetCourseCapacity(int course_ID)
        {
            int reInt = -1;
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT TOP 1 classroom.capacity
                                                    FROM ( occurred_in 
	                                                       INNER JOIN classroom
	                                                       ON occurred_in.building = classroom.building AND occurred_in.room_ID = classroom.room_id)
                                                    WHERE occurred_in.course_ID = @course_id");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@course_id", course_ID));
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    reInt = reader.GetInt32(reader.GetOrdinal("capacity"));
                }
            }
            sqlConnection.Close();
            return reInt;
        }



        //回傳是否撞已選課節數
        public bool IfSectionBump(string student_ID, int course_ID)
        {
            List<int> section_IDs = new List<int>(); ;
            List<int> courseSection_IDs = new List<int>();

            //先拿學生有的section
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT occurred_in.section_ID
                                                    FROM learn 
	                                                INNER JOIN occurred_in
	                                                ON learn.course_ID = occurred_in.course_ID AND learn.condition = @condition
                                                    WHERE learn.student_ID = @student_id");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@condition", "正在修"));
            sqlCommand.Parameters.Add(new SqlParameter("@student_id", student_ID));
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int temp = new int();
                    temp = reader.GetInt32(reader.GetOrdinal("section_ID"));
                    section_IDs.Add(temp);
                }
            }
            sqlConnection.Close();

            //再拿欲選課的section
            SqlConnection connection = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand(@"SELECT section.section_ID
                                            FROM ( occurred_in
	                                               INNER JOIN section
	                                               ON occurred_in.section_ID = section.section_ID )
                                            WHERE occurred_in.course_ID = @course_id");
            cmd.Connection = connection;
            cmd.Parameters.Add(new SqlParameter("@course_id", course_ID));
            connection.Open();

            SqlDataReader reader2 = cmd.ExecuteReader();
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    int temp = new int();
                    temp = reader2.GetInt32(reader2.GetOrdinal("section_ID"));
                    courseSection_IDs.Add(temp);
                }
            }
            connection.Close();

            //判斷後者List有沒存在於前者List中
            bool answer = false;
            foreach (int i in courseSection_IDs)
            {
                foreach (int j in section_IDs)
                {
                    if (i == j)
                    {
                        answer = true;
                    }
                }
            }

            return answer;
        }
        //回傳是否撞名已選課
        public bool IfNameBump(string student_ID, int course_ID)
        {
            List<string> studentCourseNames = new List<string>();
            string courseName = "";

            //先拿學生已選課的課程名稱
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT course.course_name
                                                    FROM learn 
	                                                INNER JOIN course
	                                                ON learn.course_ID = course.course_ID AND learn.condition = '正在修'
                                                    WHERE learn.student_ID = @student_id");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@student_id", student_ID));
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string temp = reader.GetString(reader.GetOrdinal("course_name"));
                    studentCourseNames.Add(temp);
                }
            }
            sqlConnection.Close();

            //再拿學生欲選的課的課程名稱
            SqlConnection conn = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand(@"SELECT course.course_name
                                             FROM  course
                                             WHERE course.course_ID = @course_id");
            cmd.Connection = conn;
            cmd.Parameters.Add(new SqlParameter("@course_id", course_ID));
            conn.Open();

            SqlDataReader r = cmd.ExecuteReader();
            if(r.HasRows)
            {
                while(r.Read())
                {
                    courseName = r.GetString(r.GetOrdinal("course_name"));
                }
            }
            conn.Close();

            //看欲選課名有沒有在已選課名List內
            bool answer = false;
            foreach(string i in studentCourseNames)
            {
                if(i.Equals(courseName))
                {
                    answer = true;
                }
            }

            return answer;
        }

        public int GetCreditsNow(string student_ID)
        {
            int sum = new int();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT SUM(course.credits) AS sum
                                                    FROM learn
                                                    INNER JOIN course
                                                    ON learn.course_ID = course.course_ID
                                                    WHERE learn.student_ID = @student_id");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@student_id", student_ID));
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    sum = reader.GetInt32(reader.GetOrdinal("sum"));
                }
            }
            sqlConnection.Close();

            return sum;
        }

        public int GetCourseCredits(int course_ID)
        {
            int credits = new int();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT credits
                                                    FROM course
                                                    WHERE course_ID = @course_id");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@course_id", course_ID));
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    credits = reader.GetInt32(reader.GetOrdinal("credits"));
                }
            }
            sqlConnection.Close();

            return credits;
        }

        //加選
        public void TakeCourseByStudentIDCourseID(string student_ID, int course_ID)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"INSERT 
                                                INTO learn (student_ID, course_ID, semester, condition)
                                                VALUES (@student_id, @course_id, @semester, @condition)");
            sqlCommand.Connection = sqlConnection;

            sqlCommand.Parameters.Add(new SqlParameter("@student_id", student_ID));
            sqlCommand.Parameters.Add(new SqlParameter("@course_id", course_ID));
            sqlCommand.Parameters.Add(new SqlParameter("@semester", 1112));
            sqlCommand.Parameters.Add(new SqlParameter("@condition", "正在修"));

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            
            //end, 加選成功
        }



        public List<Occurred_in> GetlernaOccurredIn(string s)
        {
            List<Occurred_in> NowOIbefores = new List<Occurred_in>();  // 創建一個 Card 對象列表。
            SqlConnection sqlConnection = new SqlConnection(ConnStr); // 創建一個 SqlConnection 對象，使用 ConnStr 字符串指定的連接字符串進行初始化。
            SqlCommand sqlCommand = new SqlCommand(@"select learn.course_ID,occurred_in.section_ID ,occurred_in.building,occurred_in.room_ID from learn 
                                                                left outer join occurred_in on learn.course_ID=occurred_in.course_ID
                                                                where learn.condition='正在修'and learn.student_ID=@s;");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@s", s));
            sqlConnection.Open();// 打開 SqlConnection 對象。

            SqlDataReader reader = sqlCommand.ExecuteReader(); 
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Occurred_in NowOIbefore = new Occurred_in()
                    {
                        Course_id = reader.GetInt32(reader.GetOrdinal("course_ID")),
                        Section_id = reader.GetInt32(reader.GetOrdinal("section_ID")),
                        Building = reader.GetString(reader.GetOrdinal("building")),
                        Room_id = reader.GetString(reader.GetOrdinal("room_ID"))
                    };
                    NowOIbefores.Add(NowOIbefore);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！");
            }
            sqlConnection.Close();
            List<Occurred_in> NowOIs = new List<Occurred_in>();

            for (int i = 0; i < 70; i++)
            {
                Occurred_in NowOI = new Occurred_in()
                {
                    Course_id = 0,
                    Section_id = 0,
                    Building = " ",
                    Room_id = " "
                };
                NowOIs.Add(NowOI);
            }
                //}
                //for(int i=0;i< NowOIbefores.Count; i++)
                //{
                //    int temp = ((NowOIbefores[i].Section_id / 100 )* (NowOIbefores[i].Section_id % 100) )- 1;
                //    NowOIs[temp].Course_id = NowOIbefores[i].Course_id;
                //    NowOIs[temp].Section_id = NowOIbefores[i].Section_id;
                //    NowOIs[temp].Building = NowOIbefores[i].Building;
                //    NowOIs[temp].Room_id = NowOIbefores[i].Room_id;
                //    Console.WriteLine("");
                //}
                for (int i=0;i<70;i++)
                {
                    for(int j=0;j< NowOIbefores.Count;j++)
                    {
                        if (NowOIbefores[j].Section_id==(i/14+1)*100+(i%14+1))
                        {
                            NowOIs[i].Course_id = NowOIbefores[j].Course_id;
                            NowOIs[i].Section_id = NowOIbefores[j].Section_id;
                            NowOIs[i].Building = NowOIbefores[j].Building;
                            NowOIs[i].Room_id = NowOIbefores[j].Room_id;
                        }
                    }
                }
            

            return NowOIs;
        }


        //回傳課程已選人數
        public int GetCoursePeople(int course_ID)
        {
            int reInt = -1;
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT COUNT(student_ID) AS count
                                                    FROM (course 
	                                                      INNER JOIN learn 
	                                                      ON course.course_ID = learn.course_ID AND learn.condition=@condition)
                                                    WHERE course.course_ID = @course_id");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@condition", "正在修"));
            sqlCommand.Parameters.Add(new SqlParameter("@course_id", course_ID));
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    reInt = reader.GetInt32(reader.GetOrdinal("count"));
                }
            }
            sqlConnection.Close();
            return reInt;
        }

        //回傳課程上限人數
        public int GetCourseCapacity(int course_ID)
        {
            int reInt = -1;
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT TOP 1 classroom.capacity
                                                    FROM ( occurred_in 
	                                                       INNER JOIN classroom
	                                                       ON occurred_in.building = classroom.building AND occurred_in.room_ID = classroom.room_id)
                                                    WHERE occurred_in.course_ID = @course_id");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@course_id", course_ID));
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    reInt = reader.GetInt32(reader.GetOrdinal("capacity"));
                }
            }
            sqlConnection.Close();
            return reInt;
        }



        //回傳是否撞已選課節數
        public bool IfSectionBump(string student_ID, int course_ID)
        {
            List<int> section_IDs = new List<int>(); ;
            List<int> courseSection_IDs = new List<int>();

            //先拿學生有的section
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT occurred_in.section_ID
                                                    FROM learn 
	                                                INNER JOIN occurred_in
	                                                ON learn.course_ID = occurred_in.course_ID AND learn.condition = @condition
                                                    WHERE learn.student_ID = @student_id");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@condition", "正在修"));
            sqlCommand.Parameters.Add(new SqlParameter("@student_id", student_ID));
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int temp = new int();
                    temp = reader.GetInt32(reader.GetOrdinal("section_ID"));
                    section_IDs.Add(temp);
                }
            }
            sqlConnection.Close();

            //再拿欲選課的section
            SqlConnection connection = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand(@"SELECT section.section_ID
                                            FROM ( occurred_in
	                                               INNER JOIN section
	                                               ON occurred_in.section_ID = section.section_ID )
                                            WHERE occurred_in.course_ID = @course_id");
            cmd.Connection = connection;
            cmd.Parameters.Add(new SqlParameter("@course_id", course_ID));
            connection.Open();

            SqlDataReader reader2 = cmd.ExecuteReader();
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    int temp = new int();
                    temp = reader2.GetInt32(reader2.GetOrdinal("section_ID"));
                    courseSection_IDs.Add(temp);
                }
            }
            connection.Close();

            //判斷後者List有沒存在於前者List中
            bool answer = false;
            foreach (int i in courseSection_IDs)
            {
                foreach (int j in section_IDs)
                {
                    if (i == j)
                    {
                        answer = true;
                    }
                }
            }

            return answer;
        }
        //回傳是否撞名已選課
        public bool IfNameBump(string student_ID, int course_ID)
        {
            List<string> studentCourseNames = new List<string>();
            string courseName = "";

            //先拿學生已選課的課程名稱
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT course.course_name
                                                    FROM learn 
	                                                INNER JOIN course
	                                                ON learn.course_ID = course.course_ID AND learn.condition = @condition
                                                    WHERE learn.student_ID = @student_id");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@condition", "正在修"));
            sqlCommand.Parameters.Add(new SqlParameter("@student_id", student_ID));
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string temp = reader.GetString(reader.GetOrdinal("course_name"));
                    studentCourseNames.Add(temp);
                }
            }
            sqlConnection.Close();

            //再拿學生欲選的課的課程名稱
            SqlConnection conn = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand(@"SELECT course.course_name
                                             FROM  course
                                             WHERE course.course_ID = @course_id");
            cmd.Connection = conn;
            cmd.Parameters.Add(new SqlParameter("@course_id", course_ID));
            conn.Open();

            SqlDataReader r = cmd.ExecuteReader();
            if(r.HasRows)
            {
                while(r.Read())
                {
                    courseName = r.GetString(r.GetOrdinal("course_name"));
                }
            }
            conn.Close();

            //看欲選課名有沒有在已選課名List內
            bool answer = false;
            foreach(string i in studentCourseNames)
            {
                if(i.Equals(courseName))
                {
                    answer = true;
                }
            }

            return answer;
        }

        //回傳當前課表總學分
        public int GetCreditsNow(string student_ID)
        {
            int sum = new int();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT SUM(course.credits) AS sum
                                                    FROM learn
                                                    INNER JOIN course
                                                    ON learn.course_ID = course.course_ID AND learn.condition = @condition
                                                    WHERE learn.student_ID = @student_id");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@condition", "正在修"));
            sqlCommand.Parameters.Add(new SqlParameter("@student_id", student_ID));
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    sum = reader.GetInt32(reader.GetOrdinal("sum"));
                }
            }
            sqlConnection.Close();

            return sum;
        }

        //回傳該課學分
        public int GetCourseCredits(int course_ID)
        {
            int credits = new int();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT credits
                                                    FROM course
                                                    WHERE course_ID = @course_id");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@course_id", course_ID));
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    credits = reader.GetInt32(reader.GetOrdinal("credits"));
                }
            }
            sqlConnection.Close();

            return credits;
        }

        //加選
        public void TakeCourseByStudentIDCourseID(string student_ID, int course_ID)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"INSERT 
                                                    INTO learn (student_ID, course_ID, semester, condition)
                                                    VALUES (@student_id, @course_id, @semester, @condition)");
            sqlCommand.Connection = sqlConnection;

            sqlCommand.Parameters.Add(new SqlParameter("@student_id", student_ID));
            sqlCommand.Parameters.Add(new SqlParameter("@course_id", course_ID));
            sqlCommand.Parameters.Add(new SqlParameter("@semester", 1112));
            sqlCommand.Parameters.Add(new SqlParameter("@condition", "正在修"));

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            
            //end, 加選成功
        }

        //判斷是否為本科必修
        public bool IfRequired(string student_ID, int course_ID) 
        {
            //先取得學生科系
            string s_department_name="";
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT department_name
                                                    FROM student
                                                    WHERE student_ID = @student_id");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@student_id", student_ID));
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                   s_department_name = reader.GetString(reader.GetOrdinal("department_name"));
                }
            }
            sqlConnection.Close();

            //再取得欲退課的科系
            string c_department_name="";
            SqlConnection conn = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand(@"SELECT department_name
                                             FROM set_up
                                             WHERE course_ID = @course_id");
            cmd.Connection = conn;
            cmd.Parameters.Add(new SqlParameter("@course_id", course_ID));
            conn.Open();

            SqlDataReader r1 = cmd.ExecuteReader();
            if (r1.HasRows)
            {
                while(r1.Read())
                {
                    c_department_name = r1.GetString(r1.GetOrdinal("department_name"));
                }
            }
            conn.Close();

            //再取得欲退選課的修別
            string c_sort = "";
            SqlConnection conn2 = new SqlConnection(ConnStr);
            SqlCommand cmd2 = new SqlCommand(@"SELECT sort
                                              FROM course
                                              WHERE course_ID = @c_id");
            cmd2.Connection = conn2;
            cmd2.Parameters.Add(new SqlParameter("@c_id", course_ID));
            conn2.Open();

            SqlDataReader r2 = cmd2.ExecuteReader();
            if(r2.HasRows)
            {
                while(r2.Read())
                {
                    c_sort = r2.GetString(r2.GetOrdinal("sort"));
                }
            }
            conn2.Close();

            //先判斷是否同科系
            if(s_department_name.Equals(c_department_name))  //兩者相不相等，回傳布林
            {
                //同科系，接著判斷修別
                if(c_sort.Equals("必修"))
                {
                    return true;
                }
            }
            return false;
        }

        //退選
        public void DropCourseByStudentIDCourseID(string student_ID, int course_ID)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"DELETE 
                                                    FROM learn
                                                    WHERE student_ID = @student_id AND course_ID = @course_id AND semester = @semester AND condition = @condition");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@student_id", student_ID));
            sqlCommand.Parameters.Add(new SqlParameter("@course_id", course_ID));
            sqlCommand.Parameters.Add(new SqlParameter("@semester", 1112));
            sqlCommand.Parameters.Add(new SqlParameter("@condition", "正在修"));

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            //end, 退選成功
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