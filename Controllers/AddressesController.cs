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
    public class AddressesController : Controller
    {
        private PurestContext db = new PurestContext();

        // GET: Addresses
        [CustomAuthFilter]
        public ActionResult Index(int id)
        {
            var addresses = db.Addresses.Where(z => z.UserDetailsID == id).Include(a => a.userdetails).ToList();
            if(addresses != null && addresses.Count > 0)
            {
                TempData["addId"] = id; 
            }
           
            return View(addresses);
        }

        // GET: Addresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // GET: Addresses/Create
        [CustomAuthFilter]
        public ActionResult Create()
        {
            ViewBag.UserDetailsID = new SelectList(db.UserDetails, "ID", "FirstName");
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string address)
        {
            Address tbladdress = new Address();
            int userid = Convert.ToInt32(Session["userid"]);
            var userd = db.UserDetails.Where(z => z.UserID == userid).FirstOrDefault();

            if (ModelState.IsValid)
            {
                tbladdress.UserDetailsID = userd.ID;
                tbladdress.address = address;
                db.Addresses.Add(tbladdress);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = userd.ID });
            }

            //ViewBag.UserDetailsID = new SelectList(db.UserDetails, "ID", "FirstName", address.UserDetailsID);
            return RedirectToAction("Index", new { id = userd.ID }); //View(address);
        }

        // GET: Addresses/Edit/5
        [CustomAuthFilter]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserDetailsID = new SelectList(db.UserDetails, "ID", "FirstName", address.UserDetailsID);
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        public ActionResult Edit(string address, int ID)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            var userd = db.UserDetails.Where(z => z.UserID == userid).FirstOrDefault();

            Address tbladdress = db.Addresses.Find(ID);

            tbladdress.address = address;
            //if (ModelState.IsValid)
            //{
            //    db.Entry(address).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = userd.ID });
            //}
            ////ViewBag.UserDetailsID = new SelectList(db.UserDetails, "ID", "FirstName", address.UserDetailsID);
            //return RedirectToAction("Index", new { id = userd.ID });
        }

        // GET: Addresses/Delete/5
        [CustomAuthFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // POST: Addresses/Delete/5
      //  [CustomAuthFilter]
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Address address = db.Addresses.Find(id);
            db.Addresses.Remove(address);
            db.SaveChanges();
            //  return RedirectToAction("Index");
            return Json(new { success = true });
        }

        [HttpPost, ActionName("SetMainAddress")]
        public ActionResult SetMainAddress(int id, int userDetailsID)
        {
            // Find the selected address
            Address selectedAddress = db.Addresses.Find(id);

            if (selectedAddress != null && selectedAddress.UserDetailsID == userDetailsID)
            {
                // Get all addresses for the user
                var userAddresses = db.Addresses.Where(a => a.UserDetailsID == userDetailsID).ToList();

                // Set all addresses to inactive
                foreach (var address in userAddresses)
                {
                    address.IsActive = false;
                    db.Entry(address).State = EntityState.Modified;
                }

                // Set the selected address to active
                selectedAddress.IsActive = true;
                db.Entry(selectedAddress).State = EntityState.Modified;

                // Save changes to the database
                db.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Address not found or user details mismatch" });
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
