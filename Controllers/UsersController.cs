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
    public class UsersController : Controller
    {
        private PurestContext db = new PurestContext();

        // GET: Users
        [AdminAuthFilter]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        [AdminAuthFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        [AdminAuthFilter]
        public ActionResult Create()
        {
            return View();
        }

     
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
     
        public ActionResult Register(string FirstName,string LastName,string Mobile, string Email, string UserName,string Password)
        {
            var user = new User();
            var userdetails = new UserDetail();
            CommonController ec = new CommonController();
            user.UserName = UserName;
            user.Password = ec.EnryptPassword(Password);
            user.UserType = "User";

            userdetails.FirstName = FirstName;
            userdetails.LastName= LastName;
            userdetails.Mobile = Mobile;
            userdetails.Email = Email;
           

            db.Users.Add(user);
            db.SaveChanges();

            userdetails.UserID = user.ID;

            db.UserDetails.Add(userdetails);
            db.SaveChanges();
            TempData["success"] = 1;
            return RedirectToAction("Index","Login");
        }
        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthFilter]
        public ActionResult Create([Bind(Include = "ID,UserName,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        [AdminAuthFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthFilter]
        public ActionResult Edit([Bind(Include = "ID,UserName,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [AdminAuthFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [AdminAuthFilter]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
