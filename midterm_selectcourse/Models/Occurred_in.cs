using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace midterm_selectcourse.Models
{
    public class Occurred_in
    {
        public int Course_id { get; set; }
        public int Section_id { get; set; }
        public string Building { get; set; }
        public string Room_id { get; set; }
    }
}