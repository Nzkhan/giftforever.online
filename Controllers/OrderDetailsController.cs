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
    public class OrderDetailsController : Controller
    {
        private PurestContext db = new PurestContext();

        // GET: OrderDetails
        public ActionResult Index()
        {
            var orderDetails = db.OrderDetails.Include(o => o.order).Include(o => o.product);
            return View(orderDetails.ToList());
        }

        public int AddOrderDetailsList(List<OrderDetail> orderdetailslist)
        {
            try
            {
                foreach (var item in orderdetailslist)
                {
                    OrderDetail orderdetail = new OrderDetail();
                    orderdetail.OrderID = item.OrderID;
                    orderdetail.ProductsID = item.ProductsID;
                    orderdetail.Quantity = item.Quantity;
                    orderdetail.Amount = item.Amount;

                    db.OrderDetails.Add(orderdetail);
                }
                db.SaveChanges();
                return 1;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        // GET: OrderDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            ViewBag.order = order;
            List<OrderDetail> orderDetail = db.OrderDetails.Where(z=> z.OrderID == order.ID).ToList();
            ViewBag.orderDetail = orderDetail;

            ViewBag.user = db.UserDetails.Where(z => z.UserID == order.UserId).FirstOrDefault();

            Address address = db.Addresses.Find(order.AddressID);
            ViewBag.address = address;

            //if (orderDetail == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.Orders, "ID", "Status");
            ViewBag.ProductsID = new SelectList(db.Products, "ID", "ProductName");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrderdID,ProductsID,Quantity,Amount,OrderID")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "ID", "Status", orderDetail.OrderID);
            ViewBag.ProductsID = new SelectList(db.Products, "ID", "ProductName", orderDetail.ProductsID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Orders, "ID", "Status", orderDetail.OrderID);
            ViewBag.ProductsID = new SelectList(db.Products, "ID", "ProductName", orderDetail.ProductsID);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderdID,ProductsID,Quantity,Amount,OrderID")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "ID", "Status", orderDetail.OrderID);
            ViewBag.ProductsID = new SelectList(db.Products, "ID", "ProductName", orderDetail.ProductsID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetail);
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
