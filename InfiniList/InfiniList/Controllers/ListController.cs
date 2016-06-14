using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InfiniList.DAL;
using InfiniList.Models;
using System;
using System.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace InfiniList.Controllers
{
    public class ListController : Controller
    {
        private InfiniBackEnd db = new InfiniBackEnd();

        //GET: List
        public ActionResult Index(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = db.Lists.Where(l => l.CollectionID == id).ToList();
            if (list == null)
            {
                return HttpNotFound();
            }

            ViewBag.CollectionID = id;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: List/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: List/Create
        public ActionResult Create(int? ID)
        {
            if(ID != null)
            {
                ViewBag.ID = ID;
            }
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext.Create()));
            var currentUser = manager.FindByEmail(User.Identity.GetUserName());


            ViewBag.CollectionID = new SelectList(db.Collections.Where(l => l.Email == currentUser.Email), "ID", "Title");
            return View();
        }

        // POST: List/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Notes,CollectionID")] List list)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Lists.Add(list);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = list.CollectionID });

                }

            }
            catch (Exception /*ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again later!");
            }

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext.Create()));
            var currentUser = manager.FindByEmail(User.Identity.GetUserName());

            ViewBag.CollectionID = new SelectList(db.Collections.Where(l => l.Email == currentUser.Email), "ID", "Title", list.CollectionID);
            return View(list);
        }

        // GET: List/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext.Create()));
            var currentUser = manager.FindByEmail(User.Identity.GetUserName());

            ViewBag.CollectionID = new SelectList(db.Collections.Where(l => l.Email == currentUser.Email), "ID", "Title", list.CollectionID);
            return View(list);
        }

        // POST: List/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Notes,CollectionID")] List list)
        {
            if (ModelState.IsValid)
            {
                db.Entry(list).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = list.CollectionID });
            }

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext.Create()));
            var currentUser = manager.FindByEmail(User.Identity.GetUserName());

            ViewBag.CollectionID = new SelectList(db.Collections.Where(l => l.Email == currentUser.Email), "ID", "Title", list.CollectionID);
            return View(list);
        }

        // GET: Lists/Delete/5
        public ActionResult Delete(int? id, int? ColID, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed please try again";
            }
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            if (ColID != null)
            {
                ViewBag.CollectionID = ColID;
            }
            return View(list);
        }



        // POST: Lists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? ColID)
        {

            try
            {
                List list = db.Lists.Find(id);

                db.Lists.Remove(list);
                db.SaveChanges();

            }
            catch (Exception)
            {

                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

            return RedirectToAction("Index", new { id = ColID });

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
