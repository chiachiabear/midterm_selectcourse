using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace midterm_selectcourse.Models
{
    public class Learns
    {
        public string Student_id { get; set; }
        public int Course_id { get; set; }
        public int Semester { get; set; }
        public string Condition { get; set; }
    }
}