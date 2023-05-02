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
            dBmanager.LoginVerify(account_number, secret_code);
            return RedirectToAction("ShowName");
        }

        public ActionResult ShowName()
        {
            return View();
        }


    }
}