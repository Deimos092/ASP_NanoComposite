using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Dynamic;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class UsersAdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UsersAdmin
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: UsersAdmin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: UsersAdmin/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Subs = db.SubModel.ToList();
            ViewBag.id = user.SubModel.SubscriptionModelID;
            return View(user);
        }

        // POST: UsersAdmin/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,APIKey,Email,EmailConfirmed,UserName,SubscriptionStartDate,SubscriptionEndDate")] User user, int idSub)
        {
            if (ModelState.IsValid)
            {
                var user2 = db.Users.Where(x => x.Id == user.Id).First();
                user2.SubModel = db.SubModel.Where(x => x.SubscriptionModelID == idSub).First();
                user2.APIKey = user.APIKey;
                user2.Email = user.Email;
                user2.EmailConfirmed = user.EmailConfirmed;
                user2.UserName = user.UserName;
                user2.SubscriptionStartDate = user.SubscriptionStartDate;
                user2.SubscriptionEndDate = user.SubscriptionEndDate;
                db.Entry(user2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Subs = db.SubModel.ToList();
            return View(user);
        }

        // GET: UsersAdmin/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: UsersAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
