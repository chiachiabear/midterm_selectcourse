using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using midterm_selectcourse.Models;

namespace midterm_selectcourse.ViewModels
{
    public class CurrentCurriculum
    {
        public Course Course { get; set; }
        public string Class_name { get; set; }
        public string Teacher_name { get; set; }

    }
}