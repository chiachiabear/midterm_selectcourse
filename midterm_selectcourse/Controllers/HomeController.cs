using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using midterm_selectcourse.Models;
using midterm_selectcourse.ViewModels;
namespace midterm_selectcourse.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string account_number, string secret_code)
        {
            DBmanager dBmanager = new DBmanager();
            int check_account=dBmanager.LoginVerify(account_number, secret_code);
            if(check_account==1)
            {
                Session["account"] = account_number;
                return RedirectToAction("ShowName", new { param = Session["account"]}) ;
            }
            else
            {
                return View();
            }
            

        }

        public ActionResult ShowName(string param)
        {
            //初始的ShowName會顯示用學號搜出來的已選課表
            DBmanager dBmanager = new DBmanager();
            List<Student> students = dBmanager.GetStudents(param);
            List<CurrentCurriculum> CCs = dBmanager.GetStudentsCurriculum(param);
            List<Occurred_in> NowOIs = dBmanager.GetlernaOccurredIn(param);
            ViewBag.students = students;
            ViewBag.CCs = CCs;  //還需要整理一下，讓開課班級有二合、讓同一個課程
            ViewBag.NowOIs = NowOIs;
            return View();
        }

        //接收ShowName回傳搜尋值
        [HttpPost]
        public ActionResult ShowName(string select_option, string course_code_value, string department, string grade, string week_value, string section_value, string course_name_value, string teacher_name_value)
        {
            DBmanager dBmanager = new DBmanager();

            Dictionary<string, int> selectDict = new Dictionary<string, int>();
            selectDict.Add("course_code", 0);
            selectDict.Add("departmentAndGrade", 1);
            selectDict.Add("weekday", 2);  //weekday and section
            selectDict.Add("course_name", 3);
            selectDict.Add("teacher_name", 4);
            List<CurrentCurriculum> CCs;
            List<Student> students;
            List<Occurred_in> NowOIs;
            NowOIs = dBmanager.GetlernaOccurredIn(Session["account"].ToString());
            ViewBag.NowOIs = NowOIs;
            switch (selectDict[select_option])
            {
                case 0:
                    //用course_code_value搜尋
                    System.Diagnostics.Debug.WriteLine(course_code_value);
                    CCs = dBmanager.GetCCsBYCourseID(course_code_value);
                    students = dBmanager.GetStudents( Session["account"].ToString());
                    
                    ViewBag.students = students;
                    ViewBag.CCs = CCs;
                    
                    break;
                case 1:
                    //用department, grade搜尋
                    System.Diagnostics.Debug.WriteLine(department, grade);
                    CCs = dBmanager.GetCCsBYdepartmentAndGradestring(department, grade);
                    students = dBmanager.GetStudents(Session["account"].ToString());
                    ViewBag.students = students;
                    ViewBag.CCs = CCs;
                    break;
                case 2:
                    //用week_value, section_value搜尋
                    System.Diagnostics.Debug.WriteLine(week_value, section_value);
                    if (week_value == "all" && section_value == "all")
                    {
                        CCs = dBmanager.GetCCsBYweekdayallwalls(week_value, section_value);
                    }
                    else if (week_value == "all")
                    {
                        CCs = dBmanager.GetCCsBYweekdayallw(week_value, section_value);
                    }
                    else if (section_value == "all")
                    {
                        CCs = dBmanager.GetCCsBYweekdayalls(week_value, section_value);
                    }
                    else
                    {
                        string section_cat = week_value + section_value;
                        CCs = dBmanager.GetCCsBYweekday(section_cat);
                    }
                    students = dBmanager.GetStudents(Session["account"].ToString());
                    ViewBag.students = students;
                    ViewBag.CCs = CCs;
                    break;
                case 3:
                    //用course_name_value搜尋
                    System.Diagnostics.Debug.WriteLine(course_name_value);
                     CCs = dBmanager.GetCCsBYcourse_name(course_name_value);
                     students = dBmanager.GetStudents(Session["account"].ToString());
                    ViewBag.CCs = CCs;
                    ViewBag.students = students;        
                    break;
                case 4:
                    //用teacher_name_value搜尋
                    System.Diagnostics.Debug.WriteLine(teacher_name_value);
                    CCs = dBmanager.GetCCsBYteacher_name(teacher_name_value);
                    students = dBmanager.GetStudents(Session["account"].ToString());
                    ViewBag.CCs = CCs;
                    ViewBag.students = students;
                    break;
                default:
                    //顯示他可以選的課表, 比他高年的年級的必修不能選, 未滿足先修課程的課不能修
                    CCs = dBmanager.GetCCsBYdefault();
                    students = dBmanager.GetStudents(Session["account"].ToString());
                    ViewBag.CCs = CCs;
                    ViewBag.students = students;
                    break;
            }


            
            return View();
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Login");
        }

        public ActionResult TakeCourse(int course_ID)
        {
            DBmanager dBmanager = new DBmanager();
            if(dBmanager.GetCoursePeople(course_ID) < dBmanager.GetCourseCapacity(course_ID))  //選課人數已滿
            {
                System.Diagnostics.Debug.WriteLine("選課人數未滿，繼續判斷");
                //繼續判斷能不能選
                if(!dBmanager.IfSectionBump(Session["account"].ToString(), course_ID))  //判斷是否撞已選課節次，回傳布林
                {
                    System.Diagnostics.Debug.WriteLine("選課沒撞節，繼續判斷");
                    //繼續判斷能不能選
                    if (!dBmanager.IfNameBump(Session["account"].ToString(), course_ID))  //判斷是否已有同名課，回傳布林
                    {
                        System.Diagnostics.Debug.WriteLine("選課沒同名，繼續判斷");
                        //繼續判斷能不能選
                        if (dBmanager.GetCreditsNow(Session["account"].ToString()) + dBmanager.GetCourseCredits(course_ID) <= 30)  //判斷現有學分, 欲選課學分加起來有沒有超過30，回傳布林
                        {
                            System.Diagnostics.Debug.WriteLine("選課加起來沒滿30，恭喜選到課");
                            //可以選
                            dBmanager.TakeCourseByStudentIDCourseID(Session["account"].ToString(), course_ID);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("選課加起來超過30，加選失敗");
                            //會超過30學分不能選
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("選課撞名，加選失敗");
                        //撞名了，不能選
                    }
                }
                else 
                {
                    System.Diagnostics.Debug.WriteLine("選課撞節，加選失敗");
                    //撞時間了，不能選
                }
            }
            else 
            {
                System.Diagnostics.Debug.WriteLine("選課人數已滿，加選失敗");
                //人滿了，不能選
            }

            //透過TempData傳暫時資料到ShowName
            //目前還沒
            return RedirectToAction("ShowName", new { param = Session["account"] });
        }

        public ActionResult DropCourse(int course_ID)
        {
            DBmanager dBmanager = new DBmanager();
            if(dBmanager.GetCreditsNow(Session["account"].ToString()) - dBmanager.GetCourseCredits(course_ID) >= 9)  //若退選後 >= 9學分，可以退
            {
                if(!dBmanager.IfRequired(Session["account"].ToString(), course_ID))  //檢查是否為本系必修
                {
                    System.Diagnostics.Debug.WriteLine("此課非本系必修，且退後大於9學分，退選成功");
                    //可以退
                    dBmanager.DropCourseByStudentIDCourseID(Session["account"].ToString(), course_ID);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("此課為本系必修，退選失敗");
                    //欲退選課為本系必修，退選失敗
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("退選後未滿9學分，退選失敗");
                //退選後不滿9學分，退選失敗
            }


            return RedirectToAction("ShowName", new { param = Session["account"] });
        }
    }
}