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
using System.Drawing;
using System.Drawing.Imaging;
using Baganova.Filters;

namespace Baganova.Controllers
{
 
    public class ProductsController : Controller
    {
        private PurestContext db = new PurestContext();
        ImageSaver savephoto = ImageSaver.CreateObject();

        // GET: Products
        [AdminAuthFilter]
        public ActionResult Index()
        {
            var products = db.Products.ToList(); //.Include(p => p.brand).Include(p => p.producttype);
            return View(products);
        }
        [AdminAuthFilter]
        public ActionResult AdminIndex()
        {
            var products = db.Products.ToList(); //.Include(p => p.brand).Include(p => p.producttype);
            return View(products);
        }

        // GET: Products/Details/5
     
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            //ViewBag.randproducts = GetRanddomProducts(product.TypeID, product.BrandID);
            //ViewBag.propReviews = db.Reviews.Where(x => x.prodid == product.ID);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //public List<Product> GetRanddomProducts(int typeid, int brandid)
        //{
        //    return db.Products.Where(z => z.BrandID == brandid || z.TypeID == typeid).OrderBy(x => Guid.NewGuid()).Take(4).ToList();
        //}

        // GET: Products/Create
        [AdminAuthFilter]
        public ActionResult Create()
        {
            ViewBag.TypeID = db.ProductTypes.Select(x => new SelectListItem { Text = x.TypeName, Value = x.ID.ToString() }).ToList();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminAuthFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductName,Price,ProductDescription,InStock,TypeID,BrandID,ProductDetails,Usage,Specification,Quantity,SKUNo,ProductMapping")] Product product,string ProductMappingValues, IEnumerable<HttpPostedFileBase> Photo)
        {
            var i = 0;
            foreach(HttpPostedFileBase file in Photo)
            {
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        string fileuploaded = savephoto.SaveImage(file, "~/Images/Products/", 1600, 85L, false);
                        if(i == 0)
                        {
                            product.Photo1 = fileuploaded;
                            i++;
                        }
                        else if(i == 1)
                        {
                            product.Photo2 = fileuploaded;
                            i++;
                        }
                        else if (i == 2)
                        {
                            product.Photo3 = fileuploaded;
                            i++;
                        }
                        else if (i == 3)
                        {
                            product.Photo4 = fileuploaded;
                            i++;
                        }
                        else if (i == 4)
                        {
                            product.Photo5 = fileuploaded;
                            i++;
                        }
                        else if (i == 5)
                        {
                            product.Photo6 = fileuploaded;
                            i++;
                        }
                        else if (i == 6)
                        {
                            product.Photo7 = fileuploaded;
                            i++;
                        }
                        else if (i == 7)
                        {
                            product.Photo8 = fileuploaded;
                            i++;
                        }
                        else if (i == 8)
                        {
                            product.Photo9 = fileuploaded;
                            i++;
                        }
                        else if (i == 9)
                        {
                            product.Photo10 = fileuploaded;
                            i++;
                        }
                        else if (i == 10)
                        {
                            product.BeneficialFor = fileuploaded;
                            i++;
                        }
                        else if (i == 11)
                        {
                            product.ProductInformation = fileuploaded;
                            i++;
                        }
                    }

                }
                else
                {
                    i++;
                }
            }

            if (ModelState.IsValid)
            {
                var prodmapping = ProductMappingValues.Split(',');
                product.CreatedDate = DateTime.Now;
                product.UpdatedDate = DateTime.Now;
                db.Products.Add(product);
                db.SaveChanges();

                foreach(var mapdata in prodmapping)
                {
                    ProductProductTypeMapping PPM = new ProductProductTypeMapping();
                    PPM.ProductID = product.ID;
                    PPM.ProductTypeID = Convert.ToInt32(mapdata);

                    db.ProductProductTypeMappings.Add(PPM);
                    db.SaveChanges();
                }
               

                return RedirectToAction("adminindex");
            }

           // ViewBag.BrandID = new SelectList(db.Brands, "ID", "BrandName", product.BrandID);
           // ViewBag.TypeID = new SelectList(db.ProductTypes, "ID", "TypeName", product.TypeID);
            return View(product);
        }

        [AdminAuthFilter]
        public ActionResult ProductsPhotos(int prodid)
        {
            ViewBag.products = db.Products.Find(prodid);
            return View();
        }

        [AdminAuthFilter]
        public ActionResult ImageRotation(int pid, int photonum)
        {
            var prod = db.Products.Find(pid);

            string photopath = "";
            string rootPath = Server.MapPath("~");

            switch (photonum)
            {
                case 1:
                    photopath = prod.Photo1;
                    break;
                case 2:
                    photopath = prod.Photo2;
                    break;
                case 3:
                    photopath = prod.Photo3;
                    break;
                case 4:
                    photopath = prod.Photo4;
                    break;
                case 5:
                    photopath = prod.Photo5;
                    break;

            }
            photopath = rootPath + photopath.Replace("./../","{}").Replace(@"/",@"\");;
            if (System.IO.File.Exists(photopath))
            {
                Image img = Image.FromFile(photopath);
                int exifOrientationID = 0x112; //274
                if (!img.PropertyIdList.Contains(exifOrientationID))
                    return View();

                var prop = img.GetPropertyItem(exifOrientationID);
                int val = BitConverter.ToUInt16(prop.Value, 0);
                var rot = RotateFlipType.RotateNoneFlipNone;

                if (val == 3 || val == 4)
                    rot = RotateFlipType.Rotate180FlipNone;
                else if (val == 5 || val == 6)
                    rot = RotateFlipType.Rotate90FlipNone;
                else if (val == 7 || val == 8)
                    rot = RotateFlipType.Rotate270FlipNone;

                if (val == 2 || val == 4 || val == 5 || val == 7)
                    rot |= RotateFlipType.RotateNoneFlipX;

                if (rot != RotateFlipType.RotateNoneFlipNone)
                    img.RotateFlip(rot);

                img.Save(photopath);
            }
            return RedirectToAction("ProductsPhotos", new { prodid = pid });
        }


        // GET: Products/changephotos/5
        [AdminAuthFilter]
        public ActionResult changephotos(int? id, int photo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            TempData["photonum"] = photo;
            ViewBag.prodid = id;
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }




        // POST: Products/changephotos/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AdminAuthFilter]
        [ValidateAntiForgeryToken]
        public ActionResult changephotos(HttpPostedFileBase Photo, int ID)
        {
            int id = ID;
            int photonum = Convert.ToInt32(TempData["photonum"].ToString());
            var product = db.Products.Find(id);

            if (Photo != null && Photo.ContentLength > 0)
            {
                string old = null;
                switch (photonum)
                {
                    case 1: old = product.Photo1; break;
                    case 2: old = product.Photo2; break;
                    case 3: old = product.Photo3; break;
                    case 4: old = product.Photo4; break;
                    case 5: old = product.Photo5; break;
                    case 6: old = product.Photo6; break;
                    case 7: old = product.Photo7; break;
                    case 8: old = product.Photo8; break;
                    case 9: old = product.Photo9; break;
                    case 10: old = product.Photo10; break;
                    case 11: old = product.BeneficialFor; break;
                    case 12: old = product.ProductInformation; break;
                }

                // Save optimized photo to Products folder, default max 1600px
                string fileuploaded = savephoto.SaveImage(Photo, "~/Images/Products/", 1600, 85L, false);

                switch (photonum)
                {
                    case 1:
                        product.Photo1 = fileuploaded;
                        break;
                    case 2:
                        product.Photo2 = fileuploaded;
                        break;
                    case 3:
                        product.Photo3 = fileuploaded;
                        break;
                    case 4:
                        product.Photo4 = fileuploaded;
                        break;
                    case 5:
                        product.Photo5 = fileuploaded;
                        break;
                    case 6:
                        product.Photo6 = fileuploaded;
                        break;
                    case 7:
                        product.Photo7 = fileuploaded;
                        break;
                    case 8:
                        product.Photo8 = fileuploaded;
                        break;
                    case 9:
                        product.Photo9 = fileuploaded;
                        break;
                    case 10:
                        product.Photo10 = fileuploaded;
                        break;
                    case 11:
                        product.BeneficialFor = fileuploaded;
                        break;
                    case 12:
                        product.ProductInformation = fileuploaded;
                        break;
                }

                if (!string.IsNullOrWhiteSpace(old))
                {
                    savephoto.Delete(old);
                }
            }
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ProductsPhotos", new { prodid = id });
        }



        // GET: Products/Edit/5
        [AdminAuthFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            ViewBag.TypeID = db.ProductTypes.Select(x => new SelectListItem { Text = x.TypeName, Value = x.ID.ToString() }).ToList();
            //list
            var data = db.ProductProductTypeMappings.Where(x => x.ProductID == id).Select(x => x.ProductTypeID).ToArray();
            ViewBag.productMapping = string.Join(",", data);
            if (product == null)
            {
                return HttpNotFound();
            }
            //ViewBag.BrandID = new SelectList(db.Brands, "ID", "BrandName", product.BrandID);
            //ViewBag.TypeID = new SelectList(db.ProductTypes, "ID", "TypeName", product.TypeID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthFilter]
        public ActionResult Edit([Bind(Include = "ID,ProductName,Price,ProductDescription,InStock,TypeID,BrandID,ProductDetails,Usage,Specification,Quantity,SKUNo")] Product product, string ProductMappingValues)
        {
            if (ModelState.IsValid)
            {
                var editproduct = db.Products.Find(product.ID);
                product.UpdatedDate = DateTime.Now;
                editproduct.ProductName = product.ProductName;
                editproduct.Price = product.Price;
                editproduct.ProductDescription = product.ProductDescription;
               // editproduct.TypeID = product.TypeID;
               // editproduct.BrandID = product.BrandID;
                editproduct.InStock = product.InStock;
                editproduct.ProductDetails = product.ProductDetails;
                editproduct.Usage = product.Usage;
                editproduct.Specification = product.Specification;
                editproduct.Quantity = product.Quantity;
                editproduct.SKUNo = product.SKUNo;

                var pmapping = db.ProductProductTypeMappings.Where(x => x.ProductID == product.ID).ToList();
                //ProductMappingValues

                db.ProductProductTypeMappings.RemoveRange(pmapping);
                // var pmappingdelete = db.ProductProductTypeMappings.Where(x => x.ProductID == product.ID).ToList();
                foreach (var mapdata in ProductMappingValues.Split(','))
                {
                    ProductProductTypeMapping PPM = new ProductProductTypeMapping();
                    PPM.ProductID = product.ID;
                    PPM.ProductTypeID = Convert.ToInt32(mapdata);

                    db.ProductProductTypeMappings.Add(PPM);
                    db.SaveChanges();
                }

                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }
           // ViewBag.BrandID = new SelectList(db.Brands, "ID", "BrandName", product.BrandID);
          //  ViewBag.TypeID = new SelectList(db.ProductTypes, "ID", "TypeName", product.TypeID);
            return View(product);
        }

        // GET: Products/Delete/5
        [AdminAuthFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [AdminAuthFilter]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            // delete all associated images
            var images = new[] { product.Photo1, product.Photo2, product.Photo3, product.Photo4, product.Photo5, product.Photo6, product.Photo7, product.Photo8, product.Photo9, product.Photo10, product.BeneficialFor, product.ProductInformation };
            foreach (var img in images)
            {
                if (!string.IsNullOrWhiteSpace(img))
                {
                    savephoto.Delete(img);
                }
            }
            db.Products.Remove(product);
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
