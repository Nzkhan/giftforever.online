using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Baganova.Controllers
{
    public class SessionTrackerController : Controller
    {
        // GET: Session
        public ActionResult Index()
        {
            int a = getuserid();
            return View();
        }
        public int getuserid()
        {
            int userid = 0;

            
            if (Session["userid"] == null)
            {
                return userid;
            }
            else
            {
                userid = Convert.ToInt32(Session["userid"].ToString());
                return userid;
            }
        }

    }
}