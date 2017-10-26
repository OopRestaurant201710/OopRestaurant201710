using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OopRestaurant201710.Models;

namespace OopRestaurant201710.Controllers
{
    public class TablesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tables
        public ActionResult Index()
        {
            //lekérdezzük az adatbásból az asztalok listáját és egy változóba mentjük
            var tables = db.Tables
                           .Include(x => x.Location)
                           .ToList(); //ekkor kéri le az adatokat ténylegesen

            //elkészítjük a ViewModel-t
            //1. kelleni fog a termeknek a listája

            var locations = db.Locations
                              .ToList();

            foreach (var location in locations)
            {
                location.Tables = tables.Where(x => x.Location.Id == location.Id)
                                        .ToList(); //Ez nem kell, mert a tables változóban van már, nem adatbázisból dolgozunk

                //Ha nem készítenénk saját változót, akkor egyből az adatbázisból is tölthetnénk 
                //a táblák listáját, ekkor a ciklus minden futásakor kinyúlnánk az adatbázisig
                ///////////////////////////////////////////////////////////////////////////////
                //location.Tables = db.Tables
                //                    .Include(x => x.Location)
                //                    .Where(x => x.Location.Id == location.Id)
                //                    .ToList(); //ekkor kéri le az adatokat ténylegesen

            }

            //majd ezt elküldjük a nézethez
            return View(locations);
        }

        // GET: Tables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // GET: Tables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Table table)
        {
            if (ModelState.IsValid)
            {
                db.Tables.Add(table);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(table);
        }

        // GET: Tables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Tables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Table table)
        {
            if (ModelState.IsValid)
            {
                db.Entry(table).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(table);
        }

        // GET: Tables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Table table = db.Tables.Find(id);
            db.Tables.Remove(table);
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
