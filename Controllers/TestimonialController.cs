using Baganova.Filters;
using Baganova.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Baganova.Controllers
{
    public class TestimonialController : Controller
    {
      private  PurestContext db = new PurestContext();
        private Testimonial t = new Testimonial();
        // GET: Testimonial
        [AdminAuthFilter]
        public ActionResult Index()
        {
            var Data = db.Testimonials.ToList();
            
            return View(Data);
        }

        [AdminAuthFilter]
        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [AdminAuthFilter]
        public ActionResult Create(Testimonial t)
        {
            if(ModelState.IsValid == true)
            {
                var createdate = DateTime.Now ;
                t.CreatedDate = createdate;
                t.UpdatedDate = createdate;
                db.Testimonials.Add(t);
                db.SaveChanges();
                ModelState.Clear();
            }
            return RedirectToAction("Index");
        }

        [AdminAuthFilter]
        public ActionResult Edit(int id)
        {
       
          
            var row = db.Testimonials.Where(model => model.ClientId == id ).FirstOrDefault();

            return View(row);
        }

        [HttpPost]
        [AdminAuthFilter]
        public ActionResult Edit(Testimonial t)
        {
            var updatedate = DateTime.Now;
            t.UpdatedDate = updatedate;

            db.Entry(t).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [AdminAuthFilter]
        public ActionResult Delete(int id)
        {
            var row = db.Testimonials.Where(model => model.ClientId == id).FirstOrDefault();
            return View(row);
        }

        [HttpPost, ActionName("Delete")]
        [AdminAuthFilter]
        public ActionResult ConfirmDelete(int id)
        {
            Testimonial find = db.Testimonials.Find(id);
            db.Testimonials.Remove(find);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

    }
}