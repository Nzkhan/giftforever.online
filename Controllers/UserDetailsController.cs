using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Baganova.Filters;
using Baganova.Models;

namespace Baganova.Controllers
{
    public class UserDetailsController : Controller
    {
        private PurestContext db = new PurestContext();

        // GET: UserDetails
        public ActionResult Index()
        {
            var userDetails = db.UserDetails.Include(u => u.user);
            return View(userDetails.ToList());
        }

        // GET: UserDetails/Details/5
        [CustomAuthFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // User user = db.Users.Find(id)
            UserDetail userDetail = db.UserDetails.Where(z => z.UserID == id).FirstOrDefault();
            Address address = db.Addresses.Where(z => z.UserDetailsID == userDetail.ID && z.IsActive == true).FirstOrDefault();

            // Check if address is null before accessing its property
            userDetail.Address = address != null ? address.address : string.Empty;
            ViewData["userdid"] = userDetail.ID;
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // GET: UserDetails/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "ID", "UserName");
            return View();
        }

        // POST: UserDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Mobile,Email,UserID")] UserDetail userDetail)
        {
            if (ModelState.IsValid)
            {
                db.UserDetails.Add(userDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "ID", "UserName", userDetail.UserID);
            return View(userDetail);
        }

        // GET: UserDetails/Edit/5
        [CustomAuthFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            //ViewBag.UserID = new SelectList(db.Users, "ID", "UserName", userDetail.UserID);
            return View(userDetail);
        }

        // POST: UserDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Mobile,Email")] UserDetail userDetail)
        {
            UserDetail userd = db.UserDetails.Find(userDetail.ID);

            userd.FirstName = userDetail.FirstName;
            userd.LastName = userDetail.LastName;
            userd.Mobile = userDetail.Mobile;
            userd.Email = userDetail.Email;

            //if (ModelState.IsValid)
            //{
            //    db.Entry(userDetail).State = EntityState.Modified;
            db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.UserID = new SelectList(db.Users, "ID", "UserName", userDetail.UserID);
            //UserDetail userDetail = db.UserDetails.Where(z => z.UserID == id).FirstOrDefault();
            //return View("Details","",new);
            return RedirectToAction("Details", "UserDetails", new { id = userd.UserID });
        }

        // GET: UserDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // POST: UserDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserDetail userDetail = db.UserDetails.Find(id);
            db.UserDetails.Remove(userDetail);
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
