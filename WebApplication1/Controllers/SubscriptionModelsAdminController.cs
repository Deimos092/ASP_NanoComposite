using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SubscriptionModelsAdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SubscriptionModels
        public ActionResult Index()
        {
            return View(db.SubModel.ToList());
        }

        // GET: SubscriptionModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionModel subscriptionModel = db.SubModel.Find(id);
            if (subscriptionModel == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionModel);
        }

        // GET: SubscriptionModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubscriptionModels/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubscriptionModelID,NumberOfProj,NumberOfShared,SubCost")] SubscriptionModel subscriptionModel)
        {
            if (ModelState.IsValid)
            {
                db.SubModel.Add(subscriptionModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subscriptionModel);
        }

        // GET: SubscriptionModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionModel subscriptionModel = db.SubModel.Find(id);
            if (subscriptionModel == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionModel);
        }

        // POST: SubscriptionModels/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubscriptionModelID,NumberOfProj,NumberOfShared,SubCost")] SubscriptionModel subscriptionModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscriptionModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subscriptionModel);
        }

        // GET: SubscriptionModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionModel subscriptionModel = db.SubModel.Find(id);
            if (subscriptionModel == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionModel);
        }

        // POST: SubscriptionModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubscriptionModel subscriptionModel = db.SubModel.Find(id);
            db.SubModel.Remove(subscriptionModel);
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
