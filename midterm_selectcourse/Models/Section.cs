using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace midterm_selectcourse.Models
{
    public class Section
    {
        public int Section_id { get; set; }
        public string Weekday { get; set; }
        public TimeSpan Start_time { get; set; }
        public TimeSpan End_time { get; set; }
    }
}