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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace InfiniList.Controllers
{
    public class CollectionController : Controller
    {
        private InfiniBackEnd db = new InfiniBackEnd();

        // GET: Collection
        [Authorize]
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;//get current sort from UI
            //ViewBag.LNameSortParm = string.IsNullOrEmpty(sortOrder) ? "lname_desc" : "";
            //ViewBag.FNameSortParm = sortOrder == "fname" ? "fname_desc" : "fname";
            //ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            //ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";

            ViewBag.TitleSortParm = string.IsNullOrEmpty(sortOrder) ? "Title_desc" : "";
            ViewBag.FormatSortParm = sortOrder == "Format" ? "Format_desc" : "Format";
            ViewBag.SizeSortParm = sortOrder == "Size" ? "Size_desc" : "Size";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;


            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return HttpNotFound();

            }

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext.Create()));
            var currentUser = manager.FindByEmail(User.Identity.GetUserName());

            var email = currentUser.Email;

            //get collection data
            var collections = from s in db.Collections where s.Email == email select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                collections = collections.Where(s => s.Title.Contains(searchString) ||
                    s.Format.Contains(searchString));
            }
            //    public async Task<ActionResult> CollectionID(int? id)
            //{
            //    if(id == null)
            //    {

            //    }
            //}

            //Apply the sort order 
            switch (sortOrder)
            {

                //Format Asc
                case "Format":
                    collections = collections.OrderBy(s => s.Format);
                    break;

                //Format Desc
                case "Format_desc":
                    collections = collections.OrderByDescending(s => s.Format);
                    break;

                //DateCreated Asc
                case "date":
                    collections = collections.OrderBy(s => s.DateCreated);
                    break;
                case "date_desc":
                    collections = collections.OrderByDescending(s => s.DateCreated);
                    break;

                //Size Asc
                case "Size":
                    collections = collections.OrderBy(s => s.Size);
                    break;

                //Size Desc
                case "Size_desc":
                    collections = collections.OrderByDescending(s => s.Size);
                    break;
                //Title Desc
                case "Title":
                    collections = collections.OrderByDescending(s => s.Title);
                    break;

                //Default Title Asc
                default:
                    collections = collections.OrderBy(s => s.Title);
                    break;
            }
            //return the collection object as a enumerable (list)
            //return View(collectionws.ToList());

            //Setup Pager
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(collections.ToPagedList(pageNumber, pageSize));


        }

        

        // GET: Collection/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        // GET: Collection/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Collection/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Format,Size,DateCreated")] Collection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    string userId = User.Identity.GetUserId();

                    if (!string.IsNullOrEmpty(userId))
                    {

                        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext.Create()));
                        var currentUser = manager.FindByEmail(User.Identity.GetUserName());

                        collection.Email = currentUser.Email;
                        db.Collections.Add(collection);
                        db.SaveChanges();


                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return HttpNotFound();
                    }

                }
            }
            catch (Exception /*ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again later!");
            }

            return View(collection);
        }

        // GET: Collection/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        // POST: Collection/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var studentToUpdate = db.Collections.Find(id);
            if (TryUpdateModel(studentToUpdate, "", new string[] { "Title", "Format", "DateCreated", "Size" }))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                }
            }
            return View(studentToUpdate);
        }

        // GET: Collection/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed please try again";
            }
            Collection collections = db.Collections.Find(id);
            if (collections == null)
            {
                return HttpNotFound();
            }
            return View(collections);
        }

        // POST: Collection/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Collection collection = db.Collections.Find(id);
                db.Collections.Remove(collection);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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
