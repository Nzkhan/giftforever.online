using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Baganova.Models;
using System.Data.Entity;
using Baganova.Filters;

namespace Baganova.Controllers
{
    public class VideoController : Controller
    {
        private PurestContext db = new PurestContext();
        

        // GET: Video
        [AdminAuthFilter]
        public ActionResult Index()
        {
            var displaylatest = db.Videos.OrderByDescending(s => s.UpdatedDate).ToList(); 
            var data = db.Videos.ToList();
            return View(displaylatest);
        }

        [AdminAuthFilter]
        public ActionResult Create()
        {
            return View();
        }

        [AdminAuthFilter]
        [HttpPost]
        public ActionResult Create(Video v)
        {
            if(ModelState.IsValid == true)
            {
                v.CreatedDate = DateTime.Now;
                v.UpdatedDate = DateTime.Now;

                string link = v.VideoLink;
                int index = link.LastIndexOf("/") + 1;
                v.VideoLink = "https://www.youtube.com/embed/" + link.Substring(index);


                db.Videos.Add(v);
                db.SaveChanges();

                ModelState.Clear();

            }
            return RedirectToAction("index");
        }

        [AdminAuthFilter]
        public ActionResult Edit(int id)
        {
            var row = db.Videos.Where(model => model.VideoId == id).FirstOrDefault();
            return View(row);
        }


        [AdminAuthFilter]
        [HttpPost]
        public ActionResult Edit(Video v)
        {         
            v.UpdatedDate = DateTime.Now;
            db.Entry(v).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [AdminAuthFilter]
        public ActionResult Delete(int id)
        {
            var row = db.Videos.Where(model => model.VideoId == id).FirstOrDefault();
            return View(row);
        }

        [AdminAuthFilter]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            Video link = db.Videos.Find(id);
            db.Videos.Remove(link);
            db.SaveChanges();
            return RedirectToAction("Index");
  
        }



    }
}