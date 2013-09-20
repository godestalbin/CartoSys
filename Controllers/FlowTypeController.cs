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
    public class FlowTypeController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        //
        // GET: /FlowType/

        public ActionResult Index()
        {
            return View(db.FlowTypes.ToList());
        }

        //
        // GET: /FlowType/Details/5

        public ActionResult Details(int id = 0)
        {
            FlowType flowtype = db.FlowTypes.Find(id);
            if (flowtype == null)
            {
                return HttpNotFound();
            }
            return View(flowtype);
        }

        //
        // GET: /FlowType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /FlowType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FlowType flowtype)
        {
            if (ModelState.IsValid)
            {
                db.FlowTypes.Add(flowtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(flowtype);
        }

        //
        // GET: /FlowType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            FlowType flowtype = db.FlowTypes.Find(id);
            if (flowtype == null)
            {
                return HttpNotFound();
            }
            return View(flowtype);
        }

        //
        // POST: /FlowType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FlowType flowtype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flowtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flowtype);
        }

        //
        // GET: /FlowType/Delete/5

        public ActionResult Delete(int id = 0)
        {
            FlowType flowtype = db.FlowTypes.Find(id);
            if (flowtype == null)
            {
                return HttpNotFound();
            }
            return View(flowtype);
        }

        //
        // POST: /FlowType/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FlowType flowtype = db.FlowTypes.Find(id);
            db.FlowTypes.Remove(flowtype);
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