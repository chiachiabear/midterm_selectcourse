using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace midterm_selectcourse.Models
{
    public class Course
    {
        public int Course_id { get; set; }
        public string Course_name { get; set; }
        public string Sort { get; set; }
        public int Credits { get; set; }
    }
}