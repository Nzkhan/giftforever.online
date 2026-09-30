using Baganova.Filters;
using Baganova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Baganova.Controllers
{
    
    public class HomeController : Controller
    {
        private PurestContext db = new PurestContext();
        
        
        // GET: Home
        public ActionResult Index()
        {
            if(Session["userid"] != null)
            {                
                ViewBag.userid = Convert.ToInt32(Session["userid"].ToString());
            }

            ViewBag.Slider = db.Sliders.ToList();

            ViewBag.ProductTypes = db.ProductTypes.ToList();
            ViewBag.Product = db.Products.OrderByDescending(x => x.UpdatedDate).Take(4).ToList();

            ViewBag.Videos = db.Videos.OrderByDescending(s => s.UpdatedDate).Take(3).ToList();
            ViewBag.Testimonials = db.Testimonials.ToList();
            return View();
        }
        public ActionResult Video()
        {
            ViewBag.Videos = db.Videos.OrderByDescending(s => s.UpdatedDate).ToList();
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult blog()
        {
            ViewBag.blog = db.blogs.ToList();
            ViewBag.Lastestfive = db.blogs.OrderByDescending(x => x.UpdatedDate).Take(5).ToList();
            ViewBag.Tag = db.tags.Take(15).ToList();
            return View(db.blogs.ToList());
        }
        public ActionResult About()
        {
            return View();
        }  
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult DeliveryCancellationPolicy()
        {
            return View();
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }

        public ActionResult ReturnAndRefundPolicy()
        {
            return View();
        }

        public ActionResult RefundAndReturnsPolicy()
        {
            return View();
        }

        public ActionResult ShippingAndDeliveryPolicy()
        {
            return View();
        }

        public ActionResult CookiesPolicy()
        {
            return View();
        }

        public ActionResult FAQs()
        {
            return View();
        }

    }
}