using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace midterm_selectcourse.Models
{
    public class Student
    {
        public string Student_id { get; set; }
        public string Department_name { get; set; }
        public string Account_number { get; set; }
        public int Grade { get; set; }
        public string Class_name { get; set; }
        public string Student_name { get; set; }

        public int Student_grade { get; set; }
        public int Total_credits { get; set; }

    }
}