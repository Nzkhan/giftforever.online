using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Baganova.Models;
using System.IO;
using Baganova.Filters;

namespace Baganova.Controllers
{
    public class SlidersController : Controller
    {
        private PurestContext db = new PurestContext();
        private readonly ImageSaver _imageSaver = ImageSaver.CreateObject();

        // GET: Sliders
        [AdminAuthFilter]
        public ActionResult Index()
        {
            return View(db.Sliders.ToList());
        }

        // GET: Sliders/Details/5
        [AdminAuthFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Sliders/Create
        [AdminAuthFilter]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sliders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthFilter]
        public ActionResult Create(Baganova.Models.Slider model, HttpPostedFileBase Photo)
        {
            Slider slider = new Slider();
            slider.Title = model.Title;
            slider.SubTitle = model.SubTitle;
            if (ModelState.IsValid)
            {
                try
                {
                    if (Photo != null && Photo.ContentLength > 0)
                    {
                        string old = slider.Photo; // should be null on create
                        string fileuploaded = _imageSaver.SaveImage(Photo, "~/Images/slider/", 1920, 85L, false);
                        slider.Photo = fileuploaded;
                        if (!string.IsNullOrWhiteSpace(old)) _imageSaver.Delete(old);
                    }
                    db.Sliders.Add(slider);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "File upload failed!!";
                    return View();
                }
            }
            return View(slider);
        }

        // GET: Sliders/Edit/5
        [AdminAuthFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            ViewBag.SliderID = id;
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Sliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthFilter]
        public ActionResult Edit(Baganova.Models.Slider model, HttpPostedFileBase Photo, int SliderID)
        {
            Slider slider = db.Sliders.Find(SliderID);
            if (slider == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    slider.Title = model.Title;
                    slider.SubTitle = model.SubTitle;
                    if (Photo != null && Photo.ContentLength > 0)
                    {
                        string old = slider.Photo;
                        string fileuploaded = _imageSaver.SaveImage(Photo, "~/Images/slider/", 1920, 85L, false);
                        slider.Photo = fileuploaded;
                        if (!string.IsNullOrWhiteSpace(old)) _imageSaver.Delete(old);
                    }
                    db.Entry(slider).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "File upload failed!!";
                    return View(model);
                }
            }
            return View(model);
        }

        // GET: Sliders/Delete/5
        [AdminAuthFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Sliders/Delete/5
        [AdminAuthFilter]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.Sliders.Find(id);
            // delete image file as well
            if (slider != null && !string.IsNullOrWhiteSpace(slider.Photo))
            {
                _imageSaver.Delete(slider.Photo);
            }
            db.Sliders.Remove(slider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
