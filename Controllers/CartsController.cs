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

using Stripe;
using Stripe.Checkout;
using static System.Net.WebRequestMethods;

namespace Baganova.Controllers
{
    public class CartsController : Controller
    {
        public CartsController()
        {
            StripeConfiguration.ApiKey = "somekey";
        }

        private PurestContext db = new PurestContext();
        Session session = null;

        [CustomAuthFilter]
        // GET: Carts
        [OutputCache(Duration = 0)]
        public ActionResult Index()
        {

            int userid = 0;
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
                           productname = p.ProductName,
                           quantity = c.Quantity,
                           total = p.Price * c.Quantity
                       };

            ViewData["total"] = getcartamount();

            return View(cart.ToList());
        }

        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName");
            ViewBag.UserID = new SelectList(db.Users, "ID", "UserName");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,ProductID,Quantity,RoleID")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName", cart.ProductID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "UserName", cart.UserID);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName", cart.ProductID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "UserName", cart.UserID);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,ProductID,Quantity,RoleID")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName", cart.ProductID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "UserName", cart.UserID);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public int getcartcount()
        {
            SessionTrackerController cc = new SessionTrackerController();

            int cartcount = 0;
            int userid = 0;
            if (Session["userid"] != null)
            {
                userid = Convert.ToInt32(Session["userid"]);
            }

            if (userid != 0)
            {
                cartcount = db.Carts.Where(c => c.UserID == userid).Count();
                return cartcount;
            }
            else
            {
                return cartcount;
            }
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

        [HttpPost]
        public ActionResult removecart(int cartid)
        {
            var carttoremove = db.Carts.Find(cartid);
            if (carttoremove == null)
            {
                return Json(new { success = false, message = "Cart item not found." });
            }

            db.Carts.Remove(carttoremove);
            db.SaveChanges();

            // Return a JSON payload so AJAX success handler is triggered
            return Json(new
            {
                success = true,
                message = "Item removed from cart.",
                cartCount = getcartcount(),
                total = getcartamount()
            });
        }

        public int addtocart(int prodid)
        {
            int userid = 0;
            if (Session["userid"] != null)
            {
                userid = Convert.ToInt32(Session["userid"]);
            }

            var cart = db.Carts.Where(c => c.UserID == userid && c.ProductID == prodid).FirstOrDefault();

            if (cart == null)
            {
                var newcart = new Cart();
                newcart.ProductID = prodid;
                newcart.Quantity = 1;
                newcart.UserID = userid;

                db.Carts.Add(newcart);
                db.SaveChanges();
            }
            else
            {
                cart.Quantity = cart.Quantity + 1;
                db.SaveChanges();
            }

            TempData["added"] = true;
            return 1;
        }

        public int changequantity(string opp, int cartid)
        {
            var cart = db.Carts.Find(cartid);

            if (opp == "add")
            {
                cart.Quantity = cart.Quantity + 1;
            }
            else
            {
                if (cart.Quantity > 1)
                {
                    cart.Quantity = cart.Quantity - 1;
                }
                else
                {
                    db.Carts.Remove(cart);
                }
            }
            db.SaveChanges();

            return 1;
        }

        [HttpPost]
        public ActionResult charge()
        {
           

            int userid = 0;
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
                           price = p.Price * 100,
                           productname = p.ProductName,
                           quantity = c.Quantity,
                           total = p.Price * c.Quantity
                       };
            List<SessionLineItemOptions> sessionLineItemOptions = new List<SessionLineItemOptions>();


            foreach (var item in cart)
            {
                sessionLineItemOptions.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)item.price,
                        Currency = "gbp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.productname,
                            Images = new List<string>()
                            {
                                "http://Baganova.com/" + item.img.Replace("./../", "")
                            },
                        },
                    },
                    Quantity = item.quantity,
                });
            }

            var options = new SessionCreateOptions
            {

                LineItems = sessionLineItemOptions,
                Mode = "payment",
                SuccessUrl = "http://localhost:47545/carts/success",
                CancelUrl = "http://localhost:47545/carts",
            };

            var service = new SessionService();
            session = service.Create(options);

            


            Response.Headers.Add("Location", session.Url);
            Session["sessionId"] = session.Id;
            return Json(session);
        }

        // GET: Carts/success
        public ActionResult success()
        {
            //Order Object For Insert Order
            Order order = new Order();
            // TransactionDetails transactionDetails = new TransactionDetails();

            // get session id from above
            Models.Address address = new Models.Address();
            int userid = 0;
            if (Session["userid"] != null)
            {
                userid = Convert.ToInt32(Session["userid"]);

                UserDetail userDetail = db.UserDetails.Where(z => z.UserID == userid).FirstOrDefault();

                address = db.Addresses.Where(z => z.UserDetailsID == userDetail.ID && z.IsActive == true).FirstOrDefault();
            }

            //session.Id
            var sessionService = new SessionService();
            var sessionresult = sessionService.Get(Session["sessionId"].ToString());

            // get payment intent
            var paymentIntentService = new PaymentIntentService();
            var paymentintentresult = paymentIntentService.Get(sessionresult.PaymentIntentId);

            // get latest charge object
            //paymentintentresult.latest_charge
            var chargeService = new ChargeService();
            var chargeresult = chargeService.Get(paymentintentresult.LatestChargeId);


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

            ViewBag.OrderDetails = cart.ToList();

            if (chargeresult != null)
            {
                order.UserId = userid;
                order.CreatedOn = DateTime.Now;
                order.Status = chargeresult.Status;
                order.PaymentMode = chargeresult.PaymentMethodDetails.Type;
                order.AddressID = address.ID;
                order.TotalAmount = Convert.ToDouble((chargeresult.Amount / 100.0M).ToString("F2"));

                db.Orders.Add(order);
                db.SaveChanges();




              


                SaveOrderDetails(userid, order.ID);

                var transactionDetails = new TransactionDetails
                {
                    ChargeId = chargeresult.Id,
                    IsPaid = chargeresult.Paid,
                    Status = chargeresult.Status,
                    PaymentIntentId = chargeresult.PaymentIntentId,
                    PaymentMethod = chargeresult.PaymentMethod,
                    ReceiptURL = chargeresult.ReceiptUrl,
                    OrderID = order.ID,
                    UserId = userid,
                    BalanceTransactionId = chargeresult.BalanceTransactionId,
                    UserEmail = chargeresult.BillingDetails.Email,
                    UserName = chargeresult.BillingDetails.Name,
                    UserphoneNumber = chargeresult.BillingDetails.Phone,
                    UserCountry = chargeresult.BillingDetails.Address.Country,
                    UserCity = chargeresult.BillingDetails.Address.City,
                    UserState = chargeresult.BillingDetails.Address.State,
                    Currency = chargeresult.Currency
                };


                db.TransactionDetails.Add(transactionDetails);
                db.SaveChanges();

              

                if (order != null)
                {
                   
                    order.TransactionId = transactionDetails.TransactionId; 
                    db.SaveChanges();
                }

            }



            // populate viewbag to display
            ViewBag.chargeID = chargeresult.Id; // - charge id
            ViewBag.IsPaid = chargeresult.Paid;
            ViewBag.Status = chargeresult.Status;
            ViewBag.PaymentIntentID = chargeresult.PaymentIntentId;
            ViewBag.PaymentMethod = chargeresult.PaymentMethod;
            ViewBag.ReceiptUrl = chargeresult.ReceiptUrl;
          
            return View();
        }

        private void SaveOrderDetails(int userid, int orderId)
        {
            var cart = from c in db.Carts
                       join p in db.Products on c.ProductID equals p.ID
                       where c.UserID == userid
                       select new CartVM
                       {
                           cartid = c.ID,
                           img = p.Photo1,
                           price = p.Price * 100,
                           productname = p.ProductName,
                           quantity = c.Quantity,
                           total = p.Price * c.Quantity,
                           productid = p.ID
                       };

            foreach (var item in cart)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderID = orderId,
                    ProductsID = item.productid,
                    Quantity = item.quantity,
                    Amount = item.total
                };

                db.OrderDetails.Add(orderDetail);
            }

            db.SaveChanges();

            // Remove records from Carts where UserID equals userid
            var cartsToRemove = db.Carts.Where(c => c.UserID == userid);
            db.Carts.RemoveRange(cartsToRemove);

            db.SaveChanges();
        }
    }
}
