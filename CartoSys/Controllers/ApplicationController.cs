using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CartoSys.Models;

namespace CartoSys.Controllers
{
    public class ApplicationController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        //
        // GET: /Application/

        public ActionResult Index()
        {
            return View(db.Applications.ToList());
        }

        //
        // GET: /Application/Details/5

        public ActionResult Details(int id = 0)
        {
            Applications applications = db.Applications.Find(id);
            if (applications == null)
            {
                return HttpNotFound();
            }
            return View(applications);
        }

        //
        // GET: /Application/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Application/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Applications applications)
        {
            if (ModelState.IsValid)
            {
                db.Applications.Add(applications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applications);
        }

        //
        // GET: /Application/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Applications applications = db.Applications.Find(id);
            if (applications == null)
            {
                return HttpNotFound();
            }
            return View(applications);
        }

        //
        // POST: /Application/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Applications applications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applications);
        }

        //
        // GET: /Application/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Applications applications = db.Applications.Find(id);
            if (applications == null)
            {
                return HttpNotFound();
            }
            return View(applications);
        }

        //
        // POST: /Application/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Applications applications = db.Applications.Find(id);
            db.Applications.Remove(applications);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}