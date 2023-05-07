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
        //[HttpPost]
        //public ActionResult ShowName(string param)
        //{
            //param1(param one)可以是課程名稱, 教師名稱
            //雖然在參數param1無法辨認說他是以上兩者哪個
            //但透過DBmanager內function
            //仍然讓他去搜尋，舉例來說 在GetCourseByTeacher內的param應是老師名稱，假如現在param1是課程名稱，會找不到，因為沒有老師會叫程式設計(III)而回傳的就會是空的
            //就可以透過兩個function哪個回傳不是空的，來判斷今天使用者是用甚麼找
            //假如皆為空，就跟使用者說查無資料
        //}


    }
}