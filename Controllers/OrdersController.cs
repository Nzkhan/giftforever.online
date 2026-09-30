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
using Baganova.ViewModels;

namespace Baganova.Controllers
{
    public class OrdersController : Controller
    {
        private PurestContext db = new PurestContext();

        // GET: Orders
        [AdminAuthFilter]
        [CustomAuthFilter]
        public ActionResult Index()
        {
            if (Session["usertype"] != null)
            {
                if (Session["usertype"].ToString() == "User")
                {
                    int userid = 0;
                    if (Session["userid"] != null)
                    {
                        userid = Convert.ToInt32(Session["userid"]);
                    }

                    var orders = db.Orders.Where(z => z.UserId == userid).Include(o => o.user);
                    return View(orders.ToList());
                }
                else
                {
                    var orders = db.Orders.Include(o => o.user);
                    return View(orders.ToList());
                }
            }
            else
            {
                return RedirectToAction("Index","Home",null);
            }

            //return View();
        }
        public ActionResult ThankYou()
        {

            return View();
        }


        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult CheckOut(int id)
        {
            int userid = 0;
            if (Session["userid"] != null)
            {
                userid = Convert.ToInt32(Session["userid"]);
            }
            var user = db.UserDetails.Where(z => z.UserID == id).FirstOrDefault();
            ViewBag.address = db.Addresses.Where(z => z.UserDetailsID == user.ID);
            ViewBag.user = user;
            var cart = from c in db.Carts
                       join p in db.Products on c.ProductID equals p.ID
                       where c.UserID == userid
                       select new CartVM
                       {
                           cartid = c.ID,
                           img = p.Photo1,
                           price = p.Price,
                           productname = p.ProductName,
                           quantity = c.Quantity,
                           total = p.Price * c.Quantity
                       };
            ViewBag.cart = cart;
            UserDetail userDetail = db.UserDetails.Where(z => z.UserID == id).FirstOrDefault();
            ViewData["userdid"] = userDetail.ID;
            ViewBag.total = getcartamount();

            return View();
        }

        public int PlaceOrder(int addressid)
        {
            int userid = 0;
            double amount = 0;

            if (Session["userid"] != null)
            {
                userid = Convert.ToInt32(Session["userid"]);
            }

            var cart = from c in db.Carts
                       join p in db.Products on c.ProductID equals p.ID
                       where c.UserID == userid
                       select new CartVM
                       {
                           cartid = c.ID,
                           img = p.Photo1,
                           price = p.Price,
                           productid = p.ID,
                           Instock = p.InStock,
                           productname = p.ProductName,
                           quantity = c.Quantity,
                           total = p.Price * c.Quantity
                       };

            if (CheckIfInStock(cart.ToList()) == true)
            {
                foreach (var i in cart)
                {
                    amount += i.total;
                }

                var order = new Order();

                //order.CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                order.CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                order.DeliveryCharges = 100;
                order.Status = "Order placed";
                order.TotalAmount = amount;
                order.UserId = userid;
                order.AddressID = addressid;

                db.Orders.Add(order);
                db.SaveChanges();

                addOrderDetails(cart.ToList(), order.ID);

                return 1;
            }
            else
            {
                //something
                ViewBag.errormsg = "Product sold out";
                return 0;
            }

        }

        public void addOrderDetails(List<CartVM> carts, int orderid)
        {
            foreach (var i in carts)
            {
                var orderd = new OrderDetail();
                orderd.Amount = i.total;
                orderd.OrderID = orderid;
                orderd.ProductsID = i.productid;
                orderd.Quantity = i.quantity;
                db.OrderDetails.Add(orderd);
                db.SaveChanges();

                var product = db.Products.Find(i.productid);
                product.InStock = product.InStock - i.quantity;
                db.SaveChanges();
            }
            ClearCart();
        }

        public void ClearCart()
        {
            int userid = 0;

            if (Session["userid"] != null)
            {
                userid = Convert.ToInt32(Session["userid"]);
            }

            var cart = db.Carts.Where(z => z.UserID == userid).ToList();
            db.Carts.RemoveRange(cart);
            db.SaveChanges();
        }

        public bool CheckIfInStock(List<CartVM> carts)
        {
            foreach (var cart in carts)
            {
                if (cart.quantity > cart.Instock)
                {
                    return false;
                }
            }
            return true;
        }

        public double getcartamount()
        {
            int userid = 0;

            if (Session["userid"] != null)
            {
                userid = Convert.ToInt32(Session["userid"]);
            }

            double amount = 0;

            var cart = from c in db.Carts
                       join p in db.Products on c.ProductID equals p.ID
                       where c.UserID == userid
                       select new CartVM
                       {
                           total = p.Price * c.Quantity
                       };
            foreach (var i in cart)
            {
                amount += i.total;
            }
            return amount;
        }


        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "ID", "UserName");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserId,CreatedOn,Status,DeliveryCharges,TotalAmount")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "ID", "UserName", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "ID", "UserName", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserId,CreatedOn,Status,DeliveryCharges,TotalAmount")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "ID", "UserName", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
