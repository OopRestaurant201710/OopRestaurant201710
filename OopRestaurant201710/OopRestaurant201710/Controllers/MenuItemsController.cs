﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OopRestaurant201710;
using OopRestaurant201710.Models;

namespace OopRestaurant201710.Controllers
{
    public class MenuItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MenuItems
        public ActionResult Index()
        {
            return View(db.MenuItems.Include(x=>x.Category).ToList());
        }

        // GET: MenuItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }

        // GET: MenuItems/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                db.MenuItems.Add(menuItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menuItem);
        }

        // GET: MenuItems/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }

            //Ahhoz, hogy legyen, be kell töltenünk a menuItem Category property-jét,
            //amit magától az EF (EntityFramework) nem tölt be
            db.Entry(menuItem)
              .Reference(x => x.Category)
              .Load();

            //hogy be tudjuk állítani a lenyílót, megadjuk az aktuális Category azonosítóját
            menuItem.CategoryId = menuItem.Category.Id;
            //leküldjük a Categories tábla tartalmát (db.Categories.ToList())
            //megadjuk, hogy melyik mező azonosítja a sort, és adja azt az értéket, ami a végeredmény (Id),
            //megadjuk, hogy a lenyíló egyes soraiba, melyik property értékei kerüljenek (Name)
            menuItem.AssignableCategories = new SelectList(db.Categories.OrderBy(x=>x.Name).ToList(), "Id", "Name");

            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] //Be kell engedni a lenyíló által kiválasztott azonosítót
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,CategoryId")] MenuItem menuItem) 
        {
            if (ModelState.IsValid)
            {
                var category = db.Categories.Find(menuItem.CategoryId);

                //a html form-ról jövő adatokat "bemutatjuk" az adatbázisnak
                db.MenuItems.Attach(menuItem);

                //az adatbázissal kapcsolatos dolgok eléréséhez kell az Entry
                var entry = db.Entry(menuItem);

                //ennek segítségével betöltjük a Category tábla adatait a menuItem.Category property-be
                entry.Reference(x => x.Category).Load();

                //majd felülírjuk azzal, ami bejött a HTML form-on
                menuItem.Category = category;

                entry.State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuItem);
        }

        // GET: MenuItems/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuItem menuItem = db.MenuItems.Find(id);
            db.MenuItems.Remove(menuItem);
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
