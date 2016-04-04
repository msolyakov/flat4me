using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YaMaps.Models;

namespace YaMaps.Controllers
{
    public class AccommodationShortLocationsController : Controller
    {
        private MapDbContext db = new MapDbContext();

        // GET: AccommodationShortLocations
        public ActionResult Index()
        {
            return View(db.AccommodationShortLocationList.ToList());
        }

        // GET: AccommodationShortLocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccommodationShortLocation accommodationShortLocation = db.AccommodationShortLocationList.Find(id);
            if (accommodationShortLocation == null)
            {
                return HttpNotFound();
            }
            return View(accommodationShortLocation);
        }

        // GET: AccommodationShortLocations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccommodationShortLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccommodationId,FullAddress,PointY,PointX,PointData")] AccommodationShortLocation accommodationShortLocation)
        {
            if (ModelState.IsValid)
            {
                db.AccommodationShortLocationList.Add(accommodationShortLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accommodationShortLocation);
        }

        // GET: AccommodationShortLocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccommodationShortLocation accommodationShortLocation = db.AccommodationShortLocationList.Find(id);
            if (accommodationShortLocation == null)
            {
                return HttpNotFound();
            }
            return View(accommodationShortLocation);
        }

        // POST: AccommodationShortLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccommodationId,FullAddress,PointY,PointX,PointData")] AccommodationShortLocation accommodationShortLocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accommodationShortLocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accommodationShortLocation);
        }

        // GET: AccommodationShortLocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccommodationShortLocation accommodationShortLocation = db.AccommodationShortLocationList.Find(id);
            if (accommodationShortLocation == null)
            {
                return HttpNotFound();
            }
            return View(accommodationShortLocation);
        }

        // POST: AccommodationShortLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccommodationShortLocation accommodationShortLocation = db.AccommodationShortLocationList.Find(id);
            db.AccommodationShortLocationList.Remove(accommodationShortLocation);
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
