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
            ViewBag.students = students;
            ViewBag.CCs = CCs;  //還需要整理一下，讓開課班級有二合、讓同一個課程
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

            switch(selectDict[select_option])
            {
                case 0:
                    //用course_code_value搜尋
                    System.Diagnostics.Debug.WriteLine(course_code_value);
                    break;
                case 1:
                    //用department, grade搜尋
                    System.Diagnostics.Debug.WriteLine(department, grade);
                    break;
                case 2:
                    //用week_value, section_value搜尋
                    System.Diagnostics.Debug.WriteLine(week_value, section_value);
                    break;
                case 3:
                    //用course_name_value搜尋
                    System.Diagnostics.Debug.WriteLine(course_name_value);
                    break;
                case 4:
                    //用teacher_name_value搜尋
                    System.Diagnostics.Debug.WriteLine(teacher_name_value);
                    break;
                default:
                    //顯示他可以選的課表, 比他高年的年級的必修不能選, 未滿足先修課程的課不能修
                    break;
            }

            
            return View();
        }

        


        public ActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}