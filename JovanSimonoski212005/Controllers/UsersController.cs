using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using JovanSimonoski212005.Models;

namespace JovanSimonoski212005.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if ((string)Session["Role"] != "Admin" && (string)Session["Role"] != "Moderator")
            {
                return Content("Unauthorized Access!");
            }
            return View(db.Users.ToList());
        }

     
        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if ((string)Session["Role"] != "Admin" && (string)Session["Role"] != "Moderator")
            {
                return Content("Unauthorized Access!");
            }
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

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Salt,HashedPassword")] User user)
        {
            if ((string)Session["Role"] != "Admin" && (string)Session["Role"] != "Moderator")
            {
                return Content("Unauthorized Access!");
            }
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if ((string)Session["Role"] != "Admin")
            {
                return Content("Unauthorized Access!");
            }
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if ((string)Session["Role"] != "Admin")
            {
                return Content("Unauthorized Access!");
            }
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
