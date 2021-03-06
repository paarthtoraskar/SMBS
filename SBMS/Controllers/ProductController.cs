﻿using SBMS.Models;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SBMS.Controllers
{
    public class ProductController : Controller
    {
        private readonly SBMSDbContext _db = new SBMSDbContext();

        // GET: /Product/

        public ActionResult Index()
        {
            return View(_db.Products.ToList());
        }

        // GET: /Product/Details/5

        public ActionResult Details(int id = 0)
        {
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: /Product/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: /Product/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: /Product/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: /Product/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(product).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: /Product/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _db.Products.Find(id);
            _db.Products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        // Customer viewable section

        public ActionResult Catalog()
        {
            if (_db.Products.ToList().Count == 0)
            {
                _db.InitializeProductsOnStartup();
                _db.SaveChanges();
            }

            return View(_db.Products.ToList());
        }

        public ActionResult JustPushPlay(Product jpp)
        {
            return View(jpp);
        }

        public ActionResult PicturePerfect(Product pp)
        {
            return View(pp);
        }

        public ActionResult MovieMan(Product mm)
        {
            return View(mm);
        }

        public ActionResult CompressIt(Product ci)
        {
            return View(ci);
        }

        public ActionResult CleanAll(Product ca)
        {
            return View(ca);
        }

        public ActionResult StartUtility()
        {
            string pathInApiDomain = HostingEnvironment.MapPath("~");
            string appRoot = Directory.GetParent(Directory.GetParent(pathInApiDomain).ToString()).ToString();
            string utilityFileName = appRoot + "\\Client\\bin\\Debug\\" + "WpfApp.exe";

            if (System.IO.File.Exists(utilityFileName))
            {
                //var psi = new ProcessStartInfo { FileName = utilityFileName };
                //var utility = new Process { StartInfo = psi };
                //utility.Start();
                return File(utilityFileName, "application/x-msdownload", "FileTranser.exe");
            }
            //return RedirectToAction("Catalog", "Product", null);
            return View("Error");
        }
    }
}