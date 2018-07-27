using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PortalToKnowledge.Models;

namespace PortalToKnowledge.Controllers
{
    public class ResourcesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Resources
        public ActionResult Index()
        {
			var resource = db.Resource.Include(r => r.City).Include(r => r.State).Include(r => r.Zipcode);
            return View(resource.ToList());
        }

        // GET: Resources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			var resource = db.Resource.Where(r => r.ResourceId == id).Include(c => c.City).Include(c => c.State).Include(c => c.Zipcode).FirstOrDefault();
			//Resource resource = db.Resource.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        // GET: Resources/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.City, "CityId", "Name");
            ViewBag.StateId = new SelectList(db.State, "StateId", "Abbreviation");
            ViewBag.ZipcodeId = new SelectList(db.Zipcode, "ZipcodeId", "Zip");
            return View();
        }

        // POST: Resources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResourceId,Name,StreetAddress,CityId,StateId,ZipcodeId")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                db.Resource.Add(resource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.City, "CityId", "Name", resource.CityId);
            ViewBag.StateId = new SelectList(db.State, "StateId", "Abbreviation", resource.StateId);
            ViewBag.ZipcodeId = new SelectList(db.Zipcode, "ZipcodeId", "Zip", resource.ZipcodeId);
            return View(resource);
        }

        // GET: Resources/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.Resource.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.City, "CityId", "Name", resource.CityId);
            ViewBag.StateId = new SelectList(db.State, "StateId", "Abbreviation", resource.StateId);
            ViewBag.ZipcodeId = new SelectList(db.Zipcode, "ZipcodeId", "Zip", resource.ZipcodeId);
            return View(resource);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResourceId,Name,StreetAddress,CityId,StateId,ZipcodeId")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.City, "CityId", "Name", resource.CityId);
            ViewBag.StateId = new SelectList(db.State, "StateId", "Abbreviation", resource.StateId);
            ViewBag.ZipcodeId = new SelectList(db.Zipcode, "ZipcodeId", "Zip", resource.ZipcodeId);
            return View(resource);
        }

        // GET: Resources/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.Resource.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Resource resource = db.Resource.Find(id);
            db.Resource.Remove(resource);
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
