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
    public class ProductTypesController : Controller
    {
        private PurestContext db = new PurestContext();
        private readonly ImageSaver _imageSaver = ImageSaver.CreateObject();

        // GET: ProductTypes
        public ActionResult Index(int? name, int? up, int? down, string fragrance = null, bool? newOnly = null, bool? bestsellerOnly = null, bool? inStockOnly = null)
        {
            if (name == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int producttype = name.Value;
            var type = db.ProductTypes.Where(x => x.ID == producttype).FirstOrDefault();
            
            if (type == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProductTypes = db.ProductTypes.ToList();
            ViewBag.Type = type;
            
            // Get all subcategory IDs (if this is a parent category)
            List<int> categoryIds = new List<int> { type.ID };
            var subCategories = db.ProductTypes.Where(pt => pt.ParentTypeID == type.ID).ToList();
            categoryIds.AddRange(subCategories.Select(sc => sc.ID));

            // Get products in all these categories
            var allCategoryMappings = db.ProductProductTypeMappings.Where(m => categoryIds.Contains(m.ProductTypeID)).Select(m => m.ProductID).ToList();
            var query = db.Products.Where(p => allCategoryMappings.Contains(p.ID));

            // Apply filters
            if (up != null && down != null)
            {
                query = query.Where(p => p.Price >= up && p.Price <= down);
            }

            if (!string.IsNullOrEmpty(fragrance))
            {
                query = query.Where(p => p.Fragrance == fragrance);
            }

            if (newOnly == true)
            {
                query = query.Where(p => p.IsNewArrival);
            }

            if (bestsellerOnly == true)
            {
                query = query.Where(p => p.IsBestseller);
            }

            if (inStockOnly == true)
            {
                query = query.Where(p => p.InStock > 0);
            }

            // Get distinct fragrances for filter
            ViewBag.Fragrances = db.Products
                .Where(p => allCategoryMappings.Contains(p.ID) && !string.IsNullOrEmpty(p.Fragrance))
                .Select(p => p.Fragrance)
                .Distinct()
                .ToList();

            List<Product> products = query.ToList();
            ViewBag.Products = products;

            return View();
        }

        [AdminAuthFilter]
        public ActionResult AdminIndex()
        {
            return View(db.ProductTypes.ToList());
        }

        [AdminAuthFilter]
        public ActionResult ProductTypePhotos(int PTid)
        {
            ViewBag.PT = db.ProductTypes.Find(PTid);
            return View();
        }

        // GET: Products/changephotos/5
        [AdminAuthFilter]
        public ActionResult changephotos(int? id, int photo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductType productType = db.ProductTypes.Find(id);
            TempData["photonum"] = photo;
            ViewBag.PTid = id;
            return View(productType);
        }

        // POST: Products/changephotos/5
        [HttpPost]
        [AdminAuthFilter]
        [ValidateAntiForgeryToken]
        public ActionResult changephotos(HttpPostedFileBase Photo, int ID)
        {
            int id = ID;
            int photonum = Convert.ToInt32(TempData["photonum"].ToString());
            var PT = db.ProductTypes.Find(id);

            if (Photo != null && Photo.ContentLength > 0)
            {
                string oldPath = null;
                string folder = photonum == 1 ? "~/Images/Type/Home/" : "~/Images/Type/TypeBanner/";
                int maxDim = photonum == 1 ? 1400 : 1920;

                if (photonum == 1)
                {
                    oldPath = PT.HomePhoto;
                }
                else if (photonum == 2)
                {
                    oldPath = PT.Photo;
                }

                string fileuploaded = _imageSaver.SaveImage(Photo, folder, maxDim, 85L, false);

                if (photonum == 1)
                {
                    PT.HomePhoto = fileuploaded;
                }
                else if (photonum == 2)
                {
                    PT.Photo = fileuploaded;
                }

                // delete old after successful assignment
                if (!string.IsNullOrWhiteSpace(oldPath))
                {
                    _imageSaver.Delete(oldPath);
                }
            }
            db.Entry(PT).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ProductTypePhotos", new { PTid = id });
        }

        // GET: ProductTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductType productType = db.ProductTypes.Find(id);
            if (productType == null)
            {
                return HttpNotFound();
            }
            return View(productType);
        }

        // GET: ProductTypes/Create
        [AdminAuthFilter]
        public ActionResult Create()
        {
            ViewBag.ParentTypeID = new SelectList(db.ProductTypes.Where(pt => pt.ParentTypeID == null), "ID", "TypeName");
            return View();
        }

        // POST: ProductTypes/Create
        [HttpPost]
        [AdminAuthFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TypeName,Description,ParentTypeID,DisplayOrder,ShowInNavigation")] ProductType productType, HttpPostedFileBase HomePhoto, HttpPostedFileBase TypePhoto)
        {
            if (HomePhoto != null && HomePhoto.ContentLength > 0)
            {
                string old = productType.HomePhoto; // should be null when creating
                string fileuploaded = _imageSaver.SaveImage(HomePhoto, "~/Images/Type/Home/", 1400, 85L, false);
                productType.HomePhoto = fileuploaded;
                if (!string.IsNullOrWhiteSpace(old)) _imageSaver.Delete(old);
            }

            if (TypePhoto != null && TypePhoto.ContentLength > 0)
            {
                string old = productType.Photo; // should be null when creating
                string fileuploaded = _imageSaver.SaveImage(TypePhoto, "~/Images/Type/TypeBanner/", 1920, 85L, false);
                productType.Photo = fileuploaded;
                if (!string.IsNullOrWhiteSpace(old)) _imageSaver.Delete(old);
            }

            if (ModelState.IsValid)
            {
                db.ProductTypes.Add(productType);
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }

            ViewBag.ParentTypeID = new SelectList(db.ProductTypes.Where(pt => pt.ParentTypeID == null), "ID", "TypeName", productType.ParentTypeID);
            return View(productType);
        }

        // GET: ProductTypes/Edit/5
        [AdminAuthFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductType productType = db.ProductTypes.Find(id);
            if (productType == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentTypeID = new SelectList(db.ProductTypes.Where(pt => pt.ParentTypeID == null && pt.ID != id), "ID", "TypeName", productType.ParentTypeID);
            return View(productType);
        }

        // POST: ProductTypes/Edit/5
        [AdminAuthFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TypeName,Description,ParentTypeID,DisplayOrder,ShowInNavigation,Photo,HomePhoto")] ProductType productType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }
            ViewBag.ParentTypeID = new SelectList(db.ProductTypes.Where(pt => pt.ParentTypeID == null && pt.ID != productType.ID), "ID", "TypeName", productType.ParentTypeID);
            return View(productType);
        }

        // GET: ProductTypes/Delete/5
        [AdminAuthFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductType productType = db.ProductTypes.Find(id);
            if (productType == null)
            {
                return HttpNotFound();
            }
            return View(productType);
        }

        // POST: ProductTypes/Delete/5
        [AdminAuthFilter]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductType productType = db.ProductTypes.Find(id);
            
            // Check if there are any products mapped to this category
            var mappedProducts = db.ProductProductTypeMappings.Where(m => m.ProductTypeID == id).ToList();
            foreach (var mapping in mappedProducts)
            {
                db.ProductProductTypeMappings.Remove(mapping);
            }
            
            // Check if there are any subcategories
            var subCategories = db.ProductTypes.Where(pt => pt.ParentTypeID == id).ToList();
            foreach (var subCategory in subCategories)
            {
                // Either reassign subcategories to parent or make them top-level
                if (productType.ParentTypeID.HasValue)
                {
                    subCategory.ParentTypeID = productType.ParentTypeID;
                }
                else
                {
                    subCategory.ParentTypeID = null;
                }
                db.Entry(subCategory).State = EntityState.Modified;
            }
            
            db.ProductTypes.Remove(productType);
            db.SaveChanges();
            return RedirectToAction("AdminIndex");
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
