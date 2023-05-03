using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using midterm_selectcourse.Models;
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
                return RedirectToAction("ShowName", new { param1 = Session["account"]}) ;
            }
            else
            {
                return View();
            }
            

        }

        public ActionResult ShowName(string param1)
        {

            DBmanager dBmanager = new DBmanager();
            List<Student> students = dBmanager.GetStudents(param1);
            ViewBag.students = students;
            return View();
        }


    }
}