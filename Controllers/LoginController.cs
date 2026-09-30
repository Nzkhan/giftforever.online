using Baganova.Filters;
using Baganova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Baganova.Controllers
{
    public class LoginController : Controller
    {
        private PurestContext db = new PurestContext();
        // GET: Login
       
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ValidateUser(string username, string password)
        {
            CommonController ec = new CommonController();
            string encrpass = ec.EnryptPassword(password);
            var user = db.Users.Where(z => z.UserName == username && z.Password == encrpass).FirstOrDefault();
            if (user != null)
            {
                ViewData["success"] = true;
                Session["userid"] = user.ID;
                Session["usertype"] = user.UserType;
                Session["username"] = user.UserName;
                if (user.UserType == "Admin")
                    return RedirectToAction("index", "Admin");
                else
                    return RedirectToAction("index", "Home");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public int logout()
        {
            //TempData["logout"] = 1; 
            Session.Abandon();
            return 1;
        }
    }
}