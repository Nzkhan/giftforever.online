using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Baganova.Models;

namespace Baganova.Controllers
{
    public class BlogsController : Controller
    {
        private PurestContext db = new PurestContext();
        ImageSaver savephoto = ImageSaver.CreateObject();
        // GET: Blogs
        public ActionResult Index()
        {
            return View(db.blogs.ToList());
        }

        // GET: Blogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.blogs.Find(id);
            var tagIdMapping = db.mappingBlogTags.Where(x => x.BlogId == blog.Id).Select(x => x.TagId).ToList();
             ViewBag.Tag = db.tags.Where(x => tagIdMapping.Contains(x.Id)).ToList();
            ViewBag.TagFour = db.tags.Where(x => tagIdMapping.Contains(x.Id)).Take(4).ToList();
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Blogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Photo")] Blog blog, string tags, HttpPostedFileBase Photo)
        {
            if(Photo != null && Photo.ContentLength > 0)
            {
                string fileuploaded = savephoto.SaveImage(Photo, "~/Images/Products/", 1600, 85L, false);
                // delete stale if any (should be null on create)
                if (!string.IsNullOrWhiteSpace(blog.Photo)) savephoto.Delete(blog.Photo);
                blog.Photo = fileuploaded;
            }
            var Tag = tags.Split(',');
            blog.CreatedDate = DateTime.Now.ToString("MMMM dd yyyy");
            blog.UpdatedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.blogs.Add(blog);
                db.SaveChanges();
                foreach (var tag in Tag)
                {
                    Tag t = new Tag();
                    MappingBlogTag MBT = new MappingBlogTag();

                    t.TagName = tag;
                    db.tags.Add(t);
                    db.SaveChanges();
                    MBT.BlogId = blog.Id;
                    MBT.TagId = t.Id;
                    db.mappingBlogTags.Add(MBT);
                    db.SaveChanges();

                }
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.blogs.Find(id);
            ViewBag.edit = blog;
            var tagid = db.mappingBlogTags.Where(x => x.BlogId == id).Select(x => x.TagId).ToList();
            var tags = db.tags.Where(x => tagid.Contains(x.Id)).Select(x => x.TagName).ToList();
            ViewBag.tag = string.Join(",", tags);

            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Photo,CreatedDate,UpdatedDate")] Blog blog, string tags)
        {
            if (ModelState.IsValid)
            {
                var blogs = db.blogs.Find(blog.Id);
                blogs.Title = blog.Title;
                blogs.Description = blog.Description;              
                blogs.UpdatedDate = DateTime.Now;
                var Tag = tags.Split(',');

                var tagmappings = db.mappingBlogTags.Where(x => x.BlogId == blog.Id).ToList();
                var tagmapping = db.mappingBlogTags.Where(x => x.BlogId == blog.Id).Select(x => x.TagId).ToList();
                var tagsdelete = db.tags.Where(x => tagmapping.Contains(x.Id)).ToList();

                db.tags.RemoveRange(tagsdelete);
                db.mappingBlogTags.RemoveRange(tagmappings);
                
                db.Entry(blogs).State = EntityState.Modified;
                db.SaveChanges();

                foreach (var tag in Tag)
                {
                    Tag t = new Tag();
                    MappingBlogTag MBT = new MappingBlogTag();

                    t.TagName = tag;
                    db.tags.Add(t);
                    db.SaveChanges();
                    MBT.BlogId = blog.Id;
                    MBT.TagId = t.Id;
                    db.mappingBlogTags.Add(MBT);
                    db.SaveChanges();

                }
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        //get: product/changephoto
        public ActionResult Changephotos(int? id, int photo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.blogs.Find(id);
            TempData["photonum"] = photo;
            ViewBag.blogid = id;
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Changephotos(HttpPostedFileBase Photo, int ID)
        {
            var id = ID;
            int photonum = Convert.ToInt32(TempData["photonum"].ToString());
            var PT = db.blogs.Find(id);
            
            if (Photo != null)
            {
                if (Photo.ContentLength > 0)
                {                 
                    string old = PT.Photo;
                    string fileuploaded = savephoto.SaveImage(Photo, "~/Images/Products/", 1600, 85L, false);
                  
                    switch (photonum)
                    {
                        case 1:                 
                            PT.Photo = fileuploaded;
                            break;                     
                    }

                    if (!string.IsNullOrWhiteSpace(old))
                    {
                        savephoto.Delete(old);
                    }
                }
            }
            db.Entry(PT).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit", new { id });         
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.blogs.Find(id);
            if (blog != null && !string.IsNullOrWhiteSpace(blog.Photo))
            {
                savephoto.Delete(blog.Photo);
            }
            db.blogs.Remove(blog);
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
