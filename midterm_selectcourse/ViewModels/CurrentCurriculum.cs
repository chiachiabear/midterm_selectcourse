using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using midterm_selectcourse.Models;

namespace midterm_selectcourse.ViewModels
{
    public class CurrentCurriculum
    {
        //Course_id, Course_name, Sort(必選修), Credits
        public Course Course { get; set; }
        //Course_id(同上), department_name, grade, class_name(三者合成開課班級)
        public Set_up Set_up { get; set; }
        public string Teacher_name { get; set; }
        //building, room_id(兩者合成地點), capacity(選修上限人數)
        public Classroom Classroom { get; set; }
        //Section_id, Weekday, Start_time, End_time(TimeSpan型別)
        public Section Section { get; set; }
        //選修人數
        public int Student_amount { get; set; }

        //constructors
        public CurrentCurriculum()
        {
            Course = new Course();
            Set_up = new Set_up();
            Classroom = new Classroom();
            Section = new Section();
        }
    }

}